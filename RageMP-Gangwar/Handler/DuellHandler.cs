using GTANetworkAPI;
using RageMP_Gangwar.Utilities;
using System;

namespace RageMP_Gangwar.Handler
{
    public class DuellHandler : Script
    {
        [Command("1vs1")]
        public void CMD_Duell(Player player, Player target)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId()) return;
                if (player == target) return;
                int pID = player.getAccountId();
                int tID = target.getAccountId();
                if (pID <= 0 || tID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(tID) <= 0) return;
                if (Models.ServerAccounts.GetDisableDuellState(tID))
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, $"[~p~1vs1~w~] {target.Name} hat eingehende Anfragen blockiert!");
                    return;
                }
                if (Models.ServerAccounts.GetPlayerFFAArena(pID) != 0 || Models.ServerAccounts.GetPlayerFFAArena(tID) != 0) { NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Xenon~w~] Du oder die ausgewählte Person sind in einer FFA Arena."); return; }
                if (player.getcurrentDuellPartner() != 0 || target.getcurrentDuellPartner() != 0) { NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Xenon~w~] Du oder die ausgewählte Person sind bereits in einem anderen Duell."); return; }
                player.SetData("duellRequestName", target.Name);
                target.SetData("duellRequestName", player.Name);
                NAPI.Chat.SendChatMessageToPlayer(player, $"[~p~1vs1~w~] Du hast {target.Name} herausgefordert!");
                NAPI.Chat.SendChatMessageToPlayer(target, $"[~p~1vs1~w~] {player.Name} fordert dich zu einem 1vs1 heraus! /accept1vs1 {player.Name} um die Anfrage anzunehmen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("disable1vs1")]
        public void DisableDuell(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                if (Models.ServerAccounts.GetDisableDuellState(player.getAccountId()))
                {
                    player.SendChatMessage($"[~p~ANSAGE~w~] Du hast eingehende 1v1 Anfragen aktiviert.");
                }
                else
                {
                    player.SendChatMessage($"[~p~ANSAGE~w~] Du hast eingehende 1vs1 Anfragen aktiviert.");
                }

                Models.ServerAccounts.SetDisableDuellState(player.getAccountId(), !Models.ServerAccounts.GetDisableDuellState(player.getAccountId()));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}"); ;
            }
        }

        [Command("accept1vs1")]
        public void CMD_DuellAccept(Player target, Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId()) return;
                if (player == target) return;
                int pID = player.getAccountId();
                int tID = target.getAccountId();
                if (pID <= 0 || tID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(tID) <= 0) return;
                if (Models.ServerAccounts.GetPlayerFFAArena(pID) != 0 || Models.ServerAccounts.GetPlayerFFAArena(tID) != 0) { NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Gambo~w~] Du oder die ausgewählte Person sind in einer FFA Arena."); return; }
                if (player.getcurrentDuellPartner() != 0 || target.getcurrentDuellPartner() != 0) { NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Gambo~w~] Du oder die ausgewählte Person sind bereits in einem anderen Duell."); return; }
                if (!player.HasData("duellRequestName") || !target.HasData("duellRequestName")) return;
                if (player.GetData("duellRequestName") != target.Name || target.GetData("duellRequestName") != player.Name) return;
                Models.ServerAccounts.SetPlayerDuellPartner(pID, tID);
                Models.ServerAccounts.SetPlayerDuellPartner(tID, pID);
                player.ResetData("duellRequestName");
                target.ResetData("duellRequestName");

                player.Health = 100;
                target.Health = 100;
                player.Armor = 100;
                target.Armor = 100;
                player.Dimension = (uint)pID + 10;
                target.Dimension = (uint)pID + 10;
                player.Position = new Vector3(-274.3609, -2038.953, 30.14561);
                target.Position = new Vector3(-249.2382, -2006.507, 30.14558);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}