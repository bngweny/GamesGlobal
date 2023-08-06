using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GamesGlobal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        // GET: api/<ShoppingListController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ShoppingListController>/5
        [HttpGet("{id}")]
        public string Get(int id) 
        {
            return "value";
        }

        // POST api/<ShoppingListController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ShoppingListController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShoppingListController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
