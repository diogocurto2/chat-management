using System.Threading.Tasks;

namespace ChatManagement.Services.UseCases.InactivateChatSessions
{
    public interface IInactivateChatSessionsUseCase
    {
        Task<InactivateChatSessionsOutPut> Execute(InactivateChatSessionsInput input);
    }
}