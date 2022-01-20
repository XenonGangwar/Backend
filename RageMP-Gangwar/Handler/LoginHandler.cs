using GTANetworkAPI;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;

namespace RageMP_Gangwar.Handler
{
    class LoginHandler : Script
    {
        [RemoteEvent("Server:tryLogin")]
        public void tryLogin(Player player, string username, string password)
        {
            if (player == null || !player.Exists) return;
            if (string.IsNullOrWhiteSpace(username))
            {
                player.TriggerEvent("Player:showLoginError", "Der Benutzername darf nicht leer sein.");
                return;
            }
        }

        [RemoteEvent("Server:Login:tryRegister")]
        public void tryRegister(Player player, string username, string password)
        {
            try
            {
                if (player == null || !player.Exists || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return;
                if (ServerAccounts.ExistPlayerByUserName(username))
                {
                    // Spieler gefunden!
                    player.TriggerEvent("Player:Login:LoginResult", 2, "Dieser Username wurde bereits verwendet. Wähle ein anderen!");
                    return;
                }
                else
                {
                    // Spieler nicht gefunden!
                    if (ServerAccounts.ExistAccountBySC(player.SocialClubName))
                    {
                        player.TriggerEvent("Player:Login:LoginResult", 2, "Es existiert bereits ein Account mit deinem Social-Club Namen.");
                        return;
                    }

                    ServerAccounts.CreateAccount(player, username, password);
                    player.TriggerEvent("Player:Login:LoginResult", 0, "Erfolgreich registriert.");
                    FinishLogin(player);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }

        [RemoteEvent("Server:Login:tryLogin")]
        public void servertryLogin(Player player, string username, string password)
        {
            try
            {
                if (player == null || !player.Exists || string.IsNullOrWhiteSpace(username)) return;
                if (ServerAccounts.ExistPlayerByUserName(username))
                {
                    //Spieler existiert
                    if (ServerAccounts.GetAccountHardwareId(username) == "unset")
                    {
                        int pID = ServerAccounts.GetAccountIdByName(username);
                        ServerAccounts.SetPlayerHwId(pID, player.Serial);
                    }

                    if (ServerAccounts.GetAccountHardwareId(username) != player.Serial || ServerAccounts.GetAccountSocialClub(username) != player.SocialClubName)
                    {
                        player.TriggerEvent("Player:Login:LoginResult", 2, "Der Account gehört dir nicht! Kontaktiere den Support.");
                        return;
                    }

                    if (ServerAccounts.GetPasswordByName(username) != password)
                    {
                        player.TriggerEvent("Player:Login:LoginResult", 2, "Passwort falsch.");
                        return;
                    }

                    player.setAccountId(ServerAccounts.GetAccountIdByName(username));

                    if (ServerAccounts.IsAccountPermBanned(player.getAccountId()))
                    {
                        player.TriggerEvent("Player:Login:LoginResult", 2, "Der Account hat ein aktiven Permanenten Bann.");
                        return;
                    }
                    else
                    {

                        if (ServerAccounts.IsAccountTimeBanned(player.getAccountId()) && ServerAccounts.GetAccountBanTimestamp(player.getAccountId()) != null && DateTime.Now.Subtract((DateTime)ServerAccounts.GetAccountBanTimestamp(player.getAccountId())).TotalHours < ServerAccounts.GetTimebanHours(player.getAccountId()))
                        {
                            player.TriggerEvent("Player:Login:LoginResult", 2, "Der Account ist noch temporär gebannt.");
                            return;
                        }
                        else if (ServerAccounts.IsAccountTimeBanned(player.getAccountId()) && ServerAccounts.GetAccountBanTimestamp(player.getAccountId()) != null && DateTime.Now.Subtract((DateTime)ServerAccounts.GetAccountBanTimestamp(player.getAccountId())).TotalHours >= ServerAccounts.GetTimebanHours(player.getAccountId()))
                        {
                            ServerAccounts.SetPlayerBanned(player.getAccountId(), ServerAccounts.IsAccountPermBanned(player.getAccountId()), false);
                            ServerAccounts.SetTimebanHours(player.getAccountId(), 0);
                        }
                    }

                    player.TriggerEvent("Player:Login:LoginResult", 0, "Erfolgreich eingeloggt.");
                    FinishLogin(player);
                }
                else
                {
                    player.TriggerEvent("Player:Login:LoginResult", 2, "Es gibt unter diesen Namen keinen Account.");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
        public static void FinishLogin(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                var pInfo = ServerAccounts.GetFullAccount(player.getAccountId());
                if (pInfo == null || pInfo.playerId <= 0) return;
                NAPI.Player.SetPlayerName(player, pInfo.playerName);
                player.Position = new Vector3(-2044.288, -1031.322, 11.98072);
                player.TriggerEvent("Player:Login:destroyLogin");
                if (ServerAccounts.GetAufnahmepflicht(player.getAccountId()) == true)
                {
                    player.TriggerEvent("Player:Aufnahmepflicht");
                    player.sendAdminMessage("Du hast eine aktive Aufnahmepflicht! Denk dran aufzunehmen.");
                }
                player.TriggerEvent("Player:HUD:showHUD", pInfo.level, pInfo.kills, pInfo.deaths, pInfo.exp, pInfo.level * 15, pInfo.money); //ToDo: letzte Variable durch Max EXP ersetzen
                TeamHandler.CreateTeamSelect(player);
                player.TriggerEvent("Player:Ped:createGaragePeds", ServerFactions.GetAllFactionGarageInfo());
                player.TriggerEvent("charreloadc");
                player.TriggerEvent("updateDiscord", player.Name);


            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
