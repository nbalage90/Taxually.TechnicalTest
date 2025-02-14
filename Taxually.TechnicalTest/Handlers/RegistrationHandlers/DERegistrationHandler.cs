using System.Xml.Serialization;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers.RegistrationHandlers;

public class DERegistrationHandler : RegistrationHandlerBase
{
    private readonly ITaxuallyQueueClient taxuallyQueueClient;

    public DERegistrationHandler(ITaxuallyQueueClient taxuallyQueueClient)
    {
        this.taxuallyQueueClient = taxuallyQueueClient;
    }

    public async override Task CreateRegistrationAsync(VatRegistrationRequest request)
    {
        // Germany requires an XML document to be uploaded to register for a VAT number
        using var stringwriter = new StringWriter();
        var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
        serializer.Serialize(stringwriter, request);
        var xml = stringwriter.ToString();
        // Queue xml doc to be processed
        await taxuallyQueueClient.EnqueueAsync("vat-registration-xml", xml);
    }
}
