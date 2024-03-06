using Jaunts.Core.Api.Models.Services.Foundations.Rides.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Rides;
using Jaunts.Core.Api.Services.Foundations.Rides;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RidesController : RESTFulController
    {
        private readonly IRideService rideService;

        public RidesController(IRideService rideService) =>
            this.rideService = rideService;

        [HttpPost]
        public async ValueTask<ActionResult<Ride>> PostRideAsync(Ride ride)
        {
            try
            {
                Ride addedRide =
                    await this.rideService.CreateRideAsync(ride);

                return Created(addedRide);
            }
            catch (RideValidationException rideValidationException)
            {
                return BadRequest(rideValidationException.InnerException);
            }
            catch (RideDependencyValidationException rideValidationException)
                when (rideValidationException.InnerException is InvalidRideReferenceException)
            {
                return FailedDependency(rideValidationException.InnerException);
            }
            catch (RideDependencyValidationException rideDependencyValidationException)
               when (rideDependencyValidationException.InnerException is AlreadyExistsRideException)
            {
                return Conflict(rideDependencyValidationException.InnerException);
            }
            catch (RideDependencyException rideDependencyException)
            {
                return InternalServerError(rideDependencyException);
            }
            catch (RideServiceException rideServiceException)
            {
                return InternalServerError(rideServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Ride>> GetAllRides()
        {
            try
            {
                IQueryable<Ride> retrievedRides =
                    this.rideService.RetrieveAllRides();

                return Ok(retrievedRides);
            }
            catch (RideDependencyException rideDependencyException)
            {
                return InternalServerError(rideDependencyException);
            }
            catch (RideServiceException rideServiceException)
            {
                return InternalServerError(rideServiceException);
            }
        }

        [HttpGet("{rideId}")]
        public async ValueTask<ActionResult<Ride>> GetRideByIdAsync(Guid rideId)
        {
            try
            {
                Ride ride = await this.rideService.RetrieveRideByIdAsync(rideId);

                return Ok(ride);
            }
            catch (RideValidationException rideValidationException)
                when (rideValidationException.InnerException is NotFoundRideException)
            {
                return NotFound(rideValidationException.InnerException);
            }
            catch (RideValidationException rideValidationException)
            {
                return BadRequest(rideValidationException.InnerException);
            }
            catch (RideDependencyException rideDependencyException)
            {
                return InternalServerError(rideDependencyException);
            }
            catch (RideServiceException rideServiceException)
            {
                return InternalServerError(rideServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Ride>> PutRideAsync(Ride ride)
        {
            try
            {
                Ride modifiedRide =
                    await this.rideService.ModifyRideAsync(ride);

                return Ok(modifiedRide);
            }
            catch (RideValidationException rideValidationException)
                when (rideValidationException.InnerException is NotFoundRideException)
            {
                return NotFound(rideValidationException.InnerException);
            }
            catch (RideValidationException rideValidationException)
            {
                return BadRequest(rideValidationException.InnerException);
            }
            catch (RideDependencyValidationException rideValidationException)
                when (rideValidationException.InnerException is InvalidRideReferenceException)
            {
                return FailedDependency(rideValidationException.InnerException);
            }
            catch (RideDependencyValidationException rideDependencyValidationException)
               when (rideDependencyValidationException.InnerException is AlreadyExistsRideException)
            {
                return Conflict(rideDependencyValidationException.InnerException);
            }
            catch (RideDependencyException rideDependencyException)
            {
                return InternalServerError(rideDependencyException);
            }
            catch (RideServiceException rideServiceException)
            {
                return InternalServerError(rideServiceException);
            }
        }

        [HttpDelete("{rideId}")]
        public async ValueTask<ActionResult<Ride>> DeleteRideByIdAsync(Guid rideId)
        {
            try
            {
                Ride deletedRide =
                    await this.rideService.RemoveRideByIdAsync(rideId);

                return Ok(deletedRide);
            }
            catch (RideValidationException rideValidationException)
                when (rideValidationException.InnerException is NotFoundRideException)
            {
                return NotFound(rideValidationException.InnerException);
            }
            catch (RideValidationException rideValidationException)
            {
                return BadRequest(rideValidationException.InnerException);
            }
            catch (RideDependencyValidationException rideDependencyValidationException)
                when (rideDependencyValidationException.InnerException is LockedRideException)
            {
                return Locked(rideDependencyValidationException.InnerException);
            }
            catch (RideDependencyValidationException rideDependencyValidationException)
            {
                return BadRequest(rideDependencyValidationException);
            }
            catch (RideDependencyException rideDependencyException)
            {
                return InternalServerError(rideDependencyException);
            }
            catch (RideServiceException rideServiceException)
            {
                return InternalServerError(rideServiceException);
            }
        }
    }
}
