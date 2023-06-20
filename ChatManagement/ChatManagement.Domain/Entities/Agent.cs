using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagement.Domain.Entities
{
    public class Agent
    {
        public Agent(string name, SeniorityLevel seniority)
        {
            Id = Guid.NewGuid();
            Name = name;
            Seniority = seniority;
            TeamId = Guid.Empty;
            IsAvailable = true;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public SeniorityLevel Seniority { get; set; }
        public Guid TeamId { get; set; }
        public bool IsAvailable { get; set; }

        public void SetTeamId(Guid teamId)
        {
            TeamId = teamId;
        }

        public void SetIsNotAvailable()
        {
            this.IsAvailable = false;
        }

        public void SetIsAvailable()
        {
            this.IsAvailable = true;
        }

    }
}
