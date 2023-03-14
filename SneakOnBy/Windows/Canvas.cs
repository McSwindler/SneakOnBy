using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.GameFonts;
using Dalamud.Game.ClientState;
using ECommons.GameFunctions;
using ECommons.DalamudServices;
using Dalamud.Interface;
using ECommons.MathHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Component.GUI;
using Dalamud.Logging;
using System.Buffers.Text;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Interface.Colors;
using Dalamud.Utility.Numerics;

namespace SneakOnBy.Windows;

public class Canvas : Window
{
    private Configuration configuration;

    public Canvas(Plugin plugin) : base(
        "Sneaky Overlay",
        ImGuiWindowFlags.NoInputs
        | ImGuiWindowFlags.NoTitleBar
        | ImGuiWindowFlags.NoScrollbar
        | ImGuiWindowFlags.NoBackground
        | ImGuiWindowFlags.AlwaysUseWindowPadding
        | ImGuiWindowFlags.NoSavedSettings
        | ImGuiWindowFlags.NoFocusOnAppearing
        , true)
    {
        this.IsOpen = true;
        this.RespectCloseHotkey = false;
        this.configuration = plugin.Configuration;
    }

    public override void PreDraw()
    {
        base.PreDraw();
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
        ImGuiHelpers.SetNextWindowPosRelativeMainViewport(Vector2.Zero);
        ImGui.SetNextWindowSize(ImGuiHelpers.MainViewport.Size);
    }

    public override bool DrawConditions()
    {
        return Svc.ClientState.LocalPlayer != null;
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (this.configuration.EnableLineOfSight)
        {
            foreach (GameObject obj in Svc.Objects)
            {
                if (obj is BattleNpc bnpc && bnpc.IsHostile() && !bnpc.StatusFlags.HasFlag(StatusFlags.InCombat) && bnpc.IsCharacterVisible()) {                    
                    Aggro aggro = DeepDungeonDex.GetMobAggroType(bnpc.NameId);
                    switch (aggro)
                    {
                        case Aggro.Sight:
                            ActorConeXZ(bnpc, bnpc.HitboxRadius + 10, Radians(-45), Radians(45), ImGuiColors.DalamudRed.WithW(0.2f));
                            break;
                        case Aggro.Proximity:
                            CircleArcXZ(bnpc.Position, bnpc.HitboxRadius + 10, 0f, TAU, ImGuiColors.DalamudRed.WithW(0.2f));
                            break;
                        case Aggro.Sound:
                            CircleArcXZ(bnpc.Position, bnpc.HitboxRadius + 10, 0f, TAU, ImGuiColors.DalamudOrange.WithW(0.2f));
                            break;
                    }
                }
            }
        }
    }

    public override void PostDraw()
    {
        base.PostDraw();
        ImGui.PopStyleVar();
    }

    internal static CardinalDirection GetDirection(GameObject bnpc)
    {
        return MathHelper.GetCardinalDirection(GetAngle(bnpc));
    }

    internal static (int min, int max) Get18PieForAngle(float a)
    {
        if (a.InRange(315, 360)) return (0, 45);
        if (a.InRange(0, 45)) return (-45, 0);

        if (a.InRange(45, 90)) return (270, 315);
        if (a.InRange(90, 135)) return (225, 270);

        if (a.InRange(135, 180)) return (180, 225);
        if (a.InRange(180, 225)) return (135, 180);

        if (a.InRange(225, 270)) return (90, 135);
        if (a.InRange(270, 315)) return (45, 90);
        return (default, default);
    }

    internal static float GetAngle(GameObject bnpc)
    {
        return (MathHelper.GetRelativeAngle(Svc.ClientState.LocalPlayer.Position, bnpc.Position) + bnpc.Rotation.RadToDeg()) % 360;
    }

    internal static void ActorConeXZ(GameObject actor, float radius, float startRads, float endRads, Vector4 color, bool lines = true)
    {
        ConeXZ(actor.Position, radius, startRads + actor.Rotation, endRads + actor.Rotation, color, lines);
    }

    internal static void ConeXZ(Vector3 center, float radius, float startRads, float endRads, Vector4 color, bool lines = true)
    {
        var shape = new ConvexShape(color);
        if (lines) shape.Point(center);
        shape.Arc(center, radius, startRads, endRads);
        if (lines) shape.Point(center);
        shape.Done();
    }

    internal static void CircleArcXZ(Vector3 gamePos, float radius, float startRads, float endRads, Vector4 color)
    {
        var shape = new ConvexShape(color);
        shape.Arc(gamePos, radius, startRads, endRads);
        shape.Done();
    }

    internal const float PI = MathF.PI;
    internal const float TAU = PI * 2f;

    static internal float Radians(float degrees)
    {
        return PI * degrees / 180.0f;
    }
}
