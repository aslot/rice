using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Own.Database
{
    [Table("Endpoint_")]
    public partial class Endpoint
    {
        [Key]
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string Name { get; set; }
    }
}
