using System.Xml.Serialization;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers.RegistrationHandlers;

public class DERegistrationHandler(ITaxuallyQueueClient taxuallyQueueClient, IConfiguration configuration) : RegistrationHandlerBase
{
    private readonly ITaxuallyQueueClient taxuallyQueueClient = taxuallyQueueClient;

    public async override Task CreateRegistrationAsync(VatRegistrationRequest request)
    {
        if (string.IsNullOrEmpty(configuration["Constants:XMLQueueName"]))
            throw new KeyNotFoundException();

        // Germany requires an XML document to be uploaded to register for a VAT number
        using var stringwriter = new StringWriter();
        var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
        serializer.Serialize(stringwriter, request);
        var xml = stringwriter.ToString();
        // Queue xml doc to be processed
        await taxuallyQueueClient.EnqueueAsync(configuration["Constants:XMLQueueName"], xml);
    }
}
