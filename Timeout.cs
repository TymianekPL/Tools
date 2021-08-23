using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Timeout
    {
        private readonly Action action;
        public Timeout(Action action)
        {
            this.action = action;
        }


        public void Start(TimeoutTime time)
        {
            int minutes = time.Hours * 60;
            int seconds = (time.Minutes + minutes) * 60;
            int ms = (time.Seconds + seconds) * 1000;
            ms += time.Miliseconds;
            Task.Run(() =>
            {
                WinAPINatives.SleepA(ms);
                action.Invoke();
            });
        }
    }

    public class TimeoutTime
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Miliseconds { get; set; }
    }
}
