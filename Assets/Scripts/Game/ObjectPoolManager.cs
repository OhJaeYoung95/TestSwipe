using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }
    public Dictionary<string, ObjectPool<Component>> objectPools = new Dictionary<string, ObjectPool<Component>>();

    public Block spadePrefab;
    public Block diamondPrefab;
    public Block heartPrefab;
    public Block cloverPrefab;
    public Block jokerPrefab;
    public Block obstaclePrefab;


    private string spadeBlockPoolKey = "SpadeBlockPool";
    private string diamondBlockPoolKey = "DiamondBlockPool";
    private string heartBlockPoolKey = "HeartBlockPool";
    private string cloverBlockPoolKey = "CloverBlockPool";
    private string jokerBlockPoolKey = "JokerBlockPool";
    private string obstacleBlockPoolKey = "ObstacleBlockPool";
    private string particlePoolKey = "ParticlePool";

    private GameObject spadePoolFolder;
    private GameObject diamondPoolFolder;
    private GameObject heartPoolFolder;
    private GameObject cloverPoolFolder;
    private GameObject jokerPoolFolder;
    private GameObject obstaclePoolFolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Init()
    {
        spadePoolFolder = new GameObject();
        spadePoolFolder.name = spadeBlockPoolKey;
        spadePoolFolder.tag = spadeBlockPoolKey;

        diamondPoolFolder = new GameObject();
        diamondPoolFolder.name = diamondBlockPoolKey;
        diamondPoolFolder.tag = diamondBlockPoolKey;

        heartPoolFolder = new GameObject();
        heartPoolFolder.name = heartBlockPoolKey;
        heartPoolFolder.tag = heartBlockPoolKey;

        cloverPoolFolder = new GameObject();
        cloverPoolFolder.name = cloverBlockPoolKey;
        cloverPoolFolder.tag = cloverBlockPoolKey;

        jokerPoolFolder = new GameObject();
        jokerPoolFolder.name = jokerBlockPoolKey;
        jokerPoolFolder.tag = jokerBlockPoolKey;

        obstaclePoolFolder = new GameObject();
        obstaclePoolFolder.name = obstacleBlockPoolKey;
        obstaclePoolFolder.tag = obstacleBlockPoolKey;

        DontDestroyOnLoad(spadePoolFolder);
        DontDestroyOnLoad(diamondPoolFolder);
        DontDestroyOnLoad(heartPoolFolder);
        DontDestroyOnLoad(cloverPoolFolder);
        DontDestroyOnLoad(jokerPoolFolder);
        DontDestroyOnLoad(obstaclePoolFolder);

        CreateObjectPool<Component>(spadeBlockPoolKey, spadePrefab, 30);
        CreateObjectPool<Component>(diamondBlockPoolKey, diamondPrefab, 30);
        CreateObjectPool<Component>(heartBlockPoolKey, heartPrefab, 30);
        CreateObjectPool<Component>(cloverBlockPoolKey, cloverPrefab, 30);
        CreateObjectPool<Component>(jokerBlockPoolKey, jokerPrefab, 30);
        CreateObjectPool<Component>(obstacleBlockPoolKey, obstaclePrefab, 30);
    }

    public void CreateObjectPool<T>(string poolName, T prefab, int initialCount) where T : Component
    {
        GameObject folder = GameObject.FindGameObjectWithTag(poolName);
        ObjectPool<Component> objectPool = new ObjectPool<Component>(
            () => Instantiate(prefab),
            folder,
            initialCount
        );
        objectPools.Add(poolName, objectPool);
    }

    public T GetObjectPool<T>(string poolName) where T : Component
    {
        if (!objectPools.ContainsKey(poolName))
        {
            Debug.Log($"Not Contains \"{poolName}\"");
            return null;
        }
        return (T)objectPools[poolName].GetObject();
    }

    public void ReturnObjectPool<T>(string poolName, T obj) where T : Component
    {
        if (!objectPools.ContainsKey(poolName))
        {
            Debug.Log($"Not Contains \"{poolName}\"");
            return;
        }
        objectPools[poolName].ReturnObject(obj);
    }

    public void ReturnAllObjectPool()
    {
        objectPools[spadeBlockPoolKey].ReturnAllObject();
        objectPools[diamondBlockPoolKey].ReturnAllObject();
        objectPools[heartBlockPoolKey].ReturnAllObject();
        objectPools[cloverBlockPoolKey].ReturnAllObject();
        objectPools[jokerBlockPoolKey].ReturnAllObject();
        objectPools[obstacleBlockPoolKey].ReturnAllObject();
    }
}