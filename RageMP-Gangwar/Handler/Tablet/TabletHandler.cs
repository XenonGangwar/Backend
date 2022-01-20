using GTANetworkAPI;
using RageMP_Gangwar.Utilities;

namespace RageMP_Gangwar.Handler.Tablet
{
    public class TabletHandler
    {

        [RemoteEvent("Tablet:Shot")]
        public void Tablet_Shot(Player player, Player target)
        {
            if (player == null || !player.Exists || !player.hasAccountId() || target == null || !target.Exists || !target.hasAccountId() || player.getAdminLevel() < 4) return;
            if (player.CurrentWeapon == WeaponHash.StunGun)
            {

            }
            else
            {
                return;
            }
        }

    }
}
