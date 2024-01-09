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
            [FromBody] UserCredentialsRequest registerUserApiRequest)
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
           [FromBody] LoginRequest loginApiRequest)
        {
            try
            {
                UserAccountDetailsResponse registeredAccount =
                    await this.accountAggregationService.LogInRequestAsync(loginApiRequest);

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
        public async ValueTask<ActionResult<bool>> ResetPasswordAsync([FromBody]ResetPasswordRequest resetPasswordApiRequest)
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

        [HttpPut]
        [Route(ApiRoutes.ForgotPassword)]
        public async ValueTask<ActionResult<bool>> ForgotPasswordAsync([FromQuery]string email)
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

        [HttpPut]
        [Route(ApiRoutes.ConfirmEmail)]
        public async ValueTask<ActionResult<UserAccountDetailsResponse>> PostEmailConfirmationAsync([FromQuery]string token,string email)
        {
            try
            {
                UserAccountDetailsResponse registeredAccount =
                    await this.accountAggregationService.EmailConfirmationAsync(token,email);

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

        [HttpPut]
        [Route(ApiRoutes.OtpLogin)]
        public async ValueTask<ActionResult<UserAccountDetailsResponse>> OtpLoginAsync([FromQuery]string code, string userNameOrEmail)
        {
            try
            {
                UserAccountDetailsResponse registeredAccount =
                    await this.accountAggregationService.OtpLoginRequestAsync(code,userNameOrEmail);

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

        [HttpPut]
        [Route(ApiRoutes.EnableTwoFactor)]
        public async ValueTask<ActionResult<UserAccountDetailsResponse>> EnableTwoFactorAsync([FromQuery]Guid id)
        {
            try
            {
                UserAccountDetailsResponse registeredAccount =
                    await this.accountAggregationService.EnableUserTwoFactorAsync(id);

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
