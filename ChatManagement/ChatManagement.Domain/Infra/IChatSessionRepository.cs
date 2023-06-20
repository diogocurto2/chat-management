using ChatManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatManagement.Domain.Infra
{
    public interface IChatSessionRepository
    {
        Task Save(ChatSession chatSession);

        Task<ChatSession> Get(Guid id);

        Task Remove(Guid id);

        Task Update(ChatSession chatSession);
    }
}