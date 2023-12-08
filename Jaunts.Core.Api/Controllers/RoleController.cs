using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Role.Exceptions;
using Jaunts.Core.Api.Services.Foundations.Role;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : RESTFulController
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService) =>
            this.roleService = roleService;

        [HttpPost]
        public async ValueTask<ActionResult<ApplicationRole>> PostRoleAsync(ApplicationRole role)
        {
            try
            {
                ApplicationRole registeredRole =
                    await this.roleService.AddRoleRequestAsync(role);

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
            catch (RoleServiceException roleServiceException)
            {
                return Problem(roleServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ApplicationRole>> GetAllRole()
        {
            try
            {
                IQueryable storageRole =
                    this.roleService.RetrieveAllRoles();

                return Ok(storageRole);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                return Problem(roleDependencyException.Message);
            }
            catch (RoleServiceException roleServiceException)
            {
                return Problem(roleServiceException.Message);
            }
        }

        [HttpGet("{roleId}")]
        public async ValueTask<ActionResult<ApplicationRole>> GetRoleByIdAsync(Guid roleId)
        {
            try
            {
                ApplicationRole storageRole =
                    await this.roleService.RetrieveRoleByIdRequestAsync(roleId);

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
            catch (RoleServiceException roleServiceException)
            {
                return Problem(roleServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ApplicationRole>> PutRoleAsync(ApplicationRole role)
        {
            try
            {
                ApplicationRole registeredRole =
                    await this.roleService.ModifyRoleRequestAsync(role);

                return Ok(registeredRole);
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
                when (roleDependencyException.InnerException is LockedRoleException)
            {
                Exception innerMessage = GetInnerMessage(roleDependencyException);

                return Locked(innerMessage);
            }
            catch (RoleDependencyException roleDependencyException)
            {
                return Problem(roleDependencyException.Message);
            }
            catch (RoleServiceException roleServiceException)
            {
                return Problem(roleServiceException.Message);
            }
        }

        [HttpDelete("{roleId}")]
        public async ValueTask<ActionResult<ApplicationRole>> DeleteRoleAsync(Guid roleId)
        {
            try
            {
                ApplicationRole storageRole =
                    await this.roleService.RemoveRoleByIdRequestAsync(roleId);

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
            catch (RoleServiceException roleServiceException)
            {
                return Problem(roleServiceException.Message);
            }
        }

        private static Exception GetInnerMessage(Exception exception) =>
            exception.InnerException;
    }
}
