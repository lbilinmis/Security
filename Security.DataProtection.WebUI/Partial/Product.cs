using System.ComponentModel.DataAnnotations.Schema;

namespace Security.DataProtection.WebUI.Models
{
    public partial class Product
    {
        [NotMapped]
        public string EncryptedId { get; set; }

    }
}
