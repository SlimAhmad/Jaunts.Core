using Jaunts.Core.Api.Models.Services.Foundations.Fleets.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Fleets;
using Jaunts.Core.Api.Services.Foundations.Fleets;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FleetsController : RESTFulController
    {
        private readonly IFleetService fleetService;

        public FleetsController(IFleetService fleetService) =>
            this.fleetService = fleetService;

        [HttpPost]
        public async ValueTask<ActionResult<Fleet>> PostFleetAsync(Fleet fleet)
        {
            try
            {
                Fleet addedFleet =
                    await this.fleetService.CreateFleetAsync(fleet);

                return Created(addedFleet);
            }
            catch (FleetValidationException fleetValidationException)
            {
                return BadRequest(fleetValidationException.InnerException);
            }
            catch (FleetDependencyValidationException fleetValidationException)
                when (fleetValidationException.InnerException is InvalidFleetReferenceException)
            {
                return FailedDependency(fleetValidationException.InnerException);
            }
            catch (FleetDependencyValidationException fleetDependencyValidationException)
               when (fleetDependencyValidationException.InnerException is AlreadyExistsFleetException)
            {
                return Conflict(fleetDependencyValidationException.InnerException);
            }
            catch (FleetDependencyException fleetDependencyException)
            {
                return InternalServerError(fleetDependencyException);
            }
            catch (FleetServiceException fleetServiceException)
            {
                return InternalServerError(fleetServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Fleet>> GetAllFleets()
        {
            try
            {
                IQueryable<Fleet> retrievedFleets =
                    this.fleetService.RetrieveAllFleets();

                return Ok(retrievedFleets);
            }
            catch (FleetDependencyException fleetDependencyException)
            {
                return InternalServerError(fleetDependencyException);
            }
            catch (FleetServiceException fleetServiceException)
            {
                return InternalServerError(fleetServiceException);
            }
        }

        [HttpGet("{fleetId}")]
        public async ValueTask<ActionResult<Fleet>> GetFleetByIdAsync(Guid fleetId)
        {
            try
            {
                Fleet fleet = await this.fleetService.RetrieveFleetByIdAsync(fleetId);

                return Ok(fleet);
            }
            catch (FleetValidationException fleetValidationException)
                when (fleetValidationException.InnerException is NotFoundFleetException)
            {
                return NotFound(fleetValidationException.InnerException);
            }
            catch (FleetValidationException fleetValidationException)
            {
                return BadRequest(fleetValidationException.InnerException);
            }
            catch (FleetDependencyException fleetDependencyException)
            {
                return InternalServerError(fleetDependencyException);
            }
            catch (FleetServiceException fleetServiceException)
            {
                return InternalServerError(fleetServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Fleet>> PutFleetAsync(Fleet fleet)
        {
            try
            {
                Fleet modifiedFleet =
                    await this.fleetService.ModifyFleetAsync(fleet);

                return Ok(modifiedFleet);
            }
            catch (FleetValidationException fleetValidationException)
                when (fleetValidationException.InnerException is NotFoundFleetException)
            {
                return NotFound(fleetValidationException.InnerException);
            }
            catch (FleetValidationException fleetValidationException)
            {
                return BadRequest(fleetValidationException.InnerException);
            }
            catch (FleetDependencyValidationException fleetValidationException)
                when (fleetValidationException.InnerException is InvalidFleetReferenceException)
            {
                return FailedDependency(fleetValidationException.InnerException);
            }
            catch (FleetDependencyValidationException fleetDependencyValidationException)
               when (fleetDependencyValidationException.InnerException is AlreadyExistsFleetException)
            {
                return Conflict(fleetDependencyValidationException.InnerException);
            }
            catch (FleetDependencyException fleetDependencyException)
            {
                return InternalServerError(fleetDependencyException);
            }
            catch (FleetServiceException fleetServiceException)
            {
                return InternalServerError(fleetServiceException);
            }
        }

        [HttpDelete("{fleetId}")]
        public async ValueTask<ActionResult<Fleet>> DeleteFleetByIdAsync(Guid fleetId)
        {
            try
            {
                Fleet deletedFleet =
                    await this.fleetService.RemoveFleetByIdAsync(fleetId);

                return Ok(deletedFleet);
            }
            catch (FleetValidationException fleetValidationException)
                when (fleetValidationException.InnerException is NotFoundFleetException)
            {
                return NotFound(fleetValidationException.InnerException);
            }
            catch (FleetValidationException fleetValidationException)
            {
                return BadRequest(fleetValidationException.InnerException);
            }
            catch (FleetDependencyValidationException fleetDependencyValidationException)
                when (fleetDependencyValidationException.InnerException is LockedFleetException)
            {
                return Locked(fleetDependencyValidationException.InnerException);
            }
            catch (FleetDependencyValidationException fleetDependencyValidationException)
            {
                return BadRequest(fleetDependencyValidationException);
            }
            catch (FleetDependencyException fleetDependencyException)
            {
                return InternalServerError(fleetDependencyException);
            }
            catch (FleetServiceException fleetServiceException)
            {
                return InternalServerError(fleetServiceException);
            }
        }
    }
}
