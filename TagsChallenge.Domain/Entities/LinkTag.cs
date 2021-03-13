namespace TagsChallenge.Domain.Entities
{
    public class LinkTag
    {
        public int LinkId { get; set; }
        public Link Link { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
