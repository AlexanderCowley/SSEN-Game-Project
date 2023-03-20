using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStation : Station
{
    [SerializeField] private GameObject _startingItemPrefab;

    public override bool IsOccupied() => ItemOccupied != null;

    private new void Start()
    {
        if (_startingItemPrefab != null)
        {
            GameObject itemInstance = Instantiate(_startingItemPrefab, _holdPosition.position, Quaternion.identity);
            ItemOccupied = itemInstance.GetComponent<Item>();
        }
        base.Start();
    }

    public override void StationAction(PlayerInventory playerInventory)
    {
        Debug.Log($"No Action to Perform", gameObject);
    }
}
