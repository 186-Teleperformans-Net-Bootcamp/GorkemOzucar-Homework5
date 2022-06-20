using Homework5.Data;
using Homework5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Homework5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GroupController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult Get([FromQuery] GroupPagingQueryParameter paging)
        {
            ICollection<Group> groups = new List<Group>();
            for (int i = 0; i < 100; i++)
            {
                Group g = new Group();
                g.GroupName = $"Teleperformance{i}";
                g.CreatedAt = DateTime.Now.AddDays(-i);
                groups.Add(g);
            }

            _context.Groups.AddRange(groups);

            _context.SaveChanges();

            IQueryable<Group> query = _context.Groups.AsQueryable();

            if (!string.IsNullOrWhiteSpace(paging.Keyword))
            {
                query = _context.Groups.Where(x => x.GroupName.Contains(paging.Keyword));
            }

            if (paging.Start.HasValue && paging.End.HasValue)
            {
                query = query.Where(x => x.CreatedAt <= paging.End.Value && x.CreatedAt >= paging.Start.Value);
            }

            var total = query.Count();
            var totalPage = (int)Math.Ceiling(total / (double)paging.Limit);

            bool hasPrevious = paging.Page > 1;
            bool hasNext = paging.Page < totalPage;

            var metaData = new
            {
                total,
                paging.Limit,
                paging.Page,
                totalPage,
                hasNext,
                hasPrevious
            };

            var data = query.Skip((paging.Page - 1) * paging.Limit).Take(paging.Limit).ToList();
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(data);
        }
    }
}
