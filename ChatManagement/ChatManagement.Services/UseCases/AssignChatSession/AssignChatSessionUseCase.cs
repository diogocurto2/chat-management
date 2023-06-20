using ChatManagement.Domain.Infra;
using System;
using System.Threading.Tasks;

namespace ChatManagement.Services.UseCases.AssignChatSession
{
    public class AssignChatSessionUseCase : IAssignChatSessionUseCase
    {
        private readonly IDateTimeManager _dateTimeManager;
        private readonly IOfficeRepository _officeRepository;
        private readonly IChatMonitorRepository _chatMonitorRepository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatSessionQueueRepository _chatSessionQueueRepository;

        public AssignChatSessionUseCase(
            IDateTimeManager dateTimeManager,
            IOfficeRepository officeRepository,
            IChatMonitorRepository chatMonitorRepository,
            IChatSessionRepository chatSessionRepository,
            IChatSessionQueueRepository chatSessionQueueRepository)
        {
            _dateTimeManager = dateTimeManager;
            _officeRepository = officeRepository;
            _chatMonitorRepository = chatMonitorRepository;
            _chatSessionRepository = chatSessionRepository;
            _chatSessionQueueRepository = chatSessionQueueRepository;
        }

        public object ChatAssignService { get; private set; }

        public async Task Execute()
        {
            var dateTimeNow = await _dateTimeManager.GetCurrentTime();
            var office = await _officeRepository.Get();

            if (!office.HasAvailableAgents(dateTimeNow))
            {
                throw new Exception("Has no Available Agents.");
            }

            var chatSession = await _chatSessionQueueRepository.Dequeue();
            var monitor = await _chatMonitorRepository.Get(chatSession.Id);
            if (!monitor.IsActive)
            {
                throw new Exception("ChatSession is not activate.");
            }
                        
            var agent = office.GetNextAvailableAgent(dateTimeNow);

            chatSession.Assign(agent);
            await _chatSessionRepository.Save(chatSession);
            await _chatMonitorRepository.Remove(chatSession.Id);
        }
    }
}
