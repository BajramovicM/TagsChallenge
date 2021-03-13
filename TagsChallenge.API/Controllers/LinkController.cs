using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TagsChallenge.BLL.Interfaces;

namespace TagsChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly ILinkService _linkService;
        
        public LinkController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Add")]
        public async Task<IActionResult> Add(string url, string userTag)
        {
            // TODO: Add user to session to use on service layer

            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var userId = claimsIdentity.FindFirst("Id")?.Value;
            var link = await _linkService.AddUserLink(url, userTag, userId);
            return Ok(link);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            // TODO: Add user to session to use on service layer

            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var userId = claimsIdentity.FindFirst("Id")?.Value;
            var links = await _linkService.GetUserLinksAsync(userId);
                
            return Ok(links);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("SearchByTag")]
        public async Task<IActionResult> GetAllByTags([FromQuery(Name = "tags")] List<string> tags)
        {

            var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            var userId = claimsIdentity.FindFirst("Id")?.Value;
            var links = await _linkService.GetAllByTagsAsync(userId, tags);

            return Ok(links);
        }
    }
}
