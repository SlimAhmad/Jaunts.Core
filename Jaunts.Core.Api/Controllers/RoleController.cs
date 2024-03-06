using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Services.Foundations.Role;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Jaunts.Core.Api.Services.Processings.Role;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : RESTFulController
    {
        private readonly IRoleProcessingService roleProcessingService;

        public RoleController(IRoleProcessingService roleProcessingService) =>
            this.roleProcessingService = roleProcessingService;

        [HttpPost]
        public async ValueTask<ActionResult<ApplicationRole>> PostRoleAsync(ApplicationRole role)
        {
            try
            {
                ApplicationRole registeredRole =
                    await this.roleProcessingService.UpsertRoleAsync(role);

                return Created(registeredRole);
            }
            catch (RoleValidationException roleValidationException)
                when (roleValidationException.InnerException is AlreadyExistsRoleException)
            {
                Exception innerMessage = GetInnerMessage(roleValidationException);

                return Conflict(innerMessage);
            }
            catch (RoleValidationException roleValidationException)
            {
                Exception innerMessage = GetInnerMessage(roleValidationException);

                return BadRequest(innerMessage);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                return Problem(roleDependencyException.Message);
            }
            catch (RoleServiceException roleProcessingServiceException)
            {
                return Problem(roleProcessingServiceException.Message);
            }
        }
        [HttpGet]
        public ActionResult<IQueryable<ApplicationRole>> GetAllRole()
        {
            try
            {
                IQueryable storageRole =
                    this.roleProcessingService.RetrieveAllRoles();

                return Ok(storageRole);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                return Problem(roleDependencyException.Message);
            }
            catch (RoleServiceException roleProcessingServiceException)
            {
                return Problem(roleProcessingServiceException.Message);
            }
        }
        [HttpGet("{roleId}")]
        public async ValueTask<ActionResult<ApplicationRole>> GetRoleByIdAsync(Guid roleId)
        {
            try
            {
                ApplicationRole storageRole =
                    await this.roleProcessingService.RetrieveRoleByIdAsync(roleId);

                return Ok(storageRole);
            }
            catch (RoleValidationException roleValidationException)
                when (roleValidationException.InnerException is NotFoundRoleException)
            {
                Exception innerMessage = GetInnerMessage(roleValidationException);

                return NotFound(innerMessage);
            }
            catch (RoleValidationException roleValidationException)
            {
                Exception innerMessage = GetInnerMessage(roleValidationException);

                return BadRequest(innerMessage);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                return Problem(roleDependencyException.Message);
            }
            catch (RoleServiceException roleProcessingServiceException)
            {
                return Problem(roleProcessingServiceException.Message);
            }
        }
        [HttpDelete("{roleId}")]
        public async ValueTask<ActionResult<bool>> DeleteRoleAsync(Guid roleId)
        {
            try
            {
                bool response =
                    await this.roleProcessingService.RemoveRoleByIdAsync(roleId);

                return Ok(response);
            }
            catch (RoleValidationException roleValidationException)
                when (roleValidationException.InnerException is NotFoundRoleException)
            {
                Exception innerMessage = GetInnerMessage(roleValidationException);

                return NotFound(innerMessage);
            }
            catch (RoleValidationException roleValidationException)
            {
                return BadRequest(roleValidationException.Message);
            }
            catch (RoleDependencyException roleDependencyException)
                when (roleDependencyException.InnerException is LockedRoleException)
            {
                Exception innerMessage = GetInnerMessage(roleDependencyException);

                return Locked(innerMessage);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                return Problem(roleDependencyException.Message);
            }
            catch (RoleServiceException roleProcessingServiceException)
            {
                return Problem(roleProcessingServiceException.Message);
            }
        }

        private static Exception GetInnerMessage(Exception exception) =>
            exception.InnerException;
    }
}
