using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Foundations.Auth.Exceptions;
using Jaunts.Core.Api.Services.Foundations.Auth;
using Jaunts.Core.Models.Auth.LoginRegister;
using Jaunts.Core.Routes;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    public class AuthController : RESTFulController
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService) =>
            this.authService = authService;

        [HttpPost]
        [Route(ApiRoutes.Register)] 
        public async ValueTask<ActionResult<RegisterResultApiResponse>> RegisterAsync(
            RegisterUserApiRequest registerUserApiRequest)
        {
            try
            {
                RegisterResultApiResponse registeredAuth =
                    await this.authService.RegisterUserRequestAsync(registerUserApiRequest);

                return Created(registeredAuth);
            }
            catch (AuthValidationException authValidationException)
                when (authValidationException.InnerException is AlreadyExistsAuthException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return Conflict(innerMessage);
            }
            catch (AuthValidationException authValidationException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return BadRequest(authValidationException.InnerException);
            }
            catch (AuthDependencyException authDependencyException)
            {
                return Problem(authDependencyException.Message);
            }
            catch (AuthServiceException authServiceException)
            {
                return Problem(authServiceException.Message);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.Login)]
        public async ValueTask<ActionResult<UserProfileDetailsApiResponse>> LoginAsync(
            LoginCredentialsApiRequest  loginCredentialsApiRequest)
        {
            try
            {
                UserProfileDetailsApiResponse registeredAuth =
                    await this.authService.LogInRequestAsync(loginCredentialsApiRequest);

                return Created(registeredAuth);
            }
            catch (AuthValidationException authValidationException)
                when (authValidationException.InnerException is AlreadyExistsAuthException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return Conflict(innerMessage);
            }
            catch (AuthValidationException authValidationException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return BadRequest(authValidationException.InnerException);
            }
            catch (AuthDependencyException authDependencyException)
            {
                return Problem(authDependencyException.Message);
            }
            catch (AuthServiceException authServiceException)
            {
                return Problem(authServiceException.Message);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.ResetPassword)]
        public async ValueTask<ActionResult<ResetPasswordApiResponse>> ResetPasswordAsync(ResetPasswordApiRequest resetPasswordApiRequest)
        {
            try
            {
                ResetPasswordApiResponse registeredAuth =
                    await this.authService.ResetPasswordRequestAsync(resetPasswordApiRequest);

                return Created(registeredAuth);
            }
            catch (AuthValidationException authValidationException)
                when (authValidationException.InnerException is AlreadyExistsAuthException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return Conflict(innerMessage);
            }
            catch (AuthValidationException authValidationException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return BadRequest(authValidationException.InnerException);
            }
            catch (AuthDependencyException authDependencyException)
            {
                return Problem(authDependencyException.Message);
            }
            catch (AuthServiceException authServiceException)
            {
                return Problem(authServiceException.Message);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.ForgotPassword)]
        public async ValueTask<ActionResult<ResetPasswordApiResponse>> ForgotPasswordAsync(string email)
        {
            try
            {
                ForgotPasswordApiResponse registeredAuth =
                    await this.authService.ForgotPasswordRequestAsync(email);

                return Created(registeredAuth);
            }
            catch (AuthValidationException authValidationException)
                when (authValidationException.InnerException is AlreadyExistsAuthException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return Conflict(innerMessage);
            }
            catch (AuthValidationException authValidationException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return BadRequest(authValidationException.InnerException);
            }
            catch (AuthDependencyException authDependencyException)
            {
                return Problem(authDependencyException.Message);
            }
            catch (AuthServiceException authServiceException)
            {
                return Problem(authServiceException.Message);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.ConfirmEmail)]
        public async ValueTask<ActionResult<ResetPasswordApiResponse>> ConfirmEmailAsync(string token,string email)
        {
            try
            {
                UserProfileDetailsApiResponse registeredAuth =
                    await this.authService.ConfirmEmailRequestAsync(token,email);

                return Created(registeredAuth);
            }
            catch (AuthValidationException authValidationException)
                when (authValidationException.InnerException is AlreadyExistsAuthException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return Conflict(innerMessage);
            }
            catch (AuthValidationException authValidationException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return BadRequest(authValidationException.InnerException);
            }
            catch (AuthDependencyException authDependencyException)
            {
                return Problem(authDependencyException.Message);
            }
            catch (AuthServiceException authServiceException)
            {
                return Problem(authServiceException.Message);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.LoginWithOTP)]
        public async ValueTask<ActionResult<ResetPasswordApiResponse>> LoginWithOTPAsync(string code, string userNameOrEmail)
        {
            try
            {
                UserProfileDetailsApiResponse registeredAuth =
                    await this.authService.LoginWithOTPRequestAsync(code,userNameOrEmail);

                return Created(registeredAuth);
            }
            catch (AuthValidationException authValidationException)
                when (authValidationException.InnerException is AlreadyExistsAuthException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return Conflict(innerMessage);
            }
            catch (AuthValidationException authValidationException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return BadRequest(authValidationException.InnerException);
            }
            catch (AuthDependencyException authDependencyException)
            {
                return Problem(authDependencyException.Message);
            }
            catch (AuthServiceException authServiceException)
            {
                return Problem(authServiceException.Message);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.Enable2FA)]
        public async ValueTask<ActionResult<Enable2FAApiResponse>> Enable2FAAsync(Guid id)
        {
            try
            {
                Enable2FAApiResponse registeredAuth =
                    await this.authService.EnableUser2FARequestAsync(id);

                return Created(registeredAuth);
            }
            catch (AuthValidationException authValidationException)
                when (authValidationException.InnerException is AlreadyExistsAuthException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return Conflict(innerMessage);
            }
            catch (AuthValidationException authValidationException)
            {
                string innerMessage = GetInnerMessage(authValidationException);

                return BadRequest(authValidationException.InnerException);
            }
            catch (AuthDependencyException authDependencyException)
            {
                return Problem(authDependencyException.Message);
            }
            catch (AuthServiceException authServiceException)
            {
                return Problem(authServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
