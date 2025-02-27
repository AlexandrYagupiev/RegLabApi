using Microsoft.AspNetCore.Mvc;
using RegLabApi.Models;
using RegLabApi.Services;

namespace RegLabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationsController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Configuration>> Get()
        {
            return Ok(_configurationService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Configuration> Get(int id)
        {
            var configuration = _configurationService.GetById(id);
            if (configuration == null)
            {
                return NotFound();
            }
            return Ok(configuration);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Configuration configuration)
        {
            _configurationService.Create(configuration);
            return CreatedAtAction(nameof(Get), new { id = configuration.Id }, configuration);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Configuration configuration)
        {
            if (id != configuration.Id)
            {
                return BadRequest();
            }

            _configurationService.Update(configuration);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _configurationService.Delete(id);
            return NoContent();
        }
    }
}