using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using om_svc_inventory.Data;
using om_svc_inventory.DTO;
using om_svc_inventory.Models;
using om_svc_inventory.Services;
using System.Net;

namespace om_svc_inventory.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly ICacheService _cacheService;
        public InventoryController(InventoryDbContext context, ICacheService cache)
        {
            this._context = context;
            this._cacheService = cache;
        }

        [HttpGet]
        [Route("category/all")]
        public async Task<IActionResult> GetAllInventoryCategoriesAsync(CancellationToken cancellationToken)
        {
            ActionResult retval;
            var cacheList = _cacheService.GetData<IEnumerable<InventoryCategory>>(key: "category/all");

            if (cacheList != null) 
            {
                retval = this.Ok(cacheList);
            }
            else
            {
                var categoryList = await this._context.InventoryCategories.OrderBy(x => x.Name).ToListAsync();
                if (!categoryList.Any() || categoryList.Count == 0)
                {
                    retval = this.StatusCode((int)HttpStatusCode.InternalServerError, "Unable to retrieve categories");
                }
                else
                {
                    _cacheService.SetData("category/all", categoryList, 10);
                    retval = this.Ok(categoryList);
                }
            }
           
            return retval;
        }

        [HttpGet]
        [Route("items/all")]
        public async Task<IActionResult> GetAllInventoryItemsAsync(CancellationToken cancellationToken)
        {
            ActionResult retval;
            var cacheList = _cacheService.GetData<IEnumerable<InventoryItem>>(key: "items/all");
            if (cacheList != null)
            {
                retval = this.Ok(cacheList);
            }
            else
            {
                var itemList = await this._context.InventoryItems.OrderBy(x => x.CategoryId).ToListAsync();

                if (!itemList.Any() || itemList.Count == 0)
                {
                    retval = this.StatusCode((int)HttpStatusCode.InternalServerError, "Unable to retrieve items");
                }
                else
                {
                    _cacheService.SetData("items/all", itemList, 10);
                    retval = this.Ok(itemList);
                }
            }

            

            return retval;
        }

        [HttpGet]
        [Route("items/category/{categoryId}")]
        public async Task<IActionResult> GetInventoryItemsByCategoryAsync(Guid categoryId)
        {
            ActionResult retval;
            var itemList = await this._context.InventoryItems.Where(x => x.CategoryId == categoryId).ToListAsync();

            if (!itemList.Any() || itemList.Count == 0)
            {
                retval = this.StatusCode((int)HttpStatusCode.InternalServerError, "Unable to retrieve items");
            }
            else
            {
                retval = this.Ok(itemList);
            }

            return retval;
        }


        [HttpPost]
        [Route("items/category/{categoryId}")]
        public async Task<IActionResult> CreateInventoryItem(InventoryItemCreateRequest item, [FromRoute] Guid categoryId)
        {
            ActionResult retval;

            var itemCategory = await this._context.InventoryCategories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

            if (itemCategory == null)
            {
                retval = this.BadRequest("Item category does not exist");
            }
            else 
            {
                InventoryItem newItem = new()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = itemCategory.Id,
                    Price = item.Price,
                    Name = item.Name,
                };

                await this._context.InventoryItems.AddAsync(newItem);

                var result = this._context.SaveChanges() > 0;

                if (!result)
                {
                    retval = this.StatusCode((int)HttpStatusCode.InternalServerError, "Unable to create new item");
                }
                else
                {
                    _cacheService.InvalidateKeys(new List<string>() { "items/all", $"items/category{categoryId}" });
                    retval = this.Ok(item);
                }
            }

            return retval;
        }


        [HttpPost]
        [Route("category")]
        public async Task<IActionResult> CreateItemCategory(InventoryCategoryCreateRequest category)
        {
            ActionResult retval;

            InventoryCategory newCategory = new() {
                Id = Guid.NewGuid(),
                Name = category.Name,
                Description = category.Description,
            };

            await this._context.InventoryCategories.AddAsync(newCategory);

            var result = this._context.SaveChanges() > 0;

            if (!result)
            {
                retval = this.StatusCode((int)HttpStatusCode.InternalServerError, "Unable to create new category");
            }
            else
            {
                _cacheService.InvalidateKeys(new List<string>() { "category/all" });
                retval = this.Ok(newCategory);
            }

            return retval;
        }


    }
}
