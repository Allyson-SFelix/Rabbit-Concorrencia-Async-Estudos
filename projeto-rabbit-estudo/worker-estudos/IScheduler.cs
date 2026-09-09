
namespace worker_estudos;
using System;

public interface IScheduler
{
    public Task Escalonar(CancellationToken stoppingToken);

    public void AddJob(Job job);
}
