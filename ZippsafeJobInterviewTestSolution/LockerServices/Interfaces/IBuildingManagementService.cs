using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerServices.Interfaces
{
    public interface IBuildingManagementService
    {
        Task ManagerLockerStateChanges(IEnumerable<LockerState> lockerState);
    }
}
