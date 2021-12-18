using System.ComponentModel.DataAnnotations;
namespace Models
{
    public partial class BlockedIpHash
    {
        [Key]
        public int Id { get; set; }
        public int IpHash { get; set; }
    }
}