using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mirror.Tests
{

    public class LogFactoryTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SameClassSameLogger()
        {
            IMirrorLogger logger1 = LogFactory.GetLogger<LogFactoryTests>();
            IMirrorLogger logger2 = LogFactory.GetLogger<LogFactoryTests>();
            Assert.That(logger1, Is.SameAs(logger2));
        }

        [Test]
        public void DifferentClassDifferentLogger()
        {
            IMirrorLogger logger1 = LogFactory.GetLogger<LogFactoryTests>();
            IMirrorLogger logger2 = LogFactory.GetLogger<NetworkManager>();
            Assert.That(logger1, Is.Not.SameAs(logger2));
        }

        [Test]
        public void LogDebugIgnore()
        {
            IMirrorLogger logger = LogFactory.GetLogger<LogFactoryTests>();
            logger.filterLogType = LogType.Warning;

            ILogHandler mockHandler = Substitute.For<ILogHandler>();
            logger.logHandler = mockHandler;
            logger.Log("This message should not be logged");
            mockHandler.DidNotReceiveWithAnyArgs().LogFormat(LogType.Log, null, null);
        }

        [Test]
        public void LogDebugFull()
        {
            IMirrorLogger logger = LogFactory.GetLogger<LogFactoryTests>();
            logger.filterLogType = LogType.Log;

            ILogHandler mockHandler = Substitute.For<ILogHandler>();
            logger.logHandler = mockHandler;
            logger.Log("This message be logged");
            mockHandler.Received().LogFormat(LogType.Log, null, "{0}", "This message be logged");
        }
    }
}
