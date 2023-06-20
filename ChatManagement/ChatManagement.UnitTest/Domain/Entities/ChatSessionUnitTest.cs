using ChatManagement.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChatManagement.UnitTest.Domain.Entities
{
    [TestClass]
    public class ChatSessionUnitTest
    {
        [TestMethod]
        public void Shold_ChatSession_Can_Be_Created_TestMethod()
        {
            //arrange
            var expectedStartDateTime = DateTime.Now;
            //act
            var result = new ChatSession(expectedStartDateTime);

            //assert
            Assert.AreNotEqual(result.Id, Guid.Empty, "Session id is empty");
            Assert.AreEqual(result.StartTime, expectedStartDateTime, "StartTime is not correct");
            Assert.IsNull(result.EndTime, "session endTime is not null");
        }

        [TestMethod]
        public void Shold_ChatSession_Can_Be_Assign_TestMethod()
        {
            //arrange
            var agent = TestDataGenerator.GetTestData_NotAvailableAgent();
            var chatSession = TestDataGenerator.GetTestData_ValidChatSession();

            //act
            chatSession.Assign(agent); ;

            //assert
            Assert.AreEqual(chatSession.Agent, agent, "Agent is incorrect");
            Assert.AreEqual(chatSession.AgentId, agent.Id, "AgentId is incorrect");
        }

        [TestMethod]
        public void Shold_ChatSession_Cant_Be_Assign_TestMethod()
        {
            //arrange
            Agent agent = null;
            var chatSession = TestDataGenerator.GetTestData_ValidChatSession();

            //act
            Action act = () => chatSession.Assign(agent);

            //assert
            Assert.ThrowsException<Exception>(act);
        }
    }
}
