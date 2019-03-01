using BalanceEnquiry.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BalanceEnquiry.Interceptors
{
    public class Logger
    {

        private readonly RequestDelegate _next;
        //IOptions<AppSettings> _settings;
        private readonly ILogger<Logger> _logger;

        public Logger(RequestDelegate next, ILogger<Logger> logger)
        {
            _next = next;
            //_settings = settings;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string reqId = DateTime.Now.ToString(Constant.TIMESTAMP_FORMAT_2);
            string logpath = string.Empty;

            context.Request.EnableRewind();

            var originalRequestBody = context.Request.Body;
            var requestText = await FormatRequest(context.Request);
            var originalResponseBody = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Request.Body.Position = 0;
                context.Response.Body = responseBody;
                context.Request.Body = originalRequestBody;

                List<string> logValues = new List<string>();
                var path = context.Request.Path + context.Request.QueryString;

                Stopwatch stopwatch = Stopwatch.StartNew();
                await _next(context);
                stopwatch.Stop();

                var responseText = await FormatResponse(context.Response);
                var timestamp = DateTimeOffset.Now.ToString(Constant.TIMESTAMP_FORMAT_1);
                var elapsed = $"{stopwatch.ElapsedMilliseconds.ToString().PadLeft(5)}ms";
                var status = context.Response.StatusCode.ToString();
                var method = context.Request.Method;

                //if (_settings.Value.loggerModeOn.Any(path.Contains))
                //{
                    try
                    {                        
                        logValues.Add("Elapsed: " + elapsed);
                        logValues.Add("Method: " + method);
                        logValues.Add("Path: " + path);
                        logValues.Add("Request: " + requestText);
                        logValues.Add("Response: " + responseText);
                        logValues.Add("Status: " + status);
                        logValues.Add("TimeStamp: " + timestamp);


                        reqId = (!string.IsNullOrEmpty(reqId)) ? reqId : DateTime.Now.ToString(Constant.TIMESTAMP_FORMAT_2);

                        _logger.LogInformation(reqId + JsonHelper.toJson(logValues));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        _logger.LogError(ex.ToString() + JsonHelper.toJson(logValues));
                    }
                //}
                await responseBody.CopyToAsync(originalResponseBody);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            return $"{bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{text}";
        }
    }

    public static class RequestResponseLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Logger>();
        }
    }
}
