using SQLite4Unity3d;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Singleton { get; private set; }
    [SerializeField]
    private string dbPath;
    private SQLiteConnection connection;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this);
            Init();
        }
        else
            Destroy(this);
        
    }
    void Init()
    {
        if (File.Exists(dbPath))
            connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite);
        else
            Debug.LogWarning("DB Was not found");
    }

    public Users GetUser(string email)
    {
        email.ToLower();
        return connection.Table<Users>().Where(u => u.Email == email).FirstOrDefault();
    }

    public void AddUser(string email,string phoneNumber, string fn, string ln, string password, bool isAdmin = false)
    {
        email.ToLower();
        Users user = new() { Email = email, PhoneNumber = phoneNumber, FirstName = fn, LastName = ln, Password = password, IsAdmin = isAdmin};
        connection.Insert(user);
    }
    public List<Meals> GetAllMeals()
    {
        return connection.Table<Meals>().ToList();
    }
    public void AddMeal(string mealName, float price, string category, bool available = true, string imagePath = "", string description = "")
    {
        var cat = connection.Table<MealCategories>().Where(c => c.CategoryName == category).FirstOrDefault();
        if(cat == null)
        {
            cat = new MealCategories();
            cat.CategoryName = category;
            connection.Insert(cat);
        }
        Meals newMeal = new() { MealName = mealName, Price = price, CategoryID = cat.CategoryID, Description = description, ImagePath = imagePath, Available = available };
        connection.Insert(newMeal);
    }
    public List<MealCategories> GetAllCategories() 
    {
        return connection.Table<MealCategories>().ToList();
    }


    public void UpdateMealPrice(int mealId, float newPrice)
    {
        var meal = connection.Table<Meals>().Where(m => m.MealID == mealId).FirstOrDefault();
        if (meal != null)
        {
            meal.Price = newPrice;
            connection.Update(meal);
        }
        else 
        {
            Debug.LogWarning("There is no such a meal");
        }
    }
    public void UpdateMealAvailability(int mealId, bool isAvailable)
    {
        var meal = connection.Table<Meals>().Where(m => m.MealID == mealId).FirstOrDefault();
        if (meal != null)
        {
            meal.Available = isAvailable;
            connection.Update(meal);
        }
        else
        {
            Debug.LogWarning("There is no such a meal");
        }
    }
    public void AddOrder(int userID, Dictionary<Meals, int> orderItems, float totalCost, float discount = 0.0f)
    {
        Orders newOrder = new() { UserID = userID, Discount = discount, TotalCost = totalCost};
        connection.Insert(newOrder);
        foreach(var item in orderItems)
        {
            AddOrderItems(newOrder.OrderID, item.Key.MealID, item.Value);
        }
    }
    private void AddOrderItems(int orderID, int mealID,int quantity)
    {
        OrderItems newOrderItem = new() { OrderID = orderID, MealID = mealID, Quantity = quantity};
        connection.Insert(newOrderItem);
    }
    public void SaveScore(int userId, int score)
    {
        GameScores newScore = new(){ UserID = userId, Score = score};
        connection.Insert(newScore);
    }

    public List<GameScores> GetLeaderboard()
    {
        return connection.Table<GameScores>().OrderByDescending(s => s.Score).Take(10).ToList();
    }
    public List<Meals> SearchMealsByName(string mealName)
    {
            if (string.IsNullOrEmpty(mealName))
            {
                return connection.Table<Meals>().ToList();
            }
            else
            {
                string sql = "SELECT * FROM Meals WHERE LOWER(MealName) LIKE ?";
                return connection.Query<Meals>(sql, $"%{mealName.ToLower()}%");
            }
    }
}