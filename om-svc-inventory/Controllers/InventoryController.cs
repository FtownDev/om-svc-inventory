using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using om_svc_inventory.Data;
using om_svc_inventory.DTO;
using om_svc_inventory.Models;
using System.Net;

namespace om_svc_inventory.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public InventoryController(InventoryDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("category/all")]
        public async Task<IActionResult> GetAllInventoryCategoriesAsync(CancellationToken cancellationToken)
        {
            ActionResult retval;
            var categoryList = await this._context.InventoryCategories.OrderBy(x => x.Name).ToListAsync();
            if (!categoryList.Any() || categoryList.Count == 0)
            {
                retval = this.StatusCode((int)HttpStatusCode.InternalServerError, "Unable to retrieve categories");
            }
            else
            {
                retval = this.Ok(categoryList);
            }
            return retval;
        }

        [HttpGet]
        [Route("items/all")]
        public async Task<IActionResult> GetAllInventoryItemsAsync(CancellationToken cancellationToken)
        {
            ActionResult retval;
            var itemList = await this._context.InventoryItems.OrderBy(x => x.CategoryId).ToListAsync();

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
                retval = this.Ok(newCategory);
            }

            return retval;
        }


    }
}
