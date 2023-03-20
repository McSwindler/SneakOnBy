using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Common.Configuration;
using ImGuiNET;

namespace SneakOnBy.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;

    public ConfigWindow(Plugin plugin) : base("Sneaky Configuration")
    {
        this.Size = new Vector2(300, 256);
        this.SizeCondition = ImGuiCond.FirstUseEver;

        this.configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        DrawLineOfSightToggle();
        ImGui.Separator();

        DrawProximityToggle();
        ImGui.Separator();

        DrawSoundToggle();
        ImGui.Separator();

        DrawUnknownToggle();
    }

    private void DrawLineOfSightToggle()
    {
        ImGui.PushID("LOS");

        var configValue = this.configuration.EnableLineOfSight;
        if (ImGui.Checkbox("Enable Line of Sight Zones", ref configValue))
        {
            this.configuration.EnableLineOfSight = configValue;
            this.configuration.Save();
        }

        ImGui.Indent();
        ImGui.BeginDisabled(!configValue);
        ImGui.Spacing();

        var losColor = this.configuration.LineOfSightColor;
        if (ImGui.ColorEdit4("Line of Sight Color", ref losColor, ImGuiColorEditFlags.NoInputs))
        {
            this.configuration.LineOfSightColor = losColor;
            this.configuration.Save();
        }

        ImGui.EndDisabled();
        ImGui.Unindent();
        ImGui.PopID();
    }

    private void DrawProximityToggle()
    {
        ImGui.PushID("prox");

        var configValue = this.configuration.EnableProximity;
        if (ImGui.Checkbox("Enable Proximity Zones", ref configValue))
        {
            this.configuration.EnableProximity = configValue;
            this.configuration.Save();
        }

        ImGui.Indent();
        ImGui.BeginDisabled(!configValue);
        ImGui.Spacing();

        var color = this.configuration.ProximityColor;
        if (ImGui.ColorEdit4("Proximity Color", ref color, ImGuiColorEditFlags.NoInputs))
        {
            this.configuration.ProximityColor = color;
            this.configuration.Save();
        }

        ImGui.EndDisabled();
        ImGui.Unindent();
        ImGui.PopID();
    }

    private void DrawSoundToggle()
    {
        ImGui.PushID("sound");

        var configValue = this.configuration.EnableSound;
        if (ImGui.Checkbox("Enable Sound Zones", ref configValue))
        {
            this.configuration.EnableSound = configValue;
            this.configuration.Save();
        }

        ImGui.Indent();
        ImGui.BeginDisabled(!configValue);
        ImGui.Spacing();

        var color = this.configuration.SoundColor;
        if (ImGui.ColorEdit4("Sound Color", ref color, ImGuiColorEditFlags.NoInputs))
        {
            this.configuration.SoundColor = color;
            this.configuration.Save();
        }

        ImGui.EndDisabled();
        ImGui.Unindent();
        ImGui.PopID();
    }

    private void DrawUnknownToggle()
    {
        ImGui.PushID("unknown");

        var configValue = this.configuration.EnableUnknown;
        if (ImGui.Checkbox("Enable Unknown Zones", ref configValue))
        {
            this.configuration.EnableUnknown = configValue;
            this.configuration.Save();
        }

        ImGui.Indent();
        ImGui.BeginDisabled(!configValue);
        ImGui.Spacing();

        var color = this.configuration.UnknownColorFront;
        if (ImGui.ColorEdit4("Unknown Line of Sight Color", ref color, ImGuiColorEditFlags.NoInputs))
        {
            this.configuration.UnknownColorFront = color;
            this.configuration.Save();
        }

        var color2 = this.configuration.UnknownColorBack;
        if (ImGui.ColorEdit4("Unknown Proximity Color", ref color2, ImGuiColorEditFlags.NoInputs))
        {
            this.configuration.UnknownColorBack = color2;
            this.configuration.Save();
        }

        ImGui.EndDisabled();
        ImGui.Unindent();
        ImGui.PopID();
    }
}
