using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    
    // (characterName, replica)
    [SerializeField] List<(string, string)> lines;
    public string questToStartAfter;
    public List<(string, string)> Lines => lines;

    public Dialog(string[] replicas)
    {
        lines = new List<(string, string)>();
        var speakers = new Dictionary<int, string>();
        foreach (var kv in replicas[0].Split(','))
        {
            var numToName = kv.Split(':');
            speakers[int.Parse(numToName[0])] = numToName[1];
        }

        lines = replicas
            .Skip(1)
            .SkipLast(1)
            .Select(line => GetNameAndReplica(line, speakers)).ToList();
        questToStartAfter = replicas.Last();
    }

    private (string, string) GetNameAndReplica(string line, Dictionary<int, string> speakers)
    {
        var num = line.TakeLast(1).FirstOrDefault();
        var name = speakers[int.Parse(num.ToString())];
        var replica = line.Remove(line.Length-1);
        return (name, replica);
    }
}
