using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContext : MonoBehaviour
{
    private RaycastDetector _playerRaycast;
    PlayerInventory _playerInventory;

    private void Start()
    {
        _playerRaycast = GameplayController.Instance.PlayerObject.GetComponent<RaycastDetector>();
        _playerInventory = GameplayController.Instance.PlayerObject.GetComponent<PlayerInventory>();
    }

    public void Action()
    {
        _playerRaycast?.CurrentClosestStation?.StationAction(_playerInventory);
    }
}
