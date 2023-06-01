using Duende.Bff;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace BffAngular.HostingExtensions;

public class BffConnectEvents : BffOpenIdConnectEvents
{
    public override async Task RedirectToIdentityProvider(RedirectContext context)
    {
        await base.RedirectToIdentityProvider(context);
    }

    public BffConnectEvents(ILogger<BffOpenIdConnectEvents> logger) : base(logger)
    {
    }

    public override async Task UserInformationReceived(UserInformationReceivedContext context)
    {
        await base.UserInformationReceived(context);
    }
}