using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public static class LogFactory
    {
        internal static readonly Dictionary<string, ILogger> loggers = new Dictionary<string, ILogger>();

        public static ILogger GetLogger<T>(LogType defaultLogLevel = LogType.Warning)
        {
            return GetLogger(typeof(T).Name, defaultLogLevel);
        }

        public static ILogger GetLogger(System.Type type, LogType defaultLogLevel = LogType.Warning)
        {
            return GetLogger(type.Name, defaultLogLevel);
        }

        public static ILogger GetLogger(string loggerName, LogType defaultLogLevel = LogType.Warning)
        {
            if (loggers.TryGetValue(loggerName, out ILogger logger))
            {
                return logger;
            }

            logger = new Logger(Debug.unityLogger)
            {
                // by default, log warnings and up
                filterLogType = defaultLogLevel
            };

            loggers[loggerName] = logger;
            return logger;
        }
    }


    public static class ILoggerExtensions
    {
        public static void LogError(this ILogger logger, object message)
        {
            logger.Log(LogType.Error, message);
        }
        public static void LogError(this ILogger logger, object message, Object context)
        {
            logger.Log(LogType.Error, message, context);
        }
        public static void LogErrorFormat(this ILogger logger, string format, params object[] args)
        {
            logger.LogFormat(LogType.Error, format, args);
        }
        public static void LogError(this ILogger logger, Object context, string format, params object[] args)
        {
            logger.LogFormat(LogType.Error, context, format, args);
        }


        [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
        public static void Assert(this ILogger logger, bool condition, string message)
        {
            if (!condition)
                logger.Log(LogType.Assert, message);
        }
        [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
        public static void Assert(this ILogger logger, bool condition, string message, params object[] args)
        {
            if (!condition)
                logger.LogFormat(LogType.Assert, message, args);
        }
        [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
        public static void Assert(this ILogger logger, bool condition, string message, Object context)
        {
            if (!condition)
                logger.Log(LogType.Assert, (object)message, context);
        }
        [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
        public static void Assert(this ILogger logger, bool condition, Object context, string message, params object[] args)
        {
            if (!condition)
                logger.LogFormat(LogType.Assert, context, message, args);
        }

        public static void LogWarning(this ILogger logger, object message)
        {
            logger.Log(LogType.Warning, message);
        }
        public static void LogWarning(this ILogger logger, object message, Object context)
        {
            logger.Log(LogType.Warning, message, context);
        }
        public static void LogWarningFormat(this ILogger logger, string format, params object[] args)
        {
            logger.LogFormat(LogType.Warning, format, args);
        }
        public static void LogWarningFormat(this ILogger logger, Object context, string format, params object[] args)
        {
            logger.LogFormat(LogType.Warning, context, format, args);
        }


        public static void Log(this ILogger logger, object message)
        {
            logger.Log(LogType.Log, message);
        }
        public static void Log(this ILogger logger, string message, Object context)
        {
            logger.Log(LogType.Log, (object)message, context);
        }
        public static void Log(this ILogger logger, object message, Object context)
        {
            logger.Log(LogType.Log, message, context);
        }
        public static void LogFormat(this ILogger logger, string format, params object[] args)
        {
            logger.LogFormat(LogType.Log, format, args);
        }
        public static void LogFormat(this ILogger logger, Object context, string format, params object[] args)
        {
            logger.LogFormat(LogType.Log, context, format, args);
        }



        public static bool LogEnabled(this ILogger logger) => logger.IsLogTypeAllowed(LogType.Log);
    }
}
