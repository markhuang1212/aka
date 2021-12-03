namespace aka;

using System.IO;
using System.Linq;
// using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

public class DataController
{
    private readonly string? dataFile;
    private readonly ILogger<DataController> _logger;
    public IDictionary<string, string> data = new ConcurrentDictionary<string, string>();
    public IDictionary<string, string> data_lower_case = new ConcurrentDictionary<string, string>();
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

    public DataController(ILogger<DataController> logger, string? dataFile)
    {
        this._logger = logger;
        if (dataFile == null) // in memory
        {
            _logger.LogInformation("No dataFile given, in memory mode");
            return;
        }
        // create if not exist
        File.Open(dataFile, FileMode.OpenOrCreate, FileAccess.ReadWrite).Dispose();

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
                data_lower_case[kv.Item1.ToLower()] = kv.Item2;
            }
            catch (Exception)
            {
                _logger.LogInformation($"Error when parsing the line: {line}");
            }
        }
        _logger.LogInformation($"Loaded {data.Count} entries from {dataFile}");
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
        data_lower_case[key.ToLower()] = value;

        var task = save();
    }

    public string? GetKeyValue(string key)
    {
        string? ret = null;
        data_lower_case.TryGetValue(key.ToLower(), out ret);
        return ret;
    }

    public void DeleteKeyValue(string key)
    {
        data.Remove(key, out _);
        data_lower_case.Remove(key.ToLower(), out _);
        Task t = save();
    }

}