using Dalamud.Configuration;
using Dalamud.Interface.Colors;
using Dalamud.Plugin;
using Dalamud.Utility.Numerics;
using System;
using System.Numerics;

namespace SneakOnBy
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool EnableLineOfSight { get; set; } = true;
        public Vector4 LineOfSightColor { get; set; } = ImGuiColors.DalamudRed.WithW(0.2f);
        public bool EnableProximity { get; set; } = true;
        public Vector4 ProximityColor { get; set; } = ImGuiColors.DalamudRed.WithW(0.2f);
        public bool EnableSound { get; set; } = true;
        public Vector4 SoundColor { get; set; } = ImGuiColors.DalamudOrange.WithW(0.2f);
        public bool EnableUnknown { get; set; } = false;
        public Vector4 UnknownColorFront { get; set; } = ImGuiColors.DalamudGrey.WithW(0.2f);
        public Vector4 UnknownColorBack { get; set; } = ImGuiColors.DalamudGrey3.WithW(0.2f);

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
