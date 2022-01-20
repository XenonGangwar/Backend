using GTANetworkAPI;
using RageMP_Gangwar.dbmodels;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;
using System.Linq;

namespace RageMP_Gangwar.Handler
{
    public class GarageHandler : Script
    {
        public static void openBrowser(Player player, ServerFactionsGarage factionGarage)
        {
            try
            {
                if (player == null || factionGarage == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0) return;
                player.TriggerEvent("Player:Garage:clearInfo");
                ServerVehicles.GetPrivateVehicles(player);
                ServerVehicles.GetPrestigeVehicles(player);
                Models.ServerFactions.GetAllFactionVehicles(player);
                player.TriggerEvent("Player:Garage:openBrowser", factionGarage.id);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [RemoteEvent("Server:Garage:takeVehicle")]
        public void Garage_TakeVeh(Player player, string displayName, int type, int garageId)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || ServerAccounts.GetAccountSelectedTeam(pID) <= 0) return;
                if (!Models.ServerFactions.ExistFactionGarage(garageId)) return;
                uint hash = 0;
                switch (type)
                {
                    case 1:
                        {
                            hash = Models.ServerFactions.GetFactionVehicleHashByName(displayName);
                            if (hash == 0 || Models.ServerFactions.GetFactionVehicleLevel(hash) == 0 || Models.ServerFactions.GetFactionVehicleLevel(hash) > Models.ServerAccounts.GetPlayerLevel(pID))
                            {
                                NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Xenon~w~] Für dieses Fahrzeug reicht dein Level nicht aus.");
                                return;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (!ServerVehicles.ExistPrivateVehicleByDisplayName(pID, displayName)) return;
                            hash = ServerVehicles.GetPrivateVehicleHashByName(pID, displayName);
                            break;
                        }
                    case 3:
                        {
                            if (!ServerVehicles.HasPlayerAccessToPrestigeCar(ServerAccounts.GetPrestigeLevel(pID), displayName)) return;
                            hash = ServerVehicles.GetPrestigeVehicleHashByName(displayName);
                            if (hash == 0 || Models.ServerVehicles.GetPrestigeVehicleLevel(hash) == 0 || Models.ServerVehicles.GetPrestigeVehicleLevel(hash) > Models.ServerAccounts.GetPrestigeLevel(pID))
                            {
                                NAPI.Chat.SendChatMessageToPlayer(player, $"[~~Xenon~w~] Für dieses Fahrzeug reicht dein Prestige-Level nicht aus.");
                                return;
                            }
                            break;
                        }
                }
                Vector3 spawn = Models.ServerFactions.GetFactionGarageSpawnPos(garageId);
                float rotation = Models.ServerFactions.GetFactionGarageSpawnRot(garageId);
                if (spawn == new Vector3(0, 0, 0) || hash == 0) return;
                var alreadyVehicle = NAPI.Pools.GetAllVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && x.HasData("ownerId") && x.GetData("ownerId") == pID);
                if (alreadyVehicle != null) alreadyVehicle.Delete();
                Vehicle veh = NAPI.Vehicle.CreateVehicle(hash, spawn, rotation, Models.ServerFactions.GetFactionColor(ServerAccounts.GetAccountSelectedTeam(pID)), Models.ServerFactions.GetFactionColor(ServerAccounts.GetAccountSelectedTeam(pID)), Models.ServerFactions.GetFactionName(ServerAccounts.GetAccountSelectedTeam(pID)), 255, false, true, player.Dimension);
                veh.SetData("ownerId", pID);
                player.SetIntoVehicle(veh, -1);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
