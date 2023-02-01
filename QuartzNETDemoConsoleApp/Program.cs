using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using QuartzNETDemoConsoleApp;
using QuartzNETDemoLibrary;
using QuartzNETDemoLibrary.Models;

//retriving data from db
var optionsBuilder = new DbContextOptionsBuilder<DBContext1>();
//optionsBuilder.UseInMemoryDatabase("myinmemorydb");
optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True;Connect Timeout=30");
var options = optionsBuilder.Options;
var db = new DBContext1(options);
var entites = await db.SchedulerConfiguration.ToArrayAsync();

var scheduler = new SchedulerService();
await scheduler.ConfigureSchedulerAsync(entites);
//await scheduler.ConfigureSchedulerAsync(new SchedulerConfiguration[]
//{
//    new SchedulerConfiguration(Guid.NewGuid(),"0/2 * * * * ?"),
//    new SchedulerConfiguration(Guid.NewGuid(),"0/5 * * * * ?"),
//    new SchedulerConfiguration(Guid.NewGuid(),"1/2 * * * * ?")
//});
await scheduler.StartAsync();

Console.ReadLine();

await scheduler.StopAsync();
