using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize(Roles = "Administrator,Viewer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> Get()
        {
            var tags = await _unitOfWork.Tags.GetAllByExpression();

            var data = _mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(tags);

            return Ok(data);
        }
    }
}
