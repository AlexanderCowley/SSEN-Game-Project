using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpContext : MonoBehaviour
{
    private RaycastDetector _playerRaycast;
    PlayerInventory _playerInventory;
    Animator _playerAnimator;

    private void Start()
    {
        _playerRaycast = GameplayController.Instance.PlayerObject.GetComponent<RaycastDetector>();
        _playerInventory = GameplayController.Instance.PlayerObject.GetComponent<PlayerInventory>();
        _playerAnimator = GameplayController.Instance.PlayerObject.GetComponentInChildren<Animator>();
    }

    public void PickUp()
    {
        // This if statement should prevent a bug that can make bottles disappear if placed down during the pouring animation
        if ((!_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PouringOil")) && (!_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PouringSoySauce")))
            _playerRaycast?.CurrentClosestStation?.Use(_playerInventory);
    }
}
