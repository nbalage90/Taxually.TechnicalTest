using Taxually.TechnicalTest.Handlers.RegistrationHandlers;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Factories;

public class CountryFactory
{
    private readonly ITaxuallyHttpClient taxuallyHttpClient = new TaxuallyHttpClient();
    private readonly ITaxuallyQueueClient taxuallyQueueClient = new TaxuallyQueueClient();

    Dictionary<string, RegistrationHandlerBase> countryRegistry = new();

    public CountryFactory()
    {
        countryRegistry.Add("GB", new GBRegistrationHandler(taxuallyHttpClient));
        countryRegistry.Add("FR", new FRRegistrationHandler(taxuallyQueueClient));
        countryRegistry.Add("DE", new DERegistrationHandler(taxuallyQueueClient));
    }

    public void Run(VatRegistrationRequest request)
    {
        if (!countryRegistry.ContainsKey(request.Country))
            throw new Exception("Country not supported");

        var handler = countryRegistry[request.Country];
        handler.CreateRegistration(request);
    }
}
