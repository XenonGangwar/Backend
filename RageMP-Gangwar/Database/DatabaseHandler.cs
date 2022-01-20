using GTANetworkAPI;
using RageMP_Gangwar.dbmodels;
using RageMP_Gangwar.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RageMP_Gangwar.Database
{
    internal class DatabaseHandler
    {
        internal static void LoadAllLogs()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Models.AdminLogs.AdminLogs_ = new List<dbmodels.AdminLogs>(db.AdminLogs);
                    Console.WriteLine($"{Models.AdminLogs.AdminLogs_.Count} Admin-Logs wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void LoadAllAccounts()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerAccounts.Accounts_ = new List<Accounts>(db.Accounts);
                    Console.WriteLine($"{ServerAccounts.Accounts_.Count} Accounts wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void LoadAllServerFactions()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Models.ServerFactions.ServerFactions_ = new List<dbmodels.ServerFactions>(db.ServerFactions);
                    Console.WriteLine($"{Models.ServerFactions.ServerFactions_.Count} Server-Factions wurden geladen.");

                    Models.ServerFactions.ServerFactionsClothes_ = new List<dbmodels.ServerFactionsClothes>(db.ServerFactionsClothes);
                    Console.WriteLine($"{Models.ServerFactions.ServerFactionsClothes_.Count} Server-Faction-Clothes wurden geladen.");

                    Models.ServerFactions.ServerFactionsVehicles_ = new List<dbmodels.ServerFactionsVehicles>(db.ServerFactionsVehicles);
                    Console.WriteLine($"{Models.ServerFactions.ServerFactionsVehicles_.Count} Server-Faction-Vehicles wurden geladen.");

                    Models.ServerFactions.ServerFactionsGarage_ = new List<dbmodels.ServerFactionsGarage>(db.ServerFactionsGarages);
                    Console.WriteLine($"{Models.ServerFactions.ServerFactionsGarage_.Count} Server-Faction-Garages wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void LoadAllGangwarZones()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Models.ServerGWZones.ServerGangwarZones_ = new List<dbmodels.ServerGangwarZones>(db.ServerGangwarZones);
                    Console.WriteLine($"{Models.ServerGWZones.ServerGangwarZones_.Count} Server-Gangwar-Zonen wurden geladen.");

                    Models.ServerGWZones.ServerGangwarFlags_ = new List<dbmodels.ServerGangwarFlags>(db.ServerGangwarFlags);
                    Console.WriteLine($"{Models.ServerGWZones.ServerGangwarFlags_.Count} Server-Gangwar-Zonen-Flags wurden geladen.");
                }

                byte color = 0;
                foreach (var zone in Models.ServerGWZones.ServerGangwarZones_)
                {
                    if (zone.currentOwner > 0) color = Convert.ToByte(Models.ServerFactions.GetFactionBlipColor(zone.currentOwner));
                    Blip blip = NAPI.Blip.CreateBlip(84, new Vector3(zone.attackPosX, zone.attackPosY, zone.attackPosZ), 1.0f, color, $"Gangwar - {zone.zoneName}", 255, 0, true);
                    blip.SetData("GangwarBlip", zone.id);
                    Marker marker = NAPI.Marker.CreateMarker(MarkerType.VerticalCylinder, new Vector3(zone.attackPosX, zone.attackPosY, zone.attackPosZ - 1), new Vector3(zone.attackPosX, zone.attackPosY, zone.attackPosZ - 1), new Vector3(0, 0, 0), 1f, new Color(255, 162, 0, 200));

                    foreach (var flag in Models.ServerGWZones.ServerGangwarFlags_.Where(x => x.zoneId == zone.id))
                    {
                        Blip blips = NAPI.Blip.CreateBlip(164, new Vector3(flag.flagX, flag.flagY, flag.flagZ), 1.0f, color, $"Flagge", 255, 0, true);
                        blips.SetData("GangwarFlag", flag.id);
                        NAPI.Marker.CreateMarker(MarkerType.CheckeredFlagRect, new Vector3(flag.flagX, flag.flagY, flag.flagZ), new Vector3(flag.flagX, flag.flagY, flag.flagZ), new Vector3(0, 0, 0), 1f, new Color(255, 162, 0, 200));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void LoadAllServerPrestigeVehicles()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    ServerVehicles.ServerPrestigeVehicles_ = new List<dbmodels.ServerPrestigeVehicles>(db.ServerPrestigeVehicles);
                    Console.WriteLine($"{ServerVehicles.ServerPrestigeVehicles_.Count} Server-Prestige-Vehicles wurden geladen.");

                    ServerVehicles.ServerPrivateVehicles_ = new List<dbmodels.ServerPrivateVehicles>(db.ServerPrivateVehicles);
                    Console.WriteLine($"{ServerVehicles.ServerPrivateVehicles_.Count} Server-Prestige-Vehicles wurden geladen.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void LoadAllServerBlips()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Models.ServerBlips.ServerBlips_ = new List<dbmodels.ServerBlips>(db.ServerBlips);
                    Console.WriteLine($"{Models.ServerBlips.ServerBlips_.Count} Server-Blips wurden geladen.");
                }

                foreach (var x in Models.ServerBlips.ServerBlips_.Where(x => x.isActive))
                {
                    Blip blip = NAPI.Blip.CreateBlip(x.sprite, new Vector3(x.posX, x.posY, x.posZ), x.scale, (byte)x.color, x.name, (byte)x.alpha, 0, x.shortRange);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void LoadAllFFAZones()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    Models.ServerFFA.ServerFFA_ = new List<dbmodels.ServerFFA>(db.ServerFFA);
                    Console.WriteLine($"{Models.ServerFFA.ServerFFA_.Count} Server-FFA-Zones wurden geladen.");

                    Models.ServerFFA.ServerFFASpawns_ = new List<dbmodels.ServerFFASpawns>(db.ServerFFASpawns);
                    Console.WriteLine($"{Models.ServerFFA.ServerFFASpawns_.Count} Server-FFA-Zones-Spawns wurden geladen.");
                }

                foreach (var zone in Models.ServerFFA.ServerFFA_)
                {
                    Blip blip = NAPI.Blip.CreateBlip(432, new Vector3(zone.posX, zone.posY, zone.posZ), 1.0f, 0, $"FFA - {zone.name}", 255, 0, true);
                    NAPI.Marker.CreateMarker(MarkerType.VerticalCylinder, new Vector3(zone.posX, zone.posY, zone.posZ), new Vector3(zone.posX, zone.posY, zone.posZ), new Vector3(0, 0, 0), 1f, new Color(255, 162, 0, 200));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static async void SaveAllStuff()
        {
            try
            {
                using (var db = new gtaContext())
                {
                    foreach (var accounts in Models.ServerAccounts.Accounts_)
                    {
                        db.Accounts.Update(accounts);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
