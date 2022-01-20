using GTANetworkAPI;
using RageMP_Gangwar.Utilities;
using System.Linq;

namespace RageMP_Gangwar.Handler
{
    public class AnticheatHandler : Script
    {

        public void sendMessageToTeamMember(Player player, string message)
        {
            foreach (var admin in NAPI.Pools.GetAllPlayers().ToList().Where(x => x != null && x.Exists && x.hasAccountId() && x.getAdminLevel() >= 4))
            {
                admin.notify("Dringender Anticheat-Verdacht", player.Name + $" hat das Anticheat ausgelöst! (Code: ${message})", 12000);

            }
        }


        [RemoteEvent("ok-ich-hab-crmnl")]
        public void hsssss(Player p)
        {
            p.Kick();
        }


        [RemoteEvent("Server:CheatKickKey")]
        public void ServerCheatKick(Player p, string key)
        {
            p.notify("ANTI-CHEAT", $"Du hast zu oft die {key} gedrückt, daher wurdest du gekickt.", 6500);
            p.Kick("");
        }

        [RemoteEvent("__ragemp_cheat_detected")]
        public void __ragemp_cheat_detected(Player player, int cheatCode)
        {
            string text = "Cheat Engine";
            switch (cheatCode)
            {
                case 0:
                case 1:
                    text = "Cheat Engine";
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    text = "Externer Hack";
                    break;
                case 7:
                    text = "Mod-Menü";
                    break;
                case 8:
                case 9:
                    text = "Speed Hack";
                    break;
                case 11:
                    text = "Nutzung von Sandboxie";
                    break;
            }
            sendMessageToTeamMember(player, text);
        }
    }
}
