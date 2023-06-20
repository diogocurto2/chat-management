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
            var expectedAgent = EntityGenerator.GetTestData_ValidAgent();
            var team = EntityGenerator.GetTestData_ValidTeam();

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
            var team = EntityGenerator.GetTestData_ValidTeam();

            //act
            Action act = () => team.AddAgent(expectedAgent);

            //assert
            Assert.ThrowsException<Exception>(act);
        }


        [TestMethod]
        public void Shold_Team_Cannot_Add_DuplicateAgent_TestMethod()
        {
            //arrange
            var expectedAgent = EntityGenerator.GetTestData_ValidAgent();
            var team = EntityGenerator.GetTestData_ValidTeam();
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
            var team = EntityGenerator.GetTestData_Team_With_AvailableAgents();

            //act
            var result = team.HasAvailableAgents();

            //assert
            Assert.IsTrue(result, "HasAvailableAgents is incorrect");
        }

        [TestMethod]
        public void Shold_Team_Cannot_HasAvailableAgents_TestMethod()
        {
            //arrange
            var team = EntityGenerator.GetTestData_Team_With_NoAvailableAgents();

            //act
            var result = team.HasAvailableAgents();

            //assert
            Assert.IsFalse(result, "HasAvailableAgents is incorrect");
        }


    }
}
