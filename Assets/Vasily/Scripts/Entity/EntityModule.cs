using System.Collections.Generic;
using System;
using UnityEngine;

public static class EntityModule
{
    public static bool IsInit;

    private static Dictionary<Type, SourceEntity> _dictionary = new Dictionary<Type, SourceEntity>();

    public static void Initialize()
    {
        IsInit = false;

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(SourceEntity).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    try
                    {
                        if (Activator.CreateInstance(type) is SourceEntity instance)
                        {
                            _dictionary.Add(type, instance.Init());
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Ошибка создания экземпляра {type.Name}: {ex.Message}\n{ex.StackTrace}");
                    }
                }
            }
        }

        IsInit = true;
    }

    public static T GetEntity<T>() where T : SourceEntity
    {
        if (_dictionary.TryGetValue(typeof(T), out SourceEntity entity))
        {
            return entity as T;
        }

        Debug.LogError($"Entity of type {typeof(T).Name} not found in dictionary.");

        return null;
    }
}
