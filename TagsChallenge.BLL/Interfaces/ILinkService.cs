using System.Collections.Generic;
using System.Threading.Tasks;
using TagsChallenge.DAL.Repositories;
using TagsChallenge.Domain.Entities;

namespace TagsChallenge.BLL.Interfaces
{
    public interface ILinkService : IRepository<Link>
    {
        Task<Link> AddUserLink(string url, string tag, string userId);
        Task<List<Link>> GetUserLinksAsync(string userId);
        Task<List<Link>> GetAllByTagsAsync(string userId, List<string> tags);
    }
}
