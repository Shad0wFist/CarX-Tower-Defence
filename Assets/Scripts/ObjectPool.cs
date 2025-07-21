using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Queue<T> objects = new Queue<T>();
    private readonly Transform parentContainer;
    private readonly List<T> objectsOnScene = new List<T>();
    public IEnumerable<T> ActiveObjects => objectsOnScene;

    public ObjectPool(T prefab, int initialSize = 10)
    {
        this.prefab = prefab;
        parentContainer = new GameObject(typeof(T).Name + "_Pool").transform;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab, parentContainer);
            obj.gameObject.SetActive(false);
            objects.Enqueue(obj);
        }
    }

    public T Get()
    {
        if (objects.Count == 0)
        {
            T obj = Object.Instantiate(prefab, parentContainer);
            objects.Enqueue(obj);
        }

        T spawned = objects.Dequeue();
        spawned.gameObject.SetActive(true);
        objectsOnScene.Add(spawned);
        return spawned;
    }

    public void Release(T toRelease)
    {
        toRelease.gameObject.SetActive(false);
        objectsOnScene.Remove(toRelease);
        objects.Enqueue(toRelease);
    }
}