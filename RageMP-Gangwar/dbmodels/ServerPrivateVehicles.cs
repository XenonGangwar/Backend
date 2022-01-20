using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RageMP_Gangwar.dbmodels
{
    public partial class ServerPrivateVehicles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int accountId { get; set; }
        public uint hash { get; set; }
        public string displayName { get; set; }
    }
}