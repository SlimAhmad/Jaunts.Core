using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategory;
using Jaunts.Core.Api.Models.Services.Foundations.ProviderCategorys.Exceptions;
using Jaunts.Core.Api.Services.Foundations.ProviderCategories;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : RESTFulController
    {
        private readonly IProviderCategoryService providerCategoryService;

        public CategoriesController(IProviderCategoryService providerCategoryService) =>
            this.providerCategoryService = providerCategoryService;

        [HttpPost]
        public async ValueTask<ActionResult<ProviderCategory>> PostProviderCategoryAsync(ProviderCategory providerCategory)
        {
            try
            {
                ProviderCategory addedProviderCategory =
                    await this.providerCategoryService.CreateProviderCategoryAsync(providerCategory);

                return Created(addedProviderCategory);
            }
            catch (ProviderCategoryValidationException providerCategoryValidationException)
            {
                return BadRequest(providerCategoryValidationException.InnerException);
            }
            catch (ProviderCategoryDependencyValidationException providerCategoryValidationException)
                when (providerCategoryValidationException.InnerException is InvalidProviderCategoryReferenceException)
            {
                return FailedDependency(providerCategoryValidationException.InnerException);
            }
            catch (ProviderCategoryDependencyValidationException providerCategoryDependencyValidationException)
               when (providerCategoryDependencyValidationException.InnerException is AlreadyExistsProviderCategoryException)
            {
                return Conflict(providerCategoryDependencyValidationException.InnerException);
            }
            catch (ProviderCategoryDependencyException providerCategoryDependencyException)
            {
                return InternalServerError(providerCategoryDependencyException);
            }
            catch (ProviderCategoryServiceException providerCategoryServiceException)
            {
                return InternalServerError(providerCategoryServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ProviderCategory>> GetAllCategories()
        {
            try
            {
                IQueryable<ProviderCategory> retrievedCategories =
                    this.providerCategoryService.RetrieveAllProviderCategories
                    ();

                return Ok(retrievedCategories);
            }
            catch (ProviderCategoryDependencyException providerCategoryDependencyException)
            {
                return InternalServerError(providerCategoryDependencyException);
            }
            catch (ProviderCategoryServiceException providerCategoryServiceException)
            {
                return InternalServerError(providerCategoryServiceException);
            }
        }

        [HttpGet("{providerCategoryId}")]
        public async ValueTask<ActionResult<ProviderCategory>> GetProviderCategoryByIdAsync(Guid providerCategoryId)
        {
            try
            {
                ProviderCategory providerCategory = await this.providerCategoryService.RetrieveProviderCategoryByIdAsync(providerCategoryId);

                return Ok(providerCategory);
            }
            catch (ProviderCategoryValidationException providerCategoryValidationException)
                when (providerCategoryValidationException.InnerException is NotFoundProviderCategoryException)
            {
                return NotFound(providerCategoryValidationException.InnerException);
            }
            catch (ProviderCategoryValidationException providerCategoryValidationException)
            {
                return BadRequest(providerCategoryValidationException.InnerException);
            }
            catch (ProviderCategoryDependencyException providerCategoryDependencyException)
            {
                return InternalServerError(providerCategoryDependencyException);
            }
            catch (ProviderCategoryServiceException providerCategoryServiceException)
            {
                return InternalServerError(providerCategoryServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ProviderCategory>> PutProviderCategoryAsync(ProviderCategory providerCategory)
        {
            try
            {
                ProviderCategory modifiedProviderCategory =
                    await this.providerCategoryService.ModifyProviderCategoryAsync(providerCategory);

                return Ok(modifiedProviderCategory);
            }
            catch (ProviderCategoryValidationException providerCategoryValidationException)
                when (providerCategoryValidationException.InnerException is NotFoundProviderCategoryException)
            {
                return NotFound(providerCategoryValidationException.InnerException);
            }
            catch (ProviderCategoryValidationException providerCategoryValidationException)
            {
                return BadRequest(providerCategoryValidationException.InnerException);
            }
            catch (ProviderCategoryDependencyValidationException providerCategoryValidationException)
                when (providerCategoryValidationException.InnerException is InvalidProviderCategoryReferenceException)
            {
                return FailedDependency(providerCategoryValidationException.InnerException);
            }
            catch (ProviderCategoryDependencyValidationException providerCategoryDependencyValidationException)
               when (providerCategoryDependencyValidationException.InnerException is AlreadyExistsProviderCategoryException)
            {
                return Conflict(providerCategoryDependencyValidationException.InnerException);
            }
            catch (ProviderCategoryDependencyException providerCategoryDependencyException)
            {
                return InternalServerError(providerCategoryDependencyException);
            }
            catch (ProviderCategoryServiceException providerCategoryServiceException)
            {
                return InternalServerError(providerCategoryServiceException);
            }
        }

        [HttpDelete("{providerCategoryId}")]
        public async ValueTask<ActionResult<ProviderCategory>> DeleteProviderCategoryByIdAsync(Guid providerCategoryId)
        {
            try
            {
                ProviderCategory deletedProviderCategory =
                    await this.providerCategoryService.RemoveProviderCategoryByIdAsync(providerCategoryId);

                return Ok(deletedProviderCategory);
            }
            catch (ProviderCategoryValidationException providerCategoryValidationException)
                when (providerCategoryValidationException.InnerException is NotFoundProviderCategoryException)
            {
                return NotFound(providerCategoryValidationException.InnerException);
            }
            catch (ProviderCategoryValidationException providerCategoryValidationException)
            {
                return BadRequest(providerCategoryValidationException.InnerException);
            }
            catch (ProviderCategoryDependencyValidationException providerCategoryDependencyValidationException)
                when (providerCategoryDependencyValidationException.InnerException is LockedProviderCategoryException)
            {
                return Locked(providerCategoryDependencyValidationException.InnerException);
            }
            catch (ProviderCategoryDependencyValidationException providerCategoryDependencyValidationException)
            {
                return BadRequest(providerCategoryDependencyValidationException);
            }
            catch (ProviderCategoryDependencyException providerCategoryDependencyException)
            {
                return InternalServerError(providerCategoryDependencyException);
            }
            catch (ProviderCategoryServiceException providerCategoryServiceException)
            {
                return InternalServerError(providerCategoryServiceException);
            }
        }
    }
}
