using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    public static List<Item> itemList = new List<Item>();
    List<ObjectPool<Item>> _itemPools = new List<ObjectPool<Item>>();
    Item _item;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
}
