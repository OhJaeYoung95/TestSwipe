using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveData
{
    public int Version { get; set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public SaveDataV1()
    {
        Version = 1;
    }

    public int Gold { get; set; }

    public override SaveData VersionUp()
    {
        return null;
    }
}

//public class SaveDataV2 : SaveData
//{
//    public SaveDataV2()
//    {
//        Version = 2;
//    }

//    public int Gold { get; set; }
//    public string Name { get; set; } = "Unknown";

//    public override SaveData VersionUp()
//    {
//        var data = new SaveDataV3();
//        data.Gold = Gold;
//        data.Name = Name;
//        return data;
//    }
//}

//public class SaveDataV3 : SaveDataV2
//{
//    public SaveDataV3()
//    {
//        Version = 3;
//    }

//    public struct CubeInfo
//    {
//        public Vector3 position;
//        public Quaternion rotation;
//        public Vector3 scale;

//        public string name;

//        public CubeInfo(Vector3 position, Quaternion rotation, Vector3 scale, string name)
//        {
//            this.position = position;
//            this.rotation = rotation;
//            this.scale = scale;
//            this.name = name;
//        }
//    }

//    //public CubeInfo[] cubeInfos { get; set; }
//    public List<CubeInfo> cubeInfos { get; set; } = new List<CubeInfo>();

//    public override SaveData VersionUp()
//    {
//        return null;
//    }
//}