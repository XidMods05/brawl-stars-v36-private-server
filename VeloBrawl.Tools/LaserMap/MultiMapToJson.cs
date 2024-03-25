﻿using Newtonsoft.Json;

namespace VeloBrawl.Tools.LaserMap;

public static class MultiMapToJson
{
    public static string ToJson(MultiMap<object, object> multiMap)
    {
        return JsonConvert.SerializeObject(multiMap, [new MultiMapJsonConverter<object, object>()]);
    }

    public static string ToJson(MultiMap<string, object> multiMap)
    {
        return JsonConvert.SerializeObject(multiMap, [new MultiMapJsonConverter<string, object>()]);
    }
}