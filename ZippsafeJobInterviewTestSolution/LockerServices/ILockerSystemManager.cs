using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerServices
{
    public interface ILockerSystemManager
    {
        Task<IEnumerable<LockerState>> SwitchEcoOn();

        Task<IEnumerable<LockerState>> SwitchEcoOff();
    }
}
