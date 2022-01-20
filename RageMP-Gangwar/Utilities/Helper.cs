using GTANetworkAPI;
using System;
using System.Linq;

namespace RageMP_Gangwar.Utilities
{
    public class Helper : Script
    {
        public static Vehicle GetClosestVehicle(Player player, float distance)
        {
            try
            {
                if (player == null || !player.Exists) return null;
                Vehicle returned = null;
                returned = NAPI.Pools.GetAllVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && player.Position.IsInRange(x.Position, distance));
                return returned;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
            return null;
        }
    }
}