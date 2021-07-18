using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerServices.Interfaces
{
    public interface IDatabaseService
    {
        Task SaveLockerStates(IEnumerable<LockerState> lockerStates);
    }
}
