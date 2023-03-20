using System.Collections.Generic;

public class ServingStation : Station
{
    List<Item> _itemsToEvaluate = new List<Item>();
    QualityEvaluation _qualityEvaluation;

    private new void Start()
    {
        _qualityEvaluation = GetComponent<QualityEvaluation>();
    }

    public override bool IsOccupied() => ItemOccupied != null;

    public override void StationAction(PlayerInventory playerInventory)
    {
        if (playerInventory.ItemHeld == null || playerInventory.ItemHeld.GetType() != typeof(Container))
            return;

        Container container = (Container)playerInventory.ItemHeld;
        if (!container.IsReady())
            return;

        _itemsToEvaluate = new List<Item>(container.ContainerList);
        container.ClearContainer();
        playerInventory.RemoveItem();
        //Should probably replace with an object pooling system, Unity has its own
        Destroy(container.gameObject);
        _qualityEvaluation.EvaluateAndDestroy(_itemsToEvaluate, GameplayController.Instance.Recipes[0]);
        _itemsToEvaluate.Clear();
    }

    protected override void Grab(PlayerInventory playerInventory){}

    protected override void Place(PlayerInventory playerInventory)
    {
        // Probably nothing
    }
}
