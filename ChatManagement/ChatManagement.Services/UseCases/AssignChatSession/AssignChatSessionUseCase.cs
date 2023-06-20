using ChatManagement.Domain.Infra;
using System;
using System.Threading.Tasks;

namespace ChatManagement.Services.UseCases.AssignChatSession
{
    public class AssignChatSessionUseCase : IAssignChatSessionUseCase
    {

        private readonly IOfficeRepository _officeRepository;
        private readonly IChatMonitorRepository _chatMonitorRepository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatSessionQueueRepository _chatSessionQueueRepository;

        public AssignChatSessionUseCase(
            IOfficeRepository officeRepository,
            IChatMonitorRepository chatMonitorRepository,
            IChatSessionRepository chatSessionRepository,
            IChatSessionQueueRepository chatSessionQueueRepository)
        {
            _officeRepository = officeRepository;
            _chatMonitorRepository = chatMonitorRepository;
            _chatSessionRepository = chatSessionRepository;
            _chatSessionQueueRepository = chatSessionQueueRepository;
        }

        public object ChatAssignService { get; private set; }

        public async Task Execute()
        {
            var chatSession = await _chatSessionQueueRepository.Dequeue();
            var office = await _officeRepository.Get();
            var agent = office.GetNextAvailableAgent();
            var monitor = await _chatMonitorRepository.Get(chatSession.Id);

            if (!monitor.IsActive)
            {
                throw new Exception("ChatSession is not activate.");
            }

            chatSession.Assign(agent);
            await _chatSessionRepository.Save(chatSession);
            await _chatMonitorRepository.Remove(chatSession.Id);
        }
    }
}
