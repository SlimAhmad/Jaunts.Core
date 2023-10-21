using System;

namespace Jaunts.Core.Api.Models.Users.Exceptions
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException(Guid userId)
            : base(message: $"Couldn't find user with id: {userId}.") { }

        public NotFoundUserException(string message)
        : base(message: message
              )
        { }
    }
}
