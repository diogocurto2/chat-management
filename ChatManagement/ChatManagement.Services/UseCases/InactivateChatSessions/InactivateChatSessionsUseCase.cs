using ChatManagement.Domain.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagement.Services.UseCases.InactivateChatSessions
{
    public class InactivateChatSessionsUseCase : IInactivateChatSessionsUseCase
    {

        private readonly IDateTimeManager _dateTimeManager;
        private readonly IChatMonitorRepository _chatMonitorRepository;

        public InactivateChatSessionsUseCase(
            IDateTimeManager dateTimeManager, 
            IChatMonitorRepository chatMonitorRepository)
        {
            _dateTimeManager = dateTimeManager;
            _chatMonitorRepository = chatMonitorRepository;
        }

        public async Task<InactivateChatSessionsOutPut> Execute(InactivateChatSessionsInput input)
        {
            var chatSessionIdList = new List<Guid>();
            var currentDateTime = await _dateTimeManager.GetCurrentTime();
            var chatMonitorList = await _chatMonitorRepository.GetNotReceivedThreePollRequests(currentDateTime);

            foreach (var chatMonitor in chatMonitorList)
            {
                chatMonitor.Deactivate(currentDateTime);
                if(!chatMonitor.IsActive)
                {
                    await _chatMonitorRepository.Update(chatMonitor);
                    chatSessionIdList.Add(chatMonitor.ChatSessionId);
                }
            }

            return new InactivateChatSessionsOutPut(chatSessionIdList);
        }
    }
}
