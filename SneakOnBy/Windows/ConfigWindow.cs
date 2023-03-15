using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SneakOnBy.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;

    public ConfigWindow(Plugin plugin) : base(
        "Sneaky Configuration",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.Size = new Vector2(232, 100);
        this.SizeCondition = ImGuiCond.Always;

        this.configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        // can't ref a property, so use a local copy
        var losConfigValue = this.configuration.EnableLineOfSight;
        if (ImGui.Checkbox("Enable Line of Sight Zones", ref losConfigValue))
        {
            this.configuration.EnableLineOfSight = losConfigValue;
            // can save immediately on change, if you don't want to provide a "Save and Close" button
            this.configuration.Save();
        }

        var proxConfigValue = this.configuration.EnableProximity;
        if (ImGui.Checkbox("Enable Proximity Zones", ref proxConfigValue))
        {
            this.configuration.EnableProximity = proxConfigValue;
            // can save immediately on change, if you don't want to provide a "Save and Close" button
            this.configuration.Save();
        }

        var soundConfigValue = this.configuration.EnableSound;
        if (ImGui.Checkbox("Enable Sound Zones", ref soundConfigValue))
        {
            this.configuration.EnableSound = soundConfigValue;
            // can save immediately on change, if you don't want to provide a "Save and Close" button
            this.configuration.Save();
        }
    }
}
