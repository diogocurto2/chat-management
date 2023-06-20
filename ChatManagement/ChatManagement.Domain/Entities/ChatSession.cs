using System;

namespace ChatManagement.Domain.Entities
{
    public class ChatSession
    {
        public Guid Id { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }

        public Guid AgentId { get; private set; }
        public Agent Agent { get; private set; }

        public ChatSession(DateTime startDateTime)
        {
            Id = Guid.NewGuid();
            StartTime = startDateTime;
            EndTime = null;
        }

        public void Assign(Agent agent)
        {
            if (agent == null)
                throw new Exception("Invalid Agent.");

            AgentId = agent.Id;
            Agent = agent;
        }


    }

}
