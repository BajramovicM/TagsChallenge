using TagsChallenge.BLL.Interfaces;
using TagsChallenge.DAL.Context;
using TagsChallenge.DAL.Repositories;
using TagsChallenge.Domain.Entities;

namespace TagsChallenge.BLL.Services
{
    public class TagService : Repository<Tag>, ITagService
    {
        private readonly ApplicationDbContext _context;
        public TagService(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
