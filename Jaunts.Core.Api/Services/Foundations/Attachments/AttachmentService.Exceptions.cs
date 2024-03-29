﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Attachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments;
using Jaunts.Core.Api.Models.Services.Foundations.Attachments.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Jaunts.Core.Api.Services.Foundations.Attachments
{
    public partial class AttachmentService : IAttachmentService
    {
        private delegate ValueTask<Attachment> ReturningAttachmentFunction();
        private delegate IQueryable<Attachment> ReturningAttachmentsFunction();

        private async ValueTask<Attachment> TryCatch(ReturningAttachmentFunction returningAttachmentFunction)
        {
            try
            {
                return await returningAttachmentFunction();
            }
            catch (NullAttachmentException nullAttachmentException)
            {
                throw CreateAndLogValidationException(nullAttachmentException);
            }
            catch (InvalidAttachmentException invalidAttachmentInputException)
            {
                throw CreateAndLogValidationException(invalidAttachmentInputException);
            }
            catch (NotFoundAttachmentException notFoundAttachmentException)
            {
                throw CreateAndLogValidationException(notFoundAttachmentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAttachmentException =
                    new AlreadyExistsAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsAttachmentException);
            }
            catch (SqlException sqlException)
            {
                var failedAttachmentStorageException =
                   new FailedAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttachmentStorageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAttachmentException = 
                    new LockedAttachmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedAttachmentException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedAttachmentStorageException =
                   new FailedAttachmentStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedAttachmentServiceException =
                    new FailedAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedAttachmentServiceException);
            }
        }

        private IQueryable<Attachment> TryCatch(ReturningAttachmentsFunction returningAttachmentsFunction)
        {
            try
            {
                return returningAttachmentsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAttachmentStorageException =
                   new FailedAttachmentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAttachmentStorageException);
            }
            catch (Exception exception)
            {
                var failedAttachmentServiceException =
                    new FailedAttachmentServiceException(exception);

                throw CreateAndLogServiceException(failedAttachmentServiceException);
            }
        }

        private AttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var attachmentValidationException = new AttachmentValidationException(exception);
            this.loggingBroker.LogError(attachmentValidationException);

            return attachmentValidationException;
        }

        private AttachmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var attachmentDependencyException = new AttachmentDependencyException(exception);
            this.loggingBroker.LogCritical(attachmentDependencyException);

            return attachmentDependencyException;
        }

        private AttachmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var attachmentDependencyException = new AttachmentDependencyException(exception);
            this.loggingBroker.LogError(attachmentDependencyException);

            return attachmentDependencyException;
        }

        private AttachmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var attachmentServiceException = new AttachmentServiceException(exception);
            this.loggingBroker.LogError(attachmentServiceException);

            return attachmentServiceException;
        }
    }
}