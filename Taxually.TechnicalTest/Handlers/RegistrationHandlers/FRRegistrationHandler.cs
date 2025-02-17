using System.Text;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers.RegistrationHandlers;

public class FRRegistrationHandler(ITaxuallyQueueClient taxuallyQueueClient, IConfiguration configuration) : RegistrationHandlerBase
{
    private readonly ITaxuallyQueueClient taxuallyQueueClient = taxuallyQueueClient;

    public async override Task CreateRegistrationAsync(VatRegistrationRequest request)
    {
        if (string.IsNullOrEmpty(configuration["Constants:CSVQueueName"]))
            throw new KeyNotFoundException();

        // France requires an excel spreadsheet to be uploaded to register for a VAT number
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("CompanyName,CompanyId");
        csvBuilder.AppendLine($"{request.CompanyName}{request.CompanyId}");
        var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
        // Queue file to be processed
        await taxuallyQueueClient.EnqueueAsync(configuration["Constants:CSVQueueName"], csv);
    }
}
