using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Spotter_Azure.Models;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spotter_Azure.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        public String Default()
        {
            return "Hgmmm";
        }

        // POST api/<ValuesController>
        [HttpGet("register")]
        public void Post([FromQuery] string code)
        {
            if (code != null)
            {
                User u = AuthFlow.FromCode(code);
                if (u != null)
                {
                    Memory.Add(u);
                    Response.StatusCode = 200;
                    Response.Redirect("/");
                    return;
                }
            }
            Response.StatusCode = 405;
            return;
        }
    }
}
