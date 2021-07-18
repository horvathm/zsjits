using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerServices
{
    public interface IDatabaseService
    {
        Task SaveLockerStates(IEnumerable<LockerState> lockerStates);
    }
}
