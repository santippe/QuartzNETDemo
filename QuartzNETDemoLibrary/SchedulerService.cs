using Quartz;
using Quartz.Impl;
using QuartzNETDemoLibrary.Models;

namespace QuartzNETDemoLibrary
{
    public class SchedulerService
    {
        private IScheduler _scheduler;

        public SchedulerService()
        {
            // Create a new scheduler
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
        }

        public async Task ConfigureSchedulerAsync(SchedulerConfiguration[] configuration)
        {
            //clear all the scheduled jobs
            await _scheduler.Clear();

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
                await _scheduler.ScheduleJob(job, trigger);
            }
        }

        public async Task StartAsync()
        {
            // Start the scheduler
            await _scheduler.Start();
        }

        public async Task StopAsync()
        {
            // Stop the scheduler
            await _scheduler.Shutdown();
        }

        private class MyJob : IJob
        {
            private Guid? _id;

            public async Task Execute(IJobExecutionContext context)
            {
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                _id = _id ?? (Guid?)dataMap.Get("id");
                //we can start a service
                Console.WriteLine($"Job {_id} running at " + DateTime.Now);
            }
        }
    }
}