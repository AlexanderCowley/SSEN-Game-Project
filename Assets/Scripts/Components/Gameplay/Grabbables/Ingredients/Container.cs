using System.Collections.Generic;
using UnityEngine;

public class Container : Item
{
    [SerializeField] private GameObject _emptyPlate, _fullPlate;

    List<Item> containerList = new List<Item>();
    public List<Item> ContainerList { get { return containerList; } }
    public bool IsReady() => containerList.Count > 0;
    public void AddItem(Item item)
    {
        Debug.Log($"Added Item {item}", gameObject);
        containerList.Add(item);
        HeatValue += item.HeatValue;
        TossValue += item.TossValue;
        ChopValue += item.ChopValue;
        //Attach object/Destroy and Spawn Next Item
    }

    private void Update()
    {
        UpdatePlateVisual();
    }

    private void UpdatePlateVisual()
    {

        _emptyPlate.SetActive(containerList.Count == 0);
        _fullPlate.SetActive(containerList.Count > 0);
    }

    public void TransferContentsToContainer(List<Item> source)
    {
        if (!IsReady())
            containerList = new List<Item>(source);
    }

    public void ClearContainer()
    {
        containerList.Clear();
    }
}
