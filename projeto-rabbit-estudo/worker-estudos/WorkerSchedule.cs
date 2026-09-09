namespace worker_estudos;


public class WorkerSchedule : BackgroundService
{
    private IScheduler scheduler;

    public WorkerSchedule(IScheduler scheduler)
    {
        this.scheduler = scheduler;  // Singleton da interface no program.cs do ASP.NET
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
            await this.scheduler.Escalonar(stoppingToken);
    }
}

