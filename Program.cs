using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Net6AngularOauth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using Net6AngularOauth2.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
    });
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<JwtHandler>();

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = true;
        options.Audience = "MyApi";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            ValidIssuer = "MyApi",
            ValidateIssuer = true,

            ValidateLifetime = true,
            RequireExpirationTime = true,

            ValidAudience = "MyApi",
            ValidateAudience = true,

            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecretkey"))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = async context =>
            {
                // Coming from Header 'Authorization'
                if (context.Request.Headers.TryGetValue("Authorization", out StringValues apiKeyHeaderValues))
                {
                    await Task.CompletedTask;
                }

                // Coming from Cookie
                if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                {
                    context.Token = context.Request.Cookies["X-Access-Token"];
                }

                await Task.CompletedTask;
            },
            OnForbidden = async context =>
            {
                // Log Ip etc etc
                await Task.CompletedTask;
            },
            OnAuthenticationFailed = async context =>
            {
                // Log Ip etc etc
                context.NoResult();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                string response = JsonConvert.SerializeObject("The access token provided is not valid.");

                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                    response = JsonConvert.SerializeObject("The access token provided has expired.");
                }

                await context.Response.WriteAsync(response);
                await Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSession(options =>
{
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.Path = "/";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        policy =>
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
    );
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.UseCors("CorsPolicy");

app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html"); ;

app.Run();
