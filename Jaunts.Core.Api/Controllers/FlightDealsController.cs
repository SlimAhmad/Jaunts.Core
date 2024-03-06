using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDeals;
using Jaunts.Core.Api.Services.Foundations.FlightDeals;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightDealsController : RESTFulController
    {
        private readonly IFlightDealService flightDealService;

        public FlightDealsController(IFlightDealService flightDealService) =>
            this.flightDealService = flightDealService;

        [HttpPost]
        public async ValueTask<ActionResult<FlightDeal>> PostFlightDealAsync(FlightDeal flightDeal)
        {
            try
            {
                FlightDeal addedFlightDeal =
                    await this.flightDealService.CreateFlightDealAsync(flightDeal);

                return Created(addedFlightDeal);
            }
            catch (FlightDealValidationException flightDealValidationException)
            {
                return BadRequest(flightDealValidationException.InnerException);
            }
            catch (FlightDealDependencyValidationException flightDealValidationException)
                when (flightDealValidationException.InnerException is InvalidFlightDealReferenceException)
            {
                return FailedDependency(flightDealValidationException.InnerException);
            }
            catch (FlightDealDependencyValidationException flightDealDependencyValidationException)
               when (flightDealDependencyValidationException.InnerException is AlreadyExistsFlightDealException)
            {
                return Conflict(flightDealDependencyValidationException.InnerException);
            }
            catch (FlightDealDependencyException flightDealDependencyException)
            {
                return InternalServerError(flightDealDependencyException);
            }
            catch (FlightDealServiceException flightDealServiceException)
            {
                return InternalServerError(flightDealServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<FlightDeal>> GetAllFlightDeals()
        {
            try
            {
                IQueryable<FlightDeal> retrievedFlightDeals =
                    this.flightDealService.RetrieveAllFlightDeals();

                return Ok(retrievedFlightDeals);
            }
            catch (FlightDealDependencyException flightDealDependencyException)
            {
                return InternalServerError(flightDealDependencyException);
            }
            catch (FlightDealServiceException flightDealServiceException)
            {
                return InternalServerError(flightDealServiceException);
            }
        }

        [HttpGet("{flightDealId}")]
        public async ValueTask<ActionResult<FlightDeal>> GetFlightDealByIdAsync(Guid flightDealId)
        {
            try
            {
                FlightDeal flightDeal = await this.flightDealService.RetrieveFlightDealByIdAsync(flightDealId);

                return Ok(flightDeal);
            }
            catch (FlightDealValidationException flightDealValidationException)
                when (flightDealValidationException.InnerException is NotFoundFlightDealException)
            {
                return NotFound(flightDealValidationException.InnerException);
            }
            catch (FlightDealValidationException flightDealValidationException)
            {
                return BadRequest(flightDealValidationException.InnerException);
            }
            catch (FlightDealDependencyException flightDealDependencyException)
            {
                return InternalServerError(flightDealDependencyException);
            }
            catch (FlightDealServiceException flightDealServiceException)
            {
                return InternalServerError(flightDealServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<FlightDeal>> PutFlightDealAsync(FlightDeal flightDeal)
        {
            try
            {
                FlightDeal modifiedFlightDeal =
                    await this.flightDealService.ModifyFlightDealAsync(flightDeal);

                return Ok(modifiedFlightDeal);
            }
            catch (FlightDealValidationException flightDealValidationException)
                when (flightDealValidationException.InnerException is NotFoundFlightDealException)
            {
                return NotFound(flightDealValidationException.InnerException);
            }
            catch (FlightDealValidationException flightDealValidationException)
            {
                return BadRequest(flightDealValidationException.InnerException);
            }
            catch (FlightDealDependencyValidationException flightDealValidationException)
                when (flightDealValidationException.InnerException is InvalidFlightDealReferenceException)
            {
                return FailedDependency(flightDealValidationException.InnerException);
            }
            catch (FlightDealDependencyValidationException flightDealDependencyValidationException)
               when (flightDealDependencyValidationException.InnerException is AlreadyExistsFlightDealException)
            {
                return Conflict(flightDealDependencyValidationException.InnerException);
            }
            catch (FlightDealDependencyException flightDealDependencyException)
            {
                return InternalServerError(flightDealDependencyException);
            }
            catch (FlightDealServiceException flightDealServiceException)
            {
                return InternalServerError(flightDealServiceException);
            }
        }

        [HttpDelete("{flightDealId}")]
        public async ValueTask<ActionResult<FlightDeal>> DeleteFlightDealByIdAsync(Guid flightDealId)
        {
            try
            {
                FlightDeal deletedFlightDeal =
                    await this.flightDealService.RemoveFlightDealByIdAsync(flightDealId);

                return Ok(deletedFlightDeal);
            }
            catch (FlightDealValidationException flightDealValidationException)
                when (flightDealValidationException.InnerException is NotFoundFlightDealException)
            {
                return NotFound(flightDealValidationException.InnerException);
            }
            catch (FlightDealValidationException flightDealValidationException)
            {
                return BadRequest(flightDealValidationException.InnerException);
            }
            catch (FlightDealDependencyValidationException flightDealDependencyValidationException)
                when (flightDealDependencyValidationException.InnerException is LockedFlightDealException)
            {
                return Locked(flightDealDependencyValidationException.InnerException);
            }
            catch (FlightDealDependencyValidationException flightDealDependencyValidationException)
            {
                return BadRequest(flightDealDependencyValidationException);
            }
            catch (FlightDealDependencyException flightDealDependencyException)
            {
                return InternalServerError(flightDealDependencyException);
            }
            catch (FlightDealServiceException flightDealServiceException)
            {
                return InternalServerError(flightDealServiceException);
            }
        }
    }
}
