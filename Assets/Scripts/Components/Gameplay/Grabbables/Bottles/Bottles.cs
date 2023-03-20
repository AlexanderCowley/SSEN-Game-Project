using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottles : Item, IUsable
{
    [SerializeField] Item _dispensedItem;
    public Item DispensedItem { get { return _dispensedItem; } }

    public void Use()
    {
        Debug.Log("Item Used", gameObject);
    }
}
