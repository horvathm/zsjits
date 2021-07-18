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


        public LockerManager(ILockerSystemManager lockerSystemManager, ILogger logger)
        {
            lockerEventSubscribers = new List<ILockerEventSubscriber>();

            this.lockerSystemManager = lockerSystemManager;
            this.logger = logger;
        }

        public async Task TurnEcoModeOn()
        {
            await SwitchEcoMode(lockerSystemManager.SwitchEcoOn);
        }

        public async Task TurnEcoModeOff()
        {
            await SwitchEcoMode(lockerSystemManager.SwitchEcoOff);
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

            return lockerEventSubscribers.Remove(subscriber);
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
