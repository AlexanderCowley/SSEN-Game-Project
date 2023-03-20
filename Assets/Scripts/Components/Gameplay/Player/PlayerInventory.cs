using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Item _itemHeld = null;
    [SerializeField] private Transform _holdPosition;
    private Animator _animator;

    private void Start()
    {
        _animator = GameplayController.Instance.PlayerObject.GetComponentInChildren<Animator>();
    }

    public Item ItemHeld 
    { 
        get { return _itemHeld; } 

        private set 
        {
            _itemHeld = value;
        } 
    }

    private void Update()
    {
        HeldItemPosition();
    }

    private void HeldItemPosition()
    {
        if (ItemHeld != null)
            ItemHeld.transform.position = _holdPosition.position;
    }

    public void HoldItem(Item item)
    {
        if(ItemHeld != null)
            RemoveItem();

        ItemHeld = item;
        ItemHeld.transform.position = _holdPosition.position;
        ItemHeld.transform.parent = _holdPosition;
        _animator.SetBool("isHoldingItem", true);
    }

    public void RemoveItem()
    {
        ItemHeld.transform.parent = null;
        ItemHeld = null;
        _animator.SetBool("isHoldingItem", false);
    }
}
