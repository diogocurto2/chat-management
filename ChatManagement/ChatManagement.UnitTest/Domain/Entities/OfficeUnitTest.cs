using ChatManagement.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatManagement.UnitTest.Domain.Entities
{
    [TestClass]
    public class OfficeUnitTest
    {

        [TestMethod]
        public void Shold_Office_Can_ConfiguteOfficeHours_TestMethod()
        {
            //arrange
            var expectedStartTime = new TimeSpan(9, 0, 0);
            var expectedEndTime = new TimeSpan(18, 0, 0);
            var expectedListOfWeekDays = new List<DayOfWeek>() 
            { 
                DayOfWeek.Monday, 
                DayOfWeek.Tuesday, 
                DayOfWeek.Wednesday, 
                DayOfWeek.Thursday, 
                DayOfWeek.Friday 
            };

            var office = new Office();

            //act
            office.ConfiguteOfficeHours(
                expectedStartTime, 
                expectedEndTime, 
                expectedListOfWeekDays);

            //assert
            Assert.AreEqual(office.StartTime, expectedStartTime, "StartTime is not correct");
            Assert.AreEqual(office.EndTime, expectedEndTime, "EndTime is not correct");
            Assert.AreEqual(office.WeekDays, expectedListOfWeekDays, "WeekDays is not correct");
        }

        [DataTestMethod]
        [DynamicData(nameof(GetTestData_IsDuringOfficeHours), DynamicDataSourceType.Method)]
        public void Shold_Office_IsDuringOfficeHours_TestMethod(DateTime timeToTest)
        {
            //arrange
            var officeStartTime = new TimeSpan(9, 0, 0);
            var officeEndTime = new TimeSpan(18, 0, 0);
            var officeListOfWeekDays = new List<DayOfWeek>()
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };
            var office = new Office();
            office.ConfiguteOfficeHours(
                officeStartTime, 
                officeEndTime,
                officeListOfWeekDays);

            //act
            var result = office.IsDuringOfficeHours(timeToTest);

            //assert
            Assert.IsTrue(result, string.Format("IsDuringOfficeHours is not correct for {0}", timeToTest));
        }

        private static IEnumerable<object[]> GetTestData_IsDuringOfficeHours()
        {
            yield return new object[] { DateTime.Parse("16/06/2023 9:00:00") };
            yield return new object[] { DateTime.Parse("16/06/2023 10:00:00") };
            yield return new object[] { DateTime.Parse("16/06/2023 14:00:00") };
            yield return new object[] { DateTime.Parse("16/06/2023 17:59:00") };
            yield return new object[] { DateTime.Parse("16/06/2023 18:00:00") };
        }


        [DataTestMethod]
        [DynamicData(nameof(GetTestData_IsOutOfficeHours), DynamicDataSourceType.Method)]
        public void Shold_Office_IsOutOfficeHours_TestMethod(DateTime timeToTest)
        {
            //arrange
            var officeStartTime = new TimeSpan(9, 0, 0);
            var officeEndTime = new TimeSpan(18, 0, 0);
            var officeListOfWeekDays = new List<DayOfWeek>()
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday
            };
            var office = new Office();
            office.ConfiguteOfficeHours(
                officeStartTime,
                officeEndTime,
                officeListOfWeekDays);

            //act
            var result = office.IsDuringOfficeHours(timeToTest);

            //assert
            Assert.IsFalse(result, string.Format("IsDuringOfficeHours is not correct for {0}", timeToTest));
        }

        private static IEnumerable<object[]> GetTestData_IsOutOfficeHours()
        {
            yield return new object[] { DateTime.Parse("16/06/2023 08:00:00") };
            yield return new object[] { DateTime.Parse("16/06/2023 08:59:00") };
            yield return new object[] { DateTime.Parse("16/06/2023 18:01:00") };
            yield return new object[] { DateTime.Parse("16/06/2023 19:00:00") };
            yield return new object[] { DateTime.Parse("17/06/2023 10:00:00") };
            yield return new object[] { DateTime.Parse("18/06/2023 14:00:00") };
            yield return new object[] { DateTime.Parse("17/06/2023 17:59:00") };
        }


        [TestMethod]
        public void Shold_Office_Can_Add_ValidTeam_TestMethod()
        {
            //arrange
            var expectedTeam = GetTestData_ValidTeam();
            var office = new Office();

            //act
            office.AddTeam(expectedTeam);

            //assert
            Assert.AreEqual(office.Teams.FirstOrDefault(), expectedTeam, "Teams is not correct");
        }

        private static Team GetTestData_ValidTeam()
        {
            var team = new Team("Valid Team");

            return team;
        }

        [TestMethod]
        public void Shold_Office_Cannot_Add_InvalidTeam_TestMethod()
        {
            //arrange
            Team expectedTeam = null;
            var office = new Office();

            //act
            Action act = () => office.AddTeam(expectedTeam, false);

            //assert
            Assert.ThrowsException<Exception>(act);
        }


        [TestMethod]
        public void Shold_Office_Cannot_Add_DuplicateTeam_TestMethod()
        {
            //arrange
            var expectedTeam = GetTestData_ValidTeam();
            var office = new Office();
            office.AddTeam(expectedTeam, false);

            //act
            Action act = () => office.AddTeam(expectedTeam, false);

            //assert
            Assert.ThrowsException<Exception>(act);
        }

        [TestMethod]
        public void Shold_Office_Can_HasAvailableAgents_TestMethod()
        {
            //arrange
            DateTime currentDateTime = TestDataGenerator.GetTestData_CurrentDateTime();
            var team1 = TestDataGenerator.GetTestData_Team_With_AvailableAgents();
            var team2 = TestDataGenerator.GetTestData_Team_With_NoAvailableAgents();
            var office = new Office();
            office.AddTeam(team1, false);
            office.AddTeam(team2, false);

            //act
            var result = office.HasAvailableAgents(currentDateTime);

            //assert
            Assert.IsTrue(result, "HasAvailableAgents is incorrect");
        }

        [TestMethod]
        public void Shold_Team_Cannot_HasAvailableAgents_TestMethod()
        {
            //arrange
            DateTime currentDateTime = TestDataGenerator.GetTestData_CurrentDateTime();
            var team1 = TestDataGenerator.GetTestData_Team_With_NoAvailableAgents();
            var team2 = TestDataGenerator.GetTestData_Team_With_NoAvailableAgents();
            var office = new Office();
            office.AddTeam(team1);
            office.AddTeam(team2);

            //act
            var result = office.HasAvailableAgents(currentDateTime);

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
            var team1 = TestDataGenerator.GetTestData_ValidTeam();
            team1.AddAgent(agent3);
            team1.AddAgent(agent2);
            var team2 = TestDataGenerator.GetTestData_ValidTeam();
            team2.AddAgent(agent1);

            var office = new Office();
            office.AddTeam(team2);
            office.AddTeam(team1);

            //act
            var result = office.GetCapacity(currentDateTime);

            //assert
            Assert.AreEqual(expectedCapacity, result, "Capacity is incorrect");
        }

        [TestMethod]
        public void Shold_Team_Can_GetMaximumQueueLength_TestMethod()
        {
            //arrange
            var expectedCapacity = 24;
            DateTime agentStartTime = TestDataGenerator.GetTestData_CurrentDateTime().AddMinutes(-1);
            DateTime currentDateTime = TestDataGenerator.GetTestData_CurrentDateTime();
            var agent1 = new Agent("Agent 1", SeniorityLevel.Junior);
            agent1.SetStartTime(agentStartTime);
            var agent2 = new Agent("Agent 2", SeniorityLevel.MidLevel);
            agent2.SetStartTime(agentStartTime);
            var agent3 = new Agent("Agent 3", SeniorityLevel.MidLevel);
            agent3.SetStartTime(agentStartTime);
            var team1 = TestDataGenerator.GetTestData_ValidTeam();
            team1.AddAgent(agent3);
            team1.AddAgent(agent2);
            var team2 = TestDataGenerator.GetTestData_ValidTeam();
            team2.AddAgent(agent1);

            var office = new Office();
            office.AddTeam(team2);
            office.AddTeam(team1);

            //act
            var result = office.GetMaximumQueueLength(currentDateTime);

            //assert
            Assert.AreEqual(expectedCapacity, result, "Capacity is incorrect");
        }

    }
}
