using Lumina.Data.Parsing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SneakOnBy.Storage
{
    public class StorageManager
    {
        public Dictionary<string, Aggro> Enemies = new();
        public bool DataReady;

        public void Load()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SneakOnBy.Data.agro.json";

            var options = new System.Text.Json.JsonSerializerOptions { IncludeFields = true };

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                this.Enemies = new JsonSerializer().Deserialize<Dictionary<string, Aggro>>(jsonTextReader);
            }

            Services.PluginLog.Debug($"Loaded {Enemies.Count} enemies");

            DataReady = Enemies != null;
        }
    }

}
