using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class DataTableManager
{
    private static Dictionary<System.Type, DataTable> tables = new Dictionary<System.Type, DataTable>();

    static DataTableManager()
    {
        tables.Clear();

        //var stringTable = new StringTable();
        var itemTable = new ItemTable();
        //tables.Add(typeof(StringTable), stringTable);
        tables.Add(typeof(ItemTable), itemTable);
    }

    public static T GetTable<T>() where T : DataTable
    {
        var id = typeof(T);
        if (!tables.ContainsKey(id))
            return null;

        return tables[id] as T;

    }
}
