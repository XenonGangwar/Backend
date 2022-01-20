using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RageMP_Gangwar.dbmodels
{
    public partial class AdminLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int accountId { get; set; }
        public int targetId { get; set; }
        public string action { get; set; }
        public string description { get; set; }
        public DateTime timestamp { get; set; }
    }
}