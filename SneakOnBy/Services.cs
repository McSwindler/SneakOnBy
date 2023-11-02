using Dalamud.IoC;
using Dalamud.Plugin.Services;
using Dalamud.Plugin;

public class Services
{
        public static void Initialize(DalamudPluginInterface pluginInterface)
            => pluginInterface.Create<Services>();

        // @formatter:off
        [PluginService][RequiredVersion("1.0")] public static IClientState ClientState { get; private set; } = null!;
        [PluginService][RequiredVersion("1.0")] public static IGameInteropProvider GameInteropProvider { get; private set; } = null!;
        [PluginService][RequiredVersion("1.0")] public static IPluginLog PluginLog { get; private set; } = null!;
        [PluginService][RequiredVersion("1.0")] public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService][RequiredVersion("1.0")] public static IFramework Framework { get; private set; } = null!;
        [PluginService][RequiredVersion("1.0")] public static ICondition Condition { get; private set; } = null!;
        [PluginService][RequiredVersion("1.0")] public static IObjectTable Objects { get; private set; } = null!;
        [PluginService][RequiredVersion("1.0")] public static IGameGui GameGui { get; private set; } = null!;
        [PluginService][RequiredVersion("1.0")] public static IKeyState KeyState { get; private set; } = null!;
        // @formatter:on

}
