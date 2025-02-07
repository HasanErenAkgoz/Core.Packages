using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Application.Common.DTOs.Notifications
{
    public class SmsDto
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}
