using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RageMP_Gangwar.dbmodels
{
    public partial class ServerGangwarZones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string zoneName { get; set; }
        public int currentOwner { get; set; }
        public float attackPosX { get; set; }
        public float attackPosY { get; set; }
        public float attackPosZ { get; set; }
        public bool isPrivate { get; set; }

        [NotMapped]
        public bool underAttack { get; set; } = false;

        [NotMapped]
        public int attacker { get; set; } = 0;

        [NotMapped]
        public int currentAttackerPoints { get; set; } = 0;

        [NotMapped]
        public int currentDefenderPoints { get; set; } = 0;

        [NotMapped]
        public int timePlayed { get; set; } = 0;
    }
}