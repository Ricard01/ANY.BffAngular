using Duende.Bff;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Serilog;

namespace BffAngular.HostingExtensions;

public class MyCookieAuthenticationEvents : CookieAuthenticationEvents
{
  

// options.Events.OnUserInformationReceived = ctx =>
    // {
    //     Console.WriteLine();
    //     Console.WriteLine("Claims from the ID token");
    //     foreach (var claim in ctx.Principal.Claims)
    //     {
    //         Console.WriteLine($"{claim.Type} - {claim.Value}");
    //     }
    //
    //     Console.WriteLine();
    //     Console.WriteLine("Claims from the UserInfo endpoint");
    //     foreach (var property in ctx.User.RootElement.EnumerateObject())
    //     {
    //         Console.WriteLine($"{property.Name} - {property.Value}");
    //     }
    //
    //     return Task.CompletedTask;
    // };


    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        await base.ValidatePrincipal(context);
    }

    public override async Task SigningIn(CookieSigningInContext context)
    {
        await base.SigningIn(context);
    }

    public override async Task SignedIn(CookieSignedInContext context)
    {
        await base.SignedIn(context);
    }

    public override async Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
    {
        await base.RedirectToLogin(context);
    }
}