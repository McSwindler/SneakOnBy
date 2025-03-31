using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Lumina;
using Lumina.Excel.Sheets;
using YamlDotNet.RepresentationModel;
using Action = Lumina.Excel.Sheets.Action;

namespace SneakOnBy.DataBuilder
{
    internal class Builder
    {

        static void Main(string[] args)
        {
            var lumina = new GameData(args[0]);
            var compendium = args[1];
            var bnpcName = lumina.Excel.GetSheet<BNpcName>()!;

            var names = new Dictionary<uint, string>();
            foreach (var row in bnpcName) names[row.RowId] = row.Singular.ToString();

            var bnpcAgro = new Dictionary<string, string>();

            var fileNameRegex = new Regex("_([a-z]+)_([0-9]+)_enemies");
            var nameRegex = new Regex("^name:\\s*?[\\'\\\"]?([\\w\\s\\-]+)[\\'\\\"]?$");
            var agroRegex = new Regex("^agro:\\s*?[\\'\\\"]?([\\w\\s]+)[\\'\\\"]?$");
            foreach (var file in Directory.GetFiles(compendium, "*.md", SearchOption.AllDirectories))
            {
                var m = fileNameRegex.Match(file);
                if (m.Success)
                {
                    using (var reader = new StreamReader(file))
                    {
                        String line;
                        String name = null;
                        String agro = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var nameM = nameRegex.Match(line);
                            if (nameM.Success)
                            {
                                name = nameM.Groups[1].Captures[0].Value.Trim();
                            }
                            var agroM = agroRegex.Match(line);
                            if (agroM.Success)
                            {
                                agro = agroM.Groups[1].Captures[0].Value.Trim();
                            }
                        }

                        if(name == null || agro == null)
                        {
                            System.Console.WriteLine($"Could not find name or agro in {file}");
                            continue;
                        }

                        foreach(var npc in names.Where(i => i.Value.ToLower().Equals(name.ToLower())))
                        {
                            var uniqueId = $"{m.Groups[1].Captures[0].Value}_{m.Groups[2].Captures[0].Value}_{npc.Key}";
                            if(bnpcAgro.ContainsKey(uniqueId) && !bnpcAgro[uniqueId].Equals(agro))
                            {
                                System.Console.WriteLine($"Found duplicate {uniqueId} {name} with different agro types.");
                            }
                            else if(!bnpcAgro.ContainsKey(uniqueId))
                            {
                                bnpcAgro.Add(uniqueId, agro);
                            }
                        }
                        //System.Console.WriteLine($"{m.Groups[1].Captures[0].Value} {npcId} {name} {agro}");
                    }
                }
            }

            System.Console.WriteLine($"Found {bnpcAgro.Count} Monsters");
            System.Console.WriteLine("Agro Types:");
            foreach(var agroType in bnpcAgro.Values.Distinct())
            {
                System.Console.WriteLine($"\t{agroType}");
            }

            Directory.CreateDirectory("./SneakOnBy/Data");
            File.WriteAllText("./SneakOnBy/Data/agro.json", JsonSerializer.Serialize(bnpcAgro));
        }
    }
}
