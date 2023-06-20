using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagement.Domain.Entities
{
    public enum SeniorityLevel
    {
        Junior,
        MidLevel,
        Senior,
        TeamLead
    }

    public static class SeniorityLevelExtensions
    {

        private const int MAXIMUM_CONCURRENCY = 10;

        private class SeniorityLevelConfig
        {
            public SeniorityLevel Level { get; set; }
            public double Capacity { get; set; }
            public int Priority { get; set; }
        }

        private static List<SeniorityLevelConfig> GetConfigRepository()
        {
            var dic = new List<SeniorityLevelConfig>()
            {
                { new SeniorityLevelConfig() { Level = SeniorityLevel.Junior, Capacity =  0.4, Priority = 1 } },
                { new SeniorityLevelConfig() { Level = SeniorityLevel.MidLevel, Capacity =   0.6, Priority = 2 } },
                { new SeniorityLevelConfig() { Level = SeniorityLevel.Senior, Capacity =  0.8, Priority = 3 } },
                { new SeniorityLevelConfig() { Level = SeniorityLevel.TeamLead, Capacity =  0.5, Priority = 4 } },
            };

            return dic;
        }

        public static int GetCapacity(this SeniorityLevel level)
        {
            var config = GetConfigRepository().Where( c=> c.Level == level).FirstOrDefault();

            var multiplier = config.Capacity;
            var capacity = (int)Math.Round(MAXIMUM_CONCURRENCY * multiplier);

            return capacity;
        }

        public static List<SeniorityLevel> GetSeniorityLevelsByPriority()
        {
            var list = GetConfigRepository()
                .OrderBy(c => c.Priority)
                .Select(c => c.Level)
                .ToList();

            return list;
        }
    }
}
