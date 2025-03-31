using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SneakOnBy.Storage
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Aggro
    {
        Sight,
        Sound,
        Proximity,
        Undefined
    }
}
