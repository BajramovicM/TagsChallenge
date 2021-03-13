using System.Collections.Generic;
using TagsChallenge.Domain.Base;

namespace TagsChallenge.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<LinkTag> LinkTags { get; set; }
    }
}
