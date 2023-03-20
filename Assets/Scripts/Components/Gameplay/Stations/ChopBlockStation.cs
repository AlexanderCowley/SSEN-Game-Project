using UnityEngine;

public class ChopBlockStation : Station
{
    private Animator _playerAnimator;

    public int ScoreValue { get; private set; }
    public override bool IsOccupied() => ItemOccupied != null;

    private new void Start()
    {
        _playerAnimator = GameplayController.Instance.PlayerObject.GetComponentInChildren<Animator>();
        base.Start();
    }

    protected override void Place(PlayerInventory playerInventory)
    {
        if (playerInventory.ItemHeld.GetType() != typeof(ChoppableItems))
            return;

        base.Place(playerInventory);
    }

    public override void StationAction(PlayerInventory playerInventory)
    {
        if (IsOccupied() && playerInventory.ItemHeld == null)
            Chop();
    }

    void Chop()
    {
        ItemOccupied.ChopValue++;
        _playerAnimator.Play("Chopping");
        _audioSource.PlayOneShot(_audioSource.clip);
        Debug.Log($"Chop Value: {ItemOccupied.ChopValue}", gameObject);
    }
}
