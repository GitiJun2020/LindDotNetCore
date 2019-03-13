using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Mvc; 
using LindDotNetCore.Logger; 
namespace LindDotNetCore.Api.Controllers 
{
	[Route ("api/[controller]")]	
	public class ValuesController:Controller 
    {
		ILogger logger; 
		public ValuesController (ILogger logger)
        {
			this.logger = logger; 
		}
		// GET api/values
		[HttpGet]
		public IEnumerable < string > Get ()
        {
			logger.Debug ("hello world api value"); 
			return new string[] 
            {"value1", "value2"}; 
		}

		// GET api/values/5
		[HttpGet ("{id}")]
		public string Get (int id)
        {
			return "value"; 
		}

		// POST api/values
		[HttpPost]
		public void Post ([FromBody] string value)
        {}

		// PUT api/values/5
		[HttpPut ("{id}")]
		public void Put (int id, [FromBody] string value)
        {}

		// DELETE api/values/5
		[HttpDelete ("{id}")]
		public void Delete (int id)
        {}
	}
}