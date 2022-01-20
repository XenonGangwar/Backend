using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RageMP_Gangwar.dbmodels
{
    public partial class Accounts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int playerId { get; set; }

        public string playerName { get; set; }

        public string password { get; set; }
        public string socialClub { get; set; }
        public string hwId { get; set; }

        public bool hasAufnahmePflicht { get; set; }

        public bool mute { get; set; }

        public int money { get; set; }

        public int selectedKit { get; set; }
        public bool isBanned { get; set; }
        public bool changedNameAlready { get; set; }
        public int timebanHours { get; set; }
        public bool isTimeBanned { get; set; }
        public DateTime banTimestamp { get; set; }
        public int warns { get; set; }
        public int adminRank { get; set; }
        public int level { get; set; }
        public int exp { get; set; }
        public int prestigeLevel { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int faction { get; set; }
        public int factionRank { get; set; }
        public DateTime lastGift { get; set; }
        public int lockState { get; set; }

        [NotMapped]
        public int curSelectedTeam { get; set; } = 0;

        [NotMapped]
        public bool isMuted { get; set; } = false;

        [NotMapped]
        public int ffaArena { get; set; } = 0;

        [NotMapped]
        public bool isAduty { get; set; } = false;

        [NotMapped]
        public int currentDuellPartner { get; set; } = 0;

        [NotMapped]
        public bool duellDisabled { get; set; } = false;

        [NotMapped]
        public bool isInEvent { get; set; } = false;
    }
}