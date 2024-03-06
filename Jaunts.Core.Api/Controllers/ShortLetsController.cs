using Jaunts.Core.Api.Models.Services.Foundations.ShortLets.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ShortLets;
using Jaunts.Core.Api.Services.Foundations.ShortLets;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortLetsController : RESTFulController
    {
        private readonly IShortLetService shortLetService;

        public ShortLetsController(IShortLetService shortLetService) =>
            this.shortLetService = shortLetService;

        [HttpPost]
        public async ValueTask<ActionResult<ShortLet>> PostShortLetAsync(ShortLet shortLet)
        {
            try
            {
                ShortLet addedShortLet =
                    await this.shortLetService.CreateShortLetAsync(shortLet);

                return Created(addedShortLet);
            }
            catch (ShortLetValidationException shortLetValidationException)
            {
                return BadRequest(shortLetValidationException.InnerException);
            }
            catch (ShortLetDependencyValidationException shortLetValidationException)
                when (shortLetValidationException.InnerException is InvalidShortLetReferenceException)
            {
                return FailedDependency(shortLetValidationException.InnerException);
            }
            catch (ShortLetDependencyValidationException shortLetDependencyValidationException)
               when (shortLetDependencyValidationException.InnerException is AlreadyExistsShortLetException)
            {
                return Conflict(shortLetDependencyValidationException.InnerException);
            }
            catch (ShortLetDependencyException shortLetDependencyException)
            {
                return InternalServerError(shortLetDependencyException);
            }
            catch (ShortLetServiceException shortLetServiceException)
            {
                return InternalServerError(shortLetServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ShortLet>> GetAllShortLets()
        {
            try
            {
                IQueryable<ShortLet> retrievedShortLets =
                    this.shortLetService.RetrieveAllShortLets();

                return Ok(retrievedShortLets);
            }
            catch (ShortLetDependencyException shortLetDependencyException)
            {
                return InternalServerError(shortLetDependencyException);
            }
            catch (ShortLetServiceException shortLetServiceException)
            {
                return InternalServerError(shortLetServiceException);
            }
        }

        [HttpGet("{shortLetId}")]
        public async ValueTask<ActionResult<ShortLet>> GetShortLetByIdAsync(Guid shortLetId)
        {
            try
            {
                ShortLet shortLet = await this.shortLetService.RetrieveShortLetByIdAsync(shortLetId);

                return Ok(shortLet);
            }
            catch (ShortLetValidationException shortLetValidationException)
                when (shortLetValidationException.InnerException is NotFoundShortLetException)
            {
                return NotFound(shortLetValidationException.InnerException);
            }
            catch (ShortLetValidationException shortLetValidationException)
            {
                return BadRequest(shortLetValidationException.InnerException);
            }
            catch (ShortLetDependencyException shortLetDependencyException)
            {
                return InternalServerError(shortLetDependencyException);
            }
            catch (ShortLetServiceException shortLetServiceException)
            {
                return InternalServerError(shortLetServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ShortLet>> PutShortLetAsync(ShortLet shortLet)
        {
            try
            {
                ShortLet modifiedShortLet =
                    await this.shortLetService.ModifyShortLetAsync(shortLet);

                return Ok(modifiedShortLet);
            }
            catch (ShortLetValidationException shortLetValidationException)
                when (shortLetValidationException.InnerException is NotFoundShortLetException)
            {
                return NotFound(shortLetValidationException.InnerException);
            }
            catch (ShortLetValidationException shortLetValidationException)
            {
                return BadRequest(shortLetValidationException.InnerException);
            }
            catch (ShortLetDependencyValidationException shortLetValidationException)
                when (shortLetValidationException.InnerException is InvalidShortLetReferenceException)
            {
                return FailedDependency(shortLetValidationException.InnerException);
            }
            catch (ShortLetDependencyValidationException shortLetDependencyValidationException)
               when (shortLetDependencyValidationException.InnerException is AlreadyExistsShortLetException)
            {
                return Conflict(shortLetDependencyValidationException.InnerException);
            }
            catch (ShortLetDependencyException shortLetDependencyException)
            {
                return InternalServerError(shortLetDependencyException);
            }
            catch (ShortLetServiceException shortLetServiceException)
            {
                return InternalServerError(shortLetServiceException);
            }
        }

        [HttpDelete("{shortLetId}")]
        public async ValueTask<ActionResult<ShortLet>> DeleteShortLetByIdAsync(Guid shortLetId)
        {
            try
            {
                ShortLet deletedShortLet =
                    await this.shortLetService.RemoveShortLetByIdAsync(shortLetId);

                return Ok(deletedShortLet);
            }
            catch (ShortLetValidationException shortLetValidationException)
                when (shortLetValidationException.InnerException is NotFoundShortLetException)
            {
                return NotFound(shortLetValidationException.InnerException);
            }
            catch (ShortLetValidationException shortLetValidationException)
            {
                return BadRequest(shortLetValidationException.InnerException);
            }
            catch (ShortLetDependencyValidationException shortLetDependencyValidationException)
                when (shortLetDependencyValidationException.InnerException is LockedShortLetException)
            {
                return Locked(shortLetDependencyValidationException.InnerException);
            }
            catch (ShortLetDependencyValidationException shortLetDependencyValidationException)
            {
                return BadRequest(shortLetDependencyValidationException);
            }
            catch (ShortLetDependencyException shortLetDependencyException)
            {
                return InternalServerError(shortLetDependencyException);
            }
            catch (ShortLetServiceException shortLetServiceException)
            {
                return InternalServerError(shortLetServiceException);
            }
        }
    }
}
