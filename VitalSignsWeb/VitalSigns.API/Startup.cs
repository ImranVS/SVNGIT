using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens;
using VitalSigns.API.Security;
using System.Security.Cryptography;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Authentication.JwtBearer;
using Newtonsoft.Json;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Diagnostics;
using System.Text;

namespace VitalSigns.API
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public static string DataBaseName { get; private set; }
        public static string ServerTypeJsonPath { get; private set; }
        public static string PowerScriptsPath { get; private set; }

        internal static RSAParameters RSAKeyParameters { get; set; }

        const string TokenAudience = "VitalSigns Web App";
        const string TokenIssuer = "VitalSigns";

        private SecurityKey key;
        private TokenAuthOptions tokenOptions;

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            ConnectionString = Configuration.Get<string>("Data:MongoDB:MongoServerSettings");
            DataBaseName = Configuration.Get<string>("Data:MongoDB:DataBaseName");

            RSAKeyParameters = RSAKeyUtils.GetKeyParameters(env.MapPath("App_Data/rsa_key.json"));
            PowerScriptsPath = env.MapPath("PowerShellScripts/");
            
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            // Security settings and JWT configuration
            key = new RsaSecurityKey(RSAKeyParameters);

            tokenOptions = new TokenAuthOptions()
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.SHA256)
            };

            services.AddInstance(tokenOptions);

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            // TODO: refine CORS when security will be set
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            }));

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            ServerTypeJsonPath = env.MapPath("App_Data/server_type_data.json");
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors("CorsPolicy");

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject(
                                new { authenticated = false, tokenExpired = true }));
                    }
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject
                            (new { success = false, error = error.Error.Message }));
                    }
                    else await next();
                });
            });

            app.UseJwtBearerAuthentication(options =>
            {
                options.TokenValidationParameters.IssuerSigningKey = key;
                options.TokenValidationParameters.ValidAudience = tokenOptions.Audience;
                options.TokenValidationParameters.ValidIssuer = tokenOptions.Issuer;
                options.TokenValidationParameters.ValidateSignature = true;
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

            app.UseMvc();
        }

        static internal string SignData(string data)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            using (SHA256Managed hash = new SHA256Managed())
            {
                RSA.ImportParameters(RSAKeyParameters);

                return Convert.ToBase64String(RSA.SignData(Encoding.UTF8.GetBytes(data), hash));
            }
        }
        /*
        static internal String EncryptString(String data)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(RSAKeyParameters);
                //for encryption, always handle bytes...
                var bytesPlainTextData = Encoding.Unicode.GetBytes(data);

                var bytesCypherText = RSA.Encrypt(bytesPlainTextData, false);

                return Convert.ToBase64String(bytesCypherText);
            }
        }
        
        static internal String DecryptString(String encryptedText)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(RSAKeyParameters);
                var bytesCypherText = Convert.FromBase64String(encryptedText);
                var bytesDecryptedCypherText = RSA.Decrypt(bytesCypherText, false);

                return Encoding.Unicode.GetString(bytesDecryptedCypherText);
            }
        }

       
        */

        static internal bool VerifyData(string originalMessage, string signedMessage)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            using (SHA256Managed hash = new SHA256Managed())
            {
                RSA.ImportParameters(RSAKeyParameters);

                byte[] bytesToVerify = Encoding.UTF8.GetBytes(originalMessage);
                byte[] signedBytes = Convert.FromBase64String(signedMessage);

                byte[] hashedData = hash.ComputeHash(signedBytes);

                return RSA.VerifyData(bytesToVerify, hash, signedBytes);
            }
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
