using Microsoft.EntityFrameworkCore;
using RageMP_Gangwar.Utilities;

namespace RageMP_Gangwar.dbmodels
{
    public partial class gtaContext : DbContext
    {
        public gtaContext() { }
        public gtaContext(DbContextOptions<gtaContext> options) : base(options) { }
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<AdminLogs> AdminLogs { get; set; }
        public virtual DbSet<ServerBlips> ServerBlips { get; set; }
        public virtual DbSet<ServerFactions> ServerFactions { get; set; }
        public virtual DbSet<ServerFactionsClothes> ServerFactionsClothes { get; set; }
        public virtual DbSet<ServerFactionsGarage> ServerFactionsGarages { get; set; }
        public virtual DbSet<ServerFactionsVehicles> ServerFactionsVehicles { get; set; }
        public virtual DbSet<ServerFFA> ServerFFA { get; set; }
        public virtual DbSet<ServerFFASpawns> ServerFFASpawns { get; set; }
        public virtual DbSet<ServerGangwarFlags> ServerGangwarFlags { get; set; }
        public virtual DbSet<ServerGangwarZones> ServerGangwarZones { get; set; }
        public virtual DbSet<ServerPrestigeVehicles> ServerPrestigeVehicles { get; set; }
        public virtual DbSet<ServerPrivateVehicles> ServerPrivateVehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql($"server={Constants.DatabaseConfig.Host};port={Constants.DatabaseConfig.Port};user={Constants.DatabaseConfig.User};password={Constants.DatabaseConfig.Password};database={Constants.DatabaseConfig.Database}");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.HasKey(e => e.playerId);
                entity.ToTable("accounts", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.playerId).HasName("id");
                entity.Property(e => e.playerId).HasColumnName("id").HasColumnType("int(11)");
                entity.Property(e => e.playerName).HasColumnName("username").HasMaxLength(128);
                entity.Property(e => e.socialClub).HasColumnName("socialClub").HasMaxLength(128);
                entity.Property(e => e.hwId).HasColumnName("hwId").HasMaxLength(512);
                entity.Property(e => e.changedNameAlready).HasColumnName("changedNameAlready");

                entity.Property(e => e.isBanned).HasColumnName("ban");
                entity.Property(e => e.isTimeBanned).HasColumnName("isTimeban");
                entity.Property(e => e.timebanHours).HasColumnName("timebanHours");
                entity.Property(e => e.banTimestamp).HasColumnName("banTimestamp");
                entity.Property(e => e.warns).HasColumnName("warns").HasColumnType("int(11)");
                entity.Property(e => e.adminRank).HasColumnName("adminrank").HasColumnType("int(11)");
                entity.Property(e => e.level).HasColumnName("level").HasColumnType("int(11)");
                entity.Property(e => e.exp).HasColumnName("exp").HasColumnType("int(11)");
                entity.Property(e => e.prestigeLevel).HasColumnName("prestigeLevel").HasColumnType("int(11)");
                entity.Property(e => e.kills).HasColumnName("kills").HasColumnType("int(11)");
                entity.Property(e => e.deaths).HasColumnName("deaths").HasColumnType("int(11)");
                entity.Property(e => e.faction).HasColumnName("faction").HasColumnType("int(11)");
                entity.Property(e => e.factionRank).HasColumnName("factionrank").HasColumnType("int(11)");
                entity.Property(e => e.lastGift).HasColumnName("lastGift");
                entity.Property(e => e.lockState).HasColumnName("lockState");
            });

            modelBuilder.Entity<AdminLogs>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("admin_logs", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.accountId).HasColumnName("accountId");
                entity.Property(e => e.targetId).HasColumnName("targetId");
                entity.Property(e => e.action).HasColumnName("action");
                entity.Property(e => e.description).HasColumnName("description");
                entity.Property(e => e.timestamp).HasColumnName("timestamp");
            });

            modelBuilder.Entity<ServerBlips>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_blips", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id").HasColumnType("int(11)");
                entity.Property(e => e.name).HasColumnName("name").HasMaxLength(128);
                entity.Property(e => e.sprite).HasColumnName("sprite").HasColumnType("int(11)");
                entity.Property(e => e.color).HasColumnName("color").HasColumnType("int(11)");
                entity.Property(e => e.scale).HasColumnName("scale");
                entity.Property(e => e.shortRange).HasColumnName("shortRange");
                entity.Property(e => e.alpha).HasColumnName("alpha").HasColumnType("int(11)");
                entity.Property(e => e.posX).HasColumnName("posX");
                entity.Property(e => e.posY).HasColumnName("posY");
                entity.Property(e => e.posZ).HasColumnName("posZ");
                entity.Property(e => e.isActive).HasColumnName("isActive");
            });

            modelBuilder.Entity<ServerFactions>(entity =>
            {
                entity.HasKey(e => e.factionId);
                entity.ToTable("server_factions", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.factionId).HasName("id");
                entity.Property(e => e.factionId).HasColumnName("id").HasColumnType("int(11)");
                entity.Property(e => e.factionName).HasColumnName("name").HasMaxLength(128);
                entity.Property(e => e.spawnX).HasColumnName("spawnX");
                entity.Property(e => e.spawnY).HasColumnName("spawnY");
                entity.Property(e => e.spawnZ).HasColumnName("spawnZ");
                entity.Property(e => e.isPrivate).HasColumnName("private");
                entity.Property(e => e.color).HasColumnName("color");
                entity.Property(e => e.blipColor).HasColumnName("blipColor");
            });

            modelBuilder.Entity<ServerFactionsClothes>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_factions_clothes", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id").HasColumnType("int(11)");
                entity.Property(e => e.factionId).HasColumnName("factionId").HasColumnType("int(11)");
                entity.Property(e => e.hat).HasColumnName("hat").HasColumnType("int(11)");
                entity.Property(e => e.hatTex).HasColumnName("hatTex").HasColumnType("int(11)");
                entity.Property(e => e.mask).HasColumnName("mask").HasColumnType("int(11)");
                entity.Property(e => e.maskTex).HasColumnName("maskTex").HasColumnType("int(11)");
                entity.Property(e => e.top).HasColumnName("top").HasColumnType("int(11)");
                entity.Property(e => e.topTex).HasColumnName("topTex").HasColumnType("int(11)");
                entity.Property(e => e.undershirt).HasColumnName("undershirt").HasColumnType("int(11)");
                entity.Property(e => e.undershirtTex).HasColumnName("undershirtTex").HasColumnType("int(11)");
                entity.Property(e => e.leg).HasColumnName("leg").HasColumnType("int(11)");
                entity.Property(e => e.legTex).HasColumnName("legTex").HasColumnType("int(11)");
                entity.Property(e => e.shoes).HasColumnName("shoes").HasColumnType("int(11)");
                entity.Property(e => e.shoesTex).HasColumnName("shoesTex").HasColumnType("int(11)");
                entity.Property(e => e.bag).HasColumnName("bag").HasColumnType("int(11)");
                entity.Property(e => e.bagTex).HasColumnName("bagTex").HasColumnType("int(11)");
                entity.Property(e => e.glasses).HasColumnName("glasses");
                entity.Property(e => e.glassesTex).HasColumnName("glassesTex");
                entity.Property(e => e.accessories).HasColumnName("accessories");
                entity.Property(e => e.accessoriesTex).HasColumnName("accessoriesTex");
                entity.Property(e => e.torso).HasColumnName("torso");
            });

            modelBuilder.Entity<ServerFactionsGarage>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_factions_garage", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.factionId).HasColumnName("factionId").HasColumnType("int(11)");
                entity.Property(e => e.pedX).HasColumnName("pedX");
                entity.Property(e => e.pedY).HasColumnName("pedY");
                entity.Property(e => e.pedZ).HasColumnName("pedZ");
                entity.Property(e => e.spawnX).HasColumnName("spawnX");
                entity.Property(e => e.spawnY).HasColumnName("spawnY");
                entity.Property(e => e.spawnZ).HasColumnName("spawnZ");
                entity.Property(e => e.pedModel).HasColumnName("pedModel");
                entity.Property(e => e.pedRot).HasColumnName("pedRot");
                entity.Property(e => e.spawnRot).HasColumnName("spawnRot");
            });

            modelBuilder.Entity<ServerFactionsVehicles>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_factions_vehicles", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id").HasColumnType("int(11)");
                entity.Property(e => e.hash).HasColumnName("hash");
                entity.Property(e => e.displayName).HasColumnName("displayName");
                entity.Property(e => e.type).HasColumnName("type");
                entity.Property(e => e.neededLevel).HasColumnName("neededLevel");
            });

            modelBuilder.Entity<ServerFFA>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_ffa", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id").HasColumnType("int(11)");
                entity.Property(e => e.name).HasColumnName("name").HasMaxLength(128);
                entity.Property(e => e.posX).HasColumnName("posX");
                entity.Property(e => e.posY).HasColumnName("posY");
                entity.Property(e => e.posZ).HasColumnName("posZ");
                entity.Property(e => e.dimension).HasColumnName("dimension");
            });

            modelBuilder.Entity<ServerFFASpawns>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_ffa_spawns", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id").HasColumnType("int(11)");
                entity.Property(e => e.zoneId).HasColumnName("zoneId").HasColumnType("int(11)");
                entity.Property(e => e.posX).HasColumnName("posX");
                entity.Property(e => e.posY).HasColumnName("posY");
                entity.Property(e => e.posZ).HasColumnName("posZ");
            });

            modelBuilder.Entity<ServerGangwarFlags>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_gangwarflags", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id").HasColumnType("int(11)");
                entity.Property(e => e.zoneId).HasColumnName("zoneId").HasColumnType("int(11)");
                entity.Property(e => e.flagX).HasColumnName("flagX");
                entity.Property(e => e.flagY).HasColumnName("flagY");
                entity.Property(e => e.flagZ).HasColumnName("flagZ");
            });

            modelBuilder.Entity<ServerGangwarZones>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_gangwarzones", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id").HasColumnType("int(11)");
                entity.Property(e => e.zoneName).HasColumnName("zoneName").HasMaxLength(128);
                entity.Property(e => e.currentOwner).HasColumnName("currentOwner");
                entity.Property(e => e.attackPosX).HasColumnName("attackPosX");
                entity.Property(e => e.attackPosY).HasColumnName("attackPosY");
                entity.Property(e => e.attackPosZ).HasColumnName("attackPosZ");
                entity.Property(e => e.isPrivate).HasColumnName("isPrivate");
            });

            modelBuilder.Entity<ServerPrestigeVehicles>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_prestige_vehicles", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.hash).HasColumnName("hash");
                entity.Property(e => e.displayName).HasColumnName("displayName");
                entity.Property(e => e.neededLevel).HasColumnName("neededLevel");
            });

            modelBuilder.Entity<ServerPrivateVehicles>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.ToTable("server_private_vehicles", Constants.DatabaseConfig.Database);
                entity.HasIndex(e => e.id).HasName("id");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.accountId).HasColumnName("accountId");
                entity.Property(e => e.hash).HasColumnName("hash");
                entity.Property(e => e.displayName).HasColumnName("displayName");
            });
        }
    }
}
