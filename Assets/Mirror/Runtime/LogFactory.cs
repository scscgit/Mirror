using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public static class LogFactory
    {
        internal static readonly Dictionary<string, IMirrorLogger> loggers = new Dictionary<string, IMirrorLogger>();

        public static IMirrorLogger GetLogger<T>(LogType defaultLogLevel = LogType.Warning)
        {
            return GetLogger(typeof(T).Name, defaultLogLevel);
        }

        public static IMirrorLogger GetLogger(System.Type type, LogType defaultLogLevel = LogType.Warning)
        {
            return GetLogger(type.Name, defaultLogLevel);
        }

        public static IMirrorLogger GetLogger(string loggerName, LogType defaultLogLevel = LogType.Warning)
        {
            if (loggers.TryGetValue(loggerName, out IMirrorLogger logger))
            {
                return logger;
            }

            // by default, log warnings and up
            logger = new MirrorLogger(Debug.unityLogger, defaultLogLevel);

            loggers[loggerName] = logger;
            return logger;
        }
    }


    public static class ILoggerExtensions
    {
        public static bool LogEnabled(this IMirrorLogger logger) => logger.IsLogTypeAllowed(LogType.Log);
    }

    public interface IMirrorLogger
    {
        void Log(string msg);
        void Log(string msg, UnityEngine.Object context);
        void LogFormat(string msg, params object[] args);
        void LogFormat(UnityEngine.Object context, string msg, params object[] args);

        void LogWarning(string msg);
        void LogWarning(string msg, UnityEngine.Object context);
        void LogWarningFormat(string msg, params object[] args);
        void LogWarningFormat(UnityEngine.Object context, string msg, params object[] args);

        void LogError(string msg);
        void LogError(string msg, UnityEngine.Object context);
        void LogErrorFormat(string msg, params object[] args);
        void LogErrorFormat(UnityEngine.Object context, string msg, params object[] args);

        void Assert(bool condition, string msg);
        void Assert(bool condition, string msg, UnityEngine.Object context);
        void AssertFormat(bool condition, string msg, params object[] args);
        void AssertFormat(bool condition, UnityEngine.Object context, string msg, params object[] args);

        void LogException(Exception exception, UnityEngine.Object context = null);

        bool IsLogTypeAllowed(LogType logType);
        LogType filterLogType { get; set; }
        ILogHandler logHandler { get; set; }
    }

    public class MirrorLogger : IMirrorLogger
    {
        public MirrorLogger(ILogHandler logHandler, LogType filterLogType)
        {
            this.logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));
            this.filterLogType = filterLogType;
        }

        public LogType filterLogType { get; set; }
        public ILogHandler logHandler { get; set; }

        void InternalAssert(UnityEngine.Object context, string format, object[] args)
        {
            if (IsLogTypeAllowed(LogType.Assert))
                logHandler.LogFormat(LogType.Assert, context, format, args);
        }

        void InternalError(UnityEngine.Object context, string format, object[] args)
        {
            if (IsLogTypeAllowed(LogType.Error))
                logHandler.LogFormat(LogType.Error, context, format, args);
        }

        void InternalWarning(UnityEngine.Object context, string format, object[] args)
        {
            if (IsLogTypeAllowed(LogType.Warning))
                logHandler.LogFormat(LogType.Warning, context, format, args);
        }

        void InternalLog(UnityEngine.Object context, string format, object[] args)
        {
            if (IsLogTypeAllowed(LogType.Log))
                logHandler.LogFormat(LogType.Log, context, format, args);
        }


        public bool IsLogTypeAllowed(LogType logType)
        {
            return filterLogType != LogType.Exception && logType <= filterLogType;
        }


        public void Assert(bool condition, string msg)
        {
            if (!condition)
                InternalAssert(null, msg, System.Array.Empty<object>());
        }

        public void Assert(bool condition, string msg, UnityEngine.Object context)
        {
            if (!condition)
                InternalAssert(context, msg, System.Array.Empty<object>());
        }

        public void AssertFormat(bool condition, string msg, params object[] args)
        {
            if (!condition)
                InternalAssert(null, msg, args);
        }

        public void AssertFormat(bool condition, UnityEngine.Object context, string msg, params object[] args)
        {
            if (!condition)
                InternalAssert(context, msg, args);
        }


        public void Log(string msg)
        {
            InternalLog(null, msg, System.Array.Empty<object>());
        }

        public void Log(string msg, UnityEngine.Object context)
        {
            InternalLog(null, msg, System.Array.Empty<object>());
        }

        public void LogFormat(string msg, params object[] args)
        {
            InternalLog(null, msg, args);
        }

        public void LogFormat(UnityEngine.Object context, string msg, params object[] args)
        {
            InternalLog(context, msg, args);
        }


        public void LogWarning(string msg)
        {
            InternalWarning(null, msg, System.Array.Empty<object>());
        }

        public void LogWarning(string msg, UnityEngine.Object context)
        {
            InternalWarning(context, msg, System.Array.Empty<object>());
        }

        public void LogWarningFormat(string msg, params object[] args)
        {
            InternalWarning(null, msg, args);
        }

        public void LogWarningFormat(UnityEngine.Object context, string msg, params object[] args)
        {
            InternalWarning(context, msg, args);
        }


        public void LogError(string msg)
        {
            InternalError(null, msg, System.Array.Empty<object>());
        }

        public void LogError(string msg, UnityEngine.Object context)
        {
            InternalError(context, msg, System.Array.Empty<object>());
        }

        public void LogErrorFormat(string msg, params object[] args)
        {
            InternalError(null, msg, args);
        }

        public void LogErrorFormat(UnityEngine.Object context, string msg, params object[] args)
        {
            InternalError(context, msg, args);
        }

        public void LogException(Exception exception, UnityEngine.Object context = null)
        {
            logHandler.LogException(exception, context);
        }
    }
}
