using Jaunts.Core.Api.Models.Services.Foundations.Providers.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Providers;
using Jaunts.Core.Api.Services.Foundations.Providers;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : RESTFulController
    {
        private readonly IProviderService providerService;

        public ProvidersController(IProviderService providerService) =>
            this.providerService = providerService;

        [HttpPost]
        public async ValueTask<ActionResult<Provider>> PostProviderAsync(Provider provider)
        {
            try
            {
                Provider addedProvider =
                    await this.providerService.CreateProviderAsync(provider);

                return Created(addedProvider);
            }
            catch (ProviderValidationException providerValidationException)
            {
                return BadRequest(providerValidationException.InnerException);
            }
            catch (ProviderDependencyValidationException providerValidationException)
                when (providerValidationException.InnerException is InvalidProviderReferenceException)
            {
                return FailedDependency(providerValidationException.InnerException);
            }
            catch (ProviderDependencyValidationException providerDependencyValidationException)
               when (providerDependencyValidationException.InnerException is AlreadyExistsProviderException)
            {
                return Conflict(providerDependencyValidationException.InnerException);
            }
            catch (ProviderDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Provider>> GetAllProviders()
        {
            try
            {
                IQueryable<Provider> retrievedProviders =
                    this.providerService.RetrieveAllProviders();

                return Ok(retrievedProviders);
            }
            catch (ProviderDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }

        [HttpGet("{providerId}")]
        public async ValueTask<ActionResult<Provider>> GetProviderByIdAsync(Guid providerId)
        {
            try
            {
                Provider provider = await this.providerService.RetrieveProviderByIdAsync(providerId);

                return Ok(provider);
            }
            catch (ProviderValidationException providerValidationException)
                when (providerValidationException.InnerException is NotFoundProviderException)
            {
                return NotFound(providerValidationException.InnerException);
            }
            catch (ProviderValidationException providerValidationException)
            {
                return BadRequest(providerValidationException.InnerException);
            }
            catch (ProviderDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Provider>> PutProviderAsync(Provider provider)
        {
            try
            {
                Provider modifiedProvider =
                    await this.providerService.ModifyProviderAsync(provider);

                return Ok(modifiedProvider);
            }
            catch (ProviderValidationException providerValidationException)
                when (providerValidationException.InnerException is NotFoundProviderException)
            {
                return NotFound(providerValidationException.InnerException);
            }
            catch (ProviderValidationException providerValidationException)
            {
                return BadRequest(providerValidationException.InnerException);
            }
            catch (ProviderDependencyValidationException providerValidationException)
                when (providerValidationException.InnerException is InvalidProviderReferenceException)
            {
                return FailedDependency(providerValidationException.InnerException);
            }
            catch (ProviderDependencyValidationException providerDependencyValidationException)
               when (providerDependencyValidationException.InnerException is AlreadyExistsProviderException)
            {
                return Conflict(providerDependencyValidationException.InnerException);
            }
            catch (ProviderDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }

        [HttpDelete("{providerId}")]
        public async ValueTask<ActionResult<Provider>> DeleteProviderByIdAsync(Guid providerId)
        {
            try
            {
                Provider deletedProvider =
                    await this.providerService.RemoveProviderByIdAsync(providerId);

                return Ok(deletedProvider);
            }
            catch (ProviderValidationException providerValidationException)
                when (providerValidationException.InnerException is NotFoundProviderException)
            {
                return NotFound(providerValidationException.InnerException);
            }
            catch (ProviderValidationException providerValidationException)
            {
                return BadRequest(providerValidationException.InnerException);
            }
            catch (ProviderDependencyValidationException providerDependencyValidationException)
                when (providerDependencyValidationException.InnerException is LockedProviderException)
            {
                return Locked(providerDependencyValidationException.InnerException);
            }
            catch (ProviderDependencyValidationException providerDependencyValidationException)
            {
                return BadRequest(providerDependencyValidationException);
            }
            catch (ProviderDependencyException providerDependencyException)
            {
                return InternalServerError(providerDependencyException);
            }
            catch (ProviderServiceException providerServiceException)
            {
                return InternalServerError(providerServiceException);
            }
        }
    }
}
