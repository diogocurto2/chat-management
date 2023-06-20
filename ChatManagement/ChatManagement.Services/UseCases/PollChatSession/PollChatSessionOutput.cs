using System;

namespace ChatManagement.Services.UseCases.PollChatSession
{
    public class PollChatSessionOutput
    {

        public Guid ChatSessionId { get; private set; }

        public PollChatSessionOutput(Guid chatSessionId)
        {
            this.ChatSessionId = chatSessionId;
        }
    }
}
