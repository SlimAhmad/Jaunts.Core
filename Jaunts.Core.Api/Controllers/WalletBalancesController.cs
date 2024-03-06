using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances;
using Jaunts.Core.Api.Models.Services.Foundations.WalletBalances.Exceptions;
using Jaunts.Core.Api.Services.Processings.WalletBalances;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletBalancesController : RESTFulController
    {
        private readonly IWalletBalanceProcessingService walletBalanceProcessingService;

        public WalletBalancesController(IWalletBalanceProcessingService walletBalanceProcessingService) =>
            this.walletBalanceProcessingService = walletBalanceProcessingService;

        [HttpPost]
        public async ValueTask<ActionResult<WalletBalance>> PostWalletBalanceAsync(WalletBalance walletBalance)
        {
            try
            {
                WalletBalance registeredWalletBalance =
                    await this.walletBalanceProcessingService.UpsertWalletBalanceAsync(walletBalance);

                return Created(registeredWalletBalance);
            }
            catch (WalletBalanceValidationException walletBalanceValidationException)
                when (walletBalanceValidationException.InnerException is AlreadyExistsWalletBalanceException)
            {
                Exception innerMessage = GetInnerMessage(walletBalanceValidationException);

                return Conflict(innerMessage);
            }
            catch (WalletBalanceValidationException walletBalanceValidationException)
            {
                Exception innerMessage = GetInnerMessage(walletBalanceValidationException);

                return BadRequest(innerMessage);
            }
            catch (WalletBalanceDependencyException walletBalanceDependencyException)
            {
                return Problem(walletBalanceDependencyException.Message);
            }
            catch (WalletBalanceServiceException walletBalanceProcessingServiceException)
            {
                return Problem(walletBalanceProcessingServiceException.Message);
            }
        }
        [HttpGet]
        public ActionResult<IQueryable<WalletBalance>> GetAllWalletBalance()
        {
            try
            {
                IQueryable storageWalletBalance =
                    this.walletBalanceProcessingService.RetrieveAllWalletBalances();

                return Ok(storageWalletBalance);
            }
            catch (WalletBalanceDependencyException walletBalanceDependencyException)
            {
                return Problem(walletBalanceDependencyException.Message);
            }
            catch (WalletBalanceServiceException walletBalanceProcessingServiceException)
            {
                return Problem(walletBalanceProcessingServiceException.Message);
            }
        }
        [HttpGet("{walletBalanceId}")]
        public async ValueTask<ActionResult<WalletBalance>> GetWalletBalanceByIdAsync(Guid walletBalanceId)
        {
            try
            {
                WalletBalance storageWalletBalance =
                    await this.walletBalanceProcessingService.RetrieveWalletBalanceByIdAsync(walletBalanceId);

                return Ok(storageWalletBalance);
            }
            catch (WalletBalanceValidationException walletBalanceValidationException)
                when (walletBalanceValidationException.InnerException is NotFoundWalletBalanceException)
            {
                Exception innerMessage = GetInnerMessage(walletBalanceValidationException);

                return NotFound(innerMessage);
            }
            catch (WalletBalanceValidationException walletBalanceValidationException)
            {
                Exception innerMessage = GetInnerMessage(walletBalanceValidationException);

                return BadRequest(innerMessage);
            }
            catch (WalletBalanceDependencyException walletBalanceDependencyException)
            {
                return Problem(walletBalanceDependencyException.Message);
            }
            catch (WalletBalanceServiceException walletBalanceProcessingServiceException)
            {
                return Problem(walletBalanceProcessingServiceException.Message);
            }
        }
        [HttpDelete("{walletBalanceId}")]
        public async ValueTask<ActionResult<bool>> DeleteWalletBalanceAsync(Guid walletBalanceId)
        {
            try
            {
                bool response =
                    await this.walletBalanceProcessingService.RemoveWalletBalanceByIdAsync(walletBalanceId);

                return Ok(response);
            }
            catch (WalletBalanceValidationException walletBalanceValidationException)
                when (walletBalanceValidationException.InnerException is NotFoundWalletBalanceException)
            {
                Exception innerMessage = GetInnerMessage(walletBalanceValidationException);

                return NotFound(innerMessage);
            }
            catch (WalletBalanceValidationException walletBalanceValidationException)
            {
                return BadRequest(walletBalanceValidationException.Message);
            }
            catch (WalletBalanceDependencyException walletBalanceDependencyException)
                when (walletBalanceDependencyException.InnerException is LockedWalletBalanceException)
            {
                Exception innerMessage = GetInnerMessage(walletBalanceDependencyException);

                return Locked(innerMessage);
            }
            catch (WalletBalanceDependencyException walletBalanceDependencyException)
            {
                return Problem(walletBalanceDependencyException.Message);
            }
            catch (WalletBalanceServiceException walletBalanceProcessingServiceException)
            {
                return Problem(walletBalanceProcessingServiceException.Message);
            }
        }

        private static Exception GetInnerMessage(Exception exception) =>
            exception.InnerException;
    }
}
