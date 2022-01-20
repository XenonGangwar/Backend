using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RageMP_Gangwar.Models
{
    class ServerGWZones
    {
        public static List<dbmodels.ServerGangwarZones> ServerGangwarZones_ = new List<dbmodels.ServerGangwarZones>();
        public static List<dbmodels.ServerGangwarFlags> ServerGangwarFlags_ = new List<dbmodels.ServerGangwarFlags>();

        public static void ResetGangwarFlags(int zoneId)
        {
            try
            {
                foreach (var gwFlags in ServerGangwarFlags_.Where(x => x.zoneId == zoneId))
                {
                    gwFlags.currentOwner = 0;

                    foreach (var blip in NAPI.Pools.GetAllBlips().ToList().Where(x => x != null && x.Exists && x.HasData("GangwarFlag") && x.GetData("GangwarFlag") == gwFlags.id))
                    {
                        blip.Delete();
                        Blip blips = NAPI.Blip.CreateBlip(164, new Vector3(gwFlags.flagX, gwFlags.flagY, gwFlags.flagZ), 1.0f, 0, $"Flagge", 255, 0, true);
                        blips.SetData("GangwarFlag", gwFlags.id);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetGangwarFlagOwner(int flag, byte color)
        {
            try
            {
                var gwFlag = ServerGangwarFlags_.FirstOrDefault(x => x.id == flag);
                if (gwFlag == null) return;
                foreach (var blip in NAPI.Pools.GetAllBlips().ToList().Where(x => x != null && x.Exists && x.HasData("GangwarFlag") && x.GetData("GangwarFlag") == gwFlag.id))
                {
                    blip.Delete();
                    Blip blips = NAPI.Blip.CreateBlip(164, new Vector3(gwFlag.flagX, gwFlag.flagY, gwFlag.flagZ), 1.0f, color, $"Flagge", 255, 0, true);
                    blips.SetData("GangwarFlag", gwFlag.id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }

        public static void SetGangwarBlipOwner(int gwZones, byte color)
        {
            try
            {
                var gwZone = ServerGangwarZones_.FirstOrDefault(x => x.id == gwZones);
                if (gwZone == null) return;
                foreach (var blip in NAPI.Pools.GetAllBlips().ToList().Where(x => x != null && x.Exists && x.HasData("GangwarBlip") && x.GetData("GangwarBlip") == gwZone.id))
                {
                    blip.Delete();
                    Blip blips = NAPI.Blip.CreateBlip(84, new Vector3(gwZone.attackPosX, gwZone.attackPosY, gwZone.attackPosZ), 1.0f, color, $"Flagge", 255, 0, true);
                    blips.SetData("GangwarBlip", gwZone.id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
