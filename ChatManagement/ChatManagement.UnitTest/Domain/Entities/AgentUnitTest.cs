using ChatManagement.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var expectedIsAvailable = true;

            //act
            var result = new Agent(expectedName, expectedSeniority);

            //assert
            Assert.AreNotEqual(result.Id, Guid.Empty, "Id is incorrect");
            Assert.AreEqual(result.Name, expectedName, "Name is incorrect");
            Assert.AreEqual(result.Seniority, expectedSeniority, "Seniority is not correct");
            Assert.AreEqual(result.TeamId, expectedTeamId, "TeamId is not correct");
            Assert.AreEqual(result.TeamId, expectedTeamId, "TeamId is not correct");
            Assert.AreEqual(result.IsAvailable, expectedIsAvailable, "IsAvailable is not correct");
        }

        [TestMethod]
        public void Shold_Agent_Can_SetIsNotAvailable_TestMethod()
        {
            //arrange
            var agent = EntityGenerator.GetTestData_AvailableAgent();

            //act
            agent.SetIsNotAvailable();

            //assert
            Assert.IsFalse(agent.IsAvailable, "IsAvailable is incorrect");
        }

        [TestMethod]
        public void Shold_Agent_Can_SetIsAvailable_TestMethod()
        {
            //arrange
            var agent = EntityGenerator.GetTestData_NotAvailableAgent();

            //act
            agent.SetIsAvailable();

            //assert
            Assert.IsTrue(agent.IsAvailable, "IsAvailable is incorrect");
        }

        [TestMethod]
        public void Shold_Agent_Can_SetTeamId_TestMethod()
        {
            //arrange
            var expectedTeamId = Guid.NewGuid();
            var agent = EntityGenerator.GetTestData_AvailableAgent();

            //act
            agent.SetTeamId(expectedTeamId);

            //assert
            Assert.AreEqual(agent.TeamId, expectedTeamId, "TeamId is incorrect");
        }

    }
}
