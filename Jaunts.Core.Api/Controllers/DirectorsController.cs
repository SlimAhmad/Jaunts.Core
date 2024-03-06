using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;
using Jaunts.Core.Api.Services.Foundations.ProvidersDirectors;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.ProvidersDirectors;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersDirectorsController : RESTFulController
    {
        private readonly IProvidersDirectorService providersDirectorService;

        public ProvidersDirectorsController(IProvidersDirectorService providersDirectorService) =>
            this.providersDirectorService = providersDirectorService;

        [HttpPost]
        public async ValueTask<ActionResult<ProvidersDirector>> PostProvidersDirectorAsync(ProvidersDirector providersDirector)
        {
            try
            {
                ProvidersDirector addedProvidersDirector =
                    await this.providersDirectorService.CreateProvidersDirectorAsync(providersDirector);

                return Created(addedProvidersDirector);
            }
            catch (ProvidersDirectorValidationException ProvidersDirectorValidationException)
            {
                return BadRequest(ProvidersDirectorValidationException.InnerException);
            }
            catch (ProvidersDirectorDependencyValidationException ProvidersDirectorValidationException)
                when (ProvidersDirectorValidationException.InnerException is InvalidProvidersDirectorReferenceException)
            {
                return FailedDependency(ProvidersDirectorValidationException.InnerException);
            }
            catch (ProvidersDirectorDependencyValidationException ProvidersDirectorDependencyValidationException)
               when (ProvidersDirectorDependencyValidationException.InnerException is AlreadyExistsProvidersDirectorException)
            {
                return Conflict(ProvidersDirectorDependencyValidationException.InnerException);
            }
            catch (ProvidersDirectorDependencyException ProvidersDirectorDependencyException)
            {
                return InternalServerError(ProvidersDirectorDependencyException);
            }
            catch (ProvidersDirectorServiceException ProvidersDirectorServiceException)
            {
                return InternalServerError(ProvidersDirectorServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ProvidersDirector>> GetAllProvidersDirectors()
        {
            try
            {
                IQueryable<ProvidersDirector> retrievedProvidersDirectors =
                    this.providersDirectorService.RetrieveAllProvidersDirectors();

                return Ok(retrievedProvidersDirectors);
            }
            catch (ProvidersDirectorDependencyException ProvidersDirectorDependencyException)
            {
                return InternalServerError(ProvidersDirectorDependencyException);
            }
            catch (ProvidersDirectorServiceException ProvidersDirectorServiceException)
            {
                return InternalServerError(ProvidersDirectorServiceException);
            }
        }

        [HttpGet("{ProvidersDirectorId}")]
        public async ValueTask<ActionResult<ProvidersDirector>> GetProvidersDirectorByIdAsync(Guid ProvidersDirectorId)
        {
            try
            {
                ProvidersDirector ProvidersDirector = await this.providersDirectorService.RetrieveProvidersDirectorByIdAsync(ProvidersDirectorId);

                return Ok(ProvidersDirector);
            }
            catch (ProvidersDirectorValidationException ProvidersDirectorValidationException)
                when (ProvidersDirectorValidationException.InnerException is NotFoundProvidersDirectorException)
            {
                return NotFound(ProvidersDirectorValidationException.InnerException);
            }
            catch (ProvidersDirectorValidationException ProvidersDirectorValidationException)
            {
                return BadRequest(ProvidersDirectorValidationException.InnerException);
            }
            catch (ProvidersDirectorDependencyException ProvidersDirectorDependencyException)
            {
                return InternalServerError(ProvidersDirectorDependencyException);
            }
            catch (ProvidersDirectorServiceException ProvidersDirectorServiceException)
            {
                return InternalServerError(ProvidersDirectorServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ProvidersDirector>> PutProvidersDirectorAsync(ProvidersDirector ProvidersDirector)
        {
            try
            {
                ProvidersDirector modifiedProvidersDirector =
                    await this.providersDirectorService.ModifyProvidersDirectorAsync(ProvidersDirector);

                return Ok(modifiedProvidersDirector);
            }
            catch (ProvidersDirectorValidationException ProvidersDirectorValidationException)
                when (ProvidersDirectorValidationException.InnerException is NotFoundProvidersDirectorException)
            {
                return NotFound(ProvidersDirectorValidationException.InnerException);
            }
            catch (ProvidersDirectorValidationException ProvidersDirectorValidationException)
            {
                return BadRequest(ProvidersDirectorValidationException.InnerException);
            }
            catch (ProvidersDirectorDependencyValidationException ProvidersDirectorValidationException)
                when (ProvidersDirectorValidationException.InnerException is InvalidProvidersDirectorReferenceException)
            {
                return FailedDependency(ProvidersDirectorValidationException.InnerException);
            }
            catch (ProvidersDirectorDependencyValidationException ProvidersDirectorDependencyValidationException)
               when (ProvidersDirectorDependencyValidationException.InnerException is AlreadyExistsProvidersDirectorException)
            {
                return Conflict(ProvidersDirectorDependencyValidationException.InnerException);
            }
            catch (ProvidersDirectorDependencyException ProvidersDirectorDependencyException)
            {
                return InternalServerError(ProvidersDirectorDependencyException);
            }
            catch (ProvidersDirectorServiceException ProvidersDirectorServiceException)
            {
                return InternalServerError(ProvidersDirectorServiceException);
            }
        }

        [HttpDelete("{ProvidersDirectorId}")]
        public async ValueTask<ActionResult<ProvidersDirector>> DeleteProvidersDirectorByIdAsync(Guid ProvidersDirectorId)
        {
            try
            {
                ProvidersDirector deletedProvidersDirector =
                    await this.providersDirectorService.RemoveProvidersDirectorByIdAsync(ProvidersDirectorId);

                return Ok(deletedProvidersDirector);
            }
            catch (ProvidersDirectorValidationException ProvidersDirectorValidationException)
                when (ProvidersDirectorValidationException.InnerException is NotFoundProvidersDirectorException)
            {
                return NotFound(ProvidersDirectorValidationException.InnerException);
            }
            catch (ProvidersDirectorValidationException ProvidersDirectorValidationException)
            {
                return BadRequest(ProvidersDirectorValidationException.InnerException);
            }
            catch (ProvidersDirectorDependencyValidationException ProvidersDirectorDependencyValidationException)
                when (ProvidersDirectorDependencyValidationException.InnerException is LockedProvidersDirectorException)
            {
                return Locked(ProvidersDirectorDependencyValidationException.InnerException);
            }
            catch (ProvidersDirectorDependencyValidationException ProvidersDirectorDependencyValidationException)
            {
                return BadRequest(ProvidersDirectorDependencyValidationException);
            }
            catch (ProvidersDirectorDependencyException ProvidersDirectorDependencyException)
            {
                return InternalServerError(ProvidersDirectorDependencyException);
            }
            catch (ProvidersDirectorServiceException ProvidersDirectorServiceException)
            {
                return InternalServerError(ProvidersDirectorServiceException);
            }
        }

    }
}
