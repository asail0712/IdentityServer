using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IdentityServer.Extension
{
    public static class SwaggerExtension
    {
        static public void GenerateSwaggerDoc(this SwaggerGenOptions c, IWebHostEnvironment env)
        {
            // === App 分頁 ===
            c.SwaggerDoc("App", new OpenApiInfo
            {
                Title       = "Proxy Service API",
                Version     = "v1",
                Description = $"現在環境：{env.EnvironmentName}"
            });

            // === Backpage 分頁 ===
            c.SwaggerDoc("BackStage", new OpenApiInfo
            {
                Title       = "Proxy BackStage API",
                Version     = "v1",
                Description = $"現在環境：{env.EnvironmentName}"
            });

            // === Test 分頁 ===
            c.SwaggerDoc("Test", new OpenApiInfo
            {
                Title       = "Proxy Service Test",
                Version     = "v1",
                Description = $"現在環境：{env.EnvironmentName}"
            });

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                // 先確定能取到 MethodInfo（方法資訊）
                if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                {
                    return false;
                }

                string derivedClassName = methodInfo.ReflectedType!.Name;       // 取得「衍生類別名稱」= 方法所在的類別名稱
                string methodName       = methodInfo.Name;                      // 取得「函數名稱」
                string combinedName     = $"{derivedClassName}.{methodName}";   // 合併

                List<string> apiInAPP = new List<string>
                {
                    "LoginController.LineLogin",
                    "LoginController.PasswordLogin",
                    "UserController.GetProfile",
                };

                List<string> apiInBackStage = new List<string>
                {
                   "UserController.GetAll",
                   "UserController.Get",
                   "UserController.Update",
                   "UserController.Delete",
                   "UserController.CreateUser",
                };

                List<string> apiInTest = new List<string>
                {
                    "ServiceTokenController.RequestServiceToken"
                };

                if (docName == "App" && apiInAPP.Contains(combinedName))
                {
                    return true;
                }
                else if (docName == "BackStage" && apiInBackStage.Contains(combinedName))
                {
                    return true;
                }
                else if (docName == "Test" && apiInTest.Contains(combinedName))
                {
                    return true;
                }

                return false;
            });
        }

        static public void SwaggerEndpoints(this SwaggerUIOptions c)
        {
            c.SwaggerEndpoint("/swagger/App/swagger.json", "前端 API");
            c.SwaggerEndpoint("/swagger/BackStage/swagger.json", "後台 API");
            c.SwaggerEndpoint("/swagger/Test/swagger.json", "測試 API");
        }
    }
}
