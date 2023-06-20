using ChatManagement.Domain;
using ChatManagement.Domain.Infra;
using System.Threading.Tasks;

namespace ChatManagement.Services.UseCases.PollChatSession
{
    public class PollChatSessionUseCase : IPollChatSessionUseCase
    {
        private readonly IDateTimeManager _dateTimeManager;
        private readonly IChatMonitorRepository _chatMonitorRepository;

        public PollChatSessionUseCase(
            IDateTimeManager dateTimeManager,
            IChatMonitorRepository chatMonitorRepository)
        {
            _dateTimeManager = dateTimeManager;
            _chatMonitorRepository = chatMonitorRepository;
        }

        public async Task<PollChatSessionOutput> Execute(PollChatSessionInput input)
        {
            var chatMonitor = await _chatMonitorRepository.Get(input.ChatSessionId);
            if (chatMonitor == null)
                throw new System.Exception("ChatMonitorId is inexistent");
            
            var currentDateTime = await _dateTimeManager.GetCurrentTime();
            chatMonitor.UpdateLastPollTime(currentDateTime);

            await _chatMonitorRepository.Update(chatMonitor);

            return new PollChatSessionOutput(input.ChatSessionId);
        }
    }
}
