// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Jaunts.Core.Api.Models.Processings.Transaction.Exceptions
{
    public class InvalidTransactionProcessingException : Xeption
    {
        public InvalidTransactionProcessingException()
            : base(message: "Invalid Transaction, Please correct the errors and try again.") 
        { }
        public InvalidTransactionProcessingException(string message)
            : base(message)
        { }
    }
}
