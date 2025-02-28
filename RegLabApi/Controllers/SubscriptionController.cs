using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RegLabApi.SignalR;
using Microsoft.AspNetCore.Http;

namespace RegLabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubscriptionController(IHubContext<NotificationHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("subscribe/{group}")]
        public IActionResult SubscribeToGroup(string group)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return BadRequest("HttpContext не доступен.");
            }

            var connectionId = _httpContextAccessor.HttpContext.Connection.Id;

            if (string.IsNullOrEmpty(connectionId))
            {
                return BadRequest("Соединение не найдено.");
            }

            _hubContext.Groups.AddToGroupAsync(connectionId, group);
            return Ok();
        }

        [HttpPost("unsubscribe/{group}")]
        public IActionResult UnsubscribeFromGroup(string group)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return BadRequest("HttpContext не доступен.");
            }

            var connectionId = _httpContextAccessor.HttpContext.Connection.Id;

            if (string.IsNullOrEmpty(connectionId))
            {
                return BadRequest("Соединение не найдено.");
            }

            _hubContext.Groups.RemoveFromGroupAsync(connectionId, group);
            return Ok();
        }
    }
}
