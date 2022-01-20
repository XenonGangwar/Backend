using GTANetworkAPI;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;
using System.Linq;

namespace RageMP_Gangwar.Handler
{
    public class AdminHandler : Script
    {
        [Command("createffazone", GreedyArg = true)]
        public void CMD_CreateFFAZone(Player player, int dimension, string name)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 9) return;
                ServerFFA.CreateFFAZone(player, name, dimension);
                var ffaZone = ServerFFA.GetFullFFAEntry(name);
                if (ffaZone == null) return;
                player.sendAdminMessage($"FFA-Zone erstellt (ID: {ffaZone.id})");
                Blip blip = NAPI.Blip.CreateBlip(432, new Vector3(ffaZone.posX, ffaZone.posY, ffaZone.posZ), 1.0f, 0, $"FFA - {ffaZone.name}", 255, 0, true);
                NAPI.Marker.CreateMarker(MarkerType.VerticalCylinder, new Vector3(ffaZone.posX, ffaZone.posY, ffaZone.posZ), new Vector3(ffaZone.posX, ffaZone.posY, ffaZone.posZ), new Vector3(0, 0, 0), 1f, new Color(255, 162, 0, 200));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("createffaspawn")]
        public void CMD_CreateFFAZoneSpawn(Player player, int ffaId)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 9) return;
                ServerFFA.CreateFFASpawn(player, ffaId);
                player.sendAdminMessage($"Spawn für FFA-Arena {ffaId} erstellt.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("unwarn")]
        public void CMD_Unwarn(Player player, Player target)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId()) return;
                int pID = player.getAccountId();
                int tID = target.getAccountId();
                if (pID <= 0 || tID <= 0 || player.getAdminLevel() < 5 || Models.ServerAccounts.GetPlayerWarns(tID) <= 0) return;
                Models.ServerAccounts.SetPlayerWarns(tID, Models.ServerAccounts.GetPlayerWarns(tID) - 1);
                player.sendAdminMessage($"Du hast {target.Name} einen Warn entfernt ({Models.ServerAccounts.GetPlayerWarns(tID)}/3 Warns).");
                target.sendAdminMessage($"{player.Name} hat dir einen Warn entfernt ({Models.ServerAccounts.GetPlayerWarns(tID)}/3 Warns).");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("warn")]
        public void CMD_Warn(Player player, Player target)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId()) return;
                int pID = player.getAccountId();
                int tID = target.getAccountId();
                if (pID <= 0 || tID <= 0 || player.getAdminLevel() < 5) return;
                Models.ServerAccounts.SetPlayerWarns(tID, Models.ServerAccounts.GetPlayerWarns(tID) + 1);
                Models.AdminLogs.AddNewAdminLog(pID, tID, "warn", $"{player.Name} hat {target.Name} verwarnt.");
                if (Models.ServerAccounts.GetPlayerWarns(tID) >= 3)
                {
                    target.Kick($"3/3 Warns, Ban!");
                    Models.ServerAccounts.SetPlayerBanned(tID, true, false);
                    player.sendAdminMessage($"Du hast {target.Name} verwarnt (3/3 Warns) - er wurde gebannt.");
                    return;
                }
                player.sendAdminMessage($"Du hast {target.Name} verwarnt ({Models.ServerAccounts.GetPlayerWarns(tID)}/3 Warns).");
                target.sendAdminMessage($"{player.Name} hat dich verwarnt ({Models.ServerAccounts.GetPlayerWarns(tID)}/3 Warns).");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        /*
        [Command("setlockstates")]
        public void CMD_SetLockState(Player player, int accId, int lockState)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId())
                    int pID = player.getAccountId();

                if (pID <= 0 || player.getAdminLevel() < 9) return;
                Models.ServerAccounts.SetPlayerLockState(accId, lockState);
                player.SendChatMessage($"Lockstate zu {lockState} geändert.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        */

        // ok k

        [Command("announce", GreedyArg = true)]
        public void CMD_MCustom(Player player, string msg)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 6) return;
                NAPI.PlayerEvent.TriggerPlayerEventForAll("Player:HUD:ModMessage", "Systembenachrichtigung", player.Name + ": " + msg, 6000);
            }
            catch (Exception e)
            {
                Console.Write($"{e}");
            }
        }
        [ServerEvent(Event.PlayerWeaponSwitch)]
        public void PlayerWeaponSwitch(Player c, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            try
            {
                if (oldWeapon == null) return;
                if (newWeapon == null) return;
                if (c == null) return;

                c.TriggerEvent("Player:weaponSwap");
                NAPI.Player.SetPlayerCurrentWeapon(c, newWeapon);
                //NAPI.Player.SetPlayerCurrentWeaponAmmo(c, 9999);
                c.Eval($"mp.game.invoke('0xDCD2A934D65CB497', mp.game.player.getPed(), {NAPI.Util.GetHashKey(newWeapon.ToString())}, 9999);");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[EXCEPTION PlayerWeaponSwitch] " + ex.Message);
                Console.WriteLine("[EXCEPTION PlayerWeaponSwitch] " + ex.StackTrace);
            }
        }
        [Command("krieg", GreedyArg = true)]
        public void CMD_Krieg(Player player, string attacker, string defender)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 6) return;
                NAPI.PlayerEvent.TriggerPlayerEventForAll("Player:HUD:SystemMessage", "Kriegsvertrag", $"Wer erklärt wem den Krieg: {attacker} vs {defender}. Grund des Krieges: Schädigung des Geschäfts, Respektlosigkeit gegenüber der {attacker} ,Beschuss der {attacker} Leaderschaft", 15000);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("discord")]
        public void DC(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0) return;
                player.sendAdminMessage("Discord: ~b~https://discord.gg/9CQSQj3JpQ");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("setcar", GreedyArg = true)]
        public void CMD_GiveCar(Player player, Player target, string vehicle)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                int accId = target.getAccountId();
                uint hash = NAPI.Util.GetHashKey(vehicle);
                if (accId == null) { return; }
                if (pID <= 0 || player.getAdminLevel() < 7) return;
                if (Models.ServerVehicles.ExistPrivateVehicleByHash(accId, hash)) return;
                Models.ServerVehicles.CreatePrivateVehicle(accId, hash, vehicle);
                player.sendAdminMessage($"Fahrzeug {vehicle} gegeben.");
                AdminLogs.AddNewAdminLog(pID, accId, "givecar", $"Fahrzeug {vehicle} gegeben (Hash: {hash})");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("pn", GreedyArg = true)]
        public void CMD_PN(Player player, Player target, string message)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId() || message == null || string.IsNullOrWhiteSpace(message)) return;

            target.SendChatMessage($"[~r~Private-Nachricht~w~] ~y~{player.Name}~w~: ~y~{message}");
            player.SendChatMessage($"[~r~Private-Nachricht~w~] Nachricht verschickt.");
        }

        [Command("removeweapon", GreedyArg = true)]
        public void CMD_PN(Player player, Player target, WeaponHash weaponhash)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId()) return;

            target.RemoveWeapon(weaponhash);
            player.sendAdminMessage($"Du hast {target.Name} die Waffe {weaponhash} entfernt!");
            target.sendAdminMessage($"Dir wurde die Waffe {weaponhash} von Teammitglied {player.Name} entfernt!");
        }

        [Command("mute")]
        public void Cmd_Mute(Player player, Player target)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId() || player.getAdminLevel() < 4) return;

            ServerAccounts.SetMuted(target.getAccountId(), true);
            player.sendAdminMessage($"Du hast {target.Name} gemutet!");
            target.sendAdminMessage($"Du wurdest vom Teammitglied {player.Name} gemutet!");
        }

        [Command("unmute")]
        public void Cmd_UnMute(Player player, Player target)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId() || player.getAdminLevel() < 4) return;

            ServerAccounts.SetMuted(target.getAccountId(), false);
            player.sendAdminMessage($"Du hast {target.Name} geunmutet!");
            target.sendAdminMessage($"Du wurdest vom Teammitglied {player.Name} unmutet!");
        }


        [Command("delprivatecar")]
        public void CMD_DelPrivateCar(Player player, int accId, uint hash)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 9) return;
                if (!Models.ServerVehicles.ExistPrivateVehicleByHash(accId, hash)) return;
                Models.ServerVehicles.RemovePrivateVehicle(accId, hash);
                player.sendAdminMessage($"Fahrzeug entfernt (Account: {accId} - Hash: {hash}).");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("setadmin")]
        public void CMD_MakeAdmin(Player player, Player target, int rank)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 11) return;
                if (target.getAdminLevel() > player.getAdminLevel()) return;
                Models.ServerAccounts.SetAccountAdminLevel(target.getAccountId(), rank);
                player.sendAdminMessage($"{target.Name} hat den Rang {rank} ({Models.ServerAccounts.GetAdminPrefix(rank)}) bekommen.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }


        [Command("goto")]
        public void CMD_Goto(Player player, Player target)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId()) return;
                int pID = player.getAccountId();
                int tID = target.getAccountId();
                if (pID <= 0 || tID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 4) return;
                player.Position = target.Position;
                player.sendAdminMessage($"Du hast dich zu {target.Name} teleportiert.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("bring")]
        public void CMD_Gethere(Player player, Player target)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId()) return;
                int pID = player.getAccountId();
                int tID = target.getAccountId();
                if (pID <= 0 || tID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(tID) <= 0 || player.getAdminLevel() < 4) return;
                target.Position = player.Position;
                player.sendAdminMessage($"Du hast {target.Name} zu dir gebracht.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("aduty")]
        public void Command_Aduty(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                int adminlevel = player.getAdminLevel();
                if (pID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 4) return;
                bool isAduty = Models.ServerAccounts.IsPlayerADuty(pID);
                Models.ServerAccounts.SetPlayerADuty(pID, !isAduty);
                player.TriggerEvent("Player:Admin:setAdutyState", !isAduty);
                player.TriggerEvent("Player:Admin:toggleAdminMode");
                player.Transparency = 255;

                if (!isAduty)
                {
                    //Aduty

                    if (adminlevel == 13)
                    {
                        // Entwicklungsleiter
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 2);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 2);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 2);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 2);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 0);
                    }

                    if (adminlevel == 12)
                    {
                        // Projektleiter
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 2);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 2);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 2);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 2);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 0);
                    }

                    if (adminlevel == 11)
                    {
                        // Projektleiter
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 2);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 2);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 2);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 2);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 0);
                    }

                    if (adminlevel == 10)
                    {
                        // Entwickler
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 2);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 2);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 2);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 2);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 0);
                    }

                    if (adminlevel == 9)
                    {
                        // Manager
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 2);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 2);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 2);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 2);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 0);
                    }

                    if (adminlevel == 8)
                    {
                        // Jr. Entwickler
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 1);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 1);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 1);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 1);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 0);
                    }

                    if (adminlevel == 7)
                    {
                        // Administrator
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 3);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 3);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 3);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 3);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 0);
                    }

                    if (adminlevel == 6)
                    {
                        // Moderator
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 4);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 4);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 4);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 4);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 0);
                    }

                    if (adminlevel == 5)
                    {
                        // Projektleiter
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 5);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 5);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 5);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 5);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 5);
                    }


                    if (adminlevel == 4)
                    {
                        // Projektleiter
                        player.TriggerEvent("ChangeClothesC", 8, 15, 0);
                        player.TriggerEvent("ChangeClothesC", 11, 287, 13);
                        player.TriggerEvent("ChangeClothesC", 1, 135, 13);
                        player.TriggerEvent("ChangeClothesC", 4, 114, 13);
                        player.TriggerEvent("ChangeClothesC", 6, 78, 13);
                        player.TriggerEvent("ChangeClothesC", 3, 17, 0);
                    }

                    player.Transparency = 0;
                    NAPI.Chat.SendChatMessageToPlayer(player, $"[~r~ADMIN~w~] Mit ~r~F6 ~w~wirst du wieder sichtbar.");
                }
                else
                {
                    player.SetSkin(PedHash.FreemodeMale01);
                    player.TriggerEvent("charreloadc");


                    player.Transparency = 255;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("getusername")]
        public void CMD_getusername(Player player, int accId)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 4 || !Models.ServerAccounts.ExistPlayerByid(accId)) return;
                //  NAPI.Chat.SendChatMessageToPlayer(player, $"[~p~ADMIN~w~] Der Name des Spielers mit der ID ({accId}) lautet: {Models.ServerAccounts.GetAccountNameById(accId)}");
                player.sendAdminMessage("Der Name des Spielers mit der ID ({accId}) lautet: {Models.ServerAccounts.GetAccountNameById(accId)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("userid")]
        public void CMD_getaccountid(Player player, string name)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 4 || !Models.ServerAccounts.ExistPlayerByUserName(name)) return;
                //  NAPI.Chat.SendChatMessageToPlayer(player, $"[~p~ADMIN~w~] Die ID des Spielers mit dem Namen {name} lautet: {Models.ServerAccounts.GetAccountIdByName(name)}");
                player.sendAdminMessage($"Die ID des Spielers mit dem Namen {name} lautet: {Models.ServerAccounts.GetAccountIdByName(name)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("teleportevent")]
        public void CMD_TPEvent(Player player, int team)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 4) return;
                if (team != Constants.EventConfig.team1 && team != Constants.EventConfig.team2) return;
                foreach (var p in NAPI.Pools.GetAllPlayers().ToList().Where(x => x != null && x.Exists))
                {
                    if (p == null || !p.Exists || !p.hasAccountId() || Models.ServerAccounts.GetAccountSelectedTeam(p.getAccountId()) != team) continue;
                    p.Position = player.Position;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("createevent")]
        public void CMD_CreateEvent(Player player, int team1, int team2)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || team1 <= 0 || team2 <= 0) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 4) return;
                if (Constants.EventConfig.isEventActive)
                {
                    player.SendChatMessage($"[~p~EVENT~w~] Es läuft bereits ein Event.");
                    return;
                }

                Constants.EventConfig.team1 = team1;
                Constants.EventConfig.team2 = team2;
                Constants.EventConfig.isEventActive = true;
                player.SendChatMessage($"Event erstellt - Team 1: {team1} | Team 2: {team2}");
                NAPI.Chat.SendChatMessageToAll($"[~y~EVENT~w~] Ein Event wurde gestartet. Teilt euch in die Teams {Models.ServerFactions.GetFactionName(team1)} und {Models.ServerFactions.GetFactionName(team2)} auf und tretet per /event bei.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("getaccountname")]
        public void CMD_GetAccName(Player player, string sc)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 4) return;
                player.sendAdminMessage($"Der Name des Spielers lautet {Models.ServerAccounts.GetAccNameBySC(sc)} (SocialClub: {sc}).");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("resethwid")]
        public void CMD_resetHwId(Player player, int accId)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || player.getAdminLevel() < 9 || !Models.ServerAccounts.ExistPlayerByid(accId)) return;
                Models.ServerAccounts.SetPlayerHwId(accId, "unset");
                player.sendAdminMessage($"Hardware-ID erfolgreich zurück gesetzt (Spieler: {accId}).");
                AdminLogs.AddNewAdminLog(pID, accId, "resethwid", $"Hardware-ID von {accId} zurückgesetzt.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("kick", GreedyArg = true)]
        public void CMD_Kick(Player player, Player target, string reason)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists) return;
                int pID = player.getAccountId();
                if (pID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 4) return;
                if (target.hasAccountId() && target.getAdminLevel() > player.getAdminLevel()) return;
                NAPI.PlayerEvent.TriggerPlayerEventForAll("Player:HUD:ModMessage", "Kick", $"{player.Name} hat {target.Name} vom Server gekickt. Grund: {reason}", 12000);
                target.Kick($"{reason}");
                Models.AdminLogs.AddNewAdminLog(pID, target.getAccountId(), "kick", $"{player.Name} hat {target.Name} gekickt. Grund: {reason}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("ban", GreedyArg = true)]
        public void CMD_Ban(Player player, Player target, string reason)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId()) return;
            int pID = player.getAccountId();
            int tID = target.getAccountId();
            if (pID <= 0 || tID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 6 || target.getAdminLevel() > player.getAdminLevel() || Models.ServerAccounts.IsAccountPermBanned(tID)) return;
            NAPI.PlayerEvent.TriggerPlayerEventForAll("Player:HUD:ModMessage", "Ban", $"{player.Name} hat {target.Name} permanent vom Server gebannt. Grund: {reason}", 12000);
            Models.ServerAccounts.SetPlayerBanned(tID, true, false);
            target.Kick($"{reason}");
            Models.AdminLogs.AddNewAdminLog(pID, tID, "ban", $"{player.Name} hat {target.Name} gebannt. Grund: {reason}");
        }

        [Command("offban", GreedyArg = true)]
        public void Cmd_offban(Player player, string target, string reason)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || target == null) return;
            if (ServerAccounts.GetAccountIdByName(target) == null)
            {
                return;
            }
            if (ServerAccounts.GetAccountIdByName(target) == 0)
            {
                return;
            }
            int pID = player.getAccountId();
            int tID = ServerAccounts.GetAccountIdByName(target);

            if (pID <= 0 || tID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 6 || Models.ServerAccounts.IsAccountPermBanned(tID)) return;

            NAPI.PlayerEvent.TriggerPlayerEventForAll("Player:HUD:ModMessage", "Ban", $"{player.Name} hat {target} offline permanent vom Server gebannt. Grund: {reason}", 12000);
            Models.ServerAccounts.SetPlayerBanned(tID, true, false);
            Models.AdminLogs.AddNewAdminLog(pID, tID, "ban", $"{player.Name} hat {target} gebannt. Grund: {reason}");
        }

        [Command("togglent")]
        public void ToggleNT_CMD(Player player)
        {
            if (player == null || !player.Exists || !player.hasAccountId()) return;
            int pID = player.getAccountId();
            if (pID <= 0 || player.getAdminLevel() < 6) return;
            player.TriggerEvent("Player:Admin:toggleNametag");
        }

        [Command("timeban", GreedyArg = true)]
        public void CMD_Timeban(Player player, Player target, int hours, string reason)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId()) return;
            int pID = player.getAccountId();
            int tID = target.getAccountId();
            if (pID <= 0 || tID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 6 || target.getAdminLevel() > player.getAdminLevel() || Models.ServerAccounts.IsAccountTimeBanned(tID)) return;
            NAPI.PlayerEvent.TriggerPlayerEventForAll("Player:HUD:ModMessage", "Ban", $"{player.Name} hat {target.Name} temporär vom Server gebannt. Grund: {reason}", 12000);
            Models.ServerAccounts.SetPlayerBanned(tID, false, true);
            Models.ServerAccounts.SetTimebanHours(tID, hours);
            target.Kick($"{reason} ({hours} Stunden)");
            Models.AdminLogs.AddNewAdminLog(pID, tID, "timeban", $"{player.Name} hat {target.Name} temporär gebannt. Grund: {reason}");
        }

        [Command("setxp")]
        public void CMD_SetXP(Player player, int multiplikator)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || multiplikator <= 0) return;
                int pID = player.getAccountId();
                if (pID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 8) return;
                Constants.ServerConfig.XPMultiplikator = multiplikator;
                NAPI.Chat.SendChatMessageToAll($"[~y~XP~w~] Der XP-Multiplikator wurde von {player.Name} auf {multiplikator}x gesetzt.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("unban")]
        public void CMD_Unban(Player player, int accId)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 6) return;
                Models.ServerAccounts.SetPlayerBanned(accId, false, false);
                player.sendAdminMessage($"{Models.ServerAccounts.GetAccountNameById(accId)} wurde erfolgreich entbannt.");
                Models.AdminLogs.AddNewAdminLog(pID, accId, "unban", $"{player.Name} hat die Spieler-ID ({accId}) entbannt.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [Command("tc", GreedyArg = true)]
        public static void CMD_AdminChat(Player player, string message)
        {
            if (player == null || !player.Exists || !player.hasAccountId()) return;
            if (player.getAdminLevel() > 4) return;

            foreach (Player target in NAPI.Pools.GetAllPlayers())
            {
                if (target.getAdminLevel() > 4) { return; }
                target.SendChatMessage($"[~r~ADMIN-CHAT~w~] {player.Name}: " + message);
            }
        }

        [Command("giveaufnahmepflicht")]
        public static void Cmd_giveaufnahmepflicht(Player player, string target)
        {
            if (player == null || !player.Exists || !player.hasAccountId()) return;
            int pID = player.getAccountId();
            int targetID = ServerAccounts.GetAccountIdByName(target);
            string targetName = ServerAccounts.GetAccountNameById(targetID);
            if (ServerAccounts.ExistPlayerByid(targetID) == null || targetName == null) { return; }
            if (pID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 4) return;

            Models.ServerAccounts.SetAufnahmepflicht(targetID, true);
            player.sendAdminMessage($"Du hast {target} eine Aufnahmepflicht gedrückt!");
        }

        [Command("removeaufnahmepflicht")]
        public static void Cmd_removeaufnahmepflicht(Player player, string target)
        {
            if (player == null || !player.Exists || !player.hasAccountId()) return;
            int pID = player.getAccountId();
            int targetID = ServerAccounts.GetAccountIdByName(target);
            string targetName = ServerAccounts.GetAccountNameById(targetID);
            if (ServerAccounts.ExistPlayerByid(targetID) == null || targetName == null) { return; }
            if (pID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 4) return;

            Models.ServerAccounts.SetAufnahmepflicht(targetID, false);
            player.sendAdminMessage($"Du hast {target} seine Aufnahmepflicht entfernt!");
        }

        [RemoteEvent("Server:Admin:toggleVanish")]
        public void AdminEvent_toggleVanish(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (pID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || player.getAdminLevel() < 4 || !Models.ServerAccounts.IsPlayerADuty(pID)) return;
                if (player.Transparency == 0) player.Transparency = 255;
                else if (player.Transparency == 255) player.Transparency = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        [Command("spectate", GreedyArg = true)]
        public void SPECTATE_CMD(Player player, Player target)
        {
            try
            {
                if (player == null || !player.Exists || !target.Exists || target == null || !player.hasAccountId() || player.getAdminLevel() < 4) return;

                player.SendNotification($"Du schaust nun {target.Name} zu!", true);
                player.Transparency = 0;
                player.TriggerEvent("spectatePlayer", target);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [Command("stopspectate")]
        public void STOPSPECTATE_CMD(Player player)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || player.getAdminLevel() < 4) return;

            player.Transparency = 255;
            player.SendNotification("Du schaust nun niemanden mehr zu!", true);
            player.TriggerEvent("stopSpectating");

        }

    }
}