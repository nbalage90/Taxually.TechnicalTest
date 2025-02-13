using System.Text;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers.RegistrationHandlers;

public class FRRegistrationHandler : RegistrationHandlerBase
{
    private readonly ITaxuallyQueueClient taxuallyQueueClient;

    public FRRegistrationHandler(ITaxuallyQueueClient taxuallyQueueClient)
    {
        this.taxuallyQueueClient = taxuallyQueueClient;
    }

    public override void CreateRegistration(VatRegistrationRequest request)
    {
        // France requires an excel spreadsheet to be uploaded to register for a VAT number
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("CompanyName,CompanyId");
        csvBuilder.AppendLine($"{request.CompanyName}{request.CompanyId}");
        var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
        // Queue file to be processed
        taxuallyQueueClient.EnqueueAsync("vat-registration-csv", csv).Wait();
    }
}
