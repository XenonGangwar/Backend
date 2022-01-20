using GTANetworkAPI;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RageMP_Gangwar
{
    public class Main : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            Console.WriteLine("Server startet.");
            NAPI.Server.SetGlobalServerChat(false);
            NAPI.Server.SetAutoSpawnOnConnect(true);
            NAPI.Server.SetCommandErrorMessage("[Xenon] Dieser Befehl wurde nicht gefunden.");
            NAPI.Server.SetAutoRespawnAfterDeath(true);

            // Marker für Weapon Laden
            NAPI.Marker.CreateMarker(MarkerType.VerticalCylinder, new Vector3(21.96738, -1106.722, 28.79703), new Vector3(21.96738, -1106.722, 28.79703), new Vector3(0, 0, 0), 1f, new Color(255, 162, 0, 200));

            Database.DatabaseHandler.LoadAllAccounts();
            Database.DatabaseHandler.LoadAllServerBlips();
            Database.DatabaseHandler.LoadAllServerFactions();
            Database.DatabaseHandler.LoadAllFFAZones();
            Database.DatabaseHandler.LoadAllServerPrestigeVehicles();
            Database.DatabaseHandler.LoadAllGangwarZones();

            NAPI.World.SetWeather(Weather.SNOW);

            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(TimeSpan.FromMinutes(5.0)).Wait();
                    NAPI.Task.Run(() =>
                    {
                        Database.DatabaseHandler.SaveAllStuff();
                    });
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(TimeSpan.FromSeconds(5.0)).Wait();
                    NAPI.Task.Run(() =>
                    {
                        NAPI.World.SetTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    });
                }
            });

            //                         NAPI.World.SetTime(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(1000 * 480 * 1).Wait();
                    NAPI.Task.Run(() =>
                    {
                    });
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(1000 * 900 * 1).Wait();
                    NAPI.Task.Run(() =>
                    {
                        foreach (var veh in NAPI.Pools.GetAllVehicles().ToList().Where(x => x != null && x.Exists))
                        {
                            if (veh.Occupants.Count == 0) veh.Delete();
                        }
                    });
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(TimeSpan.FromMinutes(2)).Wait();
                    NAPI.Task.Run(() =>
                    {
                        Handler.GangwarHandler.GangwarTimer();
                    });
                }
            });
        }


        [ServerEvent(Event.ResourceStop)]
        public void ResourceStop()
        {
            Console.WriteLine("Server gestoppt.");
        }

        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(Player player)
        {
            try
            {
                if (player == null || !player.Exists) return;

                /*
                if (player.SocialClubName == "Duchess-X" || player.SocialClubName == "Justin_Woke" || player.SocialClubName == "aoall187" || player.SocialClubName == "ghost_xtim20")
                {

                    player.TriggerEvent("Player:Login:createLoginCamera", true); //Loginkamera erstellen
                    NAPI.Player.SpawnPlayer(player, new Vector3(18.13111, 637.8425, 210.5947));
                    player.Position = new Vector3(18.13111, 637.8425, 210.5947);
                    player.Transparency = 0;
                    player.Dimension = 10;
                    player.Health = 100;
                }
                else
                {
                    player.SendNotification("~r~Du darfst nicht auf den Server connecten.", false);
                    Console.WriteLine($"[WARTUNGSARBEITEN KICK] Rage:MP Name: {player.Name} | IP: {player.Address}");
                    player.Kick("");
                    return;
                }
                */

                // Kann man nutzen für Wartungsarbeiten.

                player.TriggerEvent("Player:Login:createLoginCamera", true); //Loginkamera erstellen
                player.TriggerEvent("updateDiscord", "Nicht eingeloggt.");
                NAPI.Player.SpawnPlayer(player, new Vector3(18.13111, 637.8425, 210.5947));
                player.Position = new Vector3(18.13111, 637.8425, 210.5947);
                player.Transparency = 0;
                player.Dimension = 10;
                player.Health = 100;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
