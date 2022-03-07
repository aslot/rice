using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class Thread
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int EndpointId { get; set; }
    }
}
