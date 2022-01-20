using GTANetworkAPI;
using RageMP_Gangwar.Functions;
using RageMP_Gangwar.Utilities;
using System;
using System.Linq;

namespace RageMP_Gangwar.Handler
{
    public class KeyHandler : Script
    {
        [RemoteEvent("restoreCustomazion")]
        public void hs(Player player)
        {
            if (player == null || !player.Exists || !player.hasAccountId()) return;

            ClothesFunctions.setCorrectTeamClothes(player);
            AccountsFunctions.GiveWeapons(player);
        }

        [RemoteEvent("Server:Keyhandler:E")]
        public void KeyHandler_E(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                var factionGarage = Models.ServerFactions.ServerFactionsGarage_.FirstOrDefault(x => player.Position.IsInRange(new Vector3(x.pedX, x.pedY, x.pedZ), 2f) && x.factionId == Models.ServerAccounts.GetAccountSelectedTeam(player.getAccountId()));
                if (factionGarage != null && !player.IsInVehicle)
                {
                    GarageHandler.openBrowser(player, factionGarage);
                    return;
                }

                var ffaZone = Models.ServerFFA.ServerFFA_.FirstOrDefault(x => player.Position.IsInRange(new Vector3(x.posX, x.posY, x.posZ), 1.5f));
                if (ffaZone != null && !player.IsInVehicle)
                {
                    FFAHandler.openFFABrowser(player, ffaZone);
                    return;
                }

                if (player.Position.IsInRange(new Vector3(298.7583, -584.4252, 43.26084), 2f))
                {
                    player.TriggerEvent("CreatorlOLXd");
                    return;
                }

                if (player.Position.IsInRange(new Vector3(21.96738, -1106.722, 29.79703), 2f))
                {
                    player.TriggerEvent("Player:OpenWeaponShop");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
