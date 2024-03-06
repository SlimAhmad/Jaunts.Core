// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Wallet.Exceptions
{
    public class WalletProcessingServiceException : Xeption
    {
        public WalletProcessingServiceException(Exception innerException)
            : base(message: "Failed Wallet processing service occurred, Please contact support", innerException)
        { }
        public WalletProcessingServiceException(string message,Exception innerException)
            : base(message, innerException)
        { }
    }
}
