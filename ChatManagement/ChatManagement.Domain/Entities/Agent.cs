using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagement.Domain.Entities
{
    public class Agent
    {
        private const int HOURS_SHIFT = 8;

        private List<ChatSession> _AssignedChatSessions;

        public Agent(string name, SeniorityLevel seniority)
        {
            Id = Guid.NewGuid();
            Name = name;
            Seniority = seniority;
            TeamId = Guid.Empty;
            StartTime = null;
            _AssignedChatSessions = new List<ChatSession>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public SeniorityLevel Seniority { get; private set; }
        public Guid TeamId { get; private set; }
        public DateTime? StartTime { get; private set; }
        public DateTime? EndTime { get { return GetEndTime(); } } 
        public IEnumerable<ChatSession> AssignedChatSessions { get { return _AssignedChatSessions; } }

        public void SetTeamId(Guid teamId)
        {
            TeamId = teamId;
        }

        public bool IsAvailable(DateTime currentDateTime)
        {
            return StartTime.HasValue
                && StartTime.Value <= currentDateTime
                && EndTime >= currentDateTime
                && GetCapacity() > AssignedChatSessions.Count();
        }

        public void SetStartTime(DateTime startTime)
        {
            this.StartTime = startTime;
        }

        public int GetCapacity()
        {
            return Seniority.GetCapacity();
        }
        public DateTime? GetEndTime()
        {
            DateTime? endTime = null;
            if (StartTime.HasValue)
                endTime = this.StartTime.Value.AddHours(HOURS_SHIFT);

            return endTime;
        }

        public void AssignChat(ChatSession chatSession, DateTime dateTime)
        {
            if (!this.IsAvailable(dateTime))
                throw new Exception("Agent is not available.");

            _AssignedChatSessions.Add(chatSession);
        }

    }
}
