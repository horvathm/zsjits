using LockerManagement.Interfaces;
using LockerServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LockerManagement
{
    public class EmailServiceAdapter : IEmailServiceAdapter
    {
        private readonly IEmailService emailService;

        public EmailServiceAdapter(IEmailService databaseService)
        {
            this.emailService = databaseService;
        }
        public async Task SendNotification(IEnumerable<LockerState> lockerStates)
        {
            await emailService.SendEmail(lockerStates);
        }
    }
}
