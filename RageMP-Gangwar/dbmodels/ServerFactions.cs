using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RageMP_Gangwar.dbmodels
{
    public partial class ServerFactions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int factionId { get; set; }

        public string factionName { get; set; }
        public float spawnX { get; set; }
        public float spawnY { get; set; }
        public float spawnZ { get; set; }
        public bool isPrivate { get; set; }
        public int color { get; set; }
        public int blipColor { get; set; }

        [NotMapped]
        public int currentMembers { get; set; }
    }
}