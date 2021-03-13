using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagsChallenge.BLL.Interfaces;
using TagsChallenge.DAL.Context;
using TagsChallenge.DAL.Repositories;
using TagsChallenge.Domain.Entities;

namespace TagsChallenge.BLL.Services
{
    public class LinkService : Repository<Link>, ILinkService
    {
        private readonly ApplicationDbContext _context;
        private readonly IContentService _contentService;
        private readonly ITagService _tagService;
        
        public LinkService(
            ApplicationDbContext context,
            IContentService contentService,
            ITagService tagService
        )
            : base(context)
        {            
            _context = context;
            _contentService = contentService;
            _tagService = tagService;
        }

        public async Task<Link> AddUserLink(string url, string tag, string userId)
        {
            var linkTags = await _contentService.GetSuggestedTags(url);
            linkTags.Add(tag);

            var existingTags = await _tagService.FilterAsync(x => linkTags.Contains(x.Name));
            var allTags = AddMissingTags(linkTags, existingTags);

            var newLink = new Link()
            {
                URL = url,
                UserId = userId
            };

            List<LinkTag> newLinkTags = new List<LinkTag>();
            foreach(var t in allTags)
            {
                newLinkTags.Add(new LinkTag()
                {
                    TagId = t.Id
                });
            }

            newLink.LinkTags = newLinkTags;

            Add(newLink);

            return newLink;
        }

        public async Task<List<Link>> GetAllByTagsAsync(string userId, List<string> tags)
        {
            return await _context.Links.Include(x => x.LinkTags).ThenInclude(s => s.Tag)
                .Where(
                    x => x.UserId == userId && 
                    x.LinkTags.Any(l => tags.Contains(l.Tag.Name)))
                .ToListAsync();
        }

        public async Task<List<Link>> GetUserLinksAsync(string userId)
        {
           return await _context.Links.Include(x => x.LinkTags)
                .ThenInclude(s => s.Tag).Where(x => x.UserId == userId).ToListAsync();
        }

        private List<Tag> AddMissingTags(List<string> linkTags, IEnumerable<Tag> existingTags)
        {
            var missingTags = linkTags.Where(l => !existingTags.Any(t => l == t.Name));
            var tags = new List<Tag>();
            foreach (var tag in missingTags)
            {
                tags.Add(new Tag()
                {
                    Name = tag
                });
            }

            _tagService.AddRange(tags);

            tags.AddRange(existingTags);
            return tags;
        }
    }
}   