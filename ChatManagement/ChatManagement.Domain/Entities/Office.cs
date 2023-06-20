using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagement.Domain.Entities
{
    public class Office
    {
        public Guid Id { get; private set; }

        public TimeSpan StartTime { get; private set;}

        public TimeSpan EndTime { get; private set; }

        public IEnumerable<DayOfWeek> WeekDays { get; private set; }

        public IEnumerable<Team> Teams { get; private set; }

        public Team OverflowTeam { get; private set; }

        public Office()
        {
            Id = Guid.NewGuid();
            Teams = new List<Team>();
        }

        public void ConfiguteOfficeHours(
            TimeSpan startTime, 
            TimeSpan endTime,
            IEnumerable<DayOfWeek> weekDays)
        {

            StartTime = startTime;
            EndTime = endTime;
            WeekDays = weekDays;
        }

        public void AddTeam(Team team, bool isOverflowTeam = false)
        {
            if (team == null )
                throw new Exception("Invalid team.");

            if (Teams.Any(t => t.Id == team.Id))
                throw new Exception("This team exists in Office.");

            var teams = Teams.ToList();
            teams.Add(team);

            if (isOverflowTeam)
                OverflowTeam = team;

            Teams = teams;
        }

        public bool IsDuringOfficeHours(DateTime dateTime)
        {
            return dateTime.TimeOfDay >= StartTime && dateTime.TimeOfDay <= EndTime && WeekDays.Contains(dateTime.DayOfWeek);
        }

        public bool HasAvailableAgents(DateTime dateTime)
        {
            return Teams.Any(t => t.HasAvailableAgents(dateTime));
        }

        public int GetCapacity(DateTime dateTime)
        {
            var capacity = 0;

            foreach(var team in Teams)
            {
                capacity+= team.GetCapacity(dateTime);
            }

            return capacity;
        }

        public int GetMaximumQueueLength(DateTime dateTime)
        {
            var maximumQueueLength = 0;

            maximumQueueLength = (int)Math.Round(GetCapacity(dateTime) * 1.5);

            return maximumQueueLength;
        }

        public Agent GetNextAvailableAgent(DateTime dateTime)
        {
            Agent nextAvailableAgent = null;

            var nextAvailableTeam = GetNextAvailableTeam(dateTime);
            if(nextAvailableTeam != null)
            {
                nextAvailableAgent = nextAvailableTeam.GetNextAvailableAgent(dateTime);
            }

            return nextAvailableAgent;
        }

        private Team GetNextAvailableTeam(DateTime dateTime)
        {
            Team nextAvailableTeam = null;

            nextAvailableTeam = Teams
                .Where(t => t.HasAvailableAgents(dateTime))
                .FirstOrDefault();

            if (nextAvailableTeam == null)
                nextAvailableTeam = OverflowTeam;

            return nextAvailableTeam;
        }

    }
}
