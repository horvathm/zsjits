using LockerManagement.Interfaces;

namespace LockerManagement
{
    public class SubscriberModel
    {
        public ILockerEventSubscriber Subscriber { get; set; }

        public bool IsActive { get; set; }
    }
}