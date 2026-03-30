using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphExampleController : ControllerBase
    {

        [HttpGet]
        public virtual IActionResult RunTest()
        {

            return StatusCode(200, "success");
        }
    }

}
