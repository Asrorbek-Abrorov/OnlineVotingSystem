using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVotingSystem.Helpers;

public static class FileIO
{
    public static async Task<List<T>> ReadAsync<T>(string path)
    {
        var content = await File.ReadAllTextAsync(path);
        if (string.IsNullOrEmpty(content))
            return new List<T>();

        return JsonConvert.DeserializeObject<List<T>>(content);
    }

    public static async Task WriteAsync<T>(string path, List<T> values)
    {
        var json = JsonConvert.SerializeObject(values, Formatting.Indented);
        await File.WriteAllTextAsync(path, json);
    }
}
