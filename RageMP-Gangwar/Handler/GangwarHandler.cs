using GTANetworkAPI;
using RageMP_Gangwar.Utilities;
using System;
using System.Linq;

namespace RageMP_Gangwar.Handler
{
    class GangwarHandler : Script
    {
        [Command("startgangwar")]
        public void CMD_StartGangwar(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()) <= 0) return;
                var gwZone = Models.ServerGWZones.ServerGangwarZones_.FirstOrDefault(x => player.Position.IsInRange(new Vector3(x.attackPosX, x.attackPosY, x.attackPosZ), 5f));
                if (gwZone == null) return;
                var useGwZone = Models.ServerGWZones.ServerGangwarZones_.FirstOrDefault(x => x.underAttack && x.currentOwner == Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()) || x.attacker == Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()));
                if (useGwZone != null)
                {
                    player.SendChatMessage($"[~p~GANGWAR~w~] Dein oder das Gegnerteam sind bereits in einem Gangwar.");
                    return;
                }

                bool isPlayerInPrivateTeam = (Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()) == Models.ServerAccounts.GetAccountFaction(player.getAccountId())) && Models.ServerAccounts.GetAccountFaction(player.getAccountId()) > 0;
                if (gwZone.isPrivate && !isPlayerInPrivateTeam) return;

                if (gwZone.underAttack)
                {
                    player.SendChatMessage($"[~p~GANGWAR~w~] Dieses Gebiet wird bereits angegriffen.");
                    return;
                }

                if (gwZone.currentOwner == Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()))
                {
                    player.SendChatMessage($"[~p~GANGWAR~w~] Dieses Gebiet gehört bereits deinem Team.");
                    return;
                }

                int defenderCountOnline = NAPI.Pools.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.hasAccountId() && Models.ServerAccounts.GetAccountSelectedTeam(x.getAccountId()) == gwZone.currentOwner).Count();
                //int defenderCountOnline = Models.ServerFactions.GetFactionMemberCount(gwZone.currentOwner);
                int attackerCountInArea = NAPI.Pools.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.hasAccountId() && Models.ServerAccounts.GetAccountSelectedTeam(x.getAccountId()) == Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId())).Count();
                NAPI.Chat.SendChatMessageToAll($"Debug: {defenderCountOnline} | {attackerCountInArea} | {gwZone.currentOwner}");
                if (attackerCountInArea < 3 || defenderCountOnline < 3)
                {
                    player.SendChatMessage($"[~p~GANGWAR~w~] Dein Team oder das Gegnerteam besteht nicht aus 0 Personen (das Angreiferteam muss aus mindestens 0 Personen vor Ort bestehen).");
                    return;
                }

                gwZone.underAttack = true;
                gwZone.attacker = Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId());
                gwZone.currentAttackerPoints = 0;
                gwZone.currentDefenderPoints = 0;
                gwZone.timePlayed = 0;
                Marker marker = NAPI.Marker.CreateMarker(MarkerType.VerticalCylinder, new Vector3(gwZone.attackPosX, gwZone.attackPosY, gwZone.attackPosZ), new Vector3(gwZone.attackPosX, gwZone.attackPosY, gwZone.attackPosZ), new Vector3(0, 0, 0), 150f, new Color(0, 0, 0));
                marker.SetData("temporaryGangwarZone", gwZone.id);
                marker.Transparency = 125;
                Models.ServerGWZones.ResetGangwarFlags(gwZone.id);
                ChatHandler.SendFactionMessage(gwZone.currentOwner, $"[~p~GANGWAR~w~] Euer Gebiet '{gwZone.zoneName}' wird angegriffen, verteidigt es!");
                ChatHandler.SendFactionMessage(gwZone.attacker, $"[~p~GANGWAR~w~] Ihr greift nun das Gangwar Gebiet '{gwZone.zoneName}' an. Holt euch den Sieg!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [RemoteEvent("kick")]
        public void kick(Player p)
        {

            p.Kick();
        }

        [RemoteEvent("Server:Gangwar:captureFlag")]
        public static void CMD_CaptureFlag(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()) <= 0) return;
                var gwFlag = Models.ServerGWZones.ServerGangwarFlags_.FirstOrDefault(x => player.Position.IsInRange(new Vector3(x.flagX, x.flagY, x.flagZ), 2.5f));
                if (gwFlag == null) return;
                var gwZone = Models.ServerGWZones.ServerGangwarZones_.FirstOrDefault(x => x.id == gwFlag.zoneId);
                if (gwZone == null || !gwZone.underAttack || (Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()) != gwZone.attacker && Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()) != gwZone.currentOwner)) return;
                if (gwFlag.currentOwner == Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId())) return;
                gwFlag.currentOwner = Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId());
                ChatHandler.SendFactionMessage(gwZone.attacker, $"[~p~GANGWAR~w~] Das Team '{Models.ServerFactions.GetFactionName(Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()))}' hat eine Flagge erobert.");
                ChatHandler.SendFactionMessage(gwZone.currentOwner, $"[~p~GANGWAR~w~] Das Team '{Models.ServerFactions.GetFactionName(Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()))}' hat eine Flagge erobert.");
                byte color = Convert.ToByte(Models.ServerFactions.GetFactionBlipColor(Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId())));
                Models.ServerGWZones.SetGangwarFlagOwner(gwFlag.id, color);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        internal static void GangwarTimer()
        {
            try
            {
                foreach (var gwZone in Models.ServerGWZones.ServerGangwarZones_.Where(x => x.underAttack))
                {
                    if (gwZone.timePlayed >= 30)
                    {
                        //Gangwar zuende
                        foreach (var p in NAPI.Pools.GetAllPlayers().ToList().Where(x => x != null && x.Exists))
                        {
                            if (p == null || !p.Exists || !p.hasAccountId()) continue;
                            if (Models.ServerAccounts.GetAccountSelectedTeam(p.getAccountId()) != gwZone.attacker && Models.ServerAccounts.GetAccountSelectedTeam(p.getAccountId()) != gwZone.currentOwner) continue;
                            TeamHandler.SpawnPlayer(p);
                        }

                        bool attackerWins = gwZone.currentAttackerPoints > gwZone.currentDefenderPoints;
                        if (attackerWins)
                        {
                            ChatHandler.SendFactionMessage(gwZone.attacker, $"[~p~GANGWAR~w~] Euer Team hat das Gangwar-Gebiet '{gwZone.zoneName}' erfolgreich ~g~erobert~w~.");
                            ChatHandler.SendFactionMessage(gwZone.currentOwner, $"[~p~GANGWAR~w~] Euer Team hat das Gangwar-Gebiet '{gwZone.zoneName}' ~r~verloren~w~.");
                            gwZone.currentOwner = gwZone.attacker;
                            Models.ServerGWZones.SetGangwarBlipOwner(gwZone.id, Convert.ToByte(Models.ServerFactions.GetFactionBlipColor(gwZone.currentOwner)));
                        }
                        else
                        {
                            ChatHandler.SendFactionMessage(gwZone.attacker, $"[~p~GANGWAR~w~] Euer Team hat das Gangwar-Gebiet '{gwZone.zoneName}' ~r~nicht erobert~w~.");
                            ChatHandler.SendFactionMessage(gwZone.currentOwner, $"[~p~GANGWAR~w~] Euer Team hat das Gangwar-Gebiet '{gwZone.zoneName}' ~g~erfolgreich verteidigt~w~.");
                        }
                        gwZone.timePlayed = 0;
                        gwZone.currentAttackerPoints = 0;
                        gwZone.currentDefenderPoints = 0;
                        gwZone.underAttack = false;
                        gwZone.attacker = 0;
                        Models.ServerGWZones.ResetGangwarFlags(gwZone.id);
                        foreach (var marker in NAPI.Pools.GetAllMarkers().ToList().Where(x => x != null && x.Exists && x.HasData("temporaryGangwarZone") && x.GetData("temporaryGangwarZone") == gwZone.id)) marker.Delete();

                        using (var db = new dbmodels.gtaContext())
                        {
                            db.ServerGangwarZones.Update(gwZone);
                            db.SaveChangesAsync();
                        }
                        return;
                    }

                    int attackerFlagCount = Models.ServerGWZones.ServerGangwarFlags_.Where(x => x.zoneId == gwZone.id && x.currentOwner == gwZone.attacker).Count();
                    int defenderFlagCount = Models.ServerGWZones.ServerGangwarFlags_.Where(x => x.zoneId == gwZone.id && x.currentOwner == gwZone.currentOwner).Count();
                    gwZone.currentAttackerPoints = gwZone.currentAttackerPoints + (attackerFlagCount * 5);
                    gwZone.currentDefenderPoints = gwZone.currentDefenderPoints + (defenderFlagCount * 5);
                    gwZone.timePlayed += 2;
                    ChatHandler.SendFactionMessage(gwZone.attacker, $"[~p~GANGWAR~w~] Eroberte Flaggen: {attackerFlagCount} | Team-Punkte: {gwZone.currentAttackerPoints} | Gegner-Punkte: {gwZone.currentDefenderPoints} | Spieldauer: {gwZone.timePlayed} Minuten | Gebiet: {gwZone.zoneName}.");
                    ChatHandler.SendFactionMessage(gwZone.currentOwner, $"[~p~GANGWAR~w~] Eroberte Flaggen: {defenderFlagCount} | Team-Punkte: {gwZone.currentDefenderPoints} | Gegner-Punkte: {gwZone.currentAttackerPoints} | Spieldauer: {gwZone.timePlayed} Minuten | Gebiet: {gwZone.zoneName}.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
