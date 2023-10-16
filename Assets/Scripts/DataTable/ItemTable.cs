using CsvHelper.Configuration;
using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class ItemTable : DataTable
{
    public class Data
    {
        public string ITEMPATH { get; set; }
        public int ID { get; set; }
        public int TYPE { get; set; }
        public float VALUE { get; set; }
        public float DURATION { get; set; }
        public int PRICE { get; set; }
    }
    private Dictionary<ItemID, ItemInfo> dic = new Dictionary<ItemID, ItemInfo>();

    public ItemTable()
    {
        path = "Tables/ItemTable";
        Load();
    }
    public override void Load()
    {
        //string csvFileText =File.ReadAllText(path);
        //TextReader reader = new StringReader(csvFileText);

        var csvStr = Resources.Load<TextAsset>(path);
        TextReader reader = new StringReader(csvStr.text);
        var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
        var records = csv.GetRecords<Data>();

        foreach (var record in records)
        {
            ItemInfo itemInfo = new ItemInfo(record.ITEMPATH, record.ID, record.TYPE, 
                record.VALUE, record.DURATION, record.PRICE);
            dic.Add(itemInfo.id, itemInfo);
        }
    }

    public ItemInfo GetItemInfo(ItemID id)
    {
        if (!dic.ContainsKey(id))
        {
            return new ItemInfo();
        }

        return dic[id];
    }

    public int GetTableSize()
    {
        return dic.Count;
    }
}
