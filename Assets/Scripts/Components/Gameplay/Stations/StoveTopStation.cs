using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StoveTopStation : Station
{
    [SerializeField] int maxItems;
    List<Item> items = new List<Item>();
    float heatTimer = 0.0f;
    int tossCounter = 0;
    int tossLimit;
    bool perfectHeat;

    private Animator _playerAnimator;
    private ParticleSystem _particles;
    private AnimationObjects _playerAnimationObjects;
    private RaycastDetector _playerRaycast;

    [SerializeField] private GameObject _wokParent;
    [SerializeField] private GameObject _emptyWok;
    [SerializeField] private GameObject _oiledWok;
    [SerializeField] private GameObject _riceWok;
    [SerializeField] private GameObject _soyRiceWok;

    [SerializeField] private GameObject _focusedHelper, _unfocusedHelper;
    [SerializeField] private Sprite _oilIcon, _shallotIcon, _eggIcon, _riceIcon, _soyIcon, _greenOnionIcon;
    [SerializeField] private Image[] _iconSlots;
    [SerializeField] private TextMeshProUGUI _tossText, _heatTextFocused, _heatTextUnfocused;

    public enum WokState
    {
        Empty,
        Oiled,
        Rice,
        SoyRice
    }
    public WokState CurrentWokState { get; private set; }

    private new void Start()
    {
        _playerAnimator = GameplayController.Instance.PlayerObject.GetComponentInChildren<Animator>();
        _playerAnimationObjects = GameplayController.Instance.PlayerObject.GetComponent<AnimationObjects>();
        _playerRaycast = GameplayController.Instance.PlayerObject.GetComponent<RaycastDetector>();
        _particles = GetComponent<ParticleSystem>();
        _particles.Stop();
        base.Start();
    }

    //Replace with Specified Item Types
    //Have a Queue of Items in the Order that effect score?
    public override bool IsOccupied()
    {
        return items.Count > maxItems;
    }

    public bool IsActive() => items.Count >= 1;

    private void Update()
    {
        tossLimit = items.Count * 2;
        if (IsActive())
        {
            heatTimer += Time.deltaTime;            
        }
        UpdateWokState();
        UpdateWokVisual();
        UpdateHelperUI();
    }

    public override void StationAction(PlayerInventory playerInventory)
    {
        if (playerInventory.ItemHeld != null)
        {
            if ((playerInventory.ItemHeld.GetType() == typeof(ChoppableItems)) || (playerInventory.ItemHeld.GetType() == typeof(Item)))
            {
                AddItem(playerInventory.ItemHeld);
                GameObject instance = playerInventory.ItemHeld.gameObject;
                playerInventory.RemoveItem();
                instance.SetActive(false);
                RandomizePitch(1.0f, 1.5f);
                if (!_audioSource.isPlaying)
                    _audioSource.Play();
                _audioSource.PlayOneShot(_setDownClip);
                return;
            }

            if (playerInventory.ItemHeld.GetType() == typeof(Container))
            {
                if (items.Count > 0)
                {
                    Container container = (Container)playerInventory.ItemHeld;
                    TransferValues(items);
                    container.TransferContentsToContainer(items);
                    ResetItemList();
                    _audioSource.PlayOneShot(_pickUpClip);
                }
                return;
            }

            if (playerInventory.ItemHeld.GetType() == typeof(Bottles))
            {
                Bottles bottle = (Bottles)playerInventory.ItemHeld;
                if (bottle.Type == Item.ItemType.Oil)
                    _playerAnimator.Play("PouringOil");
                if (bottle.Type == Item.ItemType.SoySauce)
                    _playerAnimator.Play("PouringSoySauce");
                Item dispensedIngredient = Instantiate(bottle.DispensedItem);
                AddItem(dispensedIngredient);
                dispensedIngredient.gameObject.SetActive(false);
                RandomizePitch(1.5f, 2.5f);
                if (!_audioSource.isPlaying)
                    _audioSource.Play();
                _audioSource.PlayOneShot(_setDownClip);
                return;
            }
        }
        else
        {
            if (tossCounter < tossLimit)
            {
                tossCounter++;
                _playerAnimationObjects.ChangeCurrentWokState(CurrentWokState);
                _playerAnimator.Play("WokTossing");
                RandomizePitch(1.0f, 2.0f);
                if (!_audioSource.isPlaying)
                    _audioSource.Play();
            }
        }
    }

    protected override void Grab(PlayerInventory playerInventory)
    {
        // Probably nothing.
    }

    protected override void Place(PlayerInventory playerInventory)
    {
        
    }

    void AddItem(Item itemAdded)
    {
        items.Add(itemAdded);
        UpdatePerfectHeating();
    }

    void TransferValues(List<Item> itemList)
    {
        foreach (Item i in itemList)
        {
            i.HeatValue = heatTimer;
            i.TossValue = tossCounter;
        }
    }

    void ResetItemList()
    {
        items.Clear();
        heatTimer = 0.0f;
        tossCounter = 0;
        perfectHeat = false;
        _particles.Stop();
    }

    private void RandomizePitch(float min, float max)
    {
        float randomPitch = Random.Range(min, max);
        _audioSource.pitch = randomPitch;
    }

    private void UpdateWokVisual()
    {
        if (_playerRaycast.CurrentClosestStation == this)
        {
            _wokParent.SetActive(!_playerAnimationObjects.WokParent.activeInHierarchy);
        }
        else
        {
            _wokParent.SetActive(true);
        }
        _emptyWok.SetActive(CurrentWokState == WokState.Empty);
        _oiledWok.SetActive(CurrentWokState == WokState.Oiled);
        _riceWok.SetActive(CurrentWokState == WokState.Rice);
        _soyRiceWok.SetActive(CurrentWokState == WokState.SoyRice);
    }

    private void UpdateWokState()
    {
        if (items.Count == 0)
        {
            CurrentWokState = WokState.Empty;
        }
        else if (ListOnlyConsistsOf(Item.ItemType.Oil))
        {
            CurrentWokState = WokState.Oiled;
        }
        else if ((!ListOnlyConsistsOf(Item.ItemType.Oil) && (!SearchListForType(Item.ItemType.SoySauce))))
        {
            CurrentWokState = WokState.Rice;
        }
        else if ((!ListOnlyConsistsOf(Item.ItemType.Oil) && (SearchListForType(Item.ItemType.SoySauce))))
        {
            CurrentWokState = WokState.SoyRice;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(transform.position + new Vector3(0, 0.5f, 0), $"Heat = {heatTimer}\nToss = {tossCounter}\nPerfect Heat: {perfectHeat}");
    }
#endif

    private void UpdatePerfectHeating()
    {
        if (perfectHeat)
        {
            foreach (Item i in items)
            {
                i.ForcePerfectHeat();
            }
        }
    }

    public void EnablePerfectHeating()
    {
        perfectHeat = true;
        _particles.Play();
        UpdatePerfectHeating();
    }

    private bool SearchListForType(Item.ItemType typeQuery)
    {
        foreach (Item i in items)
        {
            if (i.Type == typeQuery)
            {
                return true;
            }
        }
        return false;
    }

    private bool ListOnlyConsistsOf(Item.ItemType typeQuery)
    {
        foreach (Item i in items)
        {
            if (i.Type != typeQuery)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateHelperUI()
    {
        if (_playerRaycast.CurrentClosestStation == this)
        {
            _focusedHelper.gameObject.SetActive(true);
            _unfocusedHelper.gameObject.SetActive(false);
            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (i < _iconSlots.Length)
                    {
                        _iconSlots[i].sprite = DetermineIcon(items[i]);
                        _iconSlots[i].color = new Color(_iconSlots[i].color.r, _iconSlots[i].color.g, _iconSlots[i].color.b, 1);
                    }
                }
            }
            else
            {
                for (int i = 0; i < _iconSlots.Length; i++)
                {
                    _iconSlots[i].color = new Color(_iconSlots[i].color.r, _iconSlots[i].color.g, _iconSlots[i].color.b, 0);
                }
            }
        }
        else
        {
            _focusedHelper.gameObject.SetActive(false);
            _unfocusedHelper.gameObject.SetActive(items.Count > 0);
        }

        _heatTextFocused.text = perfectHeat ? "PERFECT" : heatTimer.ToString("F1");
        _heatTextUnfocused.text = perfectHeat ? "PERFECT" : heatTimer.ToString("F1");
        _tossText.text = $"{tossCounter} / {tossLimit}";
    }

    private Sprite DetermineIcon(Item item)
    {
        Sprite sprite = null;
        switch (item.Type)
        {
            case Item.ItemType.Oil:
                sprite = _oilIcon;
                break;
            case Item.ItemType.Shallot:
                sprite = _shallotIcon;
                break;
            case Item.ItemType.Eggs:
                sprite = _eggIcon;
                break;
            case Item.ItemType.Rice:
                sprite = _riceIcon;
                break;
            case Item.ItemType.SoySauce:
                sprite = _soyIcon;
                break;
            case Item.ItemType.GreenOnion:
                sprite = _greenOnionIcon;
                break;
            default:
                break;
        }
        return sprite;
    }
}