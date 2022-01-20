using GTANetworkAPI;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;

namespace RageMP_Gangwar.Handler
{
    public class WeaponShopHandler : Script
    {


        [RemoteEvent("Server:User:WeaponShop")]
        public void WeaponShop(Player player, string weapon)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                int pID = player.getAccountId();
                if (weapon == "Marksmanrifle")
                {
                    if (ServerAccounts.getMoney(pID) > 2500)
                    {
                        player.SendChatMessage($"[~y~SHOP~w~] Du hast dir eine Marksmanrifle gekauft.");
                        player.GiveWeapon(WeaponHash.MarksmanRifle, 9999);

                        player.TriggerEvent("Player:CloseWeaponShop");
                        ServerAccounts.SetPlayerMoney(pID, ServerAccounts.getMoney(pID) - 2500);
                        player.TriggerEvent("Player:HUD:setMoney", ServerAccounts.getMoney(pID));
                    }
                    else
                    {
                        player.SendChatMessage($"[~y~SHOP~w~] Du hast keine ~g~2.500$ ~w~dabei.");
                        player.TriggerEvent("Player:CloseWeaponShop");
                        return;
                    }
                }

                if (weapon == "Revolver")
                {
                    if (ServerAccounts.getMoney(pID) > 6000)
                    {
                        player.SendChatMessage($"[~y~SHOP~w~] Du hast dir eine Revolver gekauft.");
                        player.GiveWeapon(WeaponHash.Revolver, 9999);
                        player.TriggerEvent("Player:CloseWeaponShop");
                        ServerAccounts.SetPlayerMoney(pID, ServerAccounts.getMoney(pID) - 6000);
                        player.TriggerEvent("Player:HUD:setMoney", ServerAccounts.getMoney(pID));
                    }
                    else
                    {
                        player.SendChatMessage($"[~y~SHOP~w~] Du hast keine ~g~6.000$ ~w~dabei.");
                        player.TriggerEvent("Player:CloseWeaponShop");
                        return;
                    }
                }

                if (weapon == "HeavySniper")
                {
                    if (ServerAccounts.getMoney(pID) > 22000)
                    {
                        player.SendChatMessage($"[~y~SHOP~w~] Du hast dir eine Heavy-Sniper gekauft.");
                        player.GiveWeapon(WeaponHash.HeavySniper, 9999);
                        player.TriggerEvent("Player:CloseWeaponShop");
                        ServerAccounts.SetPlayerMoney(pID, ServerAccounts.getMoney(pID) - 22000);
                        player.TriggerEvent("Player:HUD:setMoney", ServerAccounts.getMoney(pID));
                    }
                    else
                    {
                        player.SendChatMessage($"[~y~SHOP~w~] Du hast keine ~g~22.000$ ~w~dabei.");
                        player.TriggerEvent("Player:CloseWeaponShop");
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
