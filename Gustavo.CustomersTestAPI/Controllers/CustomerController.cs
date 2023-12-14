using System.Text.RegularExpressions;
using Gustavo.CustomersTestAPI.Data;
using Gustavo.CustomersTestAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gustavo.CustomersTestAPI.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepo;
        public CustomerController(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        // GET: api/<CustomerController>
        [HttpGet("all/")]
        public async Task<ActionResult> GetAll()
        {
            List<Customer> customers = await _customerRepo.GetAllAsync();
            return Ok(new { success = true, customers = customers });
        }

        // GET: api/<CustomerController>
        [HttpGet("stats/")]
        public async Task<ActionResult> GetContainer()
        {
            CustomerContainer container = await _customerRepo.GetContainerAsync();
            return Ok(new { success = true, container = container });
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null) return BadRequest(new { success = false, error_details = "Customer not found." });

            return Ok(new { success = true, customer = customer });
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<ActionResult> Post(Customer customer)
        {
            if (customer == null)
            {
                return BadRequest(new { success = false, error_details = "Client object is required." });
            }

            if (customer.ClientType.ToString() == null)
            {
                return BadRequest(new { success = false, error_details = "ClientType field is required." });
            }

            if (customer.ClientType.ToString() == "F")
            {
                if (customer.CPF == null || customer.FullName == null)
                {
                    return BadRequest(new { success = false, error_details = "Required fields missing." });
                }

                if(customer.CPF.Length != 11 || new Regex("^[0-9]+$").IsMatch(customer.CPF) == false)
                {
                    return BadRequest(new { success = false, error_details = "Invalid CPF." });
                }

                customer.CNPJ = null;
                customer.CompanyName = null;
                customer.TradeName = null;
            }

            if (customer.ClientType.ToString() == "J")
            {
                if (customer.CNPJ == null || customer.CompanyName == null)
                {
                    return BadRequest(new { success = false, error_details = "Required fields missing." });
                }

                if (customer.CNPJ.Length != 14 || new Regex("^[0-9]+$").IsMatch(customer.CNPJ) == false)
                {
                    return BadRequest(new { success = false, error_details = "Invalid CNPJ." });
                }

                customer.CPF = null;
                customer.FullName = null;
            }

            var result = await _customerRepo.SaveAsync(customer);
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
