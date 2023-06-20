using ChatManagement.Domain.Entities;
using System.Threading.Tasks;

namespace ChatManagement.Domain.Infra
{
    public interface IChatSessionQueueRepository
    {
        Task<ChatSession> Dequeue();
        Task Enqueue(ChatSession session);
        Task<bool> IsFull(int maxQueueLength);
    }
}