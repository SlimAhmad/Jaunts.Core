// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments.Exceptions;
using Jaunts.Core.Api.Models.Services.Foundations.PackageAttachments;

namespace Jaunts.Core.Api.Services.Foundations.PackagesAttachments
{
    public partial class PackageAttachmentService
    {
        private static void ValidatePackageAttachmentOnCreate(PackageAttachment packageAttachment)
        {
            ValidatePackageAttachmentIsNull(packageAttachment);
            ValidatePackageAttachmentIdIsNull(packageAttachment.PackageId, packageAttachment.AttachmentId);
        }

        private static void ValidatePackageAttachmentIsNull(PackageAttachment PackageAttachment)
        {
            if (PackageAttachment is null)
            {
                throw new NullPackageAttachmentException();
            }
        }


        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static void ValidatePackageAttachmentId(Guid PackageId)
        {
            Validate((Rule: IsInvalid(PackageId), Parameter: nameof(PackageAttachment.PackageId)));
        }
        private static void ValidateAttachmentId(Guid attachmentId)
        {
            Validate((Rule: IsInvalid(attachmentId), Parameter: nameof(PackageAttachment.AttachmentId)));
        }

        private static void ValidatePackageAttachmentIdIsNull(Guid PackageId, Guid attachmentId)
        {
            ValidateAttachmentId(attachmentId);
            ValidatePackageAttachmentId(PackageId);
        }

        private static void ValidateStoragePackageAttachment(
            PackageAttachment storagePackageAttachment,
            Guid PackageId, Guid attachmentId)
        {
            if (storagePackageAttachment is null)
            {
                throw new NotFoundPackageAttachmentException(PackageId, attachmentId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidPackageAttachmentException = new InvalidPackageAttachmentException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidPackageAttachmentException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidPackageAttachmentException.ThrowIfContainsErrors();
        }
    }
}
