using Quartz;
using Quartz.Impl;
using QuartzNETDemoLibrary.Models;

namespace QuartzNETDemoLibrary
{
    public class SchedulerService
    {
        private IScheduler _scheduler;
        private readonly object _lock = new object();


        public SchedulerService()
        {
            // Create a new scheduler
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
        }

        public void ConfigureScheduler(SchedulerConfiguration[] configuration)
        {
            //clear all the scheduled jobs
            lock (_lock)
            {
                _scheduler.Clear().GetAwaiter().GetResult();

                foreach (var configurationItem in configuration)
                {
                    var job = JobBuilder.Create<MyJob>()
                        //.WithIdentity("myJob", "group1")
                        .UsingJobData("id", configurationItem.ServiceId.Value)
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        //.WithIdentity("myTrigger", "group1")
                        //.WithCronSchedule("0 0 8 * * ?")  //8:00 AM everyday
                        .WithCronSchedule(configurationItem.ServiceTrigger)  //8:00 AM everyday                    
                        .Build();

                    // Schedule the job
                    _scheduler.ScheduleJob(job, trigger).GetAwaiter().GetResult(); ;
                }
            }
        }

        public async Task AddSchedulerTaskAsync(SchedulerConfiguration[] configuration)
        {
            foreach (var configurationItem in configuration)
            {
                var job = JobBuilder.Create<MyJob>()
                    .WithIdentity(new JobKey(configurationItem.ServiceId?.ToString()))
                    .UsingJobData("id", configurationItem.ServiceId.Value)
                    .Build();

                var trigger = TriggerBuilder.Create()
                    //.WithIdentity("myTrigger", "group1")
                    //.WithCronSchedule("0 0 8 * * ?")  //8:00 AM everyday
                    .WithCronSchedule(configurationItem.ServiceTrigger)  //8:00 AM everyday                    
                    .Build();

                // Schedule the job
                await _scheduler.ScheduleJob(job, trigger);
            }
        }

        public async Task RemoveSchedulerTaskAsync(SchedulerConfiguration[] configuration)
        {
            foreach (var configurationItem in configuration)
            {
                await _scheduler.DeleteJob(new JobKey(configurationItem.ServiceId?.ToString()));
            }
        }

        public void Start()
        {
            lock (_lock)
            {
                // Start the scheduler
                _scheduler.Start().GetAwaiter().GetResult();
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                // Start the scheduler
                _scheduler.Shutdown().GetAwaiter().GetResult();
            }
        }

        private class MyJob : IJob
        {
            private Guid? _id;

            public async Task Execute(IJobExecutionContext context)
            {
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                _id = _id ?? (Guid?)dataMap.Get("id");
                //we can start a service
                Console.WriteLine($"Job {_id} starting at {DateTime.Now.ToString("HH:mm:ss.fff")}");
                await Task.Delay(10000);
                Console.WriteLine($"Job {_id} finished at {DateTime.Now.ToString("HH:mm:ss.fff")}");
            }
        }
    }
}