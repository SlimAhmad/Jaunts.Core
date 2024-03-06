using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions;
using Jaunts.Core.Api.Services.Foundations.Amenities;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmenitiesController : RESTFulController
    {
        private readonly IAmenityService AmenityService;

        public AmenitiesController(IAmenityService AmenityService) =>
            this.AmenityService = AmenityService;

        [HttpPost]
        public async ValueTask<ActionResult<Amenity>> PostAmenityAsync(Amenity Amenity)
        {
            try
            {
                Amenity addedAmenity =
                    await this.AmenityService.CreateAmenityAsync(Amenity);

                return Created(addedAmenity);
            }
            catch (AmenityValidationException AmenityValidationException)
            {
                return BadRequest(AmenityValidationException.InnerException);
            }
            catch (AmenityDependencyValidationException AmenityValidationException)
                when (AmenityValidationException.InnerException is InvalidAmenityReferenceException)
            {
                return FailedDependency(AmenityValidationException.InnerException);
            }
            catch (AmenityDependencyValidationException AmenityDependencyValidationException)
               when (AmenityDependencyValidationException.InnerException is AlreadyExistsAmenityException)
            {
                return Conflict(AmenityDependencyValidationException.InnerException);
            }
            catch (AmenityDependencyException AmenityDependencyException)
            {
                return InternalServerError(AmenityDependencyException);
            }
            catch (AmenityServiceException AmenityServiceException)
            {
                return InternalServerError(AmenityServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Amenity>> GetAllAmenities()
        {
            try
            {
                IQueryable<Amenity> retrievedAmenities =
                    this.AmenityService.RetrieveAllAmenities();

                return Ok(retrievedAmenities);
            }
            catch (AmenityDependencyException AmenityDependencyException)
            {
                return InternalServerError(AmenityDependencyException);
            }
            catch (AmenityServiceException AmenityServiceException)
            {
                return InternalServerError(AmenityServiceException);
            }
        }

        [HttpGet("{AmenityId}")]
        public async ValueTask<ActionResult<Amenity>> GetAmenityByIdAsync(Guid AmenityId)
        {
            try
            {
                Amenity Amenity = await this.AmenityService.RetrieveAmenityByIdAsync(AmenityId);

                return Ok(Amenity);
            }
            catch (AmenityValidationException AmenityValidationException)
                when (AmenityValidationException.InnerException is NotFoundAmenityException)
            {
                return NotFound(AmenityValidationException.InnerException);
            }
            catch (AmenityValidationException AmenityValidationException)
            {
                return BadRequest(AmenityValidationException.InnerException);
            }
            catch (AmenityDependencyException AmenityDependencyException)
            {
                return InternalServerError(AmenityDependencyException);
            }
            catch (AmenityServiceException AmenityServiceException)
            {
                return InternalServerError(AmenityServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Amenity>> PutAmenityAsync(Amenity Amenity)
        {
            try
            {
                Amenity modifiedAmenity =
                    await this.AmenityService.ModifyAmenityAsync(Amenity);

                return Ok(modifiedAmenity);
            }
            catch (AmenityValidationException AmenityValidationException)
                when (AmenityValidationException.InnerException is NotFoundAmenityException)
            {
                return NotFound(AmenityValidationException.InnerException);
            }
            catch (AmenityValidationException AmenityValidationException)
            {
                return BadRequest(AmenityValidationException.InnerException);
            }
            catch (AmenityDependencyValidationException AmenityValidationException)
                when (AmenityValidationException.InnerException is InvalidAmenityReferenceException)
            {
                return FailedDependency(AmenityValidationException.InnerException);
            }
            catch (AmenityDependencyValidationException AmenityDependencyValidationException)
               when (AmenityDependencyValidationException.InnerException is AlreadyExistsAmenityException)
            {
                return Conflict(AmenityDependencyValidationException.InnerException);
            }
            catch (AmenityDependencyException AmenityDependencyException)
            {
                return InternalServerError(AmenityDependencyException);
            }
            catch (AmenityServiceException AmenityServiceException)
            {
                return InternalServerError(AmenityServiceException);
            }
        }

        [HttpDelete("{AmenityId}")]
        public async ValueTask<ActionResult<Amenity>> DeleteAmenityByIdAsync(Guid AmenityId)
        {
            try
            {
                Amenity deletedAmenity =
                    await this.AmenityService.RemoveAmenityByIdAsync(AmenityId);

                return Ok(deletedAmenity);
            }
            catch (AmenityValidationException AmenityValidationException)
                when (AmenityValidationException.InnerException is NotFoundAmenityException)
            {
                return NotFound(AmenityValidationException.InnerException);
            }
            catch (AmenityValidationException AmenityValidationException)
            {
                return BadRequest(AmenityValidationException.InnerException);
            }
            catch (AmenityDependencyValidationException AmenityDependencyValidationException)
                when (AmenityDependencyValidationException.InnerException is LockedAmenityException)
            {
                return Locked(AmenityDependencyValidationException.InnerException);
            }
            catch (AmenityDependencyValidationException AmenityDependencyValidationException)
            {
                return BadRequest(AmenityDependencyValidationException);
            }
            catch (AmenityDependencyException AmenityDependencyException)
            {
                return InternalServerError(AmenityDependencyException);
            }
            catch (AmenityServiceException AmenityServiceException)
            {
                return InternalServerError(AmenityServiceException);
            }
        }
    }
}
