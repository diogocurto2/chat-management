using ChatManagement.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ChatManagement.UnitTest.Domain.Entities
{
    [TestClass]
    public class TeamUnitTest
    {
        [TestMethod]
        public void Shold_Team_Can_Be_Created_TestMethod()
        {
            //arrange
            var expectedName = Guid.NewGuid().ToString();
            var expectedAgentListCount = 0;

            //act
            var result = new Team(expectedName);

            //assert
            Assert.AreNotEqual(result.Id, Guid.Empty, "Id is incorrect");
            Assert.AreEqual(result.Name, expectedName, "Name is incorrect");
            Assert.AreEqual(result.Agents.Count(), expectedAgentListCount, "Agents is not correct");
        }

        [TestMethod]
        public void Shold_Team_Can_Add_ValidAgent_TestMethod()
        {
            //arrange
            var expectedAgent = TestDataGenerator.GetTestData_ValidAgent();
            var team = TestDataGenerator.GetTestData_ValidTeam();

            //act
            team.AddAgent(expectedAgent);
            var result = team.Agents.FirstOrDefault();

            //assert
            Assert.AreEqual(result, expectedAgent, "Agents is not correct");
            Assert.AreEqual(result.TeamId, team.Id, "TeamId is not correct");
        }

        [TestMethod]
        public void Shold_Team_Cannot_Add_InvalidAgent_TestMethod()
        {
            //arrange
            Agent expectedAgent = null;
            var team = TestDataGenerator.GetTestData_ValidTeam();

            //act
            Action act = () => team.AddAgent(expectedAgent);

            //assert
            Assert.ThrowsException<Exception>(act);
        }


        [TestMethod]
        public void Shold_Team_Cannot_Add_DuplicateAgent_TestMethod()
        {
            //arrange
            var expectedAgent = TestDataGenerator.GetTestData_ValidAgent();
            var team = TestDataGenerator.GetTestData_ValidTeam();
            team.AddAgent(expectedAgent);

            //act
            Action act = () => team.AddAgent(expectedAgent);

            //assert
            Assert.ThrowsException<Exception>(act);
        }


        [TestMethod]
        public void Shold_Team_Can_HasAvailableAgents_TestMethod()
        {
            //arrange
            DateTime currentDateTime = TestDataGenerator.GetTestData_CurrentDateTime();
            var team = TestDataGenerator.GetTestData_Team_With_AvailableAgents();

            //act
            var result = team.HasAvailableAgents(currentDateTime);

            //assert
            Assert.IsTrue(result, "HasAvailableAgents is incorrect");
        }

        [TestMethod]
        public void Shold_Team_Cannot_HasAvailableAgents_TestMethod()
        {
            //arrange
            DateTime currentDateTime = TestDataGenerator.GetTestData_CurrentDateTime();
            var team = TestDataGenerator.GetTestData_Team_With_NoAvailableAgents();

            //act
            var result = team.HasAvailableAgents(currentDateTime);

            //assert
            Assert.IsFalse(result, "HasAvailableAgents is incorrect");
        }


        [TestMethod]
        public void Shold_Team_Can_GetCapacity_TestMethod()
        {
            //arrange
            var expectedCapacity = 16;
            DateTime agentStartTime = TestDataGenerator.GetTestData_CurrentDateTime().AddMinutes(-1);
            DateTime currentDateTime = TestDataGenerator.GetTestData_CurrentDateTime();
            var agent1 = new Agent("Agent 1", SeniorityLevel.Junior);
            agent1.SetStartTime(agentStartTime);
            var agent2 = new Agent("Agent 2", SeniorityLevel.MidLevel);
            agent2.SetStartTime(agentStartTime);
            var agent3 = new Agent("Agent 3", SeniorityLevel.MidLevel);
            agent3.SetStartTime(agentStartTime);
            var team = TestDataGenerator.GetTestData_ValidTeam();
            team.AddAgent(agent3);
            team.AddAgent(agent2);
            team.AddAgent(agent1);

            //act
            var result = team.GetCapacity(currentDateTime);

            //assert
            Assert.AreEqual(expectedCapacity, result, "Capacity is incorrect");
        }

        [TestMethod]
        public void Shold_Team_GetNextAvailableAgent_JuniorIsAvailable_TestMethod()
        {
            //arrange
            DateTime agentStartTime = TestDataGenerator.GetTestData_CurrentDateTime().AddMinutes(-1);
            DateTime currentDateTime = TestDataGenerator.GetTestData_CurrentDateTime();
            var agentJunior = new Agent("Agent 1", SeniorityLevel.Junior);
            agentJunior.SetStartTime(agentStartTime);
            agentJunior.AssignChat(new ChatSession(currentDateTime), currentDateTime);
            var agentMid = new Agent("Agent 2", SeniorityLevel.MidLevel);
            agentMid.SetStartTime(agentStartTime);
            var team = TestDataGenerator.GetTestData_ValidTeam();
            team.AddAgent(agentMid);
            team.AddAgent(agentJunior);

            //act
            var result = team.GetNextAvailableAgent(currentDateTime);

            //assert
            Assert.AreEqual(agentJunior, result, "NextAvailableAgent is incorrect");
        }

        [TestMethod]
        public void Shold_Team_GetNextAvailableAgent_JuniorIsNotAvailable_TestMethod()
        {
            //arrange
            DateTime agentStartTime = TestDataGenerator.GetTestData_CurrentDateTime().AddMinutes(-1);
            DateTime currentDateTime = TestDataGenerator.GetTestData_CurrentDateTime();
            var agentJunior = new Agent("Agent 1", SeniorityLevel.Junior);
            agentJunior.SetStartTime(agentStartTime);
            agentJunior.AssignChat(new ChatSession(currentDateTime), currentDateTime);
            agentJunior.AssignChat(new ChatSession(currentDateTime), currentDateTime);
            agentJunior.AssignChat(new ChatSession(currentDateTime), currentDateTime);
            agentJunior.AssignChat(new ChatSession(currentDateTime), currentDateTime);
            var agentMid = new Agent("Agent 2", SeniorityLevel.MidLevel);
            agentMid.SetStartTime(agentStartTime);
            var team = TestDataGenerator.GetTestData_ValidTeam();
            team.AddAgent(agentMid);
            team.AddAgent(agentJunior);

            //act
            var result = team.GetNextAvailableAgent(currentDateTime);

            //assert
            Assert.AreEqual(agentMid, result, "NextAvailableAgent is incorrect");
        }

    }
}
