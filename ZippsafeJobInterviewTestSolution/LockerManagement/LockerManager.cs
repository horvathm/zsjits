using LockerManagement.Interfaces;
using LockerServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerManagement
{
    public class LockerManager : ILockerManager
    {
        private readonly ILockerSystemManager lockerSystemManager;
        private readonly ILogger logger;
        private List<ILockerEventSubscriber> lockerEventSubscribers;


        public LockerManager(ILockerSystemManager lockerSystemManager, ILoggerFactory loggerFactory)
        {
            lockerEventSubscribers = new List<ILockerEventSubscriber>();

            this.lockerSystemManager = lockerSystemManager;
            this.logger = loggerFactory.CreateLogger<LockerManager>();
        }

        public async Task TurnEcoModeOn()
        {
            await SwitchEcoMode(lockerSystemManager.SwitchEcoOn);
            logger.LogInformation("Eco mode is turned [on] for the lockers");
        }

        public async Task TurnEcoModeOff()
        {
            await SwitchEcoMode(lockerSystemManager.SwitchEcoOff);
            logger.LogInformation("Eco mode is turned [off] for the lockers");
        }

        public bool AttachSubscriber(ILockerEventSubscriber subscriber)
        {
            if (subscriber == null)
            {
                logger.LogError("Parameter of AttachSubscriber method cannot be null", nameof(AttachSubscriber));
                throw new ArgumentNullException("Argument cannot be null");
            }

            var isSubscriberAlreadyAdded = lockerEventSubscribers.Exists(s => s == subscriber);

            if (!isSubscriberAlreadyAdded)
            {
                lockerEventSubscribers.Add(subscriber);
                logger.LogInformation("New subscriber added to the list with type: {type}", subscriber.GetType());
                return true;
            }

            return false;
        }

        public bool DetachSubscriber(ILockerEventSubscriber subscriber)
        {
            if (subscriber == null)
            {
                logger.LogError("Parameter of AttachSubscriber method cannot be null", nameof(AttachSubscriber));
                throw new ArgumentNullException("Argument cannot be null");
            }

            bool result = lockerEventSubscribers.Remove(subscriber);

            if (result)
            {
                logger.LogInformation("A subscriber was removed with type {type}", subscriber.GetType());
            }

            return result;
        }

        private async Task SwitchEcoMode(Func<Task<IEnumerable<LockerState>>> action)
        {
            var lockerStates = await action();

            if (lockerEventSubscribers != null)
            {
                await NotifySubscribers(lockerStates);
            }
        }

        private async Task NotifySubscribers(IEnumerable<LockerState> lockerStates)
        {
            foreach (var subscriber in lockerEventSubscribers)
            {
                await subscriber.SendNotification(lockerStates);
            }
        }
    }
}
