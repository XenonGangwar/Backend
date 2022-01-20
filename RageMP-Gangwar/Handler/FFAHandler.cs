using GTANetworkAPI;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;
using System.Linq;

namespace RageMP_Gangwar.Handler
{
    public class FFAHandler : Script
    {
        public static void openFFABrowser(Player player, dbmodels.ServerFFA ffaData)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || ffaData == null || ServerAccounts.GetPlayerFFAArena(player.getAccountId()) != 0) return;
                player.TriggerEvent("Player:ffamenu_c:openBrowser", ffaData.name);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [RemoteEvent("Server:FFA-Announce")]
        public void Event_FFAAnnounce(Player player, int count)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || ServerAccounts.GetPlayerFFAArena(player.getAccountId()) != 0) return;
            int accountID = player.getAccountId();
            int ffaarenaplayer = ServerAccounts.GetPlayerFFAArena(accountID);
            foreach (var p in NAPI.Pools.GetAllPlayers().ToList().Where(x => x != null && x.Exists))
            {
                int targetid = p.getAccountId();
                if (ServerAccounts.GetPlayerFFAArena(targetid) == ffaarenaplayer)
                {
                    p.TriggerEvent("Player:HUD:ModMessage", "FFA", player.Name + " ist auf einer " + count + "er Streak!", 6000);
                }
                else
                {
                    return;
                }
            }
        }

        [Command("ffa")]
        public void Event_FFA(Player player, string arenaName)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || ServerAccounts.GetPlayerFFAArena(player.getAccountId()) != 0) return;
                var pID = player.getAccountId();
                var arenaData = ServerFFA.GetFullFFAEntry(arenaName);
                if (arenaData == null || pID <= 0) return;
                if (arenaData.currentPlayers >= 15)
                {
                    player.SendChatMessage($"[~y~FFA~w~] Die FFA Arena ist ~r~voll.~w~ Wähle eine andere.");
                    return;
                }
                ServerAccounts.SetPlayerFFAArena(pID, arenaData.id);
                ServerFFA.IncreaseFFAPlayer(arenaData.id);
                TeamHandler.SpawnPlayer(player);
                player.SendChatMessage($"[~y~FFA~w~] Willkommen in der FFA Arena. Benutze ~r~/quitffa~w~ um die FFA-Arena zu verlassen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        /*
        [RemoteEvent("Server:FFA:setFFA")]
        public void Event_setFFA(Player player, string arenaName)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || ServerAccounts.GetPlayerFFAArena(player.getAccountId()) != 0) return;
                var pID = player.getAccountId();
                var arenaData = ServerFFA.GetFullFFAEntry(arenaName);
                if (arenaData == null || pID <= 0) return;
                if(arenaData.currentPlayers >= 15)
                {
                    player.SendChatMessage($"[~y~FFA~w~] Die FFA Arena ist ~r~voll.~w~ Wähle eine andere.");
                    return;
                }
                ServerAccounts.SetPlayerFFAArena(pID, arenaData.id);
                ServerFFA.IncreaseFFAPlayer(arenaData.id);
                TeamHandler.SpawnPlayer(player);
                player.SendChatMessage($"[~y~FFA~w~] Willkommen in der FFA Arena. Benutze ~r~/quitffa~w~ um die FFA-Arena zu verlassen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        */

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
        {
            try
            {
                if (!player.hasAccountId()) return;
                var pID = player.getAccountId();
                if (pID <= 0) return;
                Models.ServerAccounts.SetPlayerDuellPartner(pID, 0);
                var currentFFAArena = ServerAccounts.GetPlayerFFAArena(pID);
                if (currentFFAArena != 0)
                {
                    ServerAccounts.SetPlayerFFAArena(pID, 0);
                    ServerFFA.DecreaseFFAPlayer(currentFFAArena);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("quitffa")]
        public void CMD_QuitFFA(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || ServerAccounts.GetPlayerFFAArena(player.getAccountId()) <= 0) return;
                var pID = player.getAccountId();
                if (pID <= 0 || ServerAccounts.GetAccountSelectedTeam(pID) <= 0) return;
                ServerFFA.DecreaseFFAPlayer(ServerAccounts.GetPlayerFFAArena(pID));
                ServerAccounts.SetPlayerFFAArena(pID, 0);
                TeamHandler.SpawnPlayer(player);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
