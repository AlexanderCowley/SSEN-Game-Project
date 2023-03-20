using UnityEngine;
using UnityEngine.Pool;
public class ConsumableSpawner : Station
{
    [SerializeField] private Item _prefabToSpawn;
    ObjectPool<Item> _itemPool;

    new void Start()
    {
        _itemPool = new ObjectPool<Item>(CreateItem, OnItemGet, OnReturnToPool);
        base.Start();
    }

    public override bool IsOccupied() => ItemOccupied = _prefabToSpawn;

    protected override void Grab(PlayerInventory playerInventory)
    {
        Item instance = _itemPool.Get();
        instance.transform.position = _holdPosition.transform.position + new Vector3(0, 3, 0);
        instance.transform.rotation = Quaternion.identity;
        GameplayController.Instance.TrackIngredient(instance, false);
        playerInventory.HoldItem(instance);
        _audioSource.PlayOneShot(_pickUpClip);
    }

    public override void StationAction(PlayerInventory playerInventory)
    {
        // Probably nothing
    }

    Item CreateItem()
    {
        Item itemInstance = Instantiate(_prefabToSpawn);
        return itemInstance;
    }

    void OnItemGet(Item item)
    {
        item.gameObject.SetActive(true);
    }

    void OnReturnToPool(Item item)
    {
        item.gameObject.SetActive(false);
        item.transform.position = Vector3.zero;
    }

}
