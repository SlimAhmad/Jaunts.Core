using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaunts.Core.Models.AppSettings
{
    public class MailTrap
    {
        public string TestUrl { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string InboxId { get; set; }
    }
}
