using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApplication.Models
{
    internal class LoggedWorker
    {
        public int Id { get; set; }
        public string Username { get; set; }


        private static LoggedWorker Instance = null;

        public static LoggedWorker GetInstance() {
            if (Instance is null)
            {
                Instance = new LoggedWorker();
            }
            return Instance;
        }

        public static bool IsLoggedIn()
        {
            return Instance != null;
        }
    }
}
