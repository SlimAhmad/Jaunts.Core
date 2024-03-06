using Jaunts.Core.Api.Models.Services.Foundations.Customers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Customers;
using Jaunts.Core.Api.Services.Foundations.Customers;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Jaunts.Core.Api.Models.Services.Foundations.customers.Exceptions;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : RESTFulController
    {
        private readonly ICustomerService CustomerService;

        public CustomersController(ICustomerService CustomerService) =>
            this.CustomerService = CustomerService;

        [HttpPost]
        public async ValueTask<ActionResult<Customer>> PostCustomerAsync(Customer Customer)
        {
            try
            {
                Customer addedCustomer =
                    await this.CustomerService.CreateCustomerAsync(Customer);

                return Created(addedCustomer);
            }
            catch (CustomerValidationException CustomerValidationException)
            {
                return BadRequest(CustomerValidationException.InnerException);
            }
            catch (CustomerDependencyValidationException CustomerValidationException)
                when (CustomerValidationException.InnerException is InvalidCustomerReferenceException)
            {
                return FailedDependency(CustomerValidationException.InnerException);
            }
            catch (CustomerDependencyValidationException CustomerDependencyValidationException)
               when (CustomerDependencyValidationException.InnerException is AlreadyExistsCustomerException)
            {
                return Conflict(CustomerDependencyValidationException.InnerException);
            }
            catch (CustomerDependencyException CustomerDependencyException)
            {
                return InternalServerError(CustomerDependencyException);
            }
            catch (CustomerServiceException CustomerServiceException)
            {
                return InternalServerError(CustomerServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Customer>> GetAllCustomers()
        {
            try
            {
                IQueryable<Customer> retrievedCustomers =
                    this.CustomerService.RetrieveAllCustomers();

                return Ok(retrievedCustomers);
            }
            catch (CustomerDependencyException CustomerDependencyException)
            {
                return InternalServerError(CustomerDependencyException);
            }
            catch (CustomerServiceException CustomerServiceException)
            {
                return InternalServerError(CustomerServiceException);
            }
        }

        [HttpGet("{CustomerId}")]
        public async ValueTask<ActionResult<Customer>> GetCustomerByIdAsync(Guid CustomerId)
        {
            try
            {
                Customer Customer = await this.CustomerService.RetrieveCustomerByIdAsync(CustomerId);

                return Ok(Customer);
            }
            catch (CustomerValidationException CustomerValidationException)
                when (CustomerValidationException.InnerException is NotFoundCustomerException)
            {
                return NotFound(CustomerValidationException.InnerException);
            }
            catch (CustomerValidationException CustomerValidationException)
            {
                return BadRequest(CustomerValidationException.InnerException);
            }
            catch (CustomerDependencyException CustomerDependencyException)
            {
                return InternalServerError(CustomerDependencyException);
            }
            catch (CustomerServiceException CustomerServiceException)
            {
                return InternalServerError(CustomerServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Customer>> PutCustomerAsync(Customer Customer)
        {
            try
            {
                Customer modifiedCustomer =
                    await this.CustomerService.ModifyCustomerAsync(Customer);

                return Ok(modifiedCustomer);
            }
            catch (CustomerValidationException CustomerValidationException)
                when (CustomerValidationException.InnerException is NotFoundCustomerException)
            {
                return NotFound(CustomerValidationException.InnerException);
            }
            catch (CustomerValidationException CustomerValidationException)
            {
                return BadRequest(CustomerValidationException.InnerException);
            }
            catch (CustomerDependencyValidationException CustomerValidationException)
                when (CustomerValidationException.InnerException is InvalidCustomerReferenceException)
            {
                return FailedDependency(CustomerValidationException.InnerException);
            }
            catch (CustomerDependencyValidationException CustomerDependencyValidationException)
               when (CustomerDependencyValidationException.InnerException is AlreadyExistsCustomerException)
            {
                return Conflict(CustomerDependencyValidationException.InnerException);
            }
            catch (CustomerDependencyException CustomerDependencyException)
            {
                return InternalServerError(CustomerDependencyException);
            }
            catch (CustomerServiceException CustomerServiceException)
            {
                return InternalServerError(CustomerServiceException);
            }
        }

        [HttpDelete("{CustomerId}")]
        public async ValueTask<ActionResult<Customer>> DeleteCustomerByIdAsync(Guid CustomerId)
        {
            try
            {
                Customer deletedCustomer =
                    await this.CustomerService.RemoveCustomerByIdAsync(CustomerId);

                return Ok(deletedCustomer);
            }
            catch (CustomerValidationException CustomerValidationException)
                when (CustomerValidationException.InnerException is NotFoundCustomerException)
            {
                return NotFound(CustomerValidationException.InnerException);
            }
            catch (CustomerValidationException CustomerValidationException)
            {
                return BadRequest(CustomerValidationException.InnerException);
            }
            catch (CustomerDependencyValidationException CustomerDependencyValidationException)
                when (CustomerDependencyValidationException.InnerException is LockedCustomerException)
            {
                return Locked(CustomerDependencyValidationException.InnerException);
            }
            catch (CustomerDependencyValidationException CustomerDependencyValidationException)
            {
                return BadRequest(CustomerDependencyValidationException);
            }
            catch (CustomerDependencyException CustomerDependencyException)
            {
                return InternalServerError(CustomerDependencyException);
            }
            catch (CustomerServiceException CustomerServiceException)
            {
                return InternalServerError(CustomerServiceException);
            }
        }
    }
}
