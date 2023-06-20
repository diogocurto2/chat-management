using ChatManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagement.UnitTest
{
    public static class EntityGenerator
    {

        public static Agent GetTestData_NotAvailableAgent()
        {
            var name = Guid.NewGuid().ToString();
            var teamId = Guid.NewGuid();
            var seniority = SeniorityLevel.Junior;
            var agent = new Agent(name, seniority);
            agent.SetIsNotAvailable();

            return agent;
        }

        public static Agent GetTestData_AvailableAgent()
        {
            var name = Guid.NewGuid().ToString();
            var teamId = Guid.NewGuid();
            var seniority = SeniorityLevel.Junior;
            var agent = new Agent(name, seniority);

            return agent;
        }

        public static ChatMonitor GetTestData_ValidChatMonitor()
        {
            var chatSessionId = Guid.NewGuid();
            var validDateTime = DateTime.Now; ;
            var chatMonitor = new ChatMonitor(chatSessionId, validDateTime);

            return chatMonitor;
        }

        public static ChatSession GetTestData_ValidChatSession()
        {
            var validDateTime = DateTime.Now; ;
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
            var availableAgent1 = GetTestData_ValidAgent();
            var availableAgent2 = GetTestData_ValidAgent();
            var team = GetTestData_ValidTeam();
            team.AddAgent(availableAgent1);
            team.AddAgent(availableAgent2);

            return team;
        }

        public static Team GetTestData_Team_With_NoAvailableAgents()
        {
            var availableAgent1 = GetTestData_ValidAgent();
            availableAgent1.SetIsNotAvailable();
            var availableAgent2 = GetTestData_ValidAgent();
            availableAgent2.SetIsNotAvailable();
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
            office.AddTeam(team1);
            office.AddTeam(team2);

            return office;
        }
    }
}
