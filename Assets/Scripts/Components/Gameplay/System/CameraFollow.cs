using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _delay = 0.2f;
    private Transform _player;
    private Vector3 _cameraVelocity = Vector3.zero;

    private void Start()
    {
        _player = GameplayController.Instance.PlayerObject.transform;
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 distance = _player.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, distance, ref _cameraVelocity, _delay);
    }
}
