using Taxually.TechnicalTest.Exceptions;
using Taxually.TechnicalTest.Handlers.RegistrationHandlers;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Factories;

public class CountryFactory
{
    private readonly ITaxuallyHttpClient taxuallyHttpClient;
    private readonly ITaxuallyQueueClient taxuallyQueueClient;
    private readonly IConfiguration configuration;
    private readonly Dictionary<string, RegistrationHandlerBase> countryRegistry = [];

    public CountryFactory(ITaxuallyHttpClient taxuallyHttpClient, ITaxuallyQueueClient taxuallyQueueClient, IConfiguration configuration)
    {
        this.taxuallyHttpClient = taxuallyHttpClient;
        this.taxuallyQueueClient = taxuallyQueueClient;
        this.configuration = configuration;
        Initialize();
    }

    public async Task HandleAsync(VatRegistrationRequest request)
    {
        if (!countryRegistry.TryGetValue(request.Country, out RegistrationHandlerBase? value))
            throw new CountryNotSupportedException(request.Country);

        var handler = value;
        await handler.CreateRegistrationAsync(request);
    }

    private void Initialize()
    {
        countryRegistry.Add("GB", new GBRegistrationHandler(taxuallyHttpClient, configuration));
        countryRegistry.Add("FR", new FRRegistrationHandler(taxuallyQueueClient, configuration));
        countryRegistry.Add("DE", new DERegistrationHandler(taxuallyQueueClient, configuration));
    }
}
