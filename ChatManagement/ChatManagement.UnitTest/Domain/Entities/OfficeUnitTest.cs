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

        private static DateTime Time1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);

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
            Action act = () => office.AddTeam(expectedTeam);

            //assert
            Assert.ThrowsException<Exception>(act);
        }


        [TestMethod]
        public void Shold_Office_Cannot_Add_DuplicateTeam_TestMethod()
        {
            //arrange
            var expectedTeam = GetTestData_ValidTeam();
            var office = new Office();
            office.AddTeam(expectedTeam);

            //act
            Action act = () => office.AddTeam(expectedTeam);

            //assert
            Assert.ThrowsException<Exception>(act);
        }

        [TestMethod]
        public void Shold_Office_Can_HasAvailableAgents_TestMethod()
        {
            //arrange
            var team1 = EntityGenerator.GetTestData_Team_With_AvailableAgents();
            var team2 = EntityGenerator.GetTestData_Team_With_NoAvailableAgents();
            var office = new Office();
            office.AddTeam(team1);
            office.AddTeam(team2);

            //act
            var result = office.HasAvailableAgents();

            //assert
            Assert.IsTrue(result, "HasAvailableAgents is incorrect");
        }

        [TestMethod]
        public void Shold_Team_Cannot_HasAvailableAgents_TestMethod()
        {
            //arrange
            var team1 = EntityGenerator.GetTestData_Team_With_NoAvailableAgents();
            var team2 = EntityGenerator.GetTestData_Team_With_NoAvailableAgents();
            var office = new Office();
            office.AddTeam(team1);
            office.AddTeam(team2);

            //act
            var result = office.HasAvailableAgents();

            //assert
            Assert.IsFalse(result, "HasAvailableAgents is incorrect");
        }

    }
}
