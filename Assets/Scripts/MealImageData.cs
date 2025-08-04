using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MealImageData
{
    static Dictionary<Meals, Texture> mealsImages;

    public static Texture GetMealImage(Meals meal)
    {
        if (meal == null || mealsImages == null || !mealsImages.ContainsKey(meal))
            return null;
        return mealsImages[meal];
    }
    public static void SetMealImage(Meals meal, Texture image)
    {
        if (mealsImages == null)
        {
            mealsImages = new Dictionary<Meals, Texture>();
        }

        if (mealsImages.ContainsKey(meal))
        {
            mealsImages[meal] = image;
        }
        else
        {
            mealsImages.Add(meal, image);
        }
    }
}
