namespace aka;

using System.IO;
using System.Linq;
using System.Net;
using System.Collections.Generic;

class DataController
{
    public static DataController? shared;
    string? dataFile;
    public Dictionary<string, string> data = new Dictionary<string, string>();
    public static Tuple<string, string> ParseLineToKV(string line)
    {
        string k = "";
        string v = "";

        bool inKey = true;
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '=')
            {
                inKey = false;
            }
            else if (inKey)
            {
                k += line[i];
            }
            else
            {
                v += line[i];
            }
        }

        return new Tuple<string, string>(k, v);
    }

    public DataController(string? dataFile)
    {
        if (dataFile == null)
            return;

        this.dataFile = dataFile;
        var lines = File.ReadLines(dataFile);
        if (lines == null)
            return;

        foreach (var line in lines)
        {
            if (line == null)
                continue;

            try
            {
                var kv = ParseLineToKV(line);
                data[kv.Item1] = kv.Item2;
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }

    public async Task save()
    {
        if (dataFile == null)
            return;
        await File.WriteAllLinesAsync(dataFile, data.Select(x => $"{x.Key}={x.Value}"));
    }

    public void SetKeyValue(string key, string value)
    {
        if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
        {
            throw new Exception("Invalid value");
        }
        if (key.Any(c => !char.IsLetterOrDigit(c)))
        {
            throw new Exception("Invalud key");
        }
        Console.WriteLine($"{key}={value}");
        data[key] = value;
        var task = save();
    }

    public string? GetKeyValue(string key)
    {
        string? ret = null;
        data.TryGetValue(key, out ret);
        return ret;
    }

    public bool DeleteKeyValue(string key)
    {
        return data.Remove(key);
    }
    
}