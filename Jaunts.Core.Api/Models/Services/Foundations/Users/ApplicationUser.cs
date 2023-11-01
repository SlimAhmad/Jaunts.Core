using Microsoft.AspNetCore.Identity;

namespace Jaunts.Core.Api.Models.Services.Foundations.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        public override Guid Id
        {
            get => base.Id;
            set => base.Id = value;
        }

        public override string UserName
        {
            get => base.UserName;
            set => base.UserName = value;
        }

        public override string PhoneNumber
        {
            get => base.PhoneNumber;
            set => base.PhoneNumber = value;
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public UserStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        //[JsonIgnore]
        //public IEnumerable<UserContact> UserContacts { get; set; }


    }
}
