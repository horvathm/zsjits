using System.Threading.Tasks;

namespace LockerManagement.Interfaces
{
    public interface ILockerManager
    {
        /// <summary>
        /// Turns eco mode on for all the lockers.
        /// </summary>
        /// <returns></returns>
        Task TurnEcoModeOn();
        
        /// <summary>
        /// Turns eco mode off for all the lockers.
        /// </summary>
        /// <returns></returns>
        Task TurnEcoModeOff();

        /// <summary>
        /// Attaches a handlers to the locker state changed event.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns>Whether the addition was successful or not</returns>
        bool AttachSubscriber(ILockerEventSubscriber subscriber);

        /// <summary>
        /// Detaches a handler from the locker state changed event.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns>Whether the deletion was successful or not</returns>
        bool DetachSubscriber(ILockerEventSubscriber subscriber);

        /// <summary>
        /// Activates a subscriber to receive notification again.
        /// </summary>
        /// <param name="subscriber">A subscriber to be activated</param>
        void ActivateSubscriber(ILockerEventSubscriber subscriber);

        /// <summary>
        /// Deactivate a subscriber to stop receiving notifications.
        /// </summary>
        /// <param name="subscriber">A subscriber to be deactivated</param>
        void DeactivateSubscriber(ILockerEventSubscriber subscriber);
    }
}
