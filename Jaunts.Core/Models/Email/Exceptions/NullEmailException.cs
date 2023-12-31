﻿using System;
using Xeptions;

namespace Jaunts.Core.Models.Exceptions
{
    public partial class NullEmailException : Xeption
    {
        public NullEmailException()
            : base(message: "Email is null.")
        { }

        public NullEmailException(string message, Exception innerException)
         : base(message: message,
               innerException)
        { }
    }
}
