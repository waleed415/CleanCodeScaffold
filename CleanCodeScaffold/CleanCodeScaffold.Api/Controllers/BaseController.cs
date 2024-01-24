using CleanCodeScaffold.Application.Handlers.Interface;
using Microsoft.AspNetCore.Mvc;
using CleanCodeScaffold.Api.Util;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanCodeScaffold.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<VM> : ControllerBase where VM : class
    {
        private readonly IBaseHandler<VM> _handler;
        public BaseController(IBaseHandler<VM> handler)
        {
            _handler = handler;
        }
        // GET: api/<BaseController>
        [HttpGet]
        public virtual async Task<IActionResult> Get(int pagesize=10, int pagenumber=1)
        {
            var res = await _handler.GetAllAsync(pagesize, pagenumber);
            return res.ToResponse();
        }

        // GET api/<BaseController>/5
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(long id)
        {
            var res = await _handler.GetByIdAsync(id);
            return res.ToResponse();
        }

        // POST api/<BaseController>
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] VM model)
        {
            var res = await _handler.CreateAsync(model);
            return res.ToResponse();
        }

        // PUT api/<BaseController>/5
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(long id, [FromBody] VM model)
        {
            var res = await _handler.UpdateAsync(id, model);
            return res.ToResponse();
        }

        // DELETE api/<BaseController>/5
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(long id)
        {
            var res = await _handler.DeleteAsync(id);
            return res.ToResponse();
        }
    }
}
