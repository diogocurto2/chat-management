
using System;

namespace ChatManagement.Services.UseCases.PollChatSession
{
    public class PollChatSessionInput
    {
        public Guid ChatSessionId { get; private set; }

        public PollChatSessionInput(Guid chatSessionId)
        {
            this.ChatSessionId = chatSessionId;
        }
    }
}
