using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderServices;
using Jaunts.Core.Api.Services.Foundations.ProviderServices;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderServicesController : RESTFulController
    {
        private readonly IProviderServiceService providerService;

        public ProviderServicesController(IProviderServiceService providerService) =>
            this.providerService = providerService;

        [HttpPost]
        public async ValueTask<ActionResult<ProviderService>> PostProviderServiceAsync(ProviderService provider)
        {
            try
            {
                ProviderService addedProviderService =
                    await this.providerService.CreateProviderServiceAsync(provider);

                return Created(addedProviderService);
            }
            catch (ProviderServiceValidationException providerValidationException)
            {
                return BadRequest(providerValidationException.InnerException);
            }
            catch (ProviderServiceDependencyValidationException providerValidationException)
                when (providerValidationException.InnerException is InvalidProviderServiceReferenceException)
            {
                return FailedDependency(providerValidationException.InnerException);
            }
            catch (ProviderServiceDependencyValidationException providerDependencyValidationException)
               when (providerDependencyValidationException.InnerException is AlreadyExistsProviderServiceException)
            {
                return Conflict(providerDependencyValidationException.InnerException);
            }
            catch (ProviderServiceDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ProviderService>> GetAllProviderServices()
        {
            try
            {
                IQueryable<ProviderService> retrievedProviderServices =
                    this.providerService.RetrieveAllProviderServices();

                return Ok(retrievedProviderServices);
            }
            catch (ProviderServiceDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }

        [HttpGet("{providerId}")]
        public async ValueTask<ActionResult<ProviderService>> GetProviderServiceByIdAsync(Guid providerId)
        {
            try
            {
                ProviderService provider = await this.providerService.RetrieveProviderServiceByIdAsync(providerId);

                return Ok(provider);
            }
            catch (ProviderServiceValidationException providerValidationException)
                when (providerValidationException.InnerException is NotFoundProviderServiceException)
            {
                return NotFound(providerValidationException.InnerException);
            }
            catch (ProviderServiceValidationException providerValidationException)
            {
                return BadRequest(providerValidationException.InnerException);
            }
            catch (ProviderServiceDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ProviderService>> PutProviderServiceAsync(ProviderService provider)
        {
            try
            {
                ProviderService modifiedProviderService =
                    await this.providerService.ModifyProviderServiceAsync(provider);

                return Ok(modifiedProviderService);
            }
            catch (ProviderServiceValidationException providerValidationException)
                when (providerValidationException.InnerException is NotFoundProviderServiceException)
            {
                return NotFound(providerValidationException.InnerException);
            }
            catch (ProviderServiceValidationException providerValidationException)
            {
                return BadRequest(providerValidationException.InnerException);
            }
            catch (ProviderServiceDependencyValidationException providerValidationException)
                when (providerValidationException.InnerException is InvalidProviderServiceReferenceException)
            {
                return FailedDependency(providerValidationException.InnerException);
            }
            catch (ProviderServiceDependencyValidationException providerDependencyValidationException)
               when (providerDependencyValidationException.InnerException is AlreadyExistsProviderServiceException)
            {
                return Conflict(providerDependencyValidationException.InnerException);
            }
            catch (ProviderServiceDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }

        [HttpDelete("{providerId}")]
        public async ValueTask<ActionResult<ProviderService>> DeleteProviderServiceByIdAsync(Guid providerId)
        {
            try
            {
                ProviderService deletedProviderService =
                    await this.providerService.RemoveProviderServiceByIdAsync(providerId);

                return Ok(deletedProviderService);
            }
            catch (ProviderServiceValidationException providerValidationException)
                when (providerValidationException.InnerException is NotFoundProviderServiceException)
            {
                return NotFound(providerValidationException.InnerException);
            }
            catch (ProviderServiceValidationException providerValidationException)
            {
                return BadRequest(providerValidationException.InnerException);
            }
            catch (ProviderServiceDependencyValidationException providerDependencyValidationException)
                when (providerDependencyValidationException.InnerException is LockedProviderServiceException)
            {
                return Locked(providerDependencyValidationException.InnerException);
            }
            catch (ProviderServiceDependencyValidationException providerDependencyValidationException)
            {
                return BadRequest(providerDependencyValidationException);
            }
            catch (ProviderServiceDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }
    }
}
