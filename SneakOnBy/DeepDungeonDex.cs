using Dalamud.Logging;
using Dalamud.Plugin;
using ECommons.Reflection;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SneakOnBy
{
    internal static class DeepDungeonDex
    {
        internal static IDalamudPlugin Instance;
        internal static object StorageHandler; 

        internal static Boolean TryConnect()
        {
            if (Instance == null || StorageHandler == null)
            {
                Connect();
            }

            return Instance != null && StorageHandler != null;
        }

        static void Connect()
        {
            try
            {
                PluginLog.Information("Trying to connect to DeepDungeonDex");
                if (DalamudReflector.TryGetDalamudPlugin("DeepDungeonDex", out var plugin, false, true))
                {
                    Instance = plugin;
                    PluginLog.Information("Successfully connected to DeepDungeonDex.");
                    var provider = ReflectionHelper.GetFoP(Instance, "_provider");
                    if(provider != null)
                    {
                        PluginLog.Information("Successfully got provider");

                        Type storageHandlerType = Assembly.GetAssembly(Instance.GetType()).GetType("DeepDungeonDex.Storage.StorageHandler");
                        
                        var storageHandler = provider.GetType().GetMethod("GetService").Invoke(provider, new object[] { storageHandlerType });
                        if(storageHandler != null)
                        {
                            PluginLog.Information("Successfully got storageHandler");
                            StorageHandler = storageHandler;
                        }
                    }
                }
                else
                {
                    throw new Exception("DeepDungeonDex is not initialized");
                }
            }
            catch (Exception e)
            {
                PluginLog.Error("Can't find DeepDungeonDex plugin: " + e.Message);
                PluginLog.Error(e.StackTrace);
            }
        }

        internal static void Reset()
        {
            Instance = null;
            StorageHandler = null;
            PluginLog.Information("Disconnected from DeepDungeonDex");
        }

        public static Aggro GetMobAggroType(uint id)
        {
            if(TryConnect())
            {
                Type mobDataType = Assembly.GetAssembly(StorageHandler.GetType()).GetType("DeepDungeonDex.Storage.MobData");
                MethodInfo getInstancesMethod = StorageHandler.GetType().GetMethod("GetInstances", new Type[] { }).MakeGenericMethod(new Type[] { mobDataType });
                if(getInstancesMethod != null)
                {
                    var instances = (object[]) getInstancesMethod.Invoke(StorageHandler, new object[] { });
                    var data = GetData(instances, id);
                    if(data != null)
                    {
                        var aggro = ReflectionHelper.GetFoP(data, "Aggro");
                        string aggroStr = Enum.GetName(aggro.GetType(), aggro);
                        return (Aggro) Enum.Parse(typeof(Aggro), aggroStr);
                    }
                }
            }

            return Aggro.Undefined;
        }

        private static object? GetData(this IEnumerable<object> data, uint key)
        {
            foreach(object l in data)
            {
                var dict = ReflectionHelper.GetFoP(l, "MobDictionary") as IDictionary;
                if (dict != null && dict.Contains(key))
                {
                    return dict[key];
                }
            }
            return null;
        }
    }

    public enum Aggro
    {
        Undefined,
        Sight,
        Sound,
        Proximity,
        Boss
    }


}
