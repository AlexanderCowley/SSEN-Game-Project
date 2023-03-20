using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private bool _highlighted;
    [SerializeField] private MeshRenderer _targetMesh;
    private Material _baseMaterial;
    private Station _thisStation;
    private RaycastDetector _playerRaycast;

    private void Awake()
    {
        _baseMaterial = _targetMesh.material;
        _thisStation = GetComponent<Station>();
    }

    private void Start()
    {
        _playerRaycast = GameplayController.Instance.PlayerObject.GetComponent<RaycastDetector>();
        Material[] materials = new Material[2];
        materials[0] = _baseMaterial;
    }

    private void Update()
    {
        if (_playerRaycast.CurrentClosestStation == _thisStation)
        {
            SetHighlight(true);
        }
        else
        {
            SetHighlight(false);
        }
    }

    public void SetHighlight(bool on)
    {
        if (on)
        {
            Material[] materials = new Material[2];
            materials[0] = _baseMaterial;
            materials[1] = _highlightMaterial;
            _targetMesh.materials = materials;
        }
        else
        {
            Material[] materials = new Material[2];
            materials[0] = _baseMaterial;
            materials[1] = null;
            _targetMesh.materials = materials;
        }
    }
}
