//Daily run

class MyClass{
static Timer checkerTimer;

static MyClass()
        {
            checkerTimer = new Timer
            {
                AutoReset = true
            };
            checkerTimer.Elapsed += checkerTimer_Elapsed;
        }
private static void checkerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DoSomething();
            ScheduleChecker();
        }
private static void ScheduleChecker()
        {
            if (DateTime.TryParse(System.Configuration.ConfigurationManager.AppSettings["CheckingTime"], out DateTime scheduledtime))
            {
                if (DateTime.Now > scheduledtime)
                {
                    scheduledtime = scheduledtime.AddDays(1);
                }
                TimeSpan timeSpan = scheduledtime.Subtract(DateTime.Now);
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);
                checkerTimer.Interval = dueTime;
                checkerTimer.Start();
            }
            else
            {
                checkerTimer.Dispose();
            }
        }
}

public static void Start()
{
  ScheduleChecker();
}

public static void Stop()
{
  checkerTimer.Dispose();
}
