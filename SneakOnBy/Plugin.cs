using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using SneakOnBy.Windows;
using Dalamud.Plugin.Services;
using SneakOnBy.Storage;
using System.Threading.Tasks;

namespace SneakOnBy
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Sneak On By";
        private const string CommandName = "/sneaky";

        public static IDalamudPlugin Instance;

        private IDalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public static StorageManager StorageManager = null!;
        public WindowSystem WindowSystem = new("SneakOnBy");

        private ConfigWindow ConfigWindow { get; init; }
        private Canvas Canvas { get; init; }

        public Plugin(
            IDalamudPluginInterface pluginInterface,
            ICommandManager commandManager)
        {

            Instance = this;
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            pluginInterface.Create<Services>();

            Configuration = Services.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

            StorageManager = new StorageManager();
            Task.Run(StorageManager.Load);

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

            //ECommonsMain.Dispose();


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
