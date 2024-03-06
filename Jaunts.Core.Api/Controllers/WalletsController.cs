using Jaunts.Core.Api.Models.Services.Foundations.Wallets;
using Jaunts.Core.Api.Models.Services.Foundations.Wallets.Exceptions;
using Jaunts.Core.Api.Services.Processings.Wallets;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : RESTFulController
    {
        private readonly IWalletProcessingService walletProcessingService;

        public WalletsController(IWalletProcessingService walletProcessingService) =>
            this.walletProcessingService = walletProcessingService;

        [HttpPost]
        public async ValueTask<ActionResult<Wallet>> PostWalletAsync(Wallet wallet)
        {
            try
            {
                Wallet registeredWallet =
                    await this.walletProcessingService.UpsertWalletAsync(wallet);

                return Created(registeredWallet);
            }
            catch (WalletValidationException walletValidationException)
                when (walletValidationException.InnerException is AlreadyExistsWalletException)
            {
                Exception innerMessage = GetInnerMessage(walletValidationException);

                return Conflict(innerMessage);
            }
            catch (WalletValidationException walletValidationException)
            {
                Exception innerMessage = GetInnerMessage(walletValidationException);

                return BadRequest(innerMessage);
            }
            catch (WalletDependencyException walletDependencyException)
            {
                return Problem(walletDependencyException.Message);
            }
            catch (WalletServiceException walletProcessingServiceException)
            {
                return Problem(walletProcessingServiceException.Message);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Wallet>> GetAllWallet()
        {
            try
            {
                IQueryable storageWallet =
                    this.walletProcessingService.RetrieveAllWallets();

                return Ok(storageWallet);
            }
            catch (WalletDependencyException walletDependencyException)
            {
                return Problem(walletDependencyException.Message);
            }
            catch (WalletServiceException walletProcessingServiceException)
            {
                return Problem(walletProcessingServiceException.Message);
            }
        }

        [HttpGet("{walletId}")]
        public async ValueTask<ActionResult<Wallet>> GetWalletByIdAsync(Guid walletId)
        {
            try
            {
                Wallet storageWallet =
                    await this.walletProcessingService.RetrieveWalletByIdAsync(walletId);

                return Ok(storageWallet);
            }
            catch (WalletValidationException walletValidationException)
                when (walletValidationException.InnerException is NotFoundWalletException)
            {
                Exception innerMessage = GetInnerMessage(walletValidationException);

                return NotFound(innerMessage);
            }
            catch (WalletValidationException walletValidationException)
            {
                Exception innerMessage = GetInnerMessage(walletValidationException);

                return BadRequest(innerMessage);
            }
            catch (WalletDependencyException walletDependencyException)
            {
                return Problem(walletDependencyException.Message);
            }
            catch (WalletServiceException walletProcessingServiceException)
            {
                return Problem(walletProcessingServiceException.Message);
            }
        }

        [HttpDelete("{walletId}")]
        public async ValueTask<ActionResult<bool>> DeleteWalletAsync(Guid walletId)
        {
            try
            {
                bool response =
                    await this.walletProcessingService.RemoveWalletByIdAsync(walletId);

                return Ok(response);
            }
            catch (WalletValidationException walletValidationException)
                when (walletValidationException.InnerException is NotFoundWalletException)
            {
                Exception innerMessage = GetInnerMessage(walletValidationException);

                return NotFound(innerMessage);
            }
            catch (WalletValidationException walletValidationException)
            {
                return BadRequest(walletValidationException.Message);
            }
            catch (WalletDependencyException walletDependencyException)
                when (walletDependencyException.InnerException is LockedWalletException)
            {
                Exception innerMessage = GetInnerMessage(walletDependencyException);

                return Locked(innerMessage);
            }
            catch (WalletDependencyException walletDependencyException)
            {
                return Problem(walletDependencyException.Message);
            }
            catch (WalletServiceException walletProcessingServiceException)
            {
                return Problem(walletProcessingServiceException.Message);
            }
        }

        private static Exception GetInnerMessage(Exception exception) =>
            exception.InnerException;
    }
}
