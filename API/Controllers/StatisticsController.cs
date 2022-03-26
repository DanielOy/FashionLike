using API.Dtos;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public StatisticsController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetUserStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatisticDto>))]
        public async Task<ActionResult<IEnumerable<StatisticDto>>> GetUserStatistics()
        {
            var totals = await _userManager.Users
                .GroupBy(x => x.CreationDate.Date)
                .Select(x => new StatisticDto
                {
                    Date = x.Key.ToShortDateString(),
                    Count = x.Count()
                })
                .ToListAsync();

            return Ok(totals);
        }

        [HttpGet("GetLikeStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatisticDto>))]
        public async Task<ActionResult<IEnumerable<StatisticDto>>> GetLikeStatistics()
        {
            var data = await GetStatisticsByReactionType(Core.Enums.ReactionType.Like);

            return Ok(data);
        }

        [HttpGet("GetDislikeStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatisticDto>))]
        public async Task<ActionResult<IEnumerable<StatisticDto>>> GetDislikeStatistics()
        {
            var data = await GetStatisticsByReactionType(Core.Enums.ReactionType.Dislike);

            return Ok(data);
        }

        private async Task<List<StatisticDto>> GetStatisticsByReactionType(Core.Enums.ReactionType reaction)
        {
            var totals = await _unitOfWork.Reactions.GetAllByExpression(x => x.ReactionType == reaction);
            var data = totals.GroupBy(x => x.CreationDate.Date)
                .Select(x => new StatisticDto
                {
                    Date = x.Key.ToShortDateString(),
                    Count = x.Count()
                })
                .ToList();
            return data;
        }
    }
}
