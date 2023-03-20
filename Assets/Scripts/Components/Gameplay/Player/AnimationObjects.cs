using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObjects : MonoBehaviour
{
    [SerializeField] private GameObject _knife;
    [SerializeField] private GameObject _wokParent;
    [SerializeField] private GameObject _emptyWok;
    [SerializeField] private GameObject _oiledWok;
    [SerializeField] private GameObject _riceWok;
    [SerializeField] private GameObject _soyRiceWok;
    [SerializeField] private GameObject _oilBottle;
    [SerializeField] private GameObject _soySauceBottle;
    private Animator _animator;
    private PlayerInventory _playerInventory;
    private StoveTopStation.WokState _currentWokState;

    public GameObject WokParent { get { return _wokParent; } }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerInventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        ShowKnife();
        ShowWok();
        ShowOil();
        ShowSoySauce();
    }

    private void ShowKnife()
    {
        _knife.SetActive(_animator.GetCurrentAnimatorStateInfo(0).IsName("Chopping"));
    }

    private void ShowWok()
    {
        bool isTossing = _animator.GetCurrentAnimatorStateInfo(0).IsName("WokTossing");
        _wokParent.SetActive(isTossing);
        _emptyWok.SetActive(_currentWokState == StoveTopStation.WokState.Empty);
        _oiledWok.SetActive(_currentWokState == StoveTopStation.WokState.Oiled);
        _riceWok.SetActive(_currentWokState == StoveTopStation.WokState.Rice);
        _soyRiceWok.SetActive(_currentWokState == StoveTopStation.WokState.SoyRice);
    }

    private void ShowOil()
    {
        _oilBottle.SetActive(_animator.GetCurrentAnimatorStateInfo(0).IsName("PouringOil"));
        if (_playerInventory.ItemHeld != null)
            if (_playerInventory.ItemHeld.Type == Item.ItemType.Oil)
                _playerInventory.ItemHeld.gameObject.SetActive(!_animator.GetCurrentAnimatorStateInfo(0).IsName("PouringOil"));
    }

    private void ShowSoySauce()
    {
        _soySauceBottle.SetActive(_animator.GetCurrentAnimatorStateInfo(0).IsName("PouringSoySauce"));
        if (_playerInventory.ItemHeld != null)
            if (_playerInventory.ItemHeld.Type == Item.ItemType.SoySauce)
                _playerInventory.ItemHeld.gameObject.SetActive(!_animator.GetCurrentAnimatorStateInfo(0).IsName("PouringSoySauce"));
    }

    public void ChangeCurrentWokState(StoveTopStation.WokState newState)
    {
        _currentWokState = newState;
    }
}
