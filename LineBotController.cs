using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LineBotThuIm
{
    [Route("api/linebot")]
    public class LineBotController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly LineBotConfigModel _lineBotConfig;

        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public LineBotController(IServiceProvider serviceProvider, LineBotConfigModel lineBotConfig, ILogger<LineBotController> logger)
        {
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _httpContext = _httpContextAccessor.HttpContext;
            _lineBotConfig = lineBotConfig;
            _logger = logger;
        }

        [HttpPost("run")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var events = await _httpContext.Request.GetWebhookEventsAsync("d5481a7e604dc1266a2caad67a5bc0e7");
                var lineMessagingClient = new LineMessagingClient("d6LDOh0pCbl0oMEoJ9Y/Xitg1GYf7ZzTFQpFxf6Cn+qZSrSMYXDmEZp6bzAkwOmm/KhgXcZYf1K4NH3UGcK20syEZLbP1YyR97wBmABazofnfHXmDSbJW7/ublngiCr0eE8K9sCt/uh8wVbWiHR/rgdB04t89/1O/w1cDnyilFU=");
                var lineBotApp = new LineBotApp(lineMessagingClient);
                await lineBotApp.RunAsync(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(JsonConvert.SerializeObject("出錯了!" + ex));
            }
            return Ok();
        }
    }
}
