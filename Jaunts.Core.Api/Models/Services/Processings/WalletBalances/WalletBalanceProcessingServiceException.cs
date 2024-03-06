// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.WalletBalance.Exceptions
{
    public class WalletBalanceProcessingServiceException : Xeption
    {
        public WalletBalanceProcessingServiceException(Exception innerException)
            : base(message: "Failed WalletBalance processing service occurred, Please contact support", innerException)
        { }
        public WalletBalanceProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
