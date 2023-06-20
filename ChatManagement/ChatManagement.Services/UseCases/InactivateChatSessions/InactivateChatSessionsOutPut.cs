using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagement.Services.UseCases.InactivateChatSessions
{
    public class InactivateChatSessionsOutPut
    {

        public IEnumerable<Guid> ChatSessionsId { get; private set; }

        public InactivateChatSessionsOutPut(IEnumerable<Guid> chatSessionsId)
        {
            ChatSessionsId = chatSessionsId;
        }
    }
}
