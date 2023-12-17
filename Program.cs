using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

struct LiteralInfo
{
    public string value;
    public string address;
}

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("No args provided! Syntax: this.exe stringLiteral.json base64Filter.json");
            return;
        }
        List<LiteralInfo> decoded = JsonConvert.DeserializeObject<List<LiteralInfo>>(File.ReadAllText(args[0])) ?? throw new Exception("failed to deserialize " + args[0]);
        List<LiteralInfo> filtered = new();
        foreach (LiteralInfo l in decoded)
        {
            try
            {
                Console.Write($"Processing {l.address}... ");
                string _trmmed = l.value.Trim();
                Console.WriteLine(System.Text.Encoding.Default.GetString(Convert.FromBase64String(_trmmed.Split(null, 2).Length > 1 ? throw new Exception() : _trmmed)));
                filtered.Add(l);
            }
            catch { Console.WriteLine("Not Base64!"); }
        }
        Console.WriteLine("Done! Now writing serialized...");
        string encoded = JsonConvert.SerializeObject(filtered);
        File.WriteAllText(args[1], encoded);
        return;
    }
}
