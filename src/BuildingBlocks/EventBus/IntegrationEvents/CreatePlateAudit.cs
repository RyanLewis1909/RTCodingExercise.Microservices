namespace IntegrationEvents
{
    public class CreatePlateAudit : IntegrationEvent
    {
        public string Table { get; set; }

        public Guid TableId { get; set; }

        public string Field { get; set; }

        public string? OldValue { get; set; }

        public string NewValue { get; set; }
    }
}