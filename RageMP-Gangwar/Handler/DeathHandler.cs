using GTANetworkAPI;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;
using System.Linq;

namespace RageMP_Gangwar.Handler
{
    class DeathHandler : Script
    {
        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(Player player, Player killer, uint reason)
        {
            try
            {
                if (player == null || !player.Exists || killer == null || !killer.Exists) return;
                if (!player.hasAccountId() || !killer.hasAccountId()) return;
                int playerTeam = ServerAccounts.GetAccountSelectedTeam(player.getAccountId());
                int killerTeam = ServerAccounts.GetAccountSelectedTeam(killer.getAccountId());
                int killerId = killer.getAccountId();
                int playerId = player.getAccountId();

                bool sameTeam = playerTeam == killerTeam;

                NAPI.Task.Run(() =>
                {
                    player.TriggerEvent("Player:HUD:RespawnEvent");
                    player.StopAnimation();
                    TeamHandler.SpawnPlayer(player);
                }, delayTime: 5000);

                if (player.getcurrentDuellPartner() != 0)
                {
                    var tKiller = NAPI.Pools.GetAllPlayers().ToList().FirstOrDefault(x => x != null && x.Exists && x.hasAccountId() && x.getAccountId() == player.getcurrentDuellPartner());
                    if (tKiller != null)
                    {
                        TeamHandler.SpawnPlayer(tKiller);
                    }
                    ServerAccounts.SetPlayerDuellPartner(playerId, 0);
                    ServerAccounts.SetPlayerDuellPartner(killerId, 0);
                    ServerAccounts.SetPlayerDuellPartner(player.getcurrentDuellPartner(), 0);
                    ServerAccounts.SetPlayerDuellPartner(killer.getcurrentDuellPartner(), 0);
                    ServerAccounts.SetPlayerDuellPartner(tKiller.getcurrentDuellPartner(), 0);
                    tKiller.TriggerEvent("Player:HUD:KillEvent", false, tKiller.Name, ServerAccounts.GetPlayerKills(tKiller.getAccountId()), ServerAccounts.GetPlayerDeaths(tKiller.getAccountId()), player.Name, false);
                    player.TriggerEvent("Player:HUD:DeathEvent", false, killer.Name, ServerAccounts.GetPlayerKills(playerId), ServerAccounts.GetPlayerDeaths(playerId), player.Name);
                    TeamHandler.SpawnPlayer(killer);
                    return;
                }

                if (player.getcurrentDuellPartner() == killerId && killer.getcurrentDuellPartner() == playerId)
                {
                    Console.WriteLine($"DUELL KILL ({player.getcurrentDuellPartner()}) - ({killerId}) - {killer.getcurrentDuellPartner()}) - {playerId})");
                    ServerAccounts.SetPlayerDuellPartner(playerId, 0);
                    ServerAccounts.SetPlayerDuellPartner(killerId, 0);
                    killer.TriggerEvent("Player:HUD:KillEvent", false, killer.Name, ServerAccounts.GetPlayerKills(killerId), ServerAccounts.GetPlayerDeaths(killerId), player.Name, false);
                    player.TriggerEvent("Player:HUD:DeathEvent", false, killer.Name, ServerAccounts.GetPlayerKills(playerId), ServerAccounts.GetPlayerDeaths(playerId), player.Name);
                    TeamHandler.SpawnPlayer(killer);
                    return;
                }
                if (player == killer) return;
                if (ServerAccounts.GetPlayerLockState(killerId) > 0) killer.Health = 100; killer.Armor += 25;
                int givenEXP = 5 * Constants.ServerConfig.XPMultiplikator;
                if (ServerAccounts.GetPlayerLockState(killerId) > 1) givenEXP = 5 * Constants.ServerConfig.XPMultiplikator * ServerAccounts.GetPlayerLockState(killerId);
                if (ServerAccounts.GetPlayerFFAArena(playerId) > 0 && ServerAccounts.GetPlayerFFAArena(killerId) > 0 && ServerAccounts.GetPlayerFFAArena(playerId) == ServerAccounts.GetPlayerFFAArena(killerId))
                {
                    //In FFA
                    killer.Armor = 100;
                    ServerAccounts.IncreasePlayerDeaths(playerId);
                    ServerAccounts.IncreasePlayerKills(killerId);
                    ServerAccounts.SetPlayerMoney(killerId, ServerAccounts.getMoney(killerId) + 250);
                    ServerAccounts.SetPlayerEXP(killerId, ServerAccounts.GetPlayerEXP(killerId) + givenEXP);
                    if (killer.getAdminLevel() >= 3) ServerAccounts.SetPlayerEXP(killerId, ServerAccounts.GetPlayerEXP(killerId) + givenEXP);
                    if (ServerAccounts.GetPlayerEXP(killerId) >= ServerAccounts.GetPlayerLevel(killerId) * 15)
                    {
                        ServerAccounts.SetPlayerEXP(killerId, 0);
                        ServerAccounts.SetPlayerLevel(killerId, ServerAccounts.GetPlayerLevel(killerId) + 1);
                        killer.TriggerEvent("Player:HUD:LevelUP", ServerAccounts.GetPlayerLevel(killerId), ServerAccounts.GetPlayerLevel(killerId) * 15);
                    }
                    killer.TriggerEvent("Player:HUD:setMoney", ServerAccounts.getMoney(killerId));
                    killer.TriggerEvent("Player:HUD:KillEvent", false, killer.Name, ServerAccounts.GetPlayerKills(killerId), ServerAccounts.GetPlayerDeaths(killerId), player.Name, false);
                    player.TriggerEvent("Player:HUD:DeathEvent", false, killer.Name, ServerAccounts.GetPlayerKills(playerId), ServerAccounts.GetPlayerDeaths(playerId), player.Name);
                }
                else
                {
                    //nicht in FFA
                    if (!sameTeam)
                    {
                        ServerAccounts.IncreasePlayerDeaths(playerId);
                        ServerAccounts.IncreasePlayerKills(killerId);
                        ServerAccounts.SetPlayerMoney(killerId, ServerAccounts.getMoney(killerId) + 250);
                        ServerAccounts.SetPlayerEXP(killerId, ServerAccounts.GetPlayerEXP(killerId) + givenEXP);
                        if (killer.getAdminLevel() >= 3) ServerAccounts.SetPlayerEXP(killerId, ServerAccounts.GetPlayerEXP(killerId) + givenEXP);
                        if (ServerAccounts.GetPlayerEXP(killerId) >= ServerAccounts.GetPlayerLevel(killerId) * 15)
                        {
                            ServerAccounts.SetPlayerEXP(killerId, 0);
                            ServerAccounts.SetPlayerLevel(killerId, ServerAccounts.GetPlayerLevel(killerId) + 1);
                            killer.TriggerEvent("Player:HUD:LevelUP", ServerAccounts.GetPlayerLevel(killerId), ServerAccounts.GetPlayerLevel(killerId) * 15);
                        }
                    }
                    killer.TriggerEvent("Player:HUD:setMoney", ServerAccounts.getMoney(killerId));
                    killer.TriggerEvent("Player:HUD:KillEvent", sameTeam, killer.Name, ServerAccounts.GetPlayerKills(killerId), ServerAccounts.GetPlayerDeaths(killerId), player.Name, false);
                    player.TriggerEvent("Player:HUD:DeathEvent", sameTeam, killer.Name, ServerAccounts.GetPlayerKills(playerId), ServerAccounts.GetPlayerDeaths(playerId), player.Name);
                }

                killer.TriggerEvent("Player:HUD:setExperience", ServerAccounts.GetPlayerEXP(killerId));
                killer.TriggerEvent("Player:HUD:setKills", ServerAccounts.GetPlayerKills(killerId));
                player.TriggerEvent("Player:HUD:setDeaths", ServerAccounts.GetPlayerDeaths(playerId));

                var gwZoneKill = Models.ServerGWZones.ServerGangwarZones_.FirstOrDefault(x => x.underAttack && player.Position.IsInRange(new Vector3(x.attackPosX, x.attackPosY, x.attackPosZ), 150f) && killer.Position.IsInRange(new Vector3(x.attackPosX, x.attackPosY, x.attackPosZ), 150f) && (x.currentOwner == Models.ServerAccounts.GetAccountSelectedTeam(killerId) || x.attacker == Models.ServerAccounts.GetAccountSelectedTeam(killerId)));
                if (gwZoneKill == null) return;
                if (gwZoneKill.attacker == Models.ServerAccounts.GetAccountSelectedTeam(killerId))
                {
                    gwZoneKill.currentAttackerPoints += 2;
                }
                else if (gwZoneKill.currentOwner == Models.ServerAccounts.GetAccountSelectedTeam(killerId))
                {
                    gwZoneKill.currentDefenderPoints += 2;
                }
                ChatHandler.SendFactionMessage(Models.ServerAccounts.GetAccountSelectedTeam(killerId), $"[~p~GW Kill~w~] {killer.Name} hat {player.Name} getötet (+2 Punkte).");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
