using Newtonsoft.Json;
using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Alliance;
using VeloBrawl.Supercell.Titan.CommonUtils.HelpMap;

namespace VeloBrawl.General.GHelp;

public static class ToOpenDictionaryHelper
{
    public static Dictionary<long, AllianceMemberEntry?> ToAllianceMemberEntryDictionary(object obj)
    {
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new LongDictionaryKeysConverter() }
        };

        return JsonConvert.DeserializeObject<Dictionary<long, AllianceMemberEntry?>>(
            JsonConvert.SerializeObject(obj, settings))!;
    }
}