using ChatManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatManagement.Domain.Infra
{
    public interface IChatMonitorRepository
    {
        Task Save(ChatMonitor chatMonitor);

        Task<ChatMonitor> Get(Guid chatSessionId);

        Task<IEnumerable<ChatMonitor>> GetNotReceivedThreePollRequests(DateTime currentDateTime);

        Task Remove(Guid chatSessionId);

        Task Update(ChatMonitor chatMonitor);
    }
}