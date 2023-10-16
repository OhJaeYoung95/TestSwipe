using CsvHelper.Configuration;
using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class StringTable : DataTable
{
    public class Data
    {
        public int ID { get; set; }
        public string STRING { get; set; }
    }

    private Dictionary<int, string> dic = new Dictionary<int, string>();

    public StringTable()
    {
        //path = Path.Combine(Application.dataPath, "Tables/StringTable.csv");

        path = "Tables/StringTable";
        Load();
    }

    public override void Load()
    {
        var csvStr = Resources.Load<TextAsset>(path);
        TextReader reader = new StringReader(csvStr.text);
        var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
        var records = csv.GetRecords<Data>();

        foreach (var record in records)
        {
            dic.Add(record.ID, record.STRING);
        }

    }

    public string GetString(int id)
    {
        if (!dic.ContainsKey(id))
        {
            return string.Empty;
        }

        return dic[id];
    }

}
