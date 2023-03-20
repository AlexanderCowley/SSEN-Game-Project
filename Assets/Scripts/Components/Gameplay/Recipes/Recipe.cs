using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    [System.Serializable] public class RecipeIngredient
    {
        public Item.ItemType ingredientType;
        public int perfectChopValue;
    }

    [SerializeField] private List<RecipeIngredient> _perfectIngredients;
    [SerializeField] private float[] _perfectHeatRange = new float[2];
    [SerializeField] private int _perfectTossValue;

    public List<RecipeIngredient> PerfectIngredients { get { return _perfectIngredients; } }
    public float[] PerfectHeatRange { get { return _perfectHeatRange; } }
    public int PerfectTossValue { get { return _perfectTossValue; } }
}
