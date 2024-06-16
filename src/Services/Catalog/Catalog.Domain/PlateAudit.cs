namespace Catalog.Domain
{
    public class PlateAudit
    {
        public int Id { get; set; }

        public string Table { get; set; }

        public Guid TableId { get; set; }

        public string Field { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}