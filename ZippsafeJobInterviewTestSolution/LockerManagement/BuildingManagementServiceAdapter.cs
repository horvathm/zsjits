using LockerManagement.Interfaces;
using LockerServices;
using LockerServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerManagement
{
    public class BuildingManagementServiceAdapter : IBuildingManagementServiceAdapter
    {
        private readonly IBuildingManagementService buildingManagementService;

        public BuildingManagementServiceAdapter(IBuildingManagementService buildingManagementService)
        {
            this.buildingManagementService = buildingManagementService;
        }
        public async Task SendNotification(IEnumerable<LockerState> lockerStates)
        {
            await buildingManagementService.ManagerLockerStateChanges(lockerStates);
        }
    }
}
