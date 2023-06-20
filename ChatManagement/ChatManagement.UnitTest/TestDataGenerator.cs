using ChatManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ChatManagement.UnitTest
{
    public static class TestDataGenerator
    {

        public static DateTime GetTestData_CurrentDateTime()
        {
            return DateTime.Parse("16/06/2023 9:00:00");
        }

        public static Agent GetTestData_NotAvailableAgent()
        {
            var name = Guid.NewGuid().ToString();
            var seniority = SeniorityLevel.Junior;
            var agent = new Agent(name, seniority);

            return agent;
        }

        public static Agent GetTestData_AvailableAgent()
        {
            var name = Guid.NewGuid().ToString();
            var seniority = SeniorityLevel.Junior;
            var agent = new Agent(name, seniority);

            return agent;
        }

        public static ChatMonitor GetTestData_ValidChatMonitor()
        {
            var chatSessionId = Guid.NewGuid();
            var validDateTime = GetTestData_CurrentDateTime();
            var chatMonitor = new ChatMonitor(chatSessionId, validDateTime);

            return chatMonitor;
        }

        public static ChatSession GetTestData_ValidChatSession()
        {
            var validDateTime = GetTestData_CurrentDateTime();
            var chatSession = new ChatSession(validDateTime);

            return chatSession;
        }

        public static Team GetTestData_ValidTeam()
        {
            var team = new Team("Valid Team");

            return team;
        }

        public static Agent GetTestData_ValidAgent()
        {
            var agent = new Agent("Valid Agent", SeniorityLevel.Junior);

            return agent;
        }

        public static Team GetTestData_Team_With_AvailableAgents()
        {
            DateTime agentStartTime = GetTestData_CurrentDateTime().AddMinutes(-1);
            var availableAgent1 = GetTestData_ValidAgent();
            availableAgent1.SetStartTime(agentStartTime);
            var availableAgent2 = GetTestData_ValidAgent();
            availableAgent2.SetStartTime(agentStartTime);
            var team = GetTestData_ValidTeam();
            team.AddAgent(availableAgent1);
            team.AddAgent(availableAgent2);

            return team;
        }

        public static Team GetTestData_Team_With_NoAvailableAgents()
        {
            var availableAgent1 = GetTestData_ValidAgent();
            var availableAgent2 = GetTestData_ValidAgent();
            var team = GetTestData_ValidTeam();
            team.AddAgent(availableAgent1);
            team.AddAgent(availableAgent2);

            return team;
        }

        public static Office GetTestData_Office_With_AvailableHours()
        {
            var office = new Office();
            office.ConfiguteOfficeHours(
                new TimeSpan(9, 0, 0),
                new TimeSpan(18, 0, 0),
                new List<DayOfWeek>() { DayOfWeek.Friday });

            return office;
        }
        public static Office GetTestData_Office_With_AvailableHours_And_HasAvailableAgents()
        {
            var office = GetTestData_Office_With_AvailableHours();

            var team1 = GetTestData_Team_With_AvailableAgents();
            var team2 = GetTestData_Team_With_AvailableAgents();
            office.AddTeam(team1, false);
            office.AddTeam(team2, false);

            return office;
        }
    }
}
