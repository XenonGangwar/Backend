using GTANetworkAPI;
using RageMP_Gangwar.Functions;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;
using System.Linq;

namespace RageMP_Gangwar.Handler
{
    class Commands : Script
    {
        [Command("veh")]
        public void CMD_Veh(Player Player, string vehName)
        {
            try
            {
                if (Player == null || !Player.Exists || !Player.hasAccountId() || Player.getAdminLevel() < 5) return;
                uint hash = NAPI.Util.GetHashKey(vehName);
                Vehicle veh = NAPI.Vehicle.CreateVehicle(hash, Player.Position.Around(0), 0, 28, 28, "ADMIN", 255, false, true, Player.Dimension);

                Player.sendAdminMessage($"Model: {veh.Model}  ||| Hash: {hash}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("leaveevent")]
        public void LeaveEventCMD(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                if (!Models.ServerAccounts.IsPlayerInEvent(player.getAccountId())) return;
                player.SendChatMessage($"[~y~EVENT~w~] Event verlassen.");
                Handler.TeamHandler.SpawnPlayer(player);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("ping")]
        public void pingCMD(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                player.sendAdminMessage($"Dein Ping beträgt {player.Ping}ms.");
            }
            catch (Exception e)
            {
                Console.Write($"{e}");
            }
        }

        [Command("event")]
        public void EventCMD(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || !Constants.EventConfig.isEventActive) return;
                int ownTeam = Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId());
                if (ownTeam != Constants.EventConfig.team1 && ownTeam != Constants.EventConfig.team2) return;
                if (Models.ServerAccounts.IsPlayerInEvent(player.getAccountId())) return;
                Models.ServerAccounts.SetPlayerinEvent(player.getAccountId(), true);
                player.SendChatMessage($"[~y~EVENT~w~] Event beigetreten.");
                player.Dimension = 31;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("reset")]
        public void resetCMD(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0) return;
                ServerAccounts.ResetKilLDeaths(pID);
                player.SendChatMessage($"[~y~Stats~w~] K/D zurückgesetzt.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("prestige")]
        public void CMD_Prestige(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || ServerAccounts.GetPlayerLevel(pID) < 100 || ServerAccounts.GetPrestigeLevel(pID) > 10) return;
                ServerAccounts.SetPlayerLevel(pID, 1);
                ServerAccounts.SetPlayerEXP(pID, 1);
                ServerAccounts.SetPrestigeLevel(pID, ServerAccounts.GetPrestigeLevel(pID) + 1);
                NAPI.Chat.SendChatMessageToPlayer(player, $"Prestige Level erhöht. Aktuell: {ServerAccounts.GetPrestigeLevel(pID)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("clearchat")]
        public void CMD_ClearChat(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || player.getAdminLevel() < 4) return;
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
                NAPI.Chat.SendChatMessageToAll($"");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("waffen")]
        public void Waffen_CMD(Player Player)
        {
            try
            {
                if (Player == null || !Player.Exists || Player.getAdminLevel() < 4 || !Player.hasAccountId()) return;
                AccountsFunctions.GiveWeapons(Player);
            }
            catch (Exception e)
            {
                Console.Write($"{e}");
            }
        }

        [Command("setpos")]
        public void setposcmd(Player Player, float x, float y, float z)
        {
            if (Player == null || !Player.Exists || !Player.hasAccountId() || Player.getAdminLevel() < 4) return;
            Player.Position = new Vector3(x, y, z);
        }

        [Command("pos")]
        public void Pos_CMD(Player Player)
        {
            if (Player == null || !Player.Exists) return;
            Player.SendChatMessage($"{Player.Position.ToString()}");
        }

        [Command("setfrak")]
        public void CMD_Faction(Player player, Player target, int faction, int rank)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || player.getAdminLevel() < 5) return;
                if (target == null || !target.Exists || !target.hasAccountId()) return;
                ServerAccounts.SetAccountFaction(target.getAccountId(), faction, rank);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("dv")]
        public void CMD_DeleteVehicle(Player player)
        {
            if (player == null || !player.Exists || !player.hasAccountId()) return;
            int pID = player.getAccountId();
            if (pID <= 0 || player.getAdminLevel() < 4) return;

            if (player.IsInVehicle == true)
            {
                player.Vehicle.Delete();
                player.sendAdminMessage("Du hast dein Fahrzeug gelöscht.");
            }
            else
            {
                player.sendAdminMessage("Du sitzt in keinem Fahrzeug.");
                return;
            }
        }

        [Command("clearcars")]
        public void CMD_ClearCars(Player player, bool all = false)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 4) return;
                foreach (var veh in NAPI.Pools.GetAllVehicles().ToList().Where(x => x != null && x.Exists))
                {
                    if (all) veh.Delete();
                    else
                    {
                        if (veh.Occupants.Count == 0) veh.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("report", GreedyArg = true)]
        public void CMD_Report(Player player, Player reportedTarget, string reason)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || reportedTarget == null || !reportedTarget.Exists || !reportedTarget.hasAccountId()) return;
                foreach (var admin in NAPI.Pools.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.hasAccountId() && x.getAdminLevel() >= 4))
                {
                    admin.SendChatMessage($"~w~[~r~REPORT]~w~ ~y~{reportedTarget.Name}~w~ wurde von ~y~{player.Name}~w~ reportet. Grund: ~r~{reason}");
                }
                player.SendChatMessage($"~w~[~r~REPORT]~w~ Du hast ~y~{reportedTarget.Name}~w~ gemeldet. Grund: ~r~{reason}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [RemoteEvent("server:openteam")]
        public void RM_openteam(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                TeamHandler.CreateTeamSelect(player);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("team")]
        public void CMD_Team(Player player)
        {
            if (player == null || !player.Exists || !player.hasAccountId()) return;
            player.sendAdminMessage("Du kannst absofort nur noch die Teamauswahl per F2 öffnen.");
        }

        [Command("selectkit")]
        public void CMD_SelectKit(Player player, int kit)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || kit == null || kit == 0 || kit > 5) return;
            int pID = player.getAccountId();
            ServerAccounts.SetPlayerKit(pID, kit);
            Functions.AccountsFunctions.GiveWeapons(player);
            player.SendChatMessage($"Du hast das Kit ~y~{kit} ~w~ausgewählt.");
        }

        [Command("invite")]
        public void CMD_Invite(Player player, Player inviteTarget)
        {
            try
            {
                if (player == null || !player.Exists || inviteTarget == null || !inviteTarget.Exists) return;
                if (!player.hasAccountId() || !inviteTarget.hasAccountId()) return;
                int pID = player.getAccountId();
                int tID = inviteTarget.getAccountId();
                if (pID <= 0 || tID <= 0) return;
                int pFaction = ServerAccounts.GetAccountFaction(pID);
                if (pFaction <= 0 || ServerAccounts.GetFactionRank(pID) != 2)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Invite~w~] Du bist in keiner privaten Fraktion oder bist nicht der Leader.");
                    return;
                }

                if (ServerAccounts.GetAccountFaction(tID) != 0)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Invite~w~] Der Spieler {inviteTarget.Name} ist bereits in einer privaten Fraktion.");
                    return;
                }

                string factionName = ServerFactions.GetFactionName(pFaction);
                NAPI.Chat.SendChatMessageToPlayer(inviteTarget, $"[~r~Private Fraktionen~w~] Du wurdest von {player.Name} in die Fraktion {factionName} eingeladen.");
                NAPI.Chat.SendChatMessageToPlayer(inviteTarget, $"[~r~Private Fraktionen~w~] Nutze ~y~/acceptinvite {player.Name} ~w~um die Einladung in die Fraktion anzunehmen.");
                NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Private Fraktionen~w~] Du hast {inviteTarget.Name} in deine Fraktion eingeladen.");
                player.SetData("inviteFrak", inviteTarget.Name);
                inviteTarget.SetData("inviteFrak", player.Name);
                inviteTarget.SetData("inviteFrakId", pFaction);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("acceptinvite")]
        public void CMD_acceptInvite(Player player, Player inviter)
        {
            try
            {
                if (player == null || !player.Exists || inviter == null || !inviter.Exists) return;
                if (!player.hasAccountId() || !inviter.hasAccountId()) return;
                if (!player.HasData("inviteFrak") || !inviter.HasData("inviteFrak")) return;
                int pID = player.getAccountId();
                int iID = inviter.getAccountId();
                if (pID <= 0 || iID <= 0) return;
                if (ServerAccounts.GetAccountFaction(pID) != 0)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Xenon~w~] Du bist bereits in einer privaten Fraktion.");
                    return;
                }

                if (player.GetData("inviteFrak") != inviter.Name || inviter.GetData("inviteFrak") != player.Name) return;
                NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Private Fraktionen~w~] Du hast die Einladung der privaten Fraktion angenommen.");
                NAPI.Chat.SendChatMessageToPlayer(inviter, $"[~r~Private Fraktionen~w~] {player.Name} ist der privaten Fraktion beigetreten.");
                ServerAccounts.SetAccountFaction(pID, player.GetData("inviteFrakId"), 1);
                player.ResetData("inviteFrakId");
                player.ResetData("inviteFrak");
                inviter.ResetData("inviteFrak");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("uninvite")]
        public void CMD_Uninvite(Player player, Player target)
        {
            try
            {
                if (player == null || !player.Exists || target == null || !target.Exists) return;
                if (!player.hasAccountId() || !target.hasAccountId()) return;
                int pID = player.getAccountId();
                int tID = target.getAccountId();
                if (pID <= 0 || tID <= 0) return;
                int pFaction = ServerAccounts.GetAccountFaction(pID);
                if (pFaction <= 0 || ServerAccounts.GetFactionRank(pID) != 2)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Xenon~w~] Du bist in keiner privaten Fraktion oder bist nicht der Leader.");
                    return;
                }

                if (ServerAccounts.GetAccountFaction(tID) != pFaction)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~Xenon~w~] Der Spieler {target.Name} ist nicht in deiner Fraktion.");
                    return;
                }
                string factionName = ServerFactions.GetFactionName(pFaction);

                NAPI.Chat.SendChatMessageToPlayer(player, $"[~y~Private Fraktionen~w~] Du hast den Spieler {target.Name} aus deiner Fraktion geworfen.");
                NAPI.Chat.SendChatMessageToPlayer(target, $"[~y~Private Fraktionen~w~] Du wurdest aus der privaten Fraktion '{factionName}' geworfen.");
                ServerAccounts.SetAccountFaction(tID, 0, 0);
                target.ResetData("inviteFrakId");
                TeamHandler.CreateTeamSelect(target);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("changename")]
        public void CHANGENAME_CMD(Player player, string newname)
        {
            if (player == null || !player.Exists || !player.hasAccountId()) return;
            int accID = player.getAccountId();

            if (ServerAccounts.ExistPlayerByUserName(newname))
            {
                player.sendAdminMessage("Dieser Name ist bereits vorhanden! Wähle ein anderen aus.");
                return;
            }

            if (newname.Length < 4)
            {
                player.sendAdminMessage("Dein Name muss mindestens 4 Zeichen lang sein.");
                return;
            }

            player.sendAdminMessage($"Du hast dein Namen erfolgreich zu {newname} geändert. Bitte Reconnecten damit alles richtig geladen wird.");

            Models.ServerAccounts.SetPlayerName(accID, newname);
            player.Kick("");
        }

        [Command("t", GreedyArg = true)]
        public void T_CMD(Player player, string message)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || ServerAccounts.GetAccountSelectedTeam(pID) <= 0) return;
                int team = ServerAccounts.GetAccountSelectedTeam(pID);
                if (team <= 0) return;
                int adminLevel = player.getAdminLevel();
                int pLevel = ServerAccounts.GetPlayerLevel(player.getAccountId());
                string prefix = ServerAccounts.GetAdminPrefix(adminLevel);
                string color = ServerAccounts.GetChatRankColor(adminLevel);
                string prestigeRank = ServerAccounts.GetPrestigeRankName(ServerAccounts.GetPrestigeLevel(player.getAccountId()));

                foreach (var teamMember in NAPI.Pools.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.hasAccountId() && ServerAccounts.GetAccountSelectedTeam(x.getAccountId()) == team))
                {
                    teamMember.SendChatMessage($"[~y~TEAM-CHAT~w~] {prestigeRank} [lvl. {pLevel}] {player.Name}: {message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }

        }


    }
}
