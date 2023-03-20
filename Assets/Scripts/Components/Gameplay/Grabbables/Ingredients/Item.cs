using UnityEngine;
using UnityEngine.Pool;
public class Item : MonoBehaviour
{
	public enum ItemType
    {
        Oil,
        Shallot,
        Eggs,
        Rice,
        SoySauce,
        GreenOnion,
        Plate
    }

    [SerializeField] private ItemType _type; 
    protected int chopValue, tossValue = 0;
    protected int _maxValue = 10;
    protected float heatValue = 0.0f;
    private bool _perfectChops, _perfectHeat;

    ObjectPool<Item> _itemPool;

    public ItemType Type { get { return _type; } }
    public bool PerfectChops { get { return _perfectChops; } }
    public bool PerfectHeat { get { return _perfectHeat; } }

    //Might move this to a Scriptable Object
    public int ChopValue
    {
        get
        {
            return chopValue;
        }
        set
        {
            //Event for modifying score, sfx, etc
            chopValue = value;
        }
    }

    public float HeatValue
    {
        get { return heatValue; } 
        set { heatValue = value; }
    }

    public int TossValue
    {
        get { return tossValue; }
        set { tossValue = value; }
    }

    public void ForcePerfectChops()
    {
        _perfectChops = true;
    }

    public void ForcePerfectHeat()
    {
        _perfectHeat = true;
    }

    public void SetPool(ObjectPool<Item> poolToAssign) => _itemPool = poolToAssign;

}
