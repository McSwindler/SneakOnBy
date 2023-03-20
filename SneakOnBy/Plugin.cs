using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using SneakOnBy.Windows;
using ECommons;

namespace SneakOnBy
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Sneak On By";
        private const string CommandName = "/sneaky";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SneakOnBy");

        private ConfigWindow ConfigWindow { get; init; }
        private Canvas Canvas { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            ECommonsMain.Init(pluginInterface, this, Module.DalamudReflector, Module.ObjectFunctions);

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream

            ConfigWindow = new ConfigWindow(this);
            Canvas = new Canvas(this);
            
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(Canvas);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Shows configuration for Sneak On By"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();

            ECommonsMain.Dispose();


            ConfigWindow.Dispose();
            Canvas.Dispose();
            
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            ConfigWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
