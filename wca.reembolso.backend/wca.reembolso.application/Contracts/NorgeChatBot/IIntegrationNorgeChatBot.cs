using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wca.reembolso.application.Contracts.NorgeChatBot
{
    public interface IIntegrationNorgeChatBot
    {
        Task<Response> Send(int number, string message);
    }
}
