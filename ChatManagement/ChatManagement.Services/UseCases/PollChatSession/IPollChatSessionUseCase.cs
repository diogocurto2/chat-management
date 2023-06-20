using System.Threading.Tasks;

namespace ChatManagement.Services.UseCases.PollChatSession
{
    public interface IPollChatSessionUseCase
    {
        Task<PollChatSessionOutput> Execute(PollChatSessionInput input);
    }
}