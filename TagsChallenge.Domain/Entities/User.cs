using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TagsChallenge.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Link> Links { get; set; }
    }
}
