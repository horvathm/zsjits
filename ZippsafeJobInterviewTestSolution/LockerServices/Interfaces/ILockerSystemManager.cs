using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerServices.Interfaces
{
    public interface ILockerSystemManager
    {
        Task<IEnumerable<LockerState>> SwitchEcoOn();

        Task<IEnumerable<LockerState>> SwitchEcoOff();
    }
}
