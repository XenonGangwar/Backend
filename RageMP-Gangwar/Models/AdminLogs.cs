using RageMP_Gangwar.dbmodels;
using System;
using System.Collections.Generic;

namespace RageMP_Gangwar.Models
{
    public class AdminLogs
    {
        public static List<dbmodels.AdminLogs> AdminLogs_ = new List<dbmodels.AdminLogs>();

        public static void AddNewAdminLog(int accountId, int targetId, string action, string description)
        {
            try
            {
                using (var db = new gtaContext())
                {
                    db.AdminLogs.Add(new dbmodels.AdminLogs
                    {
                        accountId = accountId,
                        targetId = targetId,
                        action = action,
                        description = description,
                        timestamp = DateTime.Now
                    });
                    db.SaveChanges();
                    Discord.Handler.SendMessage($"Teammitglied-ID: {accountId} | Name vom Teammitglied: {Models.ServerAccounts.GetAccountNameById(accountId)} | ID vom Spieler: {targetId} | Name vom Spieler: {Models.ServerAccounts.GetAccountNameById(targetId)} | Aktion: {action} | Beschreibung: {description}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
