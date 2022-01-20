using GTANetworkAPI;
using RageMP_Gangwar.Models;
using System;
using System.Linq;

namespace RageMP_Gangwar.Utilities
{
    public static partial class PlayerExtensions
    {
        public static bool IsInRange(this Vector3 currentPosition, Vector3 otherPosition, float distance)
            => currentPosition.DistanceTo(otherPosition) <= distance;

        public static void setAccountId(this Player player, int id)
        {
            if (player == null || !player.Exists) return;
            player.SetData("accountId", id);
        }

        public static int getAccountId(this Player player)
        {
            if (player == null || !player.Exists) return 0;
            return player.GetData("accountId");
        }

        public static bool hasAccountId(this Player player)
        {
            if (player == null || !player.Exists) return false;
            return player.HasData("accountId");
        }

        public static void notify(this Player player, string title, string message, int duration)
        {
            player.TriggerEvent("Player:HUD:ModMessage", title, message, duration);
        }

        public static void sendAdminMessage(this Player player, string message)
        {
            player.SendChatMessage("[~r~SERVER~w~] " + message);
        }

        public static void removePurchasedWeapons(this Player p)
        {
            p.RemoveWeapon(WeaponHash.MarksmanRifle);
            p.RemoveWeapon(WeaponHash.Revolver);
            p.RemoveWeapon(WeaponHash.MarksmanRifle);
        }
        public static int getAdminLevel(this Player player)
        {
            if (player == null || !player.Exists) return 0;
            var playerDb = ServerAccounts.Accounts_.FirstOrDefault(x => x.socialClub == player.SocialClubName && player.getAccountId() == x.playerId);
            return playerDb == null ? 0 : Convert.ToInt32(playerDb.adminRank);
        }

        public static int getcurrentDuellPartner(this Player player)
        {
            if (player == null || !player.Exists) return 0;
            var playerDb = ServerAccounts.Accounts_.FirstOrDefault(x => x.socialClub == player.SocialClubName && player.getAccountId() == x.playerId);
            return playerDb == null ? 0 : Convert.ToInt32(playerDb.currentDuellPartner);
        }

        public static bool isChatMuted(this Player player)
        {
            if (player == null || !player.Exists) return false;
            var playerDb = ServerAccounts.Accounts_.FirstOrDefault(x => x.socialClub == player.SocialClubName && player.getAccountId() == x.playerId);
            return playerDb == null ? false : Convert.ToBoolean(playerDb.isMuted);
        }
    }
}
