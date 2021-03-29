using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_API.Modals;
using Blog_API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    public class CatogoryController: ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CatogoryController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Category>> getAllCategory()
        {
            return await _categoryRepository.Get();
            
        }
        
        [HttpPost]
        public async Task<Category> CreatePost([FromBody] Category category)
        {
            return await _categoryRepository.Create(category);
            
        }
        [HttpDelete ("{tag}")]
        public async Task<ActionResult> DeleteCategory(string tag)
        {
             _categoryRepository.Delete(tag);
            return Ok(
                new
                {
                    message = "delete success!"
                });
        }
        
    }
}