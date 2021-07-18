using LockerManagement.Interfaces;
using LockerServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerManagement
{
    public class DatabaseServiceAdapter : IDatabaseServiceAdapter
    {
        private readonly IDatabaseService databaseService;

        public DatabaseServiceAdapter(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }
        public async Task SendNotification(IEnumerable<LockerState> lockerStates)
        {
            await databaseService.SaveLockerStates(lockerStates);
        }
    }
}
