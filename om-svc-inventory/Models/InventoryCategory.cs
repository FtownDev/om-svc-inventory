using System.ComponentModel.DataAnnotations;

namespace om_svc_inventory.Models
{
    public class InventoryCategory
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
