using GTANetworkAPI;
using RageMP_Gangwar.dbmodels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RageMP_Gangwar.Models
{
    class ServerAccounts
    {
        public static List<Accounts> Accounts_ = new List<Accounts>();

        public static void CreateAccount(Player player, string username, string password)
        {
            try
            {
                if (player == null || !player.Exists || string.IsNullOrWhiteSpace(username)) return;
                var accData = new Accounts
                {
                    playerName = username,
                    password = password,
                    socialClub = player.SocialClubName,
                    money = 5000,
                    selectedKit = 1,
                    hwId = player.Serial,
                    isBanned = false,
                    isTimeBanned = false,
                    warns = 0,
                    adminRank = 0,
                    timebanHours = 0,
                    banTimestamp = DateTime.Now,
                    level = 1,
                    exp = 0,
                    prestigeLevel = 0,
                    kills = 0,
                    deaths = 0,
                    faction = 0,
                    factionRank = 0,
                    lastGift = DateTime.Now,
                    curSelectedTeam = 0,
                    lockState = 0
                };

                Accounts_.Add(accData);
                using (gtaContext db = new gtaContext())
                {
                    db.Accounts.Add(accData);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }


        public static bool IsPlayerInEvent(int id)
        {
            var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
            if (accData != null) return accData.isInEvent;
            return false;
        }

        public static void SetPlayerinEvent(int id, bool state)
        {
            var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
            if (accData != null) accData.isInEvent = state;
        }

        public static void SetDisableDuellState(int accId, bool state)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) accData.duellDisabled = state;
            }
            catch (Exception e)
            {
                Console.Write($"{e}");
            }
        }

        public static bool GetDisableDuellState(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.duellDisabled;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static void SetPlayerWarns(int accId, int warns)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) accData.warns = warns;
            }
            catch (Exception e)
            {
                Console.Write($"{e}");
            }
        }

        public static int GetPlayerWarns(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.warns;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void SetPlayerDuellPartner(int accId, int duellPartner)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) accData.currentDuellPartner = duellPartner;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static bool ExistPlayerByid(int accid)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accid);
                if (accData != null) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static void SetTimebanHours(int accid, int hour)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accid);
                if (accData != null)
                {
                    accData.timebanHours = hour;
                    using (var db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetTimebanHours(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.timebanHours;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void SetPlayerHwId(int accId, string hwId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null)
                {
                    accData.hwId = hwId;
                    using (var db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetPlayerName(int accId, string name)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null)
                {
                    accData.playerName = name;
                    accData.changedNameAlready = true;
                    using (var db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetAufnahmepflicht(int accId, bool truefalse)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null)
                {
                    accData.hasAufnahmePflicht = truefalse;
                    using (var db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static bool GetAufnahmepflicht(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.hasAufnahmePflicht;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static void SetMuted(int accId, bool truefalse)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null)
                {
                    accData.mute = truefalse;
                    using (var db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static bool GetMuted(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.mute;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static int getKit(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.selectedKit;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 1;
        }

        public static void SetPlayerKit(int id, int kit)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null)
                {
                    accData.selectedKit = kit;
                    using (var db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int getMoney(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.money;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }


        public static void SetPlayerMoney(int id, int money)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null)
                {
                    accData.money = money;
                    using (var db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static bool HasPlayerChangedNameAlready(int accID)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accID);
                if (accData != null) return accData.changedNameAlready;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static bool IsPlayerADuty(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.isAduty;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static bool IsAccountTimeBanned(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.isTimeBanned;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static DateTime? GetAccountBanTimestamp(int accId)
        {
            var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
            if (accData != null) return accData.banTimestamp;
            return null;
        }

        public static void SetPlayerBanned(int accId, bool isPermBan, bool isTimeBan)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null)
                {
                    accData.isBanned = isPermBan;
                    accData.banTimestamp = DateTime.Now;
                    accData.isTimeBanned = isTimeBan;
                    using (var db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetPlayerADuty(int accId, bool state)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) accData.isAduty = state;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetPlayerFFAArena(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.ffaArena;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void SetPlayerFFAArena(int accId, int arenaId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) accData.ffaArena = arenaId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetPlayerLockState(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.lockState;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void SetPlayerLockState(int accId, int state)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) accData.lockState = state;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetPrestigeLevel(int accId, int lvl)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) accData.prestigeLevel = lvl;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetPrestigeLevel(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.prestigeLevel;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static string GetPrestigeRankName(int prestige)
        {
            try
            {
                switch (prestige)
                {
                    case 0: return "";
                    case 1: return "!{#00ffbf}I~w~";
                    case 2: return "!{#00ffbf}II~w~";
                    case 3: return "!{#00ffbf}III~w~";
                    case 4: return "!{#00ffbf}IV~w~";
                    case 5: return "!{#00ffbf}V~w~";
                    case 6: return "!{#00ffbf}VI~w~";
                    case 7: return "!{#00ffbf}VII~w~";
                    case 8: return "!{#00ffbf}VIII~w~";
                    case 9: return "!{#00ffbf}IX~w~";
                    case 10: return "!{#00ffbf}X~w~";
                    case 11: return "!{#00ffbf}XI~w~";
                    case 12: return "!{#00ffbf}XII~w~";
                    case 13: return "!{#00ffbf}XIII~w~";
                    case 14: return "!{#00ffbf}XIV~w~";
                    case 15: return "!{#00ffbf}XV~w~";
                    case 16: return "!{#00ffbf}XVI~w~";
                    case 17: return "!{#00ffbf}XVII~w~";
                    case 18: return "!{#00ffbf}XVIII~w~";
                    case 19: return "!{#00ffbf}XIX~w~";
                    case 20: return "!{#00ffbf}XX~w~";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static string GetAdminPrefix(int adminLevel)
        {
            try
            {
                switch (adminLevel)
                {
                    case 0: return ""; //Spieler
                    case 1: return "(!{#ff7b00}Freund~w~)"; //Freund
                    case 2: return "(!{#7300ff}Streamer~w~)"; //Streamer
                    case 3: return "(!{#00f7ff}Donator~w~)"; //Donator
                    case 4: return "(!{#2b7edd}Test-Supporter~w~)"; //Test Supporter
                    case 5: return "(!{#2b7edd}Supporter~w~)"; //Supporter
                    case 6: return "(!{#2b7edd}Moderator~w~)"; //Moderator
                    case 7: return "(!{#f0e007}Admin~w~)"; //Admin
                    case 8: return "(!{#ff7700}Jr. Entwickler~w~)"; //Jr. Entwickler
                    case 9: return "(!{#e00000}Manager~w~)"; //Manager
                    case 10: return "(!{#ff00dd}Entwickler~w~)"; //Entwickler
                    case 11: return "(!{#ff7b00}Stv. Projektleitung~w~)"; //stv-Projektleitung
                    case 12: return "(!{fc0000}Projektleitung~w~)"; //Projektleitung
                    case 13: return "(!{#8c0000}Entwicklungsleiter~w~)"; //Entwicklungsleiter
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static string GetChatRankColor(int aLevel)
        {
            try
            {
                switch (aLevel)
                {
                    case 0: return "";
                    case 1: return "!{#ff7b00}";
                    case 2: return "!{#7300ff}";
                    case 3: return "!{#00f7ff}";
                    case 4: return "!{#2b7edd}";
                    case 5: return "!{#2b7edd}";
                    case 6: return "!{#2b7edd}";
                    case 7: return "!{#f0e007}";
                    case 8: return "!{#ff7700}";
                    case 9: return "!{#e00000}";
                    case 10: return "!{#ff00dd}";
                    case 11: return "!{#ff7b00}";
                    case 12: return "!{fc0000}";
                    case 13: return "!{#fc0000}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static void SetAccountAdminLevel(int accId, int level)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null)
                {
                    accData.adminRank = level;
                    using (var db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void IncreasePlayerKills(int id)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null) accData.kills += 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void ResetKilLDeaths(int id)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null)
                {
                    accData.kills = 0;
                    accData.deaths = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetPlayerEXP(int id, int exp)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null) accData.exp = exp;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetPlayerLevel(int id)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null) return accData.level;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 1;
        }

        public static void SetPlayerLevel(int id, int level)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null) accData.level = level;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetPlayerEXP(int id)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null) return accData.exp;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void IncreasePlayerDeaths(int id)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null) accData.deaths += 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetPlayerKills(int id)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null) return accData.kills;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static int GetPlayerDeaths(int id)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null) return accData.deaths;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static bool IsPlayerFrakLeader(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.faction > 0 && accData.factionRank > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static bool ExistPlayerByUserName(string username)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerName == username);
                if (accData != null) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static bool ExistAccountBySC(string sc)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.socialClub == sc);
                if (accData != null) return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static string GetAccountHardwareId(string username)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerName == username);
                if (accData != null) return accData.hwId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static string GetPasswordByName(string username)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerName == username);
                if (accData != null) return accData.password;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static string GetAccountSocialClub(string username)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerName == username);
                if (accData != null) return accData.socialClub;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }

        public static bool IsAccountPermBanned(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.isBanned;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return false;
        }

        public static string GetAccountNameById(int id)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == id);
                if (accData != null) return accData.playerName;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "";
        }


        public static int GetAccountIdByName(string username)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerName == username);
                if (accData != null) return accData.playerId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static string GetAccNameBySC(string sc)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.socialClub == sc);
                if (accData != null) return accData.playerName;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return "FEHLER";
        }

        public static Accounts GetFullAccount(int accId)
        {
            var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
            return accData;
        }

        public static int GetAccountSelectedTeam(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.curSelectedTeam;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static int GetAccountFaction(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.faction;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void SetAccountFaction(int accid, int faction, int rank)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accid);
                if (accData != null)
                {
                    accData.faction = faction;
                    accData.factionRank = rank;
                    using (gtaContext db = new gtaContext())
                    {
                        db.Accounts.Update(accData);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static int GetFactionRank(int accId)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) return accData.factionRank;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return 0;
        }

        public static void SetAccountSelectedTeam(int accId, int team)
        {
            try
            {
                var accData = Accounts_.FirstOrDefault(x => x.playerId == accId);
                if (accData != null) accData.curSelectedTeam = team;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
