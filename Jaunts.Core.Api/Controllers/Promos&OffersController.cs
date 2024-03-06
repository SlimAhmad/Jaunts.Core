using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOffers;
using Jaunts.Core.Api.Services.Foundations.PromosOffers;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Jaunts.Core.Api.Models.Services.Foundations.Promos_Offers;
using Jaunts.Core.Api.Models.Services.Foundations.PromosOfferss.Exceptions;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromosOffersController : RESTFulController
    {
        private readonly IPromosOfferService promosOfferService;

        public PromosOffersController(IPromosOfferService promosOfferService) =>
            this.promosOfferService = promosOfferService;

        [HttpPost]
        public async ValueTask<ActionResult<PromosOffer>> PostPromosOfferAsync(PromosOffer promosOffer)
        {
            try
            {
                PromosOffer addedPromosOffer =
                    await this.promosOfferService.CreatePromosOfferAsync(promosOffer);

                return Created(addedPromosOffer);
            }
            catch (PromosOffersValidationException promosOfferValidationException)
            {
                return BadRequest(promosOfferValidationException.InnerException);
            }
            catch (PromosOffersDependencyValidationException promosOfferValidationException)
                when (promosOfferValidationException.InnerException is InvalidPromosOffersReferenceException)
            {
                return FailedDependency(promosOfferValidationException.InnerException);
            }
            catch (PromosOffersDependencyValidationException promosOfferDependencyValidationException)
               when (promosOfferDependencyValidationException.InnerException is AlreadyExistsPromosOffersException)
            {
                return Conflict(promosOfferDependencyValidationException.InnerException);
            }
            catch (PromosOffersDependencyException promosOfferDependencyException)
            {
                return InternalServerError(promosOfferDependencyException);
            }
            catch (PromosOffersServiceException promosOfferServiceException)
            {
                return InternalServerError(promosOfferServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<PromosOffer>> GetAllPromosOffers()
        {
            try
            {
                IQueryable<PromosOffer> retrievedPromosOffers =
                    this.promosOfferService.RetrieveAllPromosOffers();

                return Ok(retrievedPromosOffers);
            }
            catch (PromosOffersDependencyException promosOfferDependencyException)
            {
                return InternalServerError(promosOfferDependencyException);
            }
            catch (PromosOffersServiceException promosOfferServiceException)
            {
                return InternalServerError(promosOfferServiceException);
            }
        }

        [HttpGet("{promosOfferId}")]
        public async ValueTask<ActionResult<PromosOffer>> GetPromosOfferByIdAsync(Guid promosOfferId)
        {
            try
            {
                PromosOffer promosOffer = await this.promosOfferService.RetrievePromosOfferByIdAsync(promosOfferId);

                return Ok(promosOffer);
            }
            catch (PromosOffersValidationException promosOfferValidationException)
                when (promosOfferValidationException.InnerException is NotFoundPromosOffersException)
            {
                return NotFound(promosOfferValidationException.InnerException);
            }
            catch (PromosOffersValidationException promosOfferValidationException)
            {
                return BadRequest(promosOfferValidationException.InnerException);
            }
            catch (PromosOffersDependencyException promosOfferDependencyException)
            {
                return InternalServerError(promosOfferDependencyException);
            }
            catch (PromosOffersServiceException promosOfferServiceException)
            {
                return InternalServerError(promosOfferServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<PromosOffer>> PutPromosOfferAsync(PromosOffer promosOffer)
        {
            try
            {
                PromosOffer modifiedPromosOffer =
                    await this.promosOfferService.ModifyPromosOfferAsync(promosOffer);

                return Ok(modifiedPromosOffer);
            }
            catch (PromosOffersValidationException promosOfferValidationException)
                when (promosOfferValidationException.InnerException is NotFoundPromosOffersException)
            {
                return NotFound(promosOfferValidationException.InnerException);
            }
            catch (PromosOffersValidationException promosOfferValidationException)
            {
                return BadRequest(promosOfferValidationException.InnerException);
            }
            catch (PromosOffersDependencyValidationException promosOfferValidationException)
                when (promosOfferValidationException.InnerException is InvalidPromosOffersReferenceException)
            {
                return FailedDependency(promosOfferValidationException.InnerException);
            }
            catch (PromosOffersDependencyValidationException promosOfferDependencyValidationException)
               when (promosOfferDependencyValidationException.InnerException is AlreadyExistsPromosOffersException)
            {
                return Conflict(promosOfferDependencyValidationException.InnerException);
            }
            catch (PromosOffersDependencyException promosOfferDependencyException)
            {
                return InternalServerError(promosOfferDependencyException);
            }
            catch (PromosOffersServiceException promosOfferServiceException)
            {
                return InternalServerError(promosOfferServiceException);
            }
        }

        [HttpDelete("{promosOfferId}")]
        public async ValueTask<ActionResult<PromosOffer>> DeletePromosOfferByIdAsync(Guid promosOfferId)
        {
            try
            {
                PromosOffer deletedPromosOffer =
                    await this.promosOfferService.RemovePromosOfferByIdAsync(promosOfferId);

                return Ok(deletedPromosOffer);
            }
            catch (PromosOffersValidationException promosOfferValidationException)
                when (promosOfferValidationException.InnerException is NotFoundPromosOffersException)
            {
                return NotFound(promosOfferValidationException.InnerException);
            }
            catch (PromosOffersValidationException promosOfferValidationException)
            {
                return BadRequest(promosOfferValidationException.InnerException);
            }
            catch (PromosOffersDependencyValidationException promosOfferDependencyValidationException)
                when (promosOfferDependencyValidationException.InnerException is LockedPromosOffersException)
            {
                return Locked(promosOfferDependencyValidationException.InnerException);
            }
            catch (PromosOffersDependencyValidationException promosOfferDependencyValidationException)
            {
                return BadRequest(promosOfferDependencyValidationException);
            }
            catch (PromosOffersDependencyException promosOfferDependencyException)
            {
                return InternalServerError(promosOfferDependencyException);
            }
            catch (PromosOffersServiceException promosOfferServiceException)
            {
                return InternalServerError(promosOfferServiceException);
            }
        }
    }
}
