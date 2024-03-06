using Jaunts.Core.Api.Models.Services.Foundations.Drivers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Drivers;
using Jaunts.Core.Api.Services.Foundations.Drivers;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : RESTFulController
    {
        private readonly IDriverService driverService;

        public DriversController(IDriverService driverService) =>
            this.driverService = driverService;

        [HttpPost]
        public async ValueTask<ActionResult<Driver>> PostDriverAsync(Driver driver)
        {
            try
            {
                Driver addedDriver =
                    await this.driverService.CreateDriverAsync(driver);

                return Created(addedDriver);
            }
            catch (DriverValidationException driverValidationException)
            {
                return BadRequest(driverValidationException.InnerException);
            }
            catch (DriverDependencyValidationException driverValidationException)
                when (driverValidationException.InnerException is InvalidDriverReferenceException)
            {
                return FailedDependency(driverValidationException.InnerException);
            }
            catch (DriverDependencyValidationException driverDependencyValidationException)
               when (driverDependencyValidationException.InnerException is AlreadyExistsDriverException)
            {
                return Conflict(driverDependencyValidationException.InnerException);
            }
            catch (DriverDependencyException driverDependencyException)
            {
                return InternalServerError(driverDependencyException);
            }
            catch (DriverServiceException driverServiceException)
            {
                return InternalServerError(driverServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Driver>> GetAllDrivers()
        {
            try
            {
                IQueryable<Driver> retrievedDrivers =
                    this.driverService.RetrieveAllDrivers();

                return Ok(retrievedDrivers);
            }
            catch (DriverDependencyException driverDependencyException)
            {
                return InternalServerError(driverDependencyException);
            }
            catch (DriverServiceException driverServiceException)
            {
                return InternalServerError(driverServiceException);
            }
        }

        [HttpGet("{driverId}")]
        public async ValueTask<ActionResult<Driver>> GetDriverByIdAsync(Guid driverId)
        {
            try
            {
                Driver driver = await this.driverService.RetrieveDriverByIdAsync(driverId);

                return Ok(driver);
            }
            catch (DriverValidationException driverValidationException)
                when (driverValidationException.InnerException is NotFoundDriverException)
            {
                return NotFound(driverValidationException.InnerException);
            }
            catch (DriverValidationException driverValidationException)
            {
                return BadRequest(driverValidationException.InnerException);
            }
            catch (DriverDependencyException driverDependencyException)
            {
                return InternalServerError(driverDependencyException);
            }
            catch (DriverServiceException driverServiceException)
            {
                return InternalServerError(driverServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Driver>> PutDriverAsync(Driver driver)
        {
            try
            {
                Driver modifiedDriver =
                    await this.driverService.ModifyDriverAsync(driver);

                return Ok(modifiedDriver);
            }
            catch (DriverValidationException driverValidationException)
                when (driverValidationException.InnerException is NotFoundDriverException)
            {
                return NotFound(driverValidationException.InnerException);
            }
            catch (DriverValidationException driverValidationException)
            {
                return BadRequest(driverValidationException.InnerException);
            }
            catch (DriverDependencyValidationException driverValidationException)
                when (driverValidationException.InnerException is InvalidDriverReferenceException)
            {
                return FailedDependency(driverValidationException.InnerException);
            }
            catch (DriverDependencyValidationException driverDependencyValidationException)
               when (driverDependencyValidationException.InnerException is AlreadyExistsDriverException)
            {
                return Conflict(driverDependencyValidationException.InnerException);
            }
            catch (DriverDependencyException driverDependencyException)
            {
                return InternalServerError(driverDependencyException);
            }
            catch (DriverServiceException driverServiceException)
            {
                return InternalServerError(driverServiceException);
            }
        }

        [HttpDelete("{driverId}")]
        public async ValueTask<ActionResult<Driver>> DeleteDriverByIdAsync(Guid driverId)
        {
            try
            {
                Driver deletedDriver =
                    await this.driverService.RemoveDriverByIdAsync(driverId);

                return Ok(deletedDriver);
            }
            catch (DriverValidationException driverValidationException)
                when (driverValidationException.InnerException is NotFoundDriverException)
            {
                return NotFound(driverValidationException.InnerException);
            }
            catch (DriverValidationException driverValidationException)
            {
                return BadRequest(driverValidationException.InnerException);
            }
            catch (DriverDependencyValidationException driverDependencyValidationException)
                when (driverDependencyValidationException.InnerException is LockedDriverException)
            {
                return Locked(driverDependencyValidationException.InnerException);
            }
            catch (DriverDependencyValidationException driverDependencyValidationException)
            {
                return BadRequest(driverDependencyValidationException);
            }
            catch (DriverDependencyException driverDependencyException)
            {
                return InternalServerError(driverDependencyException);
            }
            catch (DriverServiceException driverServiceException)
            {
                return InternalServerError(driverServiceException);
            }
        }
    }
}
