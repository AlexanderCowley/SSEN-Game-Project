using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualityEvaluation : MonoBehaviour
{
    [SerializeField] private AudioClip _bellRingClip;
    private string _allIngredientsMessage, _ingredientsOrderMessage, _choppingMessage, _heatingMessage, _tossingMessage, _resultMessage;
    private QiMeter _qiMeter;
    private AudioSource _audioSource;
    GourmetNight _gourmetNight;
    PracticeMode _practiceMode;

    bool PracticeModeActive = false;
    bool GourmetNightActive = false;
    private void Start()
    {
        _qiMeter = GameplayController.Instance.GetComponent<QiMeter>();
        _audioSource = GetComponent<AudioSource>();
        GourmetNightActive = LunchRush.Instance.gameObject.TryGetComponent<GourmetNight>(out _gourmetNight);
        PracticeModeActive = LunchRush.Instance.gameObject.TryGetComponent<PracticeMode>(out _practiceMode);
    }

    private int AllIngredientsExist(List<Item> ingredients, Recipe recipe)
    {
        int matchingIngredients = 0;
        int maxIngredients = recipe.PerfectIngredients.Count;

        // Creating a new list of ItemType
        List<Item.ItemType> inputIngredients = new List<Item.ItemType>();

        foreach (Item added in ingredients)
        {
            inputIngredients.Add(added.Type);
        }

        // Iterate through recipe and track if the input has what it calls for
        for (int i = 0; i < recipe.PerfectIngredients.Count; i++)
        {
            if (inputIngredients.Contains(recipe.PerfectIngredients[i].ingredientType))
            {
                matchingIngredients++;
            }
        }
        
        int multiplier = (matchingIngredients == maxIngredients) ? 1 : 0;
        _allIngredientsMessage = (matchingIngredients == maxIngredients) ? "" : "An ingredient(s) missing. ";
        return multiplier;
    }

    private int PerfectOrder(List<Item> ingredients, Recipe recipe)
    {
        if (ingredients.Count == recipe.PerfectIngredients.Count)
        {
            for (int i = 0; i < recipe.PerfectIngredients.Count; i++)
            {
                if (ingredients[i].Type != recipe.PerfectIngredients[i].ingredientType)
                {
                    _ingredientsOrderMessage = $"Improper order. ";
                    return 0;
                }
            }
            _ingredientsOrderMessage = "";
            return 1;
        }
        else
        {
            _ingredientsOrderMessage = "Ingredient(s) missing. ";
            return 0;
        }
    }

    private int PerfectChopping(List<Item> ingredients, Recipe recipe)
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            for (int j = 0; j < recipe.PerfectIngredients.Count; j++)
            {
                if (ingredients[i].Type == recipe.PerfectIngredients[j].ingredientType)
                {
                    if (!ingredients[i].PerfectChops)
                    {
                        if (ingredients[i].ChopValue != recipe.PerfectIngredients[j].perfectChopValue)
                        {
                            _choppingMessage = $"Improper cutting. ";
                            return 0;
                        }
                    }                    
                }
            }
        }
        _choppingMessage = "";
        return 1;
    }

    private int PerfectHeating(List<Item> ingredients, Recipe recipe)
    {
        if (ingredients[0].PerfectHeat)
        {
            _heatingMessage = "";
            return 1;
        }
        else
        {
            float heat = ingredients[0].HeatValue;
            int points = ((heat >= recipe.PerfectHeatRange[0]) && (heat <= recipe.PerfectHeatRange[1])) ? 1 : 0;
            _heatingMessage = ((heat >= recipe.PerfectHeatRange[0]) && (heat <= recipe.PerfectHeatRange[1])) ?
                "" :
                "Wrong heating time. ";
            return points;
        }
        // Negative points if heated too long?
    }

    private int PerfectTossing(int toss, Recipe recipe)
    {
        int points = (toss == recipe.PerfectTossValue) ? 1 : 0;
        _tossingMessage = (toss == recipe.PerfectTossValue) ? "" : "Unevenly mixed. ";
        return points;
    }

    public void EvaluateAndDestroy(List<Item> ingredients, Recipe recipe)
    {
        int totalPoints = AllIngredientsExist(ingredients, recipe) *
            (6 + PerfectOrder(ingredients, recipe) + PerfectChopping(ingredients, recipe) +
            PerfectHeating(ingredients, recipe) + PerfectTossing(ingredients[0].TossValue, recipe));

        if(GourmetNightActive && totalPoints < 10)
            _gourmetNight.End();

        if (PracticeModeActive)
        {
            _practiceMode.AddServe();
        }

        switch (totalPoints)
        {
            case int n when (n < 1):
                // Any missing ingredients =  automatic zero!
                _resultMessage = "Dish is unacceptable! ";
                break;
            case int n when (n == 6):
                // The right ingredients are there, it's just prepared very incorrectly.
                _resultMessage = "Dish is mediocre! ";
                break;
            case int n when (n < 10 && n > 6):
                // Only a couple of mistakes from perfection.
                _resultMessage = "Dish is good! ";
                break;
            case 10:
                // Everything was done right!
                _resultMessage = "Dish is perfect! ";
                _gourmetNight?.TimeMultiplier();
                break;
            default:
                break;
        }

        Debug.Log($"{_allIngredientsMessage} \n{_ingredientsOrderMessage} " +
            $"\n{_choppingMessage} \n{_heatingMessage} \n{_tossingMessage} " +
            $"\n{_resultMessage} \nTotal Points: {totalPoints}");

        GameplayController.Instance.AddPoints(totalPoints);
        GameplayController.Instance.DisplayEvaluationText($"{_resultMessage}{_allIngredientsMessage}{_ingredientsOrderMessage}{_choppingMessage}{_heatingMessage}{_tossingMessage}Earned {totalPoints} points!");
        _qiMeter.IncreaseByServing(totalPoints);
        _audioSource.PlayOneShot(_bellRingClip);

        // Destroy ingredients (ToArray avoids enumeration errors)
        foreach(Item i in ingredients.ToArray())
        {
            Destroy(i);
            ingredients.Remove(i);
            GameplayController.Instance.TrackIngredient(i, true);
        }
    }
}
