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

        public Team OverdueTeam { get; private set; }

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

        public void AddTeam(Team team)
        {
            if (team == null )
                throw new Exception("Invalid team.");

            if (Teams.Any(t => t.Id == team.Id))
                throw new Exception("This team exists in Office.");

            var teams = Teams.ToList();
            teams.Add(team);

            Teams = teams;
        }

        //public void RemoveTeam(Guid teamId)
        //{
        //    if (teamId == Guid.Empty)
        //        throw new Exception("Invalid team.");

        //    var team = Teams.FirstOrDefault(t => t.Id == teamId);
        //    if (team == null)
        //        throw new Exception("Team not found.");

        //    var teams = Teams.ToList();
        //    teams.Remove(team);

        //    Teams = teams;
        //}

        public bool IsDuringOfficeHours(DateTime time)
        {
            return time.TimeOfDay >= StartTime && time.TimeOfDay <= EndTime && WeekDays.Contains(time.DayOfWeek);
        }

        public bool HasAvailableAgents()
        {
            return Teams.Any(t => t.HasAvailableAgents());
        }

        public int GetMaxQueueLength()
        {
            var maxQueueLength = 0;

            foreach(var team in Teams)
            {
                maxQueueLength++;
            }

            return maxQueueLength;
        }

        public Agent GetNextAvailableAgent()
        {
            //int teamIndex = 0;  

            //for (int i = 0; i < teams.Count; i++)
            //{
            //    Team currentTeam = teams[teamIndex];

            //    if (currentTeam.HasAvailableAgents())
            //    {
            //        return currentTeam;  
            //    }

            //    teamIndex = (teamIndex + 1) % teams.Count;  
            //}

            //return overflowTeam;

            return GetNextAvailableTeam().Agents.Where(a => a.IsAvailable).FirstOrDefault();
        }

        private Team GetNextAvailableTeam()
        {
            //int teamIndex = 0;  

            //for (int i = 0; i < teams.Count; i++)
            //{
            //    Team currentTeam = teams[teamIndex];

            //    if (currentTeam.HasAvailableAgents())
            //    {
            //        return currentTeam;  
            //    }

            //    teamIndex = (teamIndex + 1) % teams.Count;  
            //}

            //return overflowTeam;

            return Teams.FirstOrDefault();
        }

    }
}
