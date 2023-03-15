using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using SneakOnBy.Windows;
using ECommons;
using Dalamud.Game;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using ECommons.DalamudServices;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;

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
        private MainWindow MainWindow { get; init; }
        private Canvas Canvas { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            ECommonsMain.Init(pluginInterface, this, Module.DalamudReflector, Module.ObjectFunctions);
            DeepDungeonDex.TryConnect();

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this, goatImage);
            Canvas = new Canvas(this);
            
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);
            WindowSystem.AddWindow(Canvas);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Shows configuration for Sneak On By"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            Svc.Condition.ConditionChange += ConditionChange;
            
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();

            ECommonsMain.Dispose();


            ConfigWindow.Dispose();
            MainWindow.Dispose();
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

        private void ConditionChange(ConditionFlag flag, bool value)
        {
            if(flag == ConditionFlag.InCombat && value)
            {
                if (Svc.Targets.SoftTarget is BattleNpc bnpc)
                {
                    Svc.Chat.PrintChat(new XivChatEntry
                    {
                        Message = new SeString(new TextPayload("Hello World")),
                        Type = XivChatType.Echo
                    });
                }
            }
        }
    }
}
