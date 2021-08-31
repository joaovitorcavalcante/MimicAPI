using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/words")]
    public class WordsController : ControllerBase
    {
        [HttpGet("", Name = "Index")]
        public string Index() => "Index Route";
    }
}
