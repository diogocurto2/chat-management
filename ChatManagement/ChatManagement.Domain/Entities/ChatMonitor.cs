using System;

namespace ChatManagement.Domain.Entities
{
    public class ChatMonitor
    {
        public Guid ChatSessionId { get; private set; } 
        public DateTime LastPollTime { get; private set; }
        public bool IsActive { get; private set; }

        public ChatMonitor(Guid chatSessionId, DateTime startTimeMonitor)
        {
            this.ChatSessionId= chatSessionId;
            this.LastPollTime = startTimeMonitor;
            this.IsActive = true;
        }

        public void UpdateLastPollTime(DateTime lastPollTime)
        {
            this.LastPollTime = lastPollTime;
        }

        public void Deactivate(DateTime currentDateTime)
        {
            var ts = currentDateTime - this.LastPollTime;

            if (ts.TotalSeconds >= 3)
            {
                IsActive = false;
            }
        }
        public bool CanDeactivate(DateTime currentDateTime)
        {
            var ts = currentDateTime - this.LastPollTime;

            if (ts.TotalSeconds >= 3)
            {
                return true;
            }

            return false;
        }

    }
}
