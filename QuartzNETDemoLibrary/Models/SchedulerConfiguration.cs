namespace QuartzNETDemoLibrary.Models
{
    public class SchedulerConfiguration
    {
        public Guid? ServiceId { get; set; }
        public string ServiceTrigger { get; set; }

        public SchedulerConfiguration(Guid? serviceId, string serviceTrigger)
        {
            ServiceId = serviceId;
            ServiceTrigger = serviceTrigger;
        }
    }
}
