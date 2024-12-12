using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace om_svc_inventory.Models
{
    public class InventoryItem
    {
        public Guid Id { get; set; }

        [Required]
        public required Guid CategoryId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public required decimal Price { get; set; }
    }
}
