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
            ms += time.Milliseconds;
            _ = Task.Run(() =>
            {
                Thread.Sleep(ms);
                action.Invoke();
            });
        }
    }

    public class TimeoutTime
    {
        public int Milliseconds { get; set; }
        public int Seconds { get; set; }
        public int Minutes { get; set; }
        public int Hours { get; set; }
        public int Days { get; set; }

        public static TimeoutTime operator +(TimeoutTime time, TimeSpan span)
        {
            time.Hours += span.Hours;
            time.Minutes += span.Minutes;
            time.Seconds += span.Seconds;
            time.Milliseconds += span.Milliseconds;

            return time;
        }

        public static TimeSpan operator +(TimeSpan span, TimeoutTime time)
        {
            TimeSpan _span = new(days: time.Days, hours: time.Hours, minutes: time.Minutes, seconds: time.Seconds, milliseconds: time.Milliseconds);
            _ = span.Add(_span);
            return span;
        }

        public override string ToString()
        {
            return $"{Days}/{Hours}:{Minutes}:{Seconds}.{Milliseconds}";
        }
    }
}
