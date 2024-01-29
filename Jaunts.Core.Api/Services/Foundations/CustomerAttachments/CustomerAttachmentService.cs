// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.CustomerAttachments;

namespace Jaunts.Core.Api.Services.Foundations.CustomerAttachments
{
    public partial class CustomerAttachmentService : ICustomerAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public CustomerAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<CustomerAttachment> AddCustomerAttachmentAsync(CustomerAttachment customerAttachment) =>
            TryCatch(async () =>
        {
            ValidateCustomerAttachmentOnCreate(customerAttachment);

            return await this.storageBroker.InsertCustomerAttachmentAsync(customerAttachment);
        });

        public IQueryable<CustomerAttachment> RetrieveAllCustomerAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllCustomerAttachments());

        public ValueTask<CustomerAttachment> RetrieveCustomerAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateCustomerAttachmentIdIsNull(packageId, attachmentId);

            CustomerAttachment maybeCustomerAttachment =
               await this.storageBroker.SelectCustomerAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageCustomerAttachment(maybeCustomerAttachment, packageId, attachmentId);

            return maybeCustomerAttachment;
        });

        public ValueTask<CustomerAttachment> RemoveCustomerAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateCustomerAttachmentIdIsNull(packageId, attachmentId);

            CustomerAttachment mayBeCustomerAttachment =
               await this.storageBroker.SelectCustomerAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageCustomerAttachment(mayBeCustomerAttachment, packageId, attachmentId);

            return await this.storageBroker.DeleteCustomerAttachmentAsync(mayBeCustomerAttachment);
        });
    }
}
