using LockerServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerManagement.Interfaces
{
    public interface ILockerEventSubscriber
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockerStates"></param>
        /// <returns></returns>
        Task SendNotification(IEnumerable<LockerState> lockerStates);
    }
}