using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace Homework4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IMemoryCache _memoryCache;
        
        private List<string> _cacheNames= new List<string>
        {
            "memory",
            "distrubuted",
            "customized"
        };
        public CacheController(IMemoryCache memoryCache,IDistributedCache distributedCache)=>(_memoryCache,_distributedCache)=(memoryCache,distributedCache);
        [HttpGet("distrubuted")]
        public IActionResult GetFromDistrubuted(int index)
        {
            var result = _distributedCache.Get(index.ToString());
            if (result == null)
            {
                DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };

                _distributedCache.Set(index.ToString(), Encoding.UTF8.GetBytes(_cacheNames[index]));
                return Ok(_cacheNames[index]);
            }
            var item = Encoding.UTF8.GetString(result);
            return Ok(item);

        }


        [HttpGet("memory")]
        public IActionResult GetFromMemoryCache(int item)
        {
            if (_memoryCache.TryGetValue(item, out string name))
            {
                return Ok(name);
            }
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(15),
                SlidingExpiration = TimeSpan.FromMinutes(5),
                Priority = CacheItemPriority.Normal
            };
            _memoryCache.Set(item, _cacheNames[item], options);
            return Ok(_cacheNames[item]);
        }
        

        
    }
}
