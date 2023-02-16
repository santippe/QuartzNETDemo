using Quartz;
using QuartzNETDemoLibrary;
using QuartzNETDemoLibrary.Models;

namespace QuartxNETDemoTest
{
    public class SchedulerTest
    {
        private readonly SchedulerService _schedulerService;

        public SchedulerTest()
        {
            _schedulerService = new SchedulerService();            
        }

        [Fact]
        public async Task SchedulerTestAsync()
        {
            _schedulerService.ConfigureScheduler(new SchedulerConfiguration[]
            {
                new SchedulerConfiguration(Guid.NewGuid(),"0/2 * * * * ?")
            });
            _schedulerService.Start();
            await Task.Delay(10000);
            //await scheduler.StopAsync();
        }
    }
}