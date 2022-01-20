using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RageMP_Gangwar.dbmodels
{
    public partial class ServerFactionsClothes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int factionId { get; set; }
        public int hat { get; set; }
        public int hatTex { get; set; }
        public int mask { get; set; }
        public int maskTex { get; set; }
        public int top { get; set; }
        public int topTex { get; set; }
        public int undershirt { get; set; }
        public int undershirtTex { get; set; }
        public int leg { get; set; }
        public int legTex { get; set; }
        public int shoes { get; set; }
        public int shoesTex { get; set; }
        public int bag { get; set; }
        public int bagTex { get; set; }
        public int glasses { get; set; }
        public int glassesTex { get; set; }
        public int accessories { get; set; }
        public int accessoriesTex { get; set; }
        public int torso { get; set; }
    }
}