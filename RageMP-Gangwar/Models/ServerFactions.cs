using GTANetworkAPI;
using Newtonsoft.Json;
using RageMP_Gangwar.dbmodels;
using RageMP_Gangwar.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RageMP_Gangwar.Models
{
    class ServerFactions
    {
        public static List<dbmodels.ServerFactions> ServerFactions_ = new List<dbmodels.ServerFactions>();
        public static List<dbmodels.ServerFactionsClothes> ServerFactionsClothes_ = new List<dbmodels.ServerFactionsClothes>();
        public static List<dbmodels.ServerFactionsVehicles> ServerFactionsVehicles_ = new List<dbmodels.ServerFactionsVehicles>();
        public static List<dbmodels.ServerFactionsGarage> ServerFactionsGarage_ = new List<dbmodels.ServerFactionsGarage>();

        public static string GetAllFactionGarageInfo()
        {
            var json = ServerFactionsGarage_.Select(x => new
            {
                x.pedX,
                x.pedY,
                x.pedZ,
                x.pedModel,
                x.pedRot,
            }).ToList();

            return JsonConvert.SerializeObject(json);
        }

        public static void GetAllFactionInformations(Player player)
        {
            try
            {
                if (player == null || !player.Exists) return;
                var items = ServerFactions_.Select(x => new
                {
                    x.factionId,
                    x.factionName,
                    x.isPrivate,
                    memberCount = GetFactionMemberCount(x.factionId),
                }).ToList();

                var itemCount = (int)items.Count;
                var iterations = Math.Floor((decimal)(itemCount / 5));
                var rest = itemCount % 5;
                for (var i = 0; i < iterations; i++)
                {
                    var skip = i * 5;
                    player.TriggerEvent("Player:TeamBrowser:fillInformations", JsonConvert.SerializeObject(items.Skip(skip).Take(5).ToList()));
                }

                if (rest != 0) player.TriggerEvent("Player:TeamBrowser:fillInformations", JsonConvert.SerializeObject(items.Skip((int)iterations * 5).ToList()));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void GetAllFactionVehicles(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                var items = ServerFactionsVehicles_.Select(x => new
                {
                    x.displayName,
                    x.type,
                    x.neededLevel,
                }).OrderBy(x => x.neededLevel).ToList();

                var itemCount = (int)items.Count;
                var iterations = Math.Floor((decimal)(itemCount / 10));
                var rest = itemCount % 10;

                for (var i = 0; i < iterations; i++)
                {
                    var skip = i * 10;
                    player.TriggerEvent("Player:Garage:setVehicles", JsonConvert.SerializeObject(items.Skip(skip).Take(10).ToList()));
                }

                if (rest != 0) player.TriggerEvent("Player:Garage:setVehicles", JsonConvert.SerializeObject(items.Skip((int)iterations * 10).ToList()));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static bool ExistFactionGarage(int id)
        {
            try
            {
                var garage = ServerFactionsGarage_.FirstOrDefault(x => x.id == id);
                if (garage != null) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static Vector3 GetFactionGarageSpawnPos(int id)
        {
            try
            {
                var garage = ServerFactionsGarage_.FirstOrDefault(x => x.id == id);
                if (garage != null) return new Vector3(garage.spawnX, garage.spawnY, garage.spawnZ);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return new Vector3(0, 0, 0);
        }

        public static float GetFactionGarageSpawnRot(int id)
        {
            try
            {
                var garage = ServerFactionsGarage_.FirstOrDefault(x => x.id == id);
                if (garage != null) return garage.spawnRot;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static uint GetFactionVehicleHashByName(string displayName)
        {
            try
            {
                var vehData = ServerFactionsVehicles_.FirstOrDefault(x => x.displayName == displayName);
                if (vehData != null) return vehData.hash;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static int GetFactionVehicleLevel(uint hash)
        {
            try
            {
                var vehData = ServerFactionsVehicles_.FirstOrDefault(x => x.hash == hash);
                if (vehData != null) return vehData.neededLevel;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static int GetFactionBlipColor(int facId)
        {
            try
            {
                var data = ServerFactions_.FirstOrDefault(x => x.factionId == facId);
                if (data != null) return data.blipColor;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static int GetFactionColor(int factionId)
        {
            try
            {
                var data = ServerFactions_.FirstOrDefault(x => x.factionId == factionId);
                if (data != null) return data.color;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static uint GetFactionVehicleHashByDisplayName(string displayName)
        {
            try
            {
                var vehicleData = ServerFactionsVehicles_.FirstOrDefault(x => x.displayName == displayName);
                if (vehicleData != null) return vehicleData.hash;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static string GetFactionName(int factionId)
        {
            try
            {
                var faction = ServerFactions_.FirstOrDefault(x => x.factionId == factionId);
                if (faction != null) return faction.factionName;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static int GetFactionMemberCount(int factionId)
        {
            return NAPI.Pools.GetAllPlayers().Where(x => x != null && x.Exists && x.hasAccountId() && ServerAccounts.GetAccountSelectedTeam(x.getAccountId()) == factionId).Count();
        }

        public static bool ExistFaction(int id)
        {
            try
            {
                var faction = ServerFactions_.FirstOrDefault(x => x.factionId == id);
                if (faction != null) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static bool IsFactionPrivate(int id)
        {
            try
            {
                var faction = ServerFactions_.FirstOrDefault(x => x.factionId == id);
                if (faction != null) return faction.isPrivate;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static Vector3 GetFactionSpawn(int id)
        {
            try
            {
                var faction = ServerFactions_.FirstOrDefault(x => x.factionId == id);
                if (faction != null) return new Vector3(faction.spawnX, faction.spawnY, faction.spawnZ);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return new Vector3(0, 0, 0);
        }

        public static ServerFactionsClothes GetFactionsClothes(int factionId)
        {
            var faction = ServerFactionsClothes_.FirstOrDefault(x => x.factionId == factionId);
            return faction;
        }
    }
}
