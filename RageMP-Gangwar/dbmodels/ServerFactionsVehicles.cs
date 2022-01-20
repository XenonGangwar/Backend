using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RageMP_Gangwar.dbmodels
{
    public partial class ServerFactionsVehicles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public uint hash { get; set; }
        public string displayName { get; set; }
        public string type { get; set; } //bike, car
        public int neededLevel { get; set; }
    }
}