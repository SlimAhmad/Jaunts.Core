// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using EFxceptions.Models.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace Jaunts.Core.Api.Services.Foundations.Amenities
{
    public partial class AmenityService
    {
        private delegate ValueTask<Amenity> ReturningAmenityFunction();
        private delegate IQueryable<Amenity> ReturningAmenitiesFunction();

        private async ValueTask<Amenity> TryCatch(ReturningAmenityFunction returningAmenityFunction)
        {
            try
            {
                return await returningAmenityFunction();
            }
            catch (NullAmenityException nullAmenityException)
            {
                throw CreateAndLogValidationException(nullAmenityException);
            }
            catch (InvalidAmenityException invalidAmenityException)
            {
                throw CreateAndLogValidationException(invalidAmenityException);
            }
            catch (NotFoundAmenityException nullAmenityException)
            {
                throw CreateAndLogValidationException(nullAmenityException);
            }
            catch (SqlException sqlException)
            {
                var failedAmenityStorageException =
                    new FailedAmenityStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAmenityStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAmenityException =
                    new AlreadyExistsAmenityException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAmenityException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAmenityException = new LockedAmenityException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedAmenityException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedAmenityStorageException =
                    new FailedAmenityStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedAmenityStorageException);
            }
            catch (Exception exception)
            {
                var failedAmenityServiceException =
                    new FailedAmenityServiceException(exception);

                throw CreateAndLogServiceException(failedAmenityServiceException);
            }
        }

        private IQueryable<Amenity> TryCatch(ReturningAmenitiesFunction returningAmenitiesFunction)
        {
            try
            {
                return returningAmenitiesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAmenityStorageException =
                     new FailedAmenityStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedAmenityStorageException);
            }
            catch (Exception exception)
            {
                var failedAmenityServiceException =
                    new FailedAmenityServiceException(exception);

                throw CreateAndLogServiceException(failedAmenityServiceException);
            }
        }

        private AmenityValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException = new AmenityValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
        private AmenityDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)

        {
            var studentDependencyValidationException =
                new AmenityDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }

        private AmenityDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var studentDependencyException = new AmenityDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private AmenityDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new AmenityDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private AmenityServiceException CreateAndLogServiceException(Xeption exception)
        {
            var studentServiceException = new AmenityServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }
    }
}
