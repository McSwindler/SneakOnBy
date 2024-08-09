namespace SneakOnBy.Storage
{
    public enum DeepDungeonType
    {
        PalaceOfTheDead,
        HeavenOnHigh,
        EurekaOrthos,
    }

    public static class DeepDungeonTypeExtensions
    {
        public static string Shorthand(this DeepDungeonType type) => type switch
        {
            DeepDungeonType.PalaceOfTheDead => "potd",
            DeepDungeonType.HeavenOnHigh => "hoh",
            DeepDungeonType.EurekaOrthos => "eo",
            _ => string.Empty
        };
    }
}
