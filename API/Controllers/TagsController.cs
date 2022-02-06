using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        [Authorize(Roles = "Administrator,Viewer")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TagDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<IEnumerable<TagDto>>> Get()
        {
            var tags = await _unitOfWork.Tags.GetAllByExpression();

            var data = _mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(tags);

            return Ok(data);
        }
    }
}
