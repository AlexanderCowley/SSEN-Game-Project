using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    [SerializeField] private Station.StationType _type;
    private Transform _holdPosition;
    public Item ItemOccupied { get; set; }

    private void OnEnable()
    {
        _holdPosition = transform.GetChild(0).GetComponent<Transform>();
    }

    public bool IsOccupied() => ItemOccupied != null;

    public void Use(PlayerInventory playerInventory)
    {
        if (IsOccupied() && playerInventory.ItemHeld == null)
            Grab(playerInventory);
        else if (!IsOccupied() && playerInventory.ItemHeld != null)
            Place(playerInventory);
    }

    void Grab(PlayerInventory playerInventory)
    {
        Debug.Log($"Grab {ItemOccupied}", gameObject);
        playerInventory.HoldItem(ItemOccupied);
        ItemOccupied = null;
    }
    //Call event for placing things (consuming items and adding them to list if there is one)
    void Place(PlayerInventory playerInventory)
    {
        Debug.Log($"Place {gameObject.name}", gameObject);
        ItemOccupied = playerInventory.ItemHeld;
        playerInventory.RemoveItem();
        ItemOccupied.transform.position = _holdPosition.position;
    }
}
