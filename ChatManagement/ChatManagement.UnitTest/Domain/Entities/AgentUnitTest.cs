using ChatManagement.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ChatManagement.UnitTest.Domain.Entities
{
    [TestClass]
    public class AgentUnitTest
    {
        [TestMethod]
        public void Shold_Agent_Can_Be_Created_TestMethod()
        {
            //arrange
            var expectedName = Guid.NewGuid().ToString();
            var expectedTeamId = Guid.Empty;
            var expectedSeniority = SeniorityLevel.Junior;
            var expectedAssignedChatSessions = 0;
            DateTime? expectedStartTime = null;

            //act
            var result = new Agent(expectedName, expectedSeniority);

            //assert
            Assert.AreNotEqual(result.Id, Guid.Empty, "Id is incorrect");
            Assert.AreEqual(result.Name, expectedName, "Name is incorrect");
            Assert.AreEqual(result.Seniority, expectedSeniority, "Seniority is not correct");
            Assert.AreEqual(result.TeamId, expectedTeamId, "TeamId is not correct");
            Assert.AreEqual(result.StartTime, expectedStartTime, "StartTime is not correct");
            Assert.AreEqual(result.AssignedChatSessions.Count(), expectedAssignedChatSessions, "AssignedChatSessions is not correct");
        }

        [TestMethod]
        public void Shold_Agent_Can_SetStartTime_TestMethod()
        {
            //arrange
            var expectedStartTime = DateTime.Now;
            var agent = TestDataGenerator.GetTestData_AvailableAgent();
            
            //act
            agent.SetStartTime(expectedStartTime);

            //assert
            Assert.AreEqual(agent.StartTime, expectedStartTime, "StartTime is incorrect");
        }

        [TestMethod]
        public void Shold_Agent_IsAvailable_TestMethod()
        {
            //arrange
            var agentStartTime = DateTime.Parse("16/06/2023 9:00:00"); ;
            var currentTime = DateTime.Parse("16/06/2023 17:00:00"); ;
            var agent = TestDataGenerator.GetTestData_NotAvailableAgent();
            agent.SetStartTime(agentStartTime);

            //act
            var result = agent.IsAvailable(currentTime);

            //assert
            Assert.IsTrue(result, "IsAvailable is incorrect");
        }

        [TestMethod]
        public void Shold_Agent_IsNotAvailable_BeforeShiftHour_TestMethod()
        {
            //arrange
            var agentStartTime = DateTime.Parse("16/06/2023 9:00:00"); ;
            var currentTime = DateTime.Parse("16/06/2023 8:59:00"); ;
            var agent = TestDataGenerator.GetTestData_NotAvailableAgent();
            agent.SetStartTime(agentStartTime);

            //act
            var result = agent.IsAvailable(currentTime);

            //assert
            Assert.IsFalse(result, "IsAvailable is incorrect");
        }

        [TestMethod]
        public void Shold_Agent_IsNotAvailable_AfterShiftHour_TestMethod()
        {
            //arrange
            var agentStartTime = DateTime.Parse("16/06/2023 9:00:00"); ;
            var currentTime = DateTime.Parse("16/06/2023 17:01:00"); ;
            var agent = TestDataGenerator.GetTestData_NotAvailableAgent();
            agent.SetStartTime(agentStartTime);

            //act
            var result = agent.IsAvailable(currentTime);

            //assert
            Assert.IsFalse(result, "IsAvailable is incorrect");
        }

        [TestMethod]
        public void Shold_Agent_Can_SetTeamId_TestMethod()
        {
            //arrange
            var expectedTeamId = Guid.NewGuid();
            var agent = TestDataGenerator.GetTestData_AvailableAgent();

            //act
            agent.SetTeamId(expectedTeamId);

            //assert
            Assert.AreEqual(agent.TeamId, expectedTeamId, "TeamId is incorrect");
        }

        [TestMethod]
        [DataRow(SeniorityLevel.Junior, 4)]
        [DataRow(SeniorityLevel.MidLevel, 6)]
        [DataRow(SeniorityLevel.Senior, 8)]
        [DataRow(SeniorityLevel.TeamLead, 5)]
        public void Shold_Agent_GetCapacity_TestMethod(SeniorityLevel level, int expectedResult)
        {
            //arrange
            var agent = new Agent(string.Format("Agent {0}", level.ToString()), level);
            //act
            var result = agent.GetCapacity();

            //assert
            Assert.AreEqual(expectedResult, result, "Capacity is incorrect");
        }

        [TestMethod]
        [DataRow(SeniorityLevel.Junior, 4)]
        [DataRow(SeniorityLevel.MidLevel, 6)]
        [DataRow(SeniorityLevel.Senior, 8)]
        [DataRow(SeniorityLevel.TeamLead, 5)]
        public void Shold_SeniorityLevel_GetCapacity_TestMethod(SeniorityLevel level, int expectedResult)
        {
            //arrange

            //act
            var result = level.GetCapacity();

            //assert
            Assert.AreEqual(expectedResult, result, "Capacity is incorrect");
        }

        [TestMethod]
        public void Shold_Agent_Can_AssignChat_TestMethod()
        {
            //arrange
            var agentStartTime = DateTime.Parse("16/06/2023 9:00:00");
            var chatSessionTime = DateTime.Parse("16/06/2023 09:01:00"); 
            var currentTime = DateTime.Parse("16/06/2023 09:02:00"); 
            var agent = new Agent("Test", SeniorityLevel.TeamLead);
            agent.SetStartTime(agentStartTime);
            var chatSession = new ChatSession(chatSessionTime);

            //act
            agent.AssignChat(chatSession, currentTime);

            //assert
            Assert.AreEqual(agent.AssignedChatSessions.FirstOrDefault(), chatSession, "ChatSession is incorrect");
        }
    }
}
