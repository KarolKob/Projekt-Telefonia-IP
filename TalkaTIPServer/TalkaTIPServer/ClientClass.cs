using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkaTIPSerwer
{
    class ClientClass
    {
        public string login { get; set; }
        public string addressIP { get; set; }
        public byte[] sessionKey { get; set; }
        public Dictionary<string, string> friendWithStateDict { get; set; }
        public Dictionary<string, string> blockedUsersDict { get; set; }
        public DateTime iAM { get; set; }
    }
}
