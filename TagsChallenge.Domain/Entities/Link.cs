using System.Collections.Generic;
using TagsChallenge.Domain.Base;

namespace TagsChallenge.Domain.Entities
{
    public class Link : BaseEntity
    {
        public string URL { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<LinkTag> LinkTags { get; set; }
    }
}
