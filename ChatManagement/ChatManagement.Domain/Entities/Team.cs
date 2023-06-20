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

        public Agent GetNextAvailableAgent()
        {
            //int initialIndex = nextAgentIndex;
            //do
            //{
            //    Agent currentAgent = Agents[nextAgentIndex];

            //    if (currentAgent.IsAvailable)
            //    {
            //        nextAgentIndex = (nextAgentIndex + 1) % Agents.Count;

            //        return currentAgent;
            //    }

            //    nextAgentIndex = (nextAgentIndex + 1) % Agents.Count;

            //} while (nextAgentIndex != initialIndex); 

            //return null;

            return Agents.Where(agent => agent.IsAvailable).FirstOrDefault();
        }

        public bool HasAvailableAgents()
        {
            return Agents.Any(agent => agent.IsAvailable);
        }
    }

}
