using Jaunts.Core.Api.Models.Processings.User.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Jaunts.Core.Api.Services.Foundations.Users;
using Jaunts.Core.Api.Services.Processings.User;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : RESTFulController
    {
        private readonly IUserProcessingService userProcessingService;

        public UsersController(IUserProcessingService userProcessingService) =>
            this.userProcessingService = userProcessingService;

        [HttpPost]
        public async ValueTask<ActionResult<ApplicationUser>> PostUserAsync(ApplicationUser user, string password = "Test123@eri")
        {
            try
            {
                ApplicationUser registeredUser =
                    await this.userProcessingService.UpsertUserAsync(user, password);

                return Created(registeredUser);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is AlreadyExistsUserException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return Conflict(innerMessage);
            }
            catch (UserValidationException userValidationException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return BadRequest(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserServiceException userProcessingServiceException)
            {
                return InternalServerError(userProcessingServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ApplicationUser>> GetAllUsers()
        {
            try
            {
                IQueryable storageUsers =
                    this.userProcessingService.RetrieveAllUsers();

                return Ok(storageUsers);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserProcessingServiceException userProcessingServiceException)
            {
                return InternalServerError(userProcessingServiceException);
            }
        }

        [HttpGet("{userId}")]
        public async ValueTask<ActionResult<ApplicationUser>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                ApplicationUser storageUser =
                    await this.userProcessingService.RetrieveUserById(userId);

                return Ok(storageUser);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is NotFoundUserException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return NotFound(innerMessage);
            }
            catch (UserValidationException userValidationException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return BadRequest(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserProcessingServiceException userProcessingServiceException)
            {
                return InternalServerError(userProcessingServiceException);
            }
        }

        [HttpDelete("{userId}")]
        public async ValueTask<ActionResult<bool>> DeleteUserAsync(Guid userId)
        {
            try
            {
                bool storageUser =
                    await this.userProcessingService.RemoveUserByIdAsync(userId);

                return Ok(storageUser);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is NotFoundUserException)
            {
                string innerMessage = GetInnerMessage(userValidationException);

                return NotFound(innerMessage);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException.Message);
            }
            catch (UserDependencyException userDependencyException)
                when (userDependencyException.InnerException is LockedUserException)
            {
                string innerMessage = GetInnerMessage(userDependencyException);

                return Locked(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserProcessingServiceException userProcessingServiceException)
            {
                return InternalServerError(userProcessingServiceException);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
