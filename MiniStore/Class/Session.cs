using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore
{
    public static class Session
    {
        public static string Email { get; set; }
        public static string OTP { get; set; }
        public static DateTime Expiry { get; set; }
        public static string Marole{get; set;}
    }
}
