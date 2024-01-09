// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class AlreadyExistsTransactionFeeException : Xeption
    {
        public AlreadyExistsTransactionFeeException(Exception innerException)
            : base(message: "Transaction fee  with the same id already exists.", innerException) { }
        public AlreadyExistsTransactionFeeException(string message,Exception innerException)
           : base(message, innerException) { }
    }
}
