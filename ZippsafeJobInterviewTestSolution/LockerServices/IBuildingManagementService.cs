using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerServices
{
    public interface IBuildingManagementService
    {
        Task ManagerLockerStateChanges(IEnumerable<LockerState> lockerState);
    }
}
