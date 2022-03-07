using System.ComponentModel.DataAnnotations;
namespace Own.Database
{
    public partial class Account
    {
        [Key]
        public int Id { get; set; }
        public string Nick { get; set; }
        public int Identifier { get; set; }
        public int Passphrase { get; set; }
        public int SecretHash { get; set; }
    }
}
