using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Jaunts.Core.Api.Models.User.Exceptions;
using Jaunts.Core.Api.Services.Foundations.Users;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : RESTFulController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService) =>
            this.userService = userService;

        [HttpPost]
        public async ValueTask<ActionResult<ApplicationUser>> PostUserAsync(ApplicationUser user, string password = "Test123@eri")
        {
            try
            {
                ApplicationUser registeredUser =
                    await this.userService.AddUserAsync(user, password);

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
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ApplicationUser>> GetAllUsers()
        {
            try
            {
                IQueryable storageUsers =
                    this.userService.RetrieveAllUsers();

                return Ok(storageUsers);
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        [HttpGet("{userId}")]
        public async ValueTask<ActionResult<ApplicationUser>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                ApplicationUser storageUser =
                    await this.userService.RetrieveUserByIdAsync(userId);

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
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ApplicationUser>> PutUserAsync(ApplicationUser user)
        {
            try
            {
                ApplicationUser registeredUser =
                    await this.userService.ModifyUserAsync(user);

                return Ok(registeredUser);
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
                when (userDependencyException.InnerException is LockedUserException)
            {
                string innerMessage = GetInnerMessage(userDependencyException);

                return Locked(innerMessage);
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async ValueTask<ActionResult<ApplicationUser>> DeleteUserAsync(Guid userId)
        {
            try
            {
                ApplicationUser storageUser =
                    await this.userService.RemoveUserByIdAsync(userId);

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
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
