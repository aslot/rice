using System.ComponentModel.DataAnnotations;
namespace Models
{
    public partial class PrivateMessage
    {
        [Key]
        public int Id { get; set; }
        public int SenderAccountId { get; set; }
        public int AcceptorAccountId { get; set; }
        public string PrivateText { get; set; }
    }
}
