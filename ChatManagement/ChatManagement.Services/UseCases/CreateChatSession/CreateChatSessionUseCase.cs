using ChatManagement.Domain.Entities;
using ChatManagement.Domain.Infra;
using System.Threading.Tasks;

namespace ChatManagement.Services.UseCases.CreateChatSession
{
    public class CreateChatSessionUseCase : ICreateChatSessionUseCase
    {
        private readonly IDateTimeManager _dateTimeManager;
        private readonly IChatSessionQueueRepository _chatQueueService;
        private readonly IChatMonitorRepository _chatMonitorRepository;
        private readonly IOfficeRepository _officeRepository;

        public CreateChatSessionUseCase(
            IDateTimeManager dateTimeManager,
            IChatSessionQueueRepository chatQueueService,
            IChatMonitorRepository chatMonitorRepository,
            IOfficeRepository officeRepository)
        {
            _dateTimeManager = dateTimeManager;
            _chatQueueService = chatQueueService;
            _chatMonitorRepository = chatMonitorRepository;
            _officeRepository = officeRepository;
        }

        public async Task<CreateChatSessionOutput> Execute(CreateChatSessionInput input)
        {
            var dateTimeNow = await _dateTimeManager.GetCurrentTime();
            var office = await _officeRepository.Get();

            if (!office.IsDuringOfficeHours(dateTimeNow))
                throw new System.Exception("Out of office time.");

            if (!office.HasAvailableAgents())
                throw new System.Exception("There is no available agents.");

            if(await _chatQueueService.IsFull(office.GetMaxQueueLength()))
                throw new System.Exception("The chat sessions queue is full.");

            ChatSession session = new ChatSession(dateTimeNow);
            await _chatQueueService.Enqueue(session);

            var chatMonitor = new ChatMonitor(session.Id, dateTimeNow);
            await _chatMonitorRepository.Save(chatMonitor);

            return new CreateChatSessionOutput(session.Id);
        }
    }
}
