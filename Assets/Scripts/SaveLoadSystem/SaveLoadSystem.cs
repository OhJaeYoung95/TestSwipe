using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaveDataVC = SaveDataV1;

public static class SaveLoadSystem
{
    public enum Modes
    {
        Json,
        Binary,
        EncryptedBinary
    }
    public static int SaveDataVersion { get; } = 3;
    public static string[] SaveSlotFileNames =
    {
        "Save0.json",
        "Save1.json",
        "Save2.json"
    };

    public static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    public static void Save(SaveData data, int slot)
    {
        Save(data, SaveSlotFileNames[slot]);
    }

    public static void Save(SaveData data, string fileName)
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, fileName);

        //using (var file = File.CreateText(path))
        //{
        //    var serializer = new JsonSerializer();
        //    //serializer.Converters.Add(new Vector3Converter());
        //    serializer.Serialize(file, data);
        //}

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serialize = new JsonSerializer();
            serialize.Converters.Add(new Vector3Converter());
            serialize.Converters.Add(new QuaternionConverter());
            serialize.Serialize(writer, data);
        }
    }

    public static SaveData Load(int slot)
    {
        return Load(SaveSlotFileNames[slot]);
    }

    public static SaveData Load(string fileName)
    {

        var path = Path.Combine(SaveDirectory, fileName);
        Debug.Log(path);
        if (!File.Exists(path))
            return null;


        //using (var file = File.OpenText(path))
        //{
        //    //var serialize = new JsonSerializer();
        //    ////serializer.Converters.Add(new Vector3Converter());
        //    //data = serialize.Deserialize(file, typeof(SaveDataV1)) as SaveData;

        //    var serialize = new JsonSerializer();
        //    var reader = new JsonTextReader(file);
        //    //serializer.Converters.Add(new Vector3Converter());
        //    data = serialize.Deserialize<SaveDataV1>(reader);
        //}
        SaveData data = null;
        int version = 0;

        var json = File.ReadAllText(path);
        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var jObj = JObject.Load(reader);
            version = jObj["Version"].Value<int>();
        }

        using (var reader = new JsonTextReader(new StringReader(json)))
        {
            var serialize = new JsonSerializer();
            serialize.Converters.Add(new Vector3Converter());
            serialize.Converters.Add(new QuaternionConverter());

            switch (version)
            {
                case 1:
                    data = serialize.Deserialize<SaveDataV1>(reader);
                    break;
                //case 2:
                //    data = serialize.Deserialize<SaveDataV2>(reader);
                //    break;
                //case 3:
                //    data = serialize.Deserialize<SaveDataV3>(reader);
                //    break;
            }

            while (data.Version < SaveDataVersion)
            {
                data = data.VersionUp();
            }
        }
        return data;
    }
}
