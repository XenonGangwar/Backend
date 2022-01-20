using GTANetworkAPI;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;

namespace RageMP_Gangwar.Functions
{
    public static class AccountsFunctions
    {
        public static void GiveWeapons(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || ServerAccounts.GetAccountSelectedTeam(player.getAccountId()) <= 0) return;
                /*
                player.GiveWeapon(WeaponHash.HeavyPistol, 9999);
                player.GiveWeapon(WeaponHash.BullpupRifle, 9999);
                player.GiveWeapon(WeaponHash.AdvancedRifle, 9999);
                player.GiveWeapon(WeaponHash.CarbineRifle, 9999);
                player.GiveWeapon((WeaponHash)1649403952, 9999);
                player.GiveWeapon(WeaponHash.Gusenberg, 9999);
                player.GiveWeapon(WeaponHash.Knife, 9999);
                player.GiveWeapon(WeaponHash.SpecialCarbine, 9999);
                */

                player.RemoveAllWeapons();

                int pID = player.getAccountId();

                if (ServerAccounts.getKit(pID) == 1)
                {
                    player.GiveWeapon(WeaponHash.AdvancedRifle, 9999);
                    player.GiveWeapon(WeaponHash.Gusenberg, 9999);
                    player.GiveWeapon(WeaponHash.HeavyPistol, 9999);
                }

                if (ServerAccounts.getKit(pID) == 2)
                {
                    player.GiveWeapon(WeaponHash.Revolver, 9999);
                    player.GiveWeapon(WeaponHash.Gusenberg, 9999);
                }

                if (ServerAccounts.getKit(pID) == 3)
                {
                    player.GiveWeapon(WeaponHash.AdvancedRifle, 9999);
                    player.GiveWeapon(WeaponHash.Revolver, 9999);
                }

                if (ServerAccounts.getKit(pID) == 4)
                {
                    player.GiveWeapon(WeaponHash.DoubleBarrelShotgun, 9999);
                    player.GiveWeapon(WeaponHash.MicroSMG, 9999);
                    player.GiveWeapon(WeaponHash.MiniSMG, 9999);
                }

                if (ServerAccounts.getKit(pID) == 5)
                {
                    player.GiveWeapon(WeaponHash.BullpupRifle, 9999);
                    player.GiveWeapon(WeaponHash.Gusenberg, 9999);
                    player.GiveWeapon(WeaponHash.HeavyPistol, 9999);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [RemoteEvent("Server:Kick:Kick")]
        public static void Kick_Event(Player player, string msg)
        {
            if (player == null || !player.Exists) return;
            player.Kick(msg);
        }
    }
}
