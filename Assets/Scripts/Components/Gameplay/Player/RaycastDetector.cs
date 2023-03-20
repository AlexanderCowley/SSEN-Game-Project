using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    [SerializeField] private float _detectionRange = 1.5f;
    private Movement _movement;
    private Ray _ray;
    private Station _currentClosestStation;

    public Station CurrentClosestStation { get { return _currentClosestStation; } }

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _ray = new Ray(transform.position, _movement.LookDirection);
    }

    private void FixedUpdate()
    {
        CastRay();
    }

    private void CastRay()
    {
        _ray = new Ray(transform.position, _movement.LookDirection);
        Debug.DrawRay(_ray.origin, _ray.direction * _detectionRange);

        RaycastHit hit;
        if (Physics.Raycast(_ray, out hit, _detectionRange))
        {
            if (hit.collider.CompareTag("Station"))
            {
                _currentClosestStation = hit.collider.gameObject.GetComponent<Station>();
            }
        }
        else
        {
            _currentClosestStation = null;
        }
    }
}
