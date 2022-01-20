using GTANetworkAPI;
using RageMP_Gangwar.Utilities;

namespace RageMP_Gangwar.Handler
{
    public class SpectateHandler : Script
    {

        [RemoteEvent("sync-spectatePlayer")]
        public void spectatedPlayer(Player p, int x, int y, int z)
        {
            if (p == null || !p.Exists || !p.hasAccountId()) return;
            int pID = p.getAccountId();
            if (pID <= 0 || Models.ServerAccounts.GetAccountSelectedTeam(pID) <= 0 || p.getAdminLevel() < 4) return;

            p.Position = new Vector3(x, y, z);
        }

    }
}
