using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entities.Models
{
    public partial class LoginAttempt
    {
        public int LoginAttemptId { get; set; }

        public string Email { get; set; }

        public string SessionId { get; set; }

        public DateTime Date { get; set; }

        public short LoginResultId { get; set; }

        public string WinOsSerialNumber { get; set; }

        public string WinOsUserName { get; set; }
    }
}
