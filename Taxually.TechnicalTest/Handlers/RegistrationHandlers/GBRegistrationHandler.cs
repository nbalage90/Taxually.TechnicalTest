using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers.RegistrationHandlers;

public class GBRegistrationHandler : RegistrationHandlerBase
{
    private readonly ITaxuallyHttpClient taxuallyHttpClient;

    public GBRegistrationHandler(ITaxuallyHttpClient taxuallyHttpClient)
    {
        this.taxuallyHttpClient = taxuallyHttpClient;
    }

    public async override Task CreateRegistrationAsync(VatRegistrationRequest request)
    {
        // UK has an API to register for a VAT number
        await taxuallyHttpClient.PostAsync("https://api.uktax.gov.uk", request);
    }
}
