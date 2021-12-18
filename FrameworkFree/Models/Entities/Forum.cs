using System.ComponentModel.DataAnnotations;
namespace Models
{
    public partial class Forum
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
