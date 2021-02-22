using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PeanutButterCommon.Interfaces
{
    public interface IServiceBus
    {
        Task<bool> PublishAsync(string topicName, string data);
    }
}
