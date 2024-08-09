using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.Game.InstanceContent;
using Lumina.Excel.GeneratedSheets;
using SneakOnBy.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakOnBy.Utility
{
    public static class DeepDungeonUtil
    {

        public static unsafe InstanceContentDeepDungeon* GetDirector()
        {
            var eventFramework = EventFramework.Instance();
            return eventFramework == null ? null : eventFramework->GetInstanceContentDeepDungeon();
        }

        public static unsafe bool InDeepDungeon() => GetDirector() != null;

        public static unsafe byte? GetFloor()
        {
            var director = GetDirector();
            if (director is null) return null;
            return director->Floor;
        }

        public static uint? GetFloorSetId()
        {
            if (GetFloor() is { } floor)
            {
                return (uint)((floor - 1) / 10 * 10 + 1);
            }

            return null;
        }

        public static DeepDungeonType? GetDeepDungeonType()
        {
            if (Services.DataManager.GetExcelSheet<TerritoryType>()?.GetRow(Services.ClientState.TerritoryType) is { } territoryInfo)
            {
                return territoryInfo switch
                {
                    { TerritoryIntendedUse: 31, ExVersion.Row: 0 or 1 } => DeepDungeonType.PalaceOfTheDead,
                    { TerritoryIntendedUse: 31, ExVersion.Row: 2 } => DeepDungeonType.HeavenOnHigh,
                    { TerritoryIntendedUse: 31, ExVersion.Row: 4 } => DeepDungeonType.EurekaOrthos,
                    _ => null
                };
            }

            return null;
        }

        public static String? GetUniqueBattleNpcId(uint bNpcId)
        {
            if (GetDeepDungeonType() is DeepDungeonType dungeonType && GetFloorSetId() is uint floorSet)
            {
                return $"{dungeonType.Shorthand()}_{floorSet:D3}_{bNpcId}";
            }

            return null;
        }
    }
}
