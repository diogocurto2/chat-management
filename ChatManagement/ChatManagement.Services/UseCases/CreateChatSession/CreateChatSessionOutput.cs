using ChatManagement.Domain;
using System;

namespace ChatManagement.Services.UseCases.CreateChatSession
{
    public class CreateChatSessionOutput
    {

        public Guid ChatSessionId { get; private set; }

        public CreateChatSessionOutput(Guid chatSessionId)
        {
            this.ChatSessionId = chatSessionId;
        }
    }
}
