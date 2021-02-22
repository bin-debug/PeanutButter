using Microsoft.AspNetCore.Mvc;
using PeanutButterCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeanutButterProducerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private IServiceBus _serviceBus;

        public PublishController(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"PeanutButterProducerApi V1 - {DateTime.Now}");
        }

        [HttpPost("{topic}")]
        public async Task<IActionResult> Post(string topic, [FromBody] dynamic value)
        {
            string json = Convert.ToString(value);
            var result = await _serviceBus.PublishAsync(topic, json);

            return Ok();
        }
    }
}
