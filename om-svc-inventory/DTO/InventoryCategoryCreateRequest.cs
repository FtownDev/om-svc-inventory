using System.ComponentModel.DataAnnotations;

namespace om_svc_inventory.DTO
{
    public class InventoryCategoryCreateRequest
    {
        [Required]
        public string Name { get; set; }

        public string ? Description { get; set; }
    }
}
