using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class Msg
    {
        [Key]
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public int AccountId { get; set; }
        public string MsgText { get; set; }
    }
}
