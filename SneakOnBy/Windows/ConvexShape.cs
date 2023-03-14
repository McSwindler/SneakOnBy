using ECommons.DalamudServices;
using System.Numerics;
using ImGuiNET;
using System;
using Dalamud.Interface.Colors;
using Dalamud.Utility.Numerics;

namespace SneakOnBy.Windows;

internal class ConvexShape
{
    internal readonly ImDrawListPtr DrawList;

    internal bool cullObject = true;

    internal Vector4 color;

    internal ConvexShape(Vector4 color)
    {
        this.color = color;
        DrawList = ImGui.GetWindowDrawList();
    }

    internal void Point(Vector3 worldPos)
    {
        // TODO: implement proper clipping. everything goes crazy when
        // drawing lines outside the clip window and behind the camera
        // point
        var visible = Svc.GameGui.WorldToScreen(worldPos, out Vector2 pos);
        DrawList.PathLineTo(pos);
        if (visible) { cullObject = false; }
    }

    internal void PointRadial(Vector3 center, float radius, float radians)
    {
        Point(new Vector3(
            center.X + (radius * (float)Math.Sin(radians)),
            center.Y,
            center.Z + (radius * (float)Math.Cos(radians))
        ));
    }

    internal void Arc(Vector3 center, float radius, float startRads, float endRads)
    {
        int segments = ArcSegments(startRads, endRads);
        var deltaRads = (endRads - startRads) / segments;

        for (var i = 0; i < segments + 1; i++)
        {
            PointRadial(center, radius, startRads + (deltaRads * i));
        }
    }

    internal void Done()
    {
        if (cullObject)
        {
            DrawList.PathClear();
            return;
        }

        
            DrawList.PathFillConvex(ImGui.GetColorU32(color));
        
        
            //DrawList.PathStroke(ImGui.GetColorU32(ImGuiColors.DalamudRed), ImDrawFlags.None, 3);
        
        DrawList.PathClear();
    }

    internal const float PI = MathF.PI;
    internal const float TAU = PI * 2f;
    private const int CIRCLE_SEGMENTS = 180;

    static internal int ArcSegments(float startRads, float endRads)
    {
        return (int)(MathF.Abs(endRads - startRads) * (CIRCLE_SEGMENTS / TAU)) + 1;
    }
}
