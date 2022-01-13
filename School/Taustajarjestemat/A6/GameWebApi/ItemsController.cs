using Microsoft.AspNetCore.Mvc;
using System;

using System.Threading.Tasks;

namespace GameWebApi
{

    [Route("api/players/{playerId}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private IRepository _repoManager;

        public ItemsController(IRepository repoManager)
        {
            _repoManager = repoManager;
        }

        [HttpGet]
        public Task<Item[]> GetAllItems(Guid playerId)
        {
            return _repoManager.GetAllItems(playerId);
        }

        
        
        [HttpPost]
        public async Task<Item> CreateItem(Guid playerId, [FromBody] Item item)
        {
            Player player = await _repoManager.Get(playerId);
            if (player.Level < 3 && item.itemType == ItemType.SWORD)
            {
                throw new ExpectationFilt();
            }
            else
            {
                return await _repoManager.CreateItem(playerId, item);
            }
        }

        [HttpPost("{itemId}")]
        public Task<Item> GetItem(Guid playerId, Guid itemId)
        {

            return _repoManager.GetItem(playerId, itemId);

        }
        [HttpPut]
        public Task<Item> UpdateItem(Guid playerId, [FromBody] Item item)
        {

            return _repoManager.UpdateItem(playerId, item);

        }
        [HttpDelete]
        public Task<Item> DeleteItem(Guid playerId, [FromBody] Guid itemId)
        {

            return _repoManager.DeleteItem(playerId, itemId);
        }


    }
}