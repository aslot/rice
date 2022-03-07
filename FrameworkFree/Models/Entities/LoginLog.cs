using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class LoginLog
    {
        [Key]
        public int Id { get; set; }
        public int AccountIdentifier { get; set; }
        public int IpHash { get; set; }
    }
}
