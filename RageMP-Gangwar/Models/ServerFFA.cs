using GTANetworkAPI;
using RageMP_Gangwar.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RageMP_Gangwar.Models
{
    public class ServerFFA
    {
        public static List<dbmodels.ServerFFA> ServerFFA_ = new List<dbmodels.ServerFFA>();
        public static List<dbmodels.ServerFFASpawns> ServerFFASpawns_ = new List<dbmodels.ServerFFASpawns>();

        public static void CreateFFAZone(Player player, string name, int dimension)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || player.getAdminLevel() < 9) return;
                var ffaData = new dbmodels.ServerFFA
                {
                    name = name,
                    posX = player.Position.X,
                    posY = player.Position.Y,
                    posZ = player.Position.Z - 1,
                    currentPlayers = 0,
                    dimension = dimension
                };

                ServerFFA_.Add(ffaData);
                using (dbmodels.gtaContext db = new dbmodels.gtaContext())
                {
                    db.ServerFFA.Add(ffaData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void CreateFFASpawn(Player player, int id)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || player.getAdminLevel() < 9) return;
                var ffaData = new dbmodels.ServerFFASpawns
                {
                    zoneId = id,
                    posX = player.Position.X,
                    posY = player.Position.Y,
                    posZ = player.Position.Z
                };

                ServerFFASpawns_.Add(ffaData);
                using (dbmodels.gtaContext db = new dbmodels.gtaContext())
                {
                    db.ServerFFASpawns.Add(ffaData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static dbmodels.ServerFFA GetFullFFAEntry(string arenaName)
        {
            var arenaData = ServerFFA_.FirstOrDefault(x => x.name == arenaName);
            return arenaData;
        }

        public static Vector3 GetRandomFFAZonePosition(int arenaId)
        {
            try
            {
                int random = new Random().Next(1, ServerFFASpawns_.Count - 1);
                var arenaData = ServerFFASpawns_.FirstOrDefault(x => x.id == random);
                if (arenaData != null) return new Vector3(arenaData.posX, arenaData.posY, arenaData.posZ);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return new Vector3(0, 0, 0);
        }

        public static void IncreaseFFAPlayer(int arenaId)
        {
            try
            {
                var arenaData = ServerFFA_.FirstOrDefault(x => x.id == arenaId);
                if (arenaData != null) arenaData.currentPlayers += 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void DecreaseFFAPlayer(int arenaId)
        {
            try
            {
                var arenaData = ServerFFA_.FirstOrDefault(x => x.id == arenaId);
                if (arenaData != null) arenaData.currentPlayers -= 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetFFAZoneDimension(int arenaId)
        {
            try
            {
                var arenaData = ServerFFA_.FirstOrDefault(x => x.id == arenaId);
                if (arenaData != null) return arenaData.dimension;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }
    }
}
