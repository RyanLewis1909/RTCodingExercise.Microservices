using IntegrationEvents;
using MassTransit;
using System.Text.Json;

namespace Catalog.API.Consumer
{
    public class CreatePlateAuditConsumer : IConsumer<CreatePlateAudit>
    {
        private readonly IPlateRepository _plateRepository;

        public CreatePlateAuditConsumer(IPlateRepository plateRepository)
        {
            _plateRepository = plateRepository;
        }

        public async Task Consume(ConsumeContext<CreatePlateAudit> context)
        {
            var message = context.Message;
            var newPlateAudit = new PlateAudit
            {
                Table = message.Table,
                TableId = message.TableId,
                Field = message.Field,
                OldValue = message.OldValue,
                NewValue = message.NewValue,
            };

            await _plateRepository.CreatePlateAudit(newPlateAudit);
        }
    }
}
