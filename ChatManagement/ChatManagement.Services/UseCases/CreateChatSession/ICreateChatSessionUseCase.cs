using System.Threading.Tasks;

namespace ChatManagement.Services.UseCases.CreateChatSession
{
    public interface ICreateChatSessionUseCase
    {
        Task<CreateChatSessionOutput> Execute(CreateChatSessionInput input);
    }
}