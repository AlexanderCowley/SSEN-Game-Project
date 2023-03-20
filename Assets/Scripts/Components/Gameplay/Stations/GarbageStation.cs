using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageStation : Station
{
    public override bool IsOccupied() => false;

    public override void StationAction(PlayerInventory playerInventory)
    {
        //Guard clause if the player's inventory is empty or the item is a bottle, the method ends early
        if (playerInventory.ItemHeld == null || playerInventory.ItemHeld.GetType() == typeof(Bottles))
            return;

        GameObject instance = playerInventory.ItemHeld.gameObject;
        GameplayController.Instance.TrackIngredient(playerInventory.ItemHeld, true);
        playerInventory.RemoveItem();
        _audioSource.PlayOneShot(_audioSource.clip);
        Destroy(instance);
    }

    protected override void Grab(PlayerInventory playerInventory){}
    protected override void Place(PlayerInventory playerInventory){}
}
