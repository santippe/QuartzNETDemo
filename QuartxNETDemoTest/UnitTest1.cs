using QuartzNETDemoLibrary;
using QuartzNETDemoLibrary.Models;

namespace QuartxNETDemoTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task SchedulerTestAsync()
        {
            var scheduler = new SchedulerService();
            await scheduler.ConfigureSchedulerAsync(new SchedulerConfiguration[]
            {
                new SchedulerConfiguration(Guid.NewGuid(),"0/2 * * * * ?")
            });
            await scheduler.StartAsync();
            await Task.Delay(10000);
            //await scheduler.StopAsync();
        }
    }
}