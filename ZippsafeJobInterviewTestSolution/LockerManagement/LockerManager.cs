using LockerManagement.Interfaces;
using LockerServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockerManagement
{
    public class LockerManager : ILockerManager
    {
        private readonly ILockerSystemManager lockerSystemManager;
        private readonly ILogger logger;
        private List<SubscriberModel> lockerEventSubscribers;


        public LockerManager(ILockerSystemManager lockerSystemManager, ILoggerFactory loggerFactory)
        {
            lockerEventSubscribers = new List<SubscriberModel>();

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

            var isSubscriberAlreadyAdded = lockerEventSubscribers.Exists(s => s.Subscriber == subscriber);

            if (!isSubscriberAlreadyAdded)
            {
                lockerEventSubscribers.Add(new SubscriberModel() { Subscriber = subscriber, IsActive = true });
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

            bool result = lockerEventSubscribers.Remove(
                lockerEventSubscribers.Find(s => s.Subscriber == subscriber));

            if (result)
            {
                logger.LogInformation("A subscriber was removed with type {type}", subscriber.GetType());
            }

            return result;
        }
        public void ActivateSubscriber(ILockerEventSubscriber subscriber)
        {
            ChangeSubscriberStatus(subscriber, true);
        }

        public void DeactivateSubscriber(ILockerEventSubscriber subscriber)
        {
            ChangeSubscriberStatus(subscriber, false);
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
            foreach (var subscriber in lockerEventSubscribers.Where(x => x.IsActive))
            {
                await subscriber.Subscriber.SendNotification(lockerStates);
            }
        }

        private void ChangeSubscriberStatus(ILockerEventSubscriber subscriber, bool isActive)
        {
            var storedSubscriber = lockerEventSubscribers.Find(s => s.Subscriber == subscriber);

            if (storedSubscriber != null)
            {
                storedSubscriber.IsActive = isActive;
            }
            else
            {
                throw new ArgumentException("Given subscriber doesn't exists.");
            }
        }
    }
}
