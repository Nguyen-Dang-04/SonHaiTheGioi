using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public struct Pool
    {
        public string tag;          
        public GameObject prefab;   
        public int size;      
    }

    public static ObjectPoolManager Instance;     
    public List<Pool> pools;                      
    public Dictionary<string, Queue<GameObject>> poolDictionary; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetObjectFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool với tag '" + tag + "' không tồn tại.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnObjectToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool với tag '" + tag + "' không tồn tại.");
            Destroy(objectToReturn);
            return;
        }

        objectToReturn.SetActive(false);
        objectToReturn.transform.SetParent(null);
        poolDictionary[tag].Enqueue(objectToReturn);
    }
}
