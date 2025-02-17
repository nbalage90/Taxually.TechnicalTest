using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers.RegistrationHandlers;

public class GBRegistrationHandler(ITaxuallyHttpClient taxuallyHttpClient, IConfiguration configuration) : RegistrationHandlerBase
{
    public async override Task CreateRegistrationAsync(VatRegistrationRequest request)
    {
        if (string.IsNullOrEmpty(configuration["Constants:GBApiEndpoint"]))
            throw new KeyNotFoundException();

        await taxuallyHttpClient.PostAsync(configuration["Constants:GBApiEndpoint"], request);
    }
}
