using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees;
using Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions;
using Jaunts.Core.Api.Services.Processings.TransactionFees;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionFeesController : RESTFulController
    {
        private readonly ITransactionFeeProcessingService transactionFeeProcessingService;

        public TransactionFeesController(ITransactionFeeProcessingService transactionFeeProcessingService) =>
            this.transactionFeeProcessingService = transactionFeeProcessingService;

        [HttpPost]
        public async ValueTask<ActionResult<TransactionFee>> PostTransactionFeeAsync(TransactionFee transactionFee)
        {
            try
            {
                TransactionFee registeredTransactionFee =
                    await this.transactionFeeProcessingService.UpsertTransactionFeeAsync(transactionFee);

                return Created(registeredTransactionFee);
            }
            catch (TransactionFeeValidationException transactionFeeValidationException)
                when (transactionFeeValidationException.InnerException is AlreadyExistsTransactionFeeException)
            {
                Exception innerMessage = GetInnerMessage(transactionFeeValidationException);

                return Conflict(innerMessage);
            }
            catch (TransactionFeeValidationException transactionFeeValidationException)
            {
                Exception innerMessage = GetInnerMessage(transactionFeeValidationException);

                return BadRequest(innerMessage);
            }
            catch (TransactionFeeDependencyException transactionFeeDependencyException)
            {
                return Problem(transactionFeeDependencyException.Message);
            }
            catch (TransactionFeeServiceException transactionFeeProcessingServiceException)
            {
                return Problem(transactionFeeProcessingServiceException.Message);
            }
        }
        [HttpGet]
        public ActionResult<IQueryable<TransactionFee>> GetAllTransactionFee()
        {
            try
            {
                IQueryable storageTransactionFee =
                    this.transactionFeeProcessingService.RetrieveAllTransactionFees();

                return Ok(storageTransactionFee);
            }
            catch (TransactionFeeDependencyException transactionFeeDependencyException)
            {
                return Problem(transactionFeeDependencyException.Message);
            }
            catch (TransactionFeeServiceException transactionFeeProcessingServiceException)
            {
                return Problem(transactionFeeProcessingServiceException.Message);
            }
        }
        [HttpGet("{transactionFeeId}")]
        public async ValueTask<ActionResult<TransactionFee>> GetTransactionFeeByIdAsync(Guid transactionFeeId)
        {
            try
            {
                TransactionFee storageTransactionFee =
                    await this.transactionFeeProcessingService.RetrieveTransactionFeeByIdAsync(transactionFeeId);

                return Ok(storageTransactionFee);
            }
            catch (TransactionFeeValidationException transactionFeeValidationException)
                when (transactionFeeValidationException.InnerException is NotFoundTransactionFeeException)
            {
                Exception innerMessage = GetInnerMessage(transactionFeeValidationException);

                return NotFound(innerMessage);
            }
            catch (TransactionFeeValidationException transactionFeeValidationException)
            {
                Exception innerMessage = GetInnerMessage(transactionFeeValidationException);

                return BadRequest(innerMessage);
            }
            catch (TransactionFeeDependencyException transactionFeeDependencyException)
            {
                return Problem(transactionFeeDependencyException.Message);
            }
            catch (TransactionFeeServiceException transactionFeeProcessingServiceException)
            {
                return Problem(transactionFeeProcessingServiceException.Message);
            }
        }
        [HttpDelete("{transactionFeeId}")]
        public async ValueTask<ActionResult<bool>> DeleteTransactionFeeAsync(Guid transactionFeeId)
        {
            try
            {
                bool response =
                    await this.transactionFeeProcessingService.RemoveTransactionFeeByIdAsync(transactionFeeId);

                return Ok(response);
            }
            catch (TransactionFeeValidationException transactionFeeValidationException)
                when (transactionFeeValidationException.InnerException is NotFoundTransactionFeeException)
            {
                Exception innerMessage = GetInnerMessage(transactionFeeValidationException);

                return NotFound(innerMessage);
            }
            catch (TransactionFeeValidationException transactionFeeValidationException)
            {
                return BadRequest(transactionFeeValidationException.Message);
            }
            catch (TransactionFeeDependencyException transactionFeeDependencyException)
                when (transactionFeeDependencyException.InnerException is LockedTransactionFeeException)
            {
                Exception innerMessage = GetInnerMessage(transactionFeeDependencyException);

                return Locked(innerMessage);
            }
            catch (TransactionFeeDependencyException transactionFeeDependencyException)
            {
                return Problem(transactionFeeDependencyException.Message);
            }
            catch (TransactionFeeServiceException transactionFeeProcessingServiceException)
            {
                return Problem(transactionFeeProcessingServiceException.Message);
            }
        }

        private static Exception GetInnerMessage(Exception exception) =>
            exception.InnerException;
    }
}
