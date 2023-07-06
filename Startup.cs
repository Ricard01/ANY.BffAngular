using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication;
using Serilog;

namespace BffAngular;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddBff(options =>
            {
                // options.EnforceBffMiddleware = true;
                // 401 sino coincide los headers de angular
                options.AntiForgeryHeaderName = "AnyX-CSRF";
                options.AntiForgeryHeaderValue = "7";
            })
            .AddRemoteApis()
            .AddServerSideSessions();

        services.AddAuthentication(options =>
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


            // options.Events.OnSigningIn = ctx =>
            // {
            //     Console.WriteLine();
            //     Console.WriteLine("Claims received by the Cookie handler");
            //     foreach (var claim in ctx.Principal!.Claims)
            //     {
            //         Console.WriteLine($"{claim.Type} - {claim.Value}");
            //     }
            //
            //     Console.WriteLine();
            //
            //     return Task.CompletedTask;
            // };
        }).AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://localhost:5000";
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
            options.Scope.Add("IdentityServerApi");
            options.Scope.Add("offline_access");
            // options.ClaimActions.MapUniqueJsonKey("Permissions", "Permissions");
            // ClaimActions.DeleteClaim("nonce"); https://nestenius.se/2023/03/28/missing-openid-connect-claims-in-asp-net-core/

            // Modifies Claims received by the Cookie handler doesnt make a differenci in Cookie Size 
            options.ClaimActions.MapAllExcept("preferred_username", "email_verified", "iss", "nbf", "iat", "exp", "aud",
                "nonce", "amr", "at_hash", "sid", "idp");


            // options.TokenValidationParameters = new TokenValidationParameters
            // {
            //     NameClaimType = JwtClaimTypes.Name,
            //     RoleClaimType = JwtClaimTypes.Role
            // };
            //
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
        });
    }
// #region Proxy
//
// builder.Services.AddReverseProxy()
//     .AddTransforms<AccessTokenTransformProvider>()
//     .LoadFromConfig(builder.Configuration.GetSection(("ReverseProxy")));
//
//
// #endregion

// builder.Services.AddScoped<MyCookieAuthenticationEvents>();

    public void Configure(IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
        app.UseDeveloperExceptionPage();


// Configure the HTTP request pipeline.
        // if (!app.Environment.IsDevelopment())
        // {
        //     app.UseDeveloperExceptionPage();
        //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //     app.UseHsts();
        // }

// app.UseHttpsRedirection(); sigue marcando error aun y con esto comentado 
        app.UseDefaultFiles();
        app.UseStaticFiles();

// Bff
        app.UseAuthentication();
        app.UseRouting();
        app.UseBff();
        app.UseAuthorization();


        app.UseEndpoints(endpoints =>
        {
            //no afecto cambiar el orden se lee mejor de esta manera. 
            endpoints.MapFallbackToFile("index.html");

            // login, logout, user, backchannel logout...
            endpoints.MapBffManagementEndpoints();

            // tambien se debe agregar las rutas en Angular( proxy.config.js)
            endpoints.MapRemoteBffApiEndpoint("/api2/", "https://localhost:6010")
                .RequireAccessToken(TokenType.User);
            
            // tambien se debe agregar las rutas en Angular( proxy.config.js)
            endpoints.MapRemoteBffApiEndpoint("/api3/", "https://localhost:7139")
                .RequireAccessToken(TokenType.User);

            // On this path, we require the user token
            endpoints.MapRemoteBffApiEndpoint("/api/", "https://localhost:5000")
                .RequireAccessToken(TokenType.User);

            // endpoints.MapBffReverseProxy(proxyPipeline =>
            // {
            //     proxyPipeline.Use(async (context, next) =>
            //     {
            //         Log.Information("starts, {@Context}", context);
            //         await next();
            //         Log.Information("ends {@Context}", context);
            //     });
            // });
        });

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller}/{action=Index}/{id?}");
    }
}