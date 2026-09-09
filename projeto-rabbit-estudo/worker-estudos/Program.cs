using worker_estudos;

var builder = Host.CreateApplicationBuilder(args);

List<Job> listaJob = Job.CriandoSituacao();

Scheduler schedule = new Scheduler();
foreach(Job job in listaJob)
{
    schedule.AddJob(job);
}

// apenas uma instancia e já fou a instancia (mas essa vem da api normalmente, pois ela iniciará)
builder.Services.AddSingleton<IScheduler>(schedule);
builder.Services.AddHostedService<WorkerSchedule>();



var host = builder.Build();
host.Run();
