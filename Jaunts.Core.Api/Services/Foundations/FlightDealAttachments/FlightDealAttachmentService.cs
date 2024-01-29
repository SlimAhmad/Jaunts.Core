// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Jaunts.Core.Api.Brokers.DateTimes;
using Jaunts.Core.Api.Brokers.Loggings;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.Models.Services.Foundations.FlightDealAttachments;

namespace Jaunts.Core.Api.Services.Foundations.FlightDealAttachments
{
    public partial class FlightDealAttachmentService : IFlightDealAttachmentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public FlightDealAttachmentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<FlightDealAttachment> AddFlightDealAttachmentAsync(FlightDealAttachment promosOfferAttachment) =>
            TryCatch(async () =>
        {
            ValidateFlightDealAttachmentOnCreate(promosOfferAttachment);

            return await this.storageBroker.InsertFlightDealAttachmentAsync(promosOfferAttachment);
        });

        public IQueryable<FlightDealAttachment> RetrieveAllFlightDealAttachments() =>
        TryCatch(() => this.storageBroker.SelectAllFlightDealAttachments());

        public ValueTask<FlightDealAttachment> RetrieveFlightDealAttachmentByIdAsync
            (Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateFlightDealAttachmentIdIsNull(packageId, attachmentId);

            FlightDealAttachment maybeFlightDealAttachment =
               await this.storageBroker.SelectFlightDealAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageFlightDealAttachment(maybeFlightDealAttachment, packageId, attachmentId);

            return maybeFlightDealAttachment;
        });

        public ValueTask<FlightDealAttachment> RemoveFlightDealAttachmentByIdAsync(Guid packageId, Guid attachmentId) =>
        TryCatch(async () =>
        {
            ValidateFlightDealAttachmentIdIsNull(packageId, attachmentId);

            FlightDealAttachment mayBeFlightDealAttachment =
               await this.storageBroker.SelectFlightDealAttachmentByIdAsync(packageId, attachmentId);

            ValidateStorageFlightDealAttachment(mayBeFlightDealAttachment, packageId, attachmentId);

            return await this.storageBroker.DeleteFlightDealAttachmentAsync(mayBeFlightDealAttachment);
        });
    }
}
