//Daily run
//abban az esetben, ha checkTimer.Interval -ja kisebb lesz mint a DomSomething() futási ideje, akkor a checkTimer_Elapsed-be
//többször is belefut. Ezt kezelni érdemes, ha gondot okoz, hogy a DoSomething() többször is lefut. Pl:
// a DomSomething()-ban van egy vizsgálat, hoyg éppen fut-e már (boolean értéket állítunk a Domesomething() elején és végén true->false)
// leheta DomSomething() aszinkron metódus, ha sokáig fut, és a checkTimer_Elapsed-ben NEM await-elünk rá, legalábbis nem a ScheduleChecker() előtt.

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
