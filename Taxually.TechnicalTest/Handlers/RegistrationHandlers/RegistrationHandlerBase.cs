using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Handlers.RegistrationHandlers;

public abstract class RegistrationHandlerBase
{
    public abstract Task CreateRegistrationAsync(VatRegistrationRequest request);
}
