using System;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
    [SerializeField] private StationType _type;
    protected Transform _holdPosition;
    protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _setDownClip, _pickUpClip;

    public enum StationType
    {
        Flat,
        Spawner,
        Cutting,
        Stovetop,
        Serving,
        Garbage
    }

    public StationType Type { get { return _type; } }

    public Item ItemOccupied { get; set; }

    protected void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _holdPosition = transform.GetChild(0).GetComponent<Transform>();        
    }

    public abstract bool IsOccupied();
    public abstract void StationAction(PlayerInventory playerInventory);

    public void Use(PlayerInventory playerInventory)
    {
        if (IsOccupied() && playerInventory.ItemHeld == null)
            Grab(playerInventory);
        else if (!IsOccupied() && playerInventory.ItemHeld != null)
            Place(playerInventory);
    }

    protected virtual void Grab(PlayerInventory playerInventory)
    {
        Debug.Log($"Grab {ItemOccupied}", gameObject);
        playerInventory.HoldItem(ItemOccupied);
        ItemOccupied = null;
        _audioSource.PlayOneShot(_pickUpClip);
    }

    protected virtual void Place(PlayerInventory playerInventory)
    {
        ItemOccupied = playerInventory.ItemHeld;
        playerInventory.RemoveItem();
        ItemOccupied.transform.position = _holdPosition.position;
        ItemOccupied.transform.rotation = Quaternion.identity;
        _audioSource.PlayOneShot(_setDownClip);
    }
}
