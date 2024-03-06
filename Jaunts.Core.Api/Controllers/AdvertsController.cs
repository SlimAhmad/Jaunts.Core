using FluentAssertions.Equivalency.Tracing;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts;
using Jaunts.Core.Api.Models.Services.Foundations.Adverts.Exceptions;
using Jaunts.Core.Api.Services.Foundations.Adverts;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class AdvertsController : RESTFulController
        {
        private readonly IAdvertService advertService;

        public AdvertsController(IAdvertService advertService) =>
            this.advertService = advertService;

        [HttpPost]
        public async ValueTask<ActionResult<Advert>> PostAdvertAsync(Advert advert)
        {
            try
            {
                Advert addedAdvert =
                    await this.advertService.CreateAdvertAsync(advert);

                return Created(addedAdvert);
            }
            catch (AdvertValidationException advertValidationException)
            {
                return BadRequest(advertValidationException.InnerException);
            }
            catch (AdvertDependencyValidationException advertValidationException)
                when (advertValidationException.InnerException is InvalidAdvertReferenceException)
            {
                return FailedDependency(advertValidationException.InnerException);
            }
            catch (AdvertDependencyValidationException advertDependencyValidationException)
               when (advertDependencyValidationException.InnerException is AlreadyExistsAdvertException)
            {
                return Conflict(advertDependencyValidationException.InnerException);
            }
            catch (AdvertDependencyException advertDependencyException)
            {
                return InternalServerError(advertDependencyException);
            }
            catch (AdvertServiceException advertServiceException)
            {
                return InternalServerError(advertServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Advert>> GetAllAdverts()
        {
            try
            {
                IQueryable<Advert> retrievedAdverts =
                    this.advertService.RetrieveAllAdverts();

                return Ok(retrievedAdverts);
            }
            catch (AdvertDependencyException advertDependencyException)
            {
                return InternalServerError(advertDependencyException);
            }
            catch (AdvertServiceException advertServiceException)
            {
                return InternalServerError(advertServiceException);
            }
        }

        [HttpGet("{advertId}")]
        public async ValueTask<ActionResult<Advert>> GetAdvertByIdAsync(Guid advertId)
        {
            try
            {
                Advert advert = await this.advertService.RetrieveAdvertByIdAsync(advertId);

                return Ok(advert);
            }
            catch (AdvertValidationException advertValidationException)
                when (advertValidationException.InnerException is NotFoundAdvertException)
            {
                return NotFound(advertValidationException.InnerException);
            }
            catch (AdvertValidationException advertValidationException)
            {
                return BadRequest(advertValidationException.InnerException);
            }
            catch (AdvertDependencyException advertDependencyException)
            {
                return InternalServerError(advertDependencyException);
            }
            catch (AdvertServiceException advertServiceException)
            {
                return InternalServerError(advertServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Advert>> PutAdvertAsync(Advert advert)
        {
            try
            {
                Advert modifiedAdvert =
                    await this.advertService.ModifyAdvertAsync(advert);

                return Ok(modifiedAdvert);
            }
            catch (AdvertValidationException advertValidationException)
                when (advertValidationException.InnerException is NotFoundAdvertException)
            {
                return NotFound(advertValidationException.InnerException);
            }
            catch (AdvertValidationException advertValidationException)
            {
                return BadRequest(advertValidationException.InnerException);
            }
            catch (AdvertDependencyValidationException advertValidationException)
                when (advertValidationException.InnerException is InvalidAdvertReferenceException)
            {
                return FailedDependency(advertValidationException.InnerException);
            }
            catch (AdvertDependencyValidationException advertDependencyValidationException)
               when (advertDependencyValidationException.InnerException is AlreadyExistsAdvertException)
            {
                return Conflict(advertDependencyValidationException.InnerException);
            }
            catch (AdvertDependencyException advertDependencyException)
            {
                return InternalServerError(advertDependencyException);
            }
            catch (AdvertServiceException advertServiceException)
            {
                return InternalServerError(advertServiceException);
            }
        }

        [HttpDelete("{advertId}")]
        public async ValueTask<ActionResult<Advert>> DeleteAdvertByIdAsync(Guid advertId)
        {
            try
            {
                Advert deletedAdvert =
                    await this.advertService.RemoveAdvertByIdAsync(advertId);

                return Ok(deletedAdvert);
            }
            catch (AdvertValidationException advertValidationException)
                when (advertValidationException.InnerException is NotFoundAdvertException)
            {
                return NotFound(advertValidationException.InnerException);
            }
            catch (AdvertValidationException advertValidationException)
            {
                return BadRequest(advertValidationException.InnerException);
            }
            catch (AdvertDependencyValidationException advertDependencyValidationException)
                when (advertDependencyValidationException.InnerException is LockedAdvertException)
            {
                return Locked(advertDependencyValidationException.InnerException);
            }
            catch (AdvertDependencyValidationException advertDependencyValidationException)
            {
                return BadRequest(advertDependencyValidationException);
            }
            catch (AdvertDependencyException advertDependencyException)
            {
                return InternalServerError(advertDependencyException);
            }
            catch (AdvertServiceException advertServiceException)
            {
                return InternalServerError(advertServiceException);
            }
        }
    }
}
