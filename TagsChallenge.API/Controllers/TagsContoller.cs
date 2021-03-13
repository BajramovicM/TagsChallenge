using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagsChallenge.BLL.Interfaces;

namespace TagsChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsContoller : ControllerBase
    {
        private readonly IContentService _contentService;

        public TagsContoller(IContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("GetSuggestedTags")]
        public async Task<IActionResult> GetSuggetedTags(string url)
        {
            var tags = await _contentService.GetSuggestedTags(url);
            return Ok(tags);
        }
    }
}
