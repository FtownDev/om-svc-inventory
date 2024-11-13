using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace om_svc_inventory.DTO
{
    public class InventoryItemCreateRequest
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public required decimal Price { get; set; }
    }
}
