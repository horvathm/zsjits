using LockerManagement.Interfaces;

namespace LockerManagement.Models
{
    public class SubscriberModel
    {
        public ILockerEventSubscriber Subscriber { get; set; }

        public bool IsActive { get; set; }
    }
}