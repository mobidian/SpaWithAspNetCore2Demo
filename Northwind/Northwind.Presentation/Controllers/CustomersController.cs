using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Application.Customers.Commands.CreateCustomer;
using Northwind.Application.Customers.Commands.DeleteCustomer;
using Northwind.Application.Customers.Commands.UpdateCustomer;
using Northwind.Application.Customers.Queries.GetCustomerDetail;
using Northwind.Application.Customers.Queries.GetCustomerList;
using Northwind.Domain;
using Northwind.Persistence;

namespace Northwind.Presentation.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly IGetCustomerListQuery _getCustomerListQuery;
        private readonly IGetCustomerDetailQuery _getCustomerDetailQuery;
        private readonly ICreateCustomerCommand _createCustomerCommand;
        private readonly IUpdateCustomerCommand _updateCustomerCommand;
        private readonly IDeleteCustomerCommand _deleteCustomerCommand;

        public CustomersController(
            IGetCustomerListQuery getCustomerListQuery,
            IGetCustomerDetailQuery getCustomerDetailQuery,            
            ICreateCustomerCommand createCustomerCommand,
            IUpdateCustomerCommand updateCustomerCommand,
            IDeleteCustomerCommand deleteCustomerCommand)
        {
            _getCustomerListQuery = getCustomerListQuery;
            _getCustomerDetailQuery = getCustomerDetailQuery;            
            _createCustomerCommand = createCustomerCommand;
            _updateCustomerCommand = updateCustomerCommand;
            _deleteCustomerCommand = deleteCustomerCommand;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await _getCustomerListQuery.Execute());
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] string id)
        {
            var customer = await _getCustomerDetailQuery.Execute(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] CreateCustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _createCustomerCommand.Execute(customer);

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] string id, [FromBody] UpdateCustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _updateCustomerCommand.Execute(customer);

            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] string id)
        {
            await _deleteCustomerCommand.Execute(id);

            return NoContent();
        }
    }
}