using Microsoft.AspNetCore.Mvc;
using System;

using System.Threading.Tasks;

namespace GameWebApi
{
    [Route("api/players/{playerId}/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        IRepository _repoManager = new FileRepository();

        [HttpPost]
        Task<Item[]> GetAllItems(Guid playerId)
        {
            try
            {
                return _repoManager.GetAllItems(playerId);
            }
            catch (System.Exception)
            {

                throw new NotFoundException();
            }

        }

        [HttpPost]
        Task<Item> CreateItem(Guid playerId, [FromBody] Item item)
        {
            try
            {
                return _repoManager.CreateItem(playerId, item);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpPost("{itemId}")]
        Task<Item> GetItem(Guid playerId,Guid itemId)
        {
            try
            {
                return _repoManager.GetItem(playerId,itemId);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpPut]
        Task<Item> UpdateItem(Guid playerId,[FromBody] Item item)
        {
            try
            {
                return _repoManager.UpdateItem(playerId,item);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpDelete]
        Task<Item> DeleteItem(Guid playerId,[FromBody] Guid itemId)
        {
            try
            {
                return _repoManager.DeleteItem(playerId,itemId);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }


    }
}