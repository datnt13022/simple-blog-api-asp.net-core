using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog_API.Modals;
using Blog_API.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Controllers
{
    [Route("api/post")]
    [ApiController]
    [Authorize]

    public class PostController: ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            this._postRepository = postRepository;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Post>> getPost()
        {
            return await _postRepository.Get();
        }

        [AllowAnonymous]
        [HttpGet("category/{tag}")]
        public async Task<IEnumerable<Post>> getByTag(string tag)
        {
            return await _postRepository.GetByTag(tag);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> getPost(int id)
        {
            return await _postRepository.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
        {
            var newPost = await _postRepository.Create(post);
            return CreatedAtAction(nameof(getPost), new {id = newPost.id}, newPost);
        }
        [HttpPut ("{id}")]
        public async Task<ActionResult> UpdatePost(int id, [FromBody] Post post){
            if (id != post.id)
            {
                return BadRequest(new{message="id not true!"});
            }

            await _postRepository.Update(post);
            return Ok(
                new
                {
                    message="update success!"
                });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deletePost(int id)
        {
             await _postRepository.Delete(id);
            return Ok(new {message = "delete success!"});
        }
    }
}