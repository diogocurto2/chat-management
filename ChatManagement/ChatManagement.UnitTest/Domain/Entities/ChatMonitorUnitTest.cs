using ChatManagement.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChatManagement.UnitTest.Domain.Entities
{
    [TestClass]
    public class ChatMonitorUnitTest
    {
        [TestMethod]
        public void Shold_ChatMonitor_Can_Be_Created_TestMethod()
        {
            //arrange
            var expectedLastPollTime = DateTime.Now;
            var expectedChatSession = new ChatSession(expectedLastPollTime);

            //act
            var result = new ChatMonitor(expectedChatSession.Id, expectedLastPollTime);

            //assert
            Assert.AreEqual(result.ChatSessionId, expectedChatSession.Id, "ChatSession is incorrect");
            Assert.AreEqual(result.LastPollTime, expectedLastPollTime, "LastPollTime is not correct");
            Assert.IsTrue(result.IsActive, "session is not active");
        }

        [TestMethod]
        public void Shold_ChatMonitor_Deactivate_TestMethod()
        {
            //arrange
            var chatMonitor = EntityGenerator.GetTestData_ValidChatMonitor();
            var currentDateTime = chatMonitor.LastPollTime.AddSeconds(3);

            //act
            chatMonitor.Deactivate(currentDateTime);

            //assert
            Assert.IsFalse(chatMonitor.IsActive, "IsActive is incorrect");
        }


        [TestMethod]
        public void Shold_ChatMonitor_Not_Deactivate_TestMethod()
        {
            //arrange
            var chatMonitor = EntityGenerator.GetTestData_ValidChatMonitor();
            var currentDateTime = chatMonitor.LastPollTime.AddSeconds(2);

            //act
            chatMonitor.Deactivate(currentDateTime);

            //assert
            Assert.IsTrue(chatMonitor.IsActive, "IsActive is incorrect");
        }


        [TestMethod]
        public void Shold_ChatMonitor_Can_Deactivate_TestMethod()
        {
            //arrange
            var chatMonitor = EntityGenerator.GetTestData_ValidChatMonitor();
            var currentDateTime = chatMonitor.LastPollTime.AddSeconds(3);

            //act
            var result = chatMonitor.CanDeactivate(currentDateTime);

            //assert
            Assert.IsTrue(result, "Can Deactivate is incorrect");
        }


        [TestMethod]
        public void Shold_ChatMonitor_Cannot_Deactivate_TestMethod()
        {
            //arrange
            var chatMonitor = EntityGenerator.GetTestData_ValidChatMonitor();
            var currentDateTime = chatMonitor.LastPollTime.AddSeconds(2);

            //act
            var result = chatMonitor.CanDeactivate(currentDateTime);

            //assert
            Assert.IsFalse(result, "Can Deactivate is incorrect");
        }
    }
}
