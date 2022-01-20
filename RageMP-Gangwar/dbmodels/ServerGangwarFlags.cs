using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RageMP_Gangwar.dbmodels
{
    public partial class ServerGangwarFlags
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int zoneId { get; set; }
        public float flagX { get; set; }
        public float flagY { get; set; }
        public float flagZ { get; set; }

        [NotMapped]
        public int currentOwner { get; set; } = 0;
    }
}