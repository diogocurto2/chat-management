using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagement.Domain.Entities
{
    public class Team
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        //private int nextAgentIndex;

        public IEnumerable<Agent> Agents { get; private set; }

        public Team(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Agents = new List<Agent>();
            //nextAgentIndex = 0;
        }

        public void AddAgent(Agent agent)
        {
            if (agent == null)
                throw new Exception("Invalid agent.");

            if (Agents.Any(t => t.Id == agent.Id))
                throw new Exception("This agent exists in team.");

            var agents = Agents.ToList();
            agent.SetTeamId(this.Id);
            agents.Add(agent);

            Agents = agents;
        }

        public int GetCapacity(DateTime dateTime)
        {
            var capacity = 0;

            foreach (var agent in GetAvailableAgents(dateTime))
            {
                capacity += agent.GetCapacity();
            }

            return capacity;
        }

        public Agent GetNextAvailableAgent(DateTime dateTime)
        {
            Agent nextAgent = null;
            var availableAgents = GetAvailableAgents(dateTime);
            foreach(var level in SeniorityLevelExtensions.GetSeniorityLevelsByPriority())
            {
                nextAgent = availableAgents
                    .Where(agent => agent.Seniority == level)
                    .FirstOrDefault();

                if (nextAgent != null) break;
            }

            return nextAgent;
        }

        private IEnumerable<Agent> GetAvailableAgents(DateTime dateTime)
        {
            return Agents.Where(agent => agent.IsAvailable(dateTime)).ToList();
        }

        public bool HasAvailableAgents(DateTime dateTime)
        {
            return GetAvailableAgents(dateTime).Any();
        }
    }

}
