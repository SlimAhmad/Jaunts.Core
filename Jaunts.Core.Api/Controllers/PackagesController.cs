using Jaunts.Core.Api.Models.Services.Foundations.Packages.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Packages;
using Jaunts.Core.Api.Services.Foundations.Packages;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Jaunts.Core.Api.Models.Services.Foundations.VacationPackages.Exceptions;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : RESTFulController
    {
        private readonly IPackageService packageService;

        public PackagesController(IPackageService packageService) =>
            this.packageService = packageService;

        [HttpPost]
        public async ValueTask<ActionResult<Package>> PostPackageAsync(Package package)
        {
            try
            {
                Package addedPackage =
                    await this.packageService.CreatePackageAsync(package);

                return Created(addedPackage);
            }
            catch (PackageValidationException packageValidationException)
            {
                return BadRequest(packageValidationException.InnerException);
            }
            catch (PackageDependencyValidationException packageValidationException)
                when (packageValidationException.InnerException is InvalidPackageReferenceException)
            {
                return FailedDependency(packageValidationException.InnerException);
            }
            catch (PackageDependencyValidationException packageDependencyValidationException)
               when (packageDependencyValidationException.InnerException is AlreadyExistsPackageException)
            {
                return Conflict(packageDependencyValidationException.InnerException);
            }
            catch (PackageDependencyException packageDependencyException)
            {
                return InternalServerError(packageDependencyException);
            }
            catch (PackageServiceException packageServiceException)
            {
                return InternalServerError(packageServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Package>> GetAllPackages()
        {
            try
            {
                IQueryable<Package> retrievedPackages =
                    this.packageService.RetrieveAllPackages();

                return Ok(retrievedPackages);
            }
            catch (PackageDependencyException packageDependencyException)
            {
                return InternalServerError(packageDependencyException);
            }
            catch (PackageServiceException packageServiceException)
            {
                return InternalServerError(packageServiceException);
            }
        }

        [HttpGet("{packageId}")]
        public async ValueTask<ActionResult<Package>> GetPackageByIdAsync(Guid packageId)
        {
            try
            {
                Package package = await this.packageService.RetrievePackageByIdAsync(packageId);

                return Ok(package);
            }
            catch (PackageValidationException packageValidationException)
                when (packageValidationException.InnerException is NotFoundPackageException)
            {
                return NotFound(packageValidationException.InnerException);
            }
            catch (PackageValidationException packageValidationException)
            {
                return BadRequest(packageValidationException.InnerException);
            }
            catch (PackageDependencyException packageDependencyException)
            {
                return InternalServerError(packageDependencyException);
            }
            catch (PackageServiceException packageServiceException)
            {
                return InternalServerError(packageServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Package>> PutPackageAsync(Package package)
        {
            try
            {
                Package modifiedPackage =
                    await this.packageService.ModifyPackageAsync(package);

                return Ok(modifiedPackage);
            }
            catch (PackageValidationException packageValidationException)
                when (packageValidationException.InnerException is NotFoundPackageException)
            {
                return NotFound(packageValidationException.InnerException);
            }
            catch (PackageValidationException packageValidationException)
            {
                return BadRequest(packageValidationException.InnerException);
            }
            catch (PackageDependencyValidationException packageValidationException)
                when (packageValidationException.InnerException is InvalidPackageReferenceException)
            {
                return FailedDependency(packageValidationException.InnerException);
            }
            catch (PackageDependencyValidationException packageDependencyValidationException)
               when (packageDependencyValidationException.InnerException is AlreadyExistsPackageException)
            {
                return Conflict(packageDependencyValidationException.InnerException);
            }
            catch (PackageDependencyException packageDependencyException)
            {
                return InternalServerError(packageDependencyException);
            }
            catch (PackageServiceException packageServiceException)
            {
                return InternalServerError(packageServiceException);
            }
        }

        [HttpDelete("{packageId}")]
        public async ValueTask<ActionResult<Package>> DeletePackageByIdAsync(Guid packageId)
        {
            try
            {
                Package deletedPackage =
                    await this.packageService.RemovePackageByIdAsync(packageId);

                return Ok(deletedPackage);
            }
            catch (PackageValidationException packageValidationException)
                when (packageValidationException.InnerException is NotFoundPackageException)
            {
                return NotFound(packageValidationException.InnerException);
            }
            catch (PackageValidationException packageValidationException)
            {
                return BadRequest(packageValidationException.InnerException);
            }
            catch (PackageDependencyValidationException packageDependencyValidationException)
                when (packageDependencyValidationException.InnerException is LockedPackageException)
            {
                return Locked(packageDependencyValidationException.InnerException);
            }
            catch (PackageDependencyValidationException packageDependencyValidationException)
            {
                return BadRequest(packageDependencyValidationException);
            }
            catch (PackageDependencyException packageDependencyException)
            {
                return InternalServerError(packageDependencyException);
            }
            catch (PackageServiceException packageServiceException)
            {
                return InternalServerError(packageServiceException);
            }
        }
    }
}
