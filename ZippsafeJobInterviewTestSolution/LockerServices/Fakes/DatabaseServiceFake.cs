using LockerServices.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockerServices.Fakes
{
    public class DatabaseServiceFake : IDatabaseService
    {
        private List<LockerState> lockerStates;
        private readonly ILogger logger;

        public DatabaseServiceFake(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<DatabaseServiceFake>();
        }

        public Task SaveLockerStates(IEnumerable<LockerState> lockerStates)
        {
            this.lockerStates = lockerStates.ToList();
            logger.LogInformation("Notification received for the database service");
            return Task.CompletedTask;
        }
    }
}
