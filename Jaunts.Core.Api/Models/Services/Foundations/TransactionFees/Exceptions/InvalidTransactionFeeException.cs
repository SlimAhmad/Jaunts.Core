﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Services.Foundations.TransactionFees.Exceptions
{
    public class InvalidTransactionFeeException : Xeption
    {
        public InvalidTransactionFeeException()
            : base(message: "Invalid TransactionFee. Please fix the errors and try again.")
        { }
        public InvalidTransactionFeeException(string message)
            : base(message)
        { }
    }
}