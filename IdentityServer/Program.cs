using AetherCore.Utility;
using AetherCore.Utility.Convention;
using AetherCore.Utility.Filter;
using AetherCore.Utility.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

using IdentityServer.Extension;

namespace IdentityServer
{
    public class Program
    {
        /********************************************
         * Application Services settings
         * ******************************************/
        private static async Task AddApplicationServices(WebApplicationBuilder builder)
        {
            /********************************************
             * 加上 Filter 
             * ******************************************/
            //builder.Services.AddExceptionHandling<ProxyServiceErrorFilter>();

            /********************************************
             * 註冊AutoMapper
             * ******************************************/
            // 直接指定 Profile
            //builder.Services.AddAutoMapper(
            //    cfg => cfg.AddMaps(typeof(AuthProfile).Assembly),
            //    typeof(AuthProfile).Assembly
            //);

            builder.Services.AddAutoMapperProfiles(LoggerFactory.Create(builder =>
            {
                builder.AddConsole(); // 或其他你需要的設定
            }));

            /********************************************
             * 加上Database Settings
             * ******************************************/
            //builder.Services.InitialMongoDB(builder.Configuration);
            await builder.Services.InitialMongoDBEntity(builder.Configuration);

            /********************************************
             * 設定記憶體快取
             * ******************************************/
            builder.Services.AddCacheSettings(builder.Configuration);
        }

        /********************************************
         * register & Inject
         * ******************************************/
        private static void AddDependencyInjection(WebApplicationBuilder builder)
        {
            // 自動註冊 AppSettings，掃描指定前綴的assembly，找到帶有 AppSettingsAttribute 的類別並註冊
            builder.Services.AutoRegisterAppSettings(builder.Configuration, new[] { "Common" });
            // 自動註冊服務，掃描指定前綴的assembly，找到帶有 AutoInjectAttribute 的類別並註冊
            builder.Services.AutoInject(new[] { "DataAccess", "Repository", "Service", "IdentityServer" });
            // 手動註冊服務
            builder.Services.SlaveServices();
        }

        /********************************************
         * 註冊 JWT 認證
         * ******************************************/
        private static void AddJwtAuthentication(WebApplicationBuilder builder, out List<JwtOptions> jwtOptions)
        {
            JwtOptions adminJwtOptions = new JwtOptions
            {
                JwtName                     = "AdminJwt",
                ValidateIssuer              = true,
                ValidateAudience            = true,
                ValidateLifetime            = true,
                ValidateIssuerSigningKey    = true,
                Issuer                      = builder.Configuration["Jwt:Admin:Issuer"],
                Audience                    = builder.Configuration["Jwt:Admin:Audience"],
                Secret                      = builder.Configuration["Jwt:Admin:Secret"]
            };

            JwtOptions appJwtOptions = new JwtOptions
            {
                JwtName                     = "AppJwt",
                ValidateIssuer              = true,
                ValidateAudience            = true,
                ValidateLifetime            = true,
                ValidateIssuerSigningKey    = true,
                Issuer                      = builder.Configuration["Jwt:App:Issuer"],
                Audience                    = builder.Configuration["Jwt:App:Audience"],
                Secret                      = builder.Configuration["Jwt:App:Secret"]
            };

            JwtOptions serviceJwtOptions = new JwtOptions
            {
                JwtName                     = "ServiceJwt",
                ValidateIssuer              = true,
                ValidateAudience            = true,
                ValidateLifetime            = true,
                ValidateIssuerSigningKey    = true,
                Issuer                      = builder.Configuration["Jwt:Service:Issuer"],
                Audience                    = builder.Configuration["Jwt:Service:Audience"],
                Secret                      = builder.Configuration["Jwt:Service:Secret"]
            };

            // 第一個 JwtOptions 為預設方案
            jwtOptions = new List<JwtOptions>
            {
                appJwtOptions,
                adminJwtOptions,
                serviceJwtOptions
            };

            builder.Services.AddJwtAuthentication(jwtOptions);
        }

        /********************************************
         * 加上Controller
         * ******************************************/
        private static void AddApiControllers(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(o =>
            {
                o.Conventions.Add(new AuthorizeConvention());       // 套用 CRUD Authorize
            });
        }

        /********************************************
         * Swagger 設定
         * ******************************************/
        private static void AddSwagger(WebApplicationBuilder builder, List<JwtOptions> jwtOptions)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.GenerateSwaggerDoc(builder.Environment);                      // 建立Swagger分頁 與API 要出現的分頁

                c.DocInclusionPredicate(SwaggerApiDocumentInclusion.Include);   // 設定API顯示在哪個分頁

                c.OperationFilter<AddSummaryOperationFilter>();                 // 使用 Operation Filter 來給API加上註解                
                c.OperationFilter<EndpointMetadataAuthorizeLockFilter>();       // 讓每個 API 依照授權(AutoAuthorize)需求自動帶出鎖頭

                if (jwtOptions == null) return;

                foreach (var option in jwtOptions)
                {
                    // 加入 JWT 認證設定
                    c.AddSecurityDefinition(option.JwtName, new OpenApiSecurityScheme
                    {
                        Type            = SecuritySchemeType.Http,
                        Scheme          = "bearer",
                        BearerFormat    = "JWT",
                        In              = ParameterLocation.Header,
                        Description     = "請輸入 " + option.JwtName + " 的 JWT",
                    });
                }
            });
        }

        /***********************************************************************************************
        * 啟用 WebSockets，設置保活間隔與收包緩衝
        ************************************************************************************************/
        //private static void ConfigureWebSockets(WebApplication app)
        //{
        //    var wsOptions = new WebSocketOptions
        //    {
        //        KeepAliveInterval = TimeSpan.FromSeconds(20),
        //        ReceiveBufferSize = 64 * 1024
        //    };

        //    app.UseWebSockets(wsOptions);

        //    // WebSocket 端點：/ws
        //    app.Map<OpenAIProxyWsHub>("/ws");
        //    app.MapGet("/", () => "WS backend running").ExcludeFromDescription();
        //}

        /***********************************************************************************************
        * 啟用 Swagger
        ************************************************************************************************/
        private static void ConfigureSwagger(WebApplication app)
        {
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoints();
                });
            }
        }

        /***********************************************************************************************
        * 其他
        ************************************************************************************************/
        private static void ConfigureRequestPipeline(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();              // 先認證身分
            app.UseAuthorization();               // 再授權權限

            app.MapControllers();
        }

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            List<JwtOptions> jwtOptions = null;

            AddJwtAuthentication(builder, out jwtOptions);      // 註冊 JWT 認證
            AddDependencyInjection(builder);                    // register & Inject
            AddApiControllers(builder);                         // 加上Controller
            AddSwagger(builder, jwtOptions);                    // Swagger 設定
            await AddApplicationServices(builder);              // other settings

            var app = builder.Build();

            //ConfigureWebSockets(app);                           // 啟用 WebSockets，設置保活間隔與收包緩衝
            ConfigureSwagger(app);                              // 啟用 Swagger
            ConfigureRequestPipeline(app);                      // 其他

            await app.RunAsync();
        }
    }
}
