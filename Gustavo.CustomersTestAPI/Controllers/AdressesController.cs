using Gustavo.CustomersTestAPI.Data;
using Gustavo.CustomersTestAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gustavo.CustomersTestAPI.Controllers
{
    [Route("api/adresses")]
    [ApiController]
    public class AdressesController : ControllerBase
    {
        private readonly IAdressesRepository _adressesRepo;
        public AdressesController(IAdressesRepository adressesRepo)
        {
            _adressesRepo = adressesRepo;
        }

        // GET: api/<CustomerController>
        [HttpGet("all/")]
        public async Task<ActionResult> GetAll()
        {
            List<CustomerAddress> adresses = await _adressesRepo.GetAllAsync();
            return Ok(new { success = true, adresses = adresses });
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var address = await _adressesRepo.GetByIdAsync(id);
            if (address == null) return BadRequest(new { success = false, error_details = "Address not found." });

            return Ok(new { success = true, address = address });
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<ActionResult> Post(CustomerAddress address)
        {
            if (address == null)
            {
                return BadRequest(new { success = false, error_details = "Address object is required." });
            }

            if (address.CustomerId.ToString() == null || address.CustomerId == 0)
            {
                return BadRequest(new { success = false, error_details = "CustomerId field is required." });
            }

            var result = await _adressesRepo.SaveAsync(address);
            return Ok(result);
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
