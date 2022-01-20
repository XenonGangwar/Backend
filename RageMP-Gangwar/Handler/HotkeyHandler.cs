using GTANetworkAPI;
using RageMP_Gangwar.Models;
using RageMP_Gangwar.Utilities;
using System;

namespace RageMP_Gangwar.Handler
{
    public class HotkeyHandler : Script
    {
        [RemoteEvent("Server:User:useFirstAidKit")]
        public void useFirstAidKit(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                NAPI.Player.PlayPlayerAnimation(player, (int)(Constants.AnimationFlags.Loop | Constants.AnimationFlags.AllowPlayerControl), "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01");
                NAPI.Task.Run(() =>
                {
                    player.Health = 100;
                    NAPI.Player.StopPlayerAnimation(player);
                }, delayTime: 4000);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [RemoteEvent("Server:User:useVest")]
        public void useVest(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId()) return;
                NAPI.Player.PlayPlayerAnimation(player, (int)(Constants.AnimationFlags.Loop | Constants.AnimationFlags.AllowPlayerControl), "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01");
                NAPI.Task.Run(() =>
                {
                    NAPI.Player.StopPlayerAnimation(player);
                    player.Armor = 100;
                    player.SetClothes(9, 15, 2);
                }, delayTime: 4000);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        [RemoteEvent("Server:User:repairVehicle")]
        public void repairVehicle(Player player)
        {
            try
            {
                if (player == null || !player.Exists || !player.hasAccountId() || player.IsInVehicle) return;
                var Vehicle = Helper.GetClosestVehicle(player, 5f);
                if (Vehicle == null) return;
                NAPI.Player.PlayPlayerAnimation(player, (int)(Constants.AnimationFlags.Loop | Constants.AnimationFlags.AllowPlayerControl), "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01");
                Vehicle.Repair();
                NAPI.Task.Run(() =>
                {
                    player.StopAnimation();
                    player.SendChatMessage($"Fahrzeug erfolgreich repartiert.");
                }, delayTime: 4000);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
                throw;
            }
        }

        [RemoteEvent("Server:User:toggleMask")]
        public void toggleMask(Player player)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || player.IsInVehicle) return;
            int teamId = ServerAccounts.GetAccountSelectedTeam(player.getAccountId());
            if (teamId <= 0) return;
            var factionClothes = ServerFactions.GetFactionsClothes(teamId);
            if (factionClothes == null) return;
            if (player.GetData("maske") == true)
            {
                player.TriggerEvent("ChangeClothesC", 1, 0, 0);
                player.SetData("maske", false);
            }
            else
            {
                player.TriggerEvent("ChangeClothesC", 1, factionClothes.mask, factionClothes.maskTex);
                player.SetData("maske", true);
            }
        }
    }
}
