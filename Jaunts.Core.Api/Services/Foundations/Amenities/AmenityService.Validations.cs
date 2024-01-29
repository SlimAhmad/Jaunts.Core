// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.Amenities;
using Jaunts.Core.Api.Models.Services.Foundations.Amenitys.Exceptions;

namespace Jaunts.Core.Api.Services.Foundations.Amenities
{
    public partial class AmenityService
    {
        private void ValidateAmenityOnCreate(Amenity amenity)
        {
            ValidateAmenity(amenity);

            Validate(
                 (Rule: IsInvalid(amenity.Id), Parameter: nameof(Amenity.Id)),
                (Rule: IsInvalid(amenity.Description), Parameter: nameof(Amenity.Description)),
                (Rule: IsInvalid(amenity.Name), Parameter: nameof(Amenity.Name)),
                (Rule: IsInvalid(amenity.ShortLetId), Parameter: nameof(Amenity.ShortLetId)),
                (Rule: IsInvalid(amenity.CreatedBy), Parameter: nameof(Amenity.CreatedBy)),
                (Rule: IsInvalid(amenity.UpdatedBy), Parameter: nameof(Amenity.UpdatedBy)),
                (Rule: IsInvalid(amenity.CreatedDate), Parameter: nameof(Amenity.CreatedDate)),
                (Rule: IsInvalid(amenity.UpdatedDate), Parameter: nameof(Amenity.UpdatedDate)),
                (Rule: IsNotRecent(amenity.CreatedDate), Parameter: nameof(Amenity.CreatedDate)),

                (Rule: IsNotSame(firstId: amenity.UpdatedBy,
                    secondId: amenity.CreatedBy,
                    secondIdName: nameof(Amenity.CreatedBy)),
                    Parameter: nameof(Amenity.UpdatedBy)),

                (Rule: IsNotSame(firstDate: amenity.UpdatedDate,
                    secondDate: amenity.CreatedDate,
                    secondDateName: nameof(Amenity.CreatedDate)),
                    Parameter: nameof(Amenity.UpdatedDate))
            );
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private dynamic IsNotRecent(DateTimeOffset dateTimeOffset) => new
        {
            Condition = IsDateNotRecent(dateTimeOffset),
            Message = "Date is not recent"
        };


        private static void ValidateAmenityId(Guid amenityId)
        {
            Validate((Rule: IsInvalid(amenityId), Parameter: nameof(Amenity.Id)));
        }

        private static void ValidateStorageAmenity(Amenity storageAmenity, Guid amenityId)
        {
            if (storageAmenity is null)
            {
                throw new NotFoundAmenityException(amenityId);
            }
        }

        private void ValidateAmenityOnModify(Amenity amenity)
        {
            ValidateAmenity(amenity);

            Validate(
                (Rule: IsInvalid(amenity.Id), Parameter: nameof(Amenity.Id)),
                (Rule: IsInvalid(amenity.Description), Parameter: nameof(Amenity.Description)),
                (Rule: IsInvalid(amenity.Name), Parameter: nameof(Amenity.Name)),
                (Rule: IsInvalid(amenity.ShortLetId), Parameter: nameof(Amenity.ShortLetId)),
                (Rule: IsInvalid(amenity.CreatedBy), Parameter: nameof(Amenity.CreatedBy)),
                (Rule: IsInvalid(amenity.UpdatedBy), Parameter: nameof(Amenity.UpdatedBy)),
                (Rule: IsInvalid(amenity.CreatedDate), Parameter: nameof(Amenity.CreatedDate)),
                (Rule: IsInvalid(amenity.UpdatedDate), Parameter: nameof(Amenity.UpdatedDate)),
                (Rule: IsNotRecent(amenity.UpdatedDate), Parameter: nameof(Amenity.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: amenity.UpdatedDate,
                    secondDate: amenity.CreatedDate,
                    secondDateName: nameof(Amenity.CreatedDate)),
                    Parameter: nameof(Amenity.UpdatedDate))
            );
        }

        public void ValidateAgainstStorageAmenityOnModify(Amenity inputAmenity, Amenity storageAmenity)
        {
            Validate(
                (Rule: IsNotSame(
                    firstDate: inputAmenity.CreatedDate,
                    secondDate: storageAmenity.CreatedDate,
                    secondDateName: nameof(Amenity.CreatedDate)),
                    Parameter: nameof(Amenity.CreatedDate)),

                (Rule: IsSame(
                    firstDate: inputAmenity.UpdatedDate,
                    secondDate: storageAmenity.UpdatedDate,
                    secondDateName: nameof(Amenity.UpdatedDate)),
                    Parameter: nameof(Amenity.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: inputAmenity.CreatedBy,
                    secondId: storageAmenity.CreatedBy,
                    secondIdName: nameof(Amenity.CreatedBy)),
                    Parameter: nameof(Amenity.CreatedBy))
            );
        }

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void ValidateAmenity(Amenity amenity)
        {
            if (amenity is null)
            {
                throw new NullAmenityException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAmenityException = new InvalidAmenityException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAmenityException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidAmenityException.ThrowIfContainsErrors();
        }
    }
}