using System;
using Jaunts.Core.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Models.Services.Foundations.Role
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public Permissions Permissions { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
