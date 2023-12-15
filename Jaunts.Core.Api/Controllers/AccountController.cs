using Jaunts.Core.Api.Models.Auth;
using Jaunts.Core.Api.Models.Services.Orchestration.Account.Exceptions;
using Jaunts.Core.Api.Services.Aggregations.Account;
using Jaunts.Core.Models.Auth.LoginRegister;
using Jaunts.Core.Routes;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    public class AccountController : RESTFulController
    {
        private readonly IAccountAggregationService accountAggregationService;

        public AccountController(IAccountAggregationService accountAggregationService) =>
            this.accountAggregationService = accountAggregationService;

        [HttpPost]
        [Route(ApiRoutes.Register)] 
        public async ValueTask<ActionResult<UserAccountDetailsResponse>> RegisterAsync(
            RegisterUserApiRequest registerUserApiRequest)
        {
            try
            {
                UserAccountDetailsResponse registeredAccount =
                    await this.accountAggregationService.RegisterUserRequestAsync(registerUserApiRequest);

                return Created(registeredAccount);
            }
            catch (AccountOrchestrationDependencyValidationException accountOrchestrationDependencyValidationException)
            {
                return BadRequest(accountOrchestrationDependencyValidationException.InnerException);
            }
            catch (AccountOrchestrationDependencyException accountOrchestrationDependencyException)
            {
                return InternalServerError(accountOrchestrationDependencyException);
            }
            catch (AccountOrchestrationServiceException accountOrchestrationServiceException)
            {
                return InternalServerError(accountOrchestrationServiceException);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.Login)]
        public async ValueTask<ActionResult<UserAccountDetailsResponse>> LoginAsync(
            LoginCredentialsApiRequest  loginCredentialsApiRequest)
        {
            try
            {
                UserAccountDetailsResponse registeredAccount =
                    await this.accountAggregationService.LogInRequestAsync(loginCredentialsApiRequest);

                return Created(registeredAccount);
            }
            catch (AccountOrchestrationDependencyValidationException accountOrchestrationDependencyValidationException)
            {
                return BadRequest(accountOrchestrationDependencyValidationException.InnerException);
            }
            catch (AccountOrchestrationDependencyException accountOrchestrationDependencyException)
            {
                return InternalServerError(accountOrchestrationDependencyException);
            }
            catch (AccountOrchestrationServiceException accountOrchestrationServiceException)
            {
                return InternalServerError(accountOrchestrationServiceException);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.ResetPassword)]
        public async ValueTask<ActionResult<bool>> ResetPasswordAsync(ResetPasswordApiRequest resetPasswordApiRequest)
        {
            try
            {
                bool registeredAccount =
                    await this.accountAggregationService.ResetPasswordRequestAsync(resetPasswordApiRequest);

                return Ok(registeredAccount);
            }
            catch (AccountOrchestrationDependencyValidationException accountOrchestrationDependencyValidationException)
            {
                return BadRequest(accountOrchestrationDependencyValidationException.InnerException);
            }
            catch (AccountOrchestrationDependencyException accountOrchestrationDependencyException)
            {
                return InternalServerError(accountOrchestrationDependencyException);
            }
            catch (AccountOrchestrationServiceException accountOrchestrationServiceException)
            {
                return InternalServerError(accountOrchestrationServiceException);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.ForgotPassword)]
        public async ValueTask<ActionResult<bool>> ForgotPasswordAsync(string email)
        {
            try
            {
                bool registeredAccount =
                    await this.accountAggregationService.ForgotPasswordRequestAsync(email);

                return Created(registeredAccount);
            }
            catch (AccountOrchestrationDependencyValidationException accountOrchestrationDependencyValidationException)
            {
                return BadRequest(accountOrchestrationDependencyValidationException.InnerException);
            }
            catch (AccountOrchestrationDependencyException accountOrchestrationDependencyException)
            {
                return InternalServerError(accountOrchestrationDependencyException);
            }
            catch (AccountOrchestrationServiceException accountOrchestrationServiceException)
            {
                return InternalServerError(accountOrchestrationServiceException);
            }

         }

            [HttpPost]
        [Route(ApiRoutes.ConfirmEmail)]
        public async ValueTask<ActionResult<UserAccountDetailsResponse>> ConfirmEmailAsync(string token,string email)
        {
            try
            {
                UserAccountDetailsResponse registeredAccount =
                    await this.accountAggregationService.ConfirmEmailRequestAsync(token,email);

                return Ok(registeredAccount);
            }
            catch (AccountOrchestrationDependencyValidationException accountOrchestrationDependencyValidationException)
            {
                return BadRequest(accountOrchestrationDependencyValidationException.InnerException);
            }
            catch (AccountOrchestrationDependencyException accountOrchestrationDependencyException)
            {
                return InternalServerError(accountOrchestrationDependencyException);
            }
            catch (AccountOrchestrationServiceException accountOrchestrationServiceException)
            {
                return InternalServerError(accountOrchestrationServiceException);
            }
        }

        [HttpPost]
        [Route(ApiRoutes.LoginWithOTP)]
        public async ValueTask<ActionResult<UserAccountDetailsResponse>> LoginWithOTPAsync(string code, string userNameOrEmail)
        {
            try
            {
                UserAccountDetailsResponse registeredAccount =
                    await this.accountAggregationService.LoginWithOTPRequestAsync(code,userNameOrEmail);

                return Created(registeredAccount);
            }
            catch (AccountOrchestrationDependencyValidationException accountOrchestrationDependencyValidationException)
            {
                return BadRequest(accountOrchestrationDependencyValidationException.InnerException);
            }
            catch (AccountOrchestrationDependencyException accountOrchestrationDependencyException)
            {
                return InternalServerError(accountOrchestrationDependencyException);
            }
            catch (AccountOrchestrationServiceException accountOrchestrationServiceException)
            {
                return InternalServerError(accountOrchestrationServiceException);
            }


        }

        [HttpPost]
        [Route(ApiRoutes.Enable2FA)]
        public async ValueTask<ActionResult<UserAccountDetailsResponse>> Enable2FAAsync(Guid id)
        {
            try
            {
                UserAccountDetailsResponse registeredAccount =
                    await this.accountAggregationService.EnableUser2FARequestAsync(id);

                return Ok(registeredAccount);
            }
            catch (AccountOrchestrationDependencyValidationException accountOrchestrationDependencyValidationException)
            {
                return BadRequest(accountOrchestrationDependencyValidationException.InnerException);
            }
            catch (AccountOrchestrationDependencyException accountOrchestrationDependencyException)
            {
                return InternalServerError(accountOrchestrationDependencyException);
            }
            catch (AccountOrchestrationServiceException accountOrchestrationServiceException)
            {
                return InternalServerError(accountOrchestrationServiceException);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
