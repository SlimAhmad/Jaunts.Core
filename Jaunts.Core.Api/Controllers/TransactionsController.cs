using Jaunts.Core.Api.Models.Services.Foundations.Transactions;
using Jaunts.Core.Api.Models.Services.Foundations.Transactions.Exceptions;
using Jaunts.Core.Api.Services.Processings.Transactions;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Jaunts.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : RESTFulController
    {
        private readonly ITransactionProcessingService transactionProcessingService;

        public TransactionsController(ITransactionProcessingService transactionProcessingService) =>
            this.transactionProcessingService = transactionProcessingService;

        [HttpPost]
        public async ValueTask<ActionResult<Transaction>> PostTransactionAsync(Transaction transaction)
        {
            try
            {
                Transaction registeredTransaction =
                    await this.transactionProcessingService.UpsertTransactionAsync(transaction);

                return Created(registeredTransaction);
            }
            catch (TransactionValidationException transactionValidationException)
                when (transactionValidationException.InnerException is AlreadyExistsTransactionException)
            {
                Exception innerMessage = GetInnerMessage(transactionValidationException);

                return Conflict(innerMessage);
            }
            catch (TransactionValidationException transactionValidationException)
            {
                Exception innerMessage = GetInnerMessage(transactionValidationException);

                return BadRequest(innerMessage);
            }
            catch (TransactionDependencyException transactionDependencyException)
            {
                return Problem(transactionDependencyException.Message);
            }
            catch (TransactionServiceException transactionProcessingServiceException)
            {
                return Problem(transactionProcessingServiceException.Message);
            }
        }
        [HttpGet]
        public ActionResult<IQueryable<Transaction>> GetAllTransaction()
        {
            try
            {
                IQueryable storageTransaction =
                    this.transactionProcessingService.RetrieveAllTransactions();

                return Ok(storageTransaction);
            }
            catch (TransactionDependencyException transactionDependencyException)
            {
                return Problem(transactionDependencyException.Message);
            }
            catch (TransactionServiceException transactionProcessingServiceException)
            {
                return Problem(transactionProcessingServiceException.Message);
            }
        }
        [HttpGet("{transactionId}")]
        public async ValueTask<ActionResult<Transaction>> GetTransactionByIdAsync(Guid transactionId)
        {
            try
            {
                Transaction storageTransaction =
                    await this.transactionProcessingService.RetrieveTransactionByIdAsync(transactionId);

                return Ok(storageTransaction);
            }
            catch (TransactionValidationException transactionValidationException)
                when (transactionValidationException.InnerException is NotFoundTransactionException)
            {
                Exception innerMessage = GetInnerMessage(transactionValidationException);

                return NotFound(innerMessage);
            }
            catch (TransactionValidationException transactionValidationException)
            {
                Exception innerMessage = GetInnerMessage(transactionValidationException);

                return BadRequest(innerMessage);
            }
            catch (TransactionDependencyException transactionDependencyException)
            {
                return Problem(transactionDependencyException.Message);
            }
            catch (TransactionServiceException transactionProcessingServiceException)
            {
                return Problem(transactionProcessingServiceException.Message);
            }
        }
        [HttpDelete("{transactionId}")]
        public async ValueTask<ActionResult<bool>> DeleteTransactionAsync(Guid transactionId)
        {
            try
            {
                bool response =
                    await this.transactionProcessingService.RemoveTransactionAsync(transactionId);

                return Ok(response);
            }
            catch (TransactionValidationException transactionValidationException)
                when (transactionValidationException.InnerException is NotFoundTransactionException)
            {
                Exception innerMessage = GetInnerMessage(transactionValidationException);

                return NotFound(innerMessage);
            }
            catch (TransactionValidationException transactionValidationException)
            {
                return BadRequest(transactionValidationException.Message);
            }
            catch (TransactionDependencyException transactionDependencyException)
                when (transactionDependencyException.InnerException is LockedTransactionException)
            {
                Exception innerMessage = GetInnerMessage(transactionDependencyException);

                return Locked(innerMessage);
            }
            catch (TransactionDependencyException transactionDependencyException)
            {
                return Problem(transactionDependencyException.Message);
            }
            catch (TransactionServiceException transactionProcessingServiceException)
            {
                return Problem(transactionProcessingServiceException.Message);
            }
        }

        private static Exception GetInnerMessage(Exception exception) =>
            exception.InnerException;
    }
}
