using GTANetworkAPI;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;

namespace RageMP_Gangwar.Functions
{
    public class ClothesFunctions
    {
        public static void setCorrectTeamClothes(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || ServerAccounts.GetAccountSelectedTeam(player.getAccountId()) <= 0) return;
                int teamId = ServerAccounts.GetAccountSelectedTeam(player.getAccountId());
                if (teamId <= 0) return;
                var factionClothes = ServerFactions.GetFactionsClothes(teamId);
                if (factionClothes == null) return;
                player.SetAccessories(0, factionClothes.hat, factionClothes.hatTex);
                player.SetAccessories(1, factionClothes.glasses, factionClothes.glassesTex);
                player.TriggerEvent("ChangeClothesC", 1, factionClothes.mask, factionClothes.maskTex);
                player.TriggerEvent("ChangeClothesC", 3, factionClothes.torso, 0);
                player.TriggerEvent("ChangeClothesC", 4, factionClothes.leg, factionClothes.legTex);
                player.TriggerEvent("ChangeClothesC", 5, factionClothes.bag, factionClothes.bagTex);
                player.TriggerEvent("ChangeClothesC", 6, factionClothes.shoes, factionClothes.shoesTex);
                player.TriggerEvent("ChangeClothesC", 7, factionClothes.accessories, factionClothes.accessoriesTex);
                player.TriggerEvent("ChangeClothesC", 8, factionClothes.undershirt, factionClothes.undershirtTex);
                player.TriggerEvent("ChangeClothesC", 11, factionClothes.top, factionClothes.topTex);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
