using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Infrastructure.Configurations.Notifications
{
    public class EmailOptions
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
        public string FromEmail { get; set; }
        public string ApiKey { get; set; } // SendGrid için
    }
}
