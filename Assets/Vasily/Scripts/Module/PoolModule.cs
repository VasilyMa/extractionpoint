using System.Collections.Generic;

using UnityEngine;

public class PoolModule
{
    public static PoolModule Instance;

    private Dictionary<string, PoolObject<IPool>> PoolDictionary = new Dictionary<string, PoolObject<IPool>>();

    public PoolModule()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void InitPool<T>(IPool poolObject, int capacity) where T : IPool
    {
        if (!PoolDictionary.ContainsKey(poolObject.PoolKeyID))
        {
            var pool = new PoolObject<IPool>(poolObject, 1, poolObject.PoolKeyID);
            PoolDictionary.Add(poolObject.PoolKeyID, pool);
            pool.InitPool(capacity);
        }
    }

    public void ReturnToPool(IPool returnedToPool)
    {
        if (PoolDictionary.ContainsKey(returnedToPool.PoolKeyID))
        {
            PoolDictionary[returnedToPool.PoolKeyID].ReturnToPool(returnedToPool);
        }
    }

    public T GetFromPool<T>(IPool poolObject, bool isActive = true) where T : IPool
    {
        if (TryGetFromPool<T>(poolObject, out var getPoolObject, isActive))
        {
            return (T)getPoolObject;
        }

        return (T)CreatePool<T>(poolObject, isActive);
    }

    bool TryGetFromPool<T>(IPool poolObject, out IPool getObject, bool isActive) where T : IPool
    {
        if (PoolDictionary.ContainsKey(poolObject.PoolKeyID))
        {
            getObject = PoolDictionary[poolObject.PoolKeyID].GetFromPool<T>(isActive);

            return true;
        }
        getObject = null;

        return false;
    }
    IPool CreatePool<T>(IPool poolObject, bool isActive) where T : IPool
    {
        if (!PoolDictionary.ContainsKey(poolObject.PoolKeyID))
        {
            var pool = new PoolObject<IPool>(poolObject, 1, poolObject.PoolKeyID);
            PoolDictionary.Add(poolObject.PoolKeyID, pool);

            return pool.GetFromPool<T>(isActive);
        }

        return null;
    }
    public void Dispose()
    {
        foreach (var pool in PoolDictionary)
        {
            pool.Value.Dispose();
        }

        PoolDictionary.Clear();
    }
}

public class PoolObject<T> where T : IPool
{
    private Queue<IPool> ObjPool;
    private GameObject _prefab;
    private Transform _parent;

    public PoolObject(IPool poolPrefab, int quantity, string parentObjectName)
    {
        _prefab = poolPrefab.ThisGameObject;

        var obj = new GameObject(parentObjectName);

        _parent = obj.transform;
        ObjPool = new Queue<IPool>();
    }

    public void InitPool(int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            var newObj = GameObject.Instantiate(_prefab, _parent).GetComponent<IPool>();
            newObj.InitPool();
            newObj.ThisGameObject.SetActive(false);
            ObjPool.Enqueue(newObj);
        }
    }

    private T CreateNewObject<T>(bool isActive) where T : IPool
    {
        var newObj = GameObject.Instantiate(_prefab, _parent).GetComponent<T>();
        newObj.InitPool();
        newObj.ThisGameObject.SetActive(isActive);
        return newObj;
    }

    public void ReturnToPool(IPool returnedToPool)
    {
        returnedToPool.ThisGameObject.SetActive(false);
        returnedToPool.IsAvaiable = true;

        ObjPool.Enqueue(returnedToPool);
    }

    public T GetFromPool<T>(bool isActive) where T : IPool
    {
        // Проверяем, пока есть доступные объекты
        while (ObjPool.Count > 0)
        {
            var genObject = ObjPool.Dequeue();

            // Проверяем доступность текущего объекта
            if (genObject.IsAvaiable)
            {
                genObject.IsAvaiable = false;
                genObject.ThisGameObject.SetActive(isActive);
                return (T)genObject; // Возвращаем доступный объект
            }

            // Если объект недоступен, мы его не добавляем обратно в пул
        }

        // Если доступных объектов нет, создаем новый
        return CreateNewObject<T>(isActive); // Предполагается, что CreateNewObject возвращает объект типа `IPool`.
    }

    public void Dispose()
    {
        ObjPool.Clear();
    }
}

public interface IPool
{
    public GameObject ThisGameObject { get; }
    public bool IsAvaiable { get; set; }
    public string PoolKeyID { get; }
    public void InitPool();
    public void ReturnToPool();
}