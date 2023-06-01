using Duende.Bff;
using Duende.Bff.Yarp;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddBff(options =>
{
    options.EnforceBffMiddleware = true;
    options.AntiForgeryHeaderName = "AnyX-CSRF";
    options.AntiForgeryHeaderValue = "7";
});

#region Proxy

builder.Services.AddReverseProxy()
    .AddTransforms<AccessTokenTransformProvider>()
    .LoadFromMemory(
        new[]
        {
            new RouteConfig()
            {
                RouteId = "weather", // Unique game 
                ClusterId = "cluster1",

                Match = new RouteMatch
                {
                    Path = "/weatherForecast/{**catch-all}"
                }
            }.WithAccessToken(TokenType.User),

            // new RouteConfig()
            // {
            //     RouteId = "identityApi",
            //     ClusterId = "cluster2",
            //
            //     Match = new RouteMatch
            //     {
            //         Path = "//{**catch-all}"
            //     }
            // }.WithAccessToken(TokenType.User),
        },
        new[]
        {
            new ClusterConfig
            {
                ClusterId = "cluster1",

                Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                {
                    { "destination1", new DestinationConfig() { Address = "https://localhost:7163" } },
                }
            },
            // new ClusterConfig
            // {
            //     ClusterId = "cluster2",
            //
            //     Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
            //     {
            //         { "destination1", new DestinationConfig() { Address = "https://localhost:4000" } },
            //     }
            // }
        });

// registers HTTP client that uses the managed user access token
//builder.Services.AddUserAccessTokenHttpClient("api_client", configureClient: client =>
//{
//    client.BaseAddress = new Uri("https://localhost:5002/");
//});

#endregion

// builder.Services.AddScoped<MyCookieAuthenticationEvents>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";
}).AddCookie("cookie", options =>
{
    // Time for cookie expiration (if SlidingExpiration is true re-issue a new cookie (if active)
    // other wise 8 hours to login again
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.Cookie.MaxAge = options.ExpireTimeSpan; // optional
    options.SlidingExpiration = true;


    options.Cookie.Name = "__Spa-Bff";
    options.Cookie.SameSite = SameSiteMode.Strict;
    
    options.Events.OnSigningIn = ctx =>
    {
        Console.WriteLine();
        Console.WriteLine("Claims received by the Cookie handler");
        foreach (var claim in ctx.Principal.Claims)
        {
            Console.WriteLine($"{claim.Type} - {claim.Value}");
        }
        Console.WriteLine();

        return Task.CompletedTask;
    };
}).AddOpenIdConnect("oidc", options =>
{

    options.Events.OnUserInformationReceived = ctx =>
    {
        Console.WriteLine();
        Console.WriteLine("Claims from the ID token");
        foreach (var claim in ctx.Principal.Claims)
        {
            Console.WriteLine($"{claim.Type} - {claim.Value}");
        }
        Console.WriteLine();
        Console.WriteLine("Claims from the UserInfo endpoint");
        foreach (var property in ctx.User.RootElement.EnumerateObject())
        {
            Console.WriteLine($"{property.Name} - {property.Value}");
        }
        return Task.CompletedTask;
    };

    // options.Events =  new BffConnectEvents();
    // options.EventsType = typeof(MyCookieAuthenticationEvents);
    // options.EventsType = typeof(MyCookieAuthenticationEvents); // new MyCookieAuthenticationEvents();

    options.Authority = "https://localhost:7001";
    options.ClientId = "bffAngular";

    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.ResponseMode = "query";

    // get claims without mappings
    options.MapInboundClaims = false;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("identity.api");
    options.Scope.Add("offline_access");
    // ClaimActions.DeleteClaim("nonce"); https://nestenius.se/2023/03/28/missing-openid-connect-claims-in-asp-net-core/
    options.ClaimActions.MapAllExcept("iss", "nbf","iat","exp", "aud", "nonce","amr", "at_hash","sid", "idp");
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.Name,
        RoleClaimType = JwtClaimTypes.Role,
    };
});


// builder.Services.Configure<BffOpenIdConnectEvents>(options =>
// {
//     options.OnRedirectToIdentityProvider = async ctx => { await Task.Yield(); };
//     options.OnUserInformationReceived = async ctx => { await Task.Yield(); };
// });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection(); sigue marcando error aun y con esto comentado 
app.UseStaticFiles();
app.UseRouting();

// Bff
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    // login, logout, user, backchannel logout...
    endpoints.MapBffManagementEndpoints();

    endpoints.MapBffReverseProxy();
    // endpoints.MapRemoteBffApiEndpoint("/weatherforecast", "https://localhost:5043")
    //     .RequireAccessToken(Duende.Bff.TokenType.User);
});

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");


app.Run();