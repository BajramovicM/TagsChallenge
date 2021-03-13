using System.Collections.Generic;
using System.Threading.Tasks;

namespace TagsChallenge.BLL.Interfaces
{
    public interface IContentService
    {
        Task<List<string>> GetSuggestedTags(string url);
    }
}
