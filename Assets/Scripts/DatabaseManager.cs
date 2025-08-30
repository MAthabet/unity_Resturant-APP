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
    private SQLiteConnection ro_connection;
    private DateTime lastWeeklyEntry;
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
    private void Start()
    {
        CheckAndRunWeeklyUpdate();
    }
    void Init()
    {
        if (File.Exists(dbPath))
        {
            connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite);
            ro_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);
        }
        else
            Debug.LogWarning("DB Was not found");
    }

    public Users GetUser(string email)
    {
        email.ToLower();
        return connection.Table<Users>().Where(u => u.Email == email).FirstOrDefault();
    }
    public Users GetUser(int userID)
    {
        return connection.Table<Users>().Where(u => u.UserID == userID).FirstOrDefault();
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
        MealCategories cat = AddCategory(category);
        Meals newMeal = new() { MealName = mealName, Price = price, CategoryID = cat.CategoryID, Description = description, ImagePath = imagePath, Available = available };
        connection.Insert(newMeal);
    }
    public void UpdateMeal(Meals meal, string category)
    {
        MealCategories cat = AddCategory(category);
        meal.CategoryID = cat.CategoryID;
        connection.Update(meal);
    }
    public List<MealCategories> GetAllCategories() 
    {
        return connection.Table<MealCategories>().ToList();
    }
    public String GetCategoryName(int categoryID) 
    {
        MealCategories cat = connection.Table<MealCategories>().Where(c => c.CategoryID == categoryID).FirstOrDefault();
        return cat.CategoryName;
    }
    private MealCategories AddCategory(string category)
    {
        var cat = connection.Table<MealCategories>().Where(c => c.CategoryName == category).FirstOrDefault();
        if (cat == null)
        {
            cat = new MealCategories();
            cat.CategoryName = category;
            connection.Insert(cat);
        }

        return cat;
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
        Orders newOrder = new() { UserID = userID, Discount = discount, TotalCost = totalCost, OrderDate = DateTime.UtcNow};
        connection.Insert(newOrder);
        foreach(var item in orderItems)
        {
            AddOrderItems(newOrder.OrderID, item.Key.MealID, item.Value);
        }
        if(discount > 0.0f)
        {
            UseWinnerDiscount(userID);
        }
    }
    private void AddOrderItems(int orderID, int mealID,int quantity)
    {
        OrderItems newOrderItem = new() { OrderID = orderID, MealID = mealID, Quantity = quantity};
        connection.Insert(newOrderItem);
    }
    public Dictionary<string,int> GetOrderItems(int orderID)
    {
        Dictionary<string, int> orderItems = new Dictionary<string, int>();
        var items = connection.Table<OrderItems>().Where(oi => oi.OrderID == orderID).ToList();
        foreach (var item in items)
        {
            var meal = connection.Table<Meals>().Where(m => m.MealID == item.MealID).FirstOrDefault();
            if (meal != null)
            {
                orderItems[meal.MealName] = item.Quantity;
            }
        }
        return orderItems;
    }
    public List<Orders> GetAllOrdersByStatus(OrderStatus status)
    {
        return ro_connection.Table<Orders>().Where(o => o.Status == status).ToList();
    }
    public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        var order = connection.Table<Orders>().Where(o => o.OrderID == orderId).FirstOrDefault();
        if (order != null)
        {
            order.Status = newStatus;
            connection.Update(order);
        }
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
    public void SaveScore(int userId, int score)
    {
        GameScores newScore = new() { UserID = userId, Score = score };
        connection.Insert(newScore);
    }
    public void AddScore(int score, int userID)
    {
        connection.Insert(new GameScores { UserID = userID, Score = score, ScoreTime = DateTime.UtcNow });
    }
    public void CheckAndRunWeeklyUpdate()
    {
        var newestWinner = connection.Table<WeeklyWinners>().OrderByDescending(w => w.EnteryDate).FirstOrDefault();

        bool needsUpdate = false;

        if (newestWinner == null)
        {
            needsUpdate = true;
        }
        else
        {
            DateTime oneWeekAgo = DateTime.UtcNow.AddDays(-7);
            if (newestWinner.EnteryDate < oneWeekAgo)
            {
                needsUpdate = true;
            }
            
        }

        if (needsUpdate)
        {
            UpdateWeeklyWinners(3);
            
        }
        else
            lastWeeklyEntry = newestWinner.EnteryDate;
    }
    private void UpdateWeeklyWinners(int limit)
    {
        List<GameScores> topPlayers = GetLeaderboard(limit);
        DeleteDublicatedWinners(topPlayers);

        foreach (var player in topPlayers)
        {
            WeeklyWinners newWinner = new() { UserID = player.UserID, EnteryDate = DateTime.UtcNow };
            connection.Insert(newWinner);
        }
        lastWeeklyEntry = DateTime.UtcNow;
    }
    private void DeleteDublicatedWinners(List<GameScores> topPlayers)
    {
        for (int i = 0; i < topPlayers.Count; i++)
        {
            for (int j = i + 1; j < topPlayers.Count; j++)
            {
                if (topPlayers[i].UserID == topPlayers[j].UserID)
                {
                    if (topPlayers[i].Score < topPlayers[j].Score)
                    {
                        topPlayers.RemoveAt(i);
                        i--;
                        break;
                    }
                    else
                    {
                        topPlayers.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
    }
    public List<GameScores> GetLeaderboard(int limit)
    {
        DateTime oneWeekAgo = DateTime.UtcNow.AddDays(-7);
        return connection.Table<GameScores>().OrderByDescending(s => s.Score).Where(s => s.ScoreTime > oneWeekAgo).Take(limit).ToList();
    }
    public int IsUserEligableForDiscount(int userId)
    {
        WeeklyWinners eligibleWin = getWinner(userId);

        if (eligibleWin != null)
            return eligibleWin.WinnerID;
        else
            return -1;
    }

    private WeeklyWinners getWinner(int userId)
    {
        DateTime oneWeekAgo = DateTime.UtcNow.AddDays(-7);

        var eligibleWin = connection.Table<WeeklyWinners>().FirstOrDefault(w => w.UserID == userId && w.UsedDate == null && w.EnteryDate >= oneWeekAgo);
        return eligibleWin;
    }

    private void UseWinnerDiscount(int userID)
    {
        var winnerRecord = getWinner(userID);

        if (winnerRecord != null)
        {
            winnerRecord.UsedDate = DateTime.UtcNow;
            connection.Update(winnerRecord);
        }
    }

    public List<MealNameCount> GetOrdersStats()
    {
        var result = connection.Query<MealNameCount>("select MealName, count (MealName) as count from (SELECT MealName From OrderItems inner join meals where OrderItems.MealID = meals.MealID) GROUP by MealName");
        
        return result;
    }
    public List<UserOrderStatistic> GetUserOrderStatistics()
    {
        using (var connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly))
        {
            // This query joins Users, Orders, and OrderItems, groups the results
            // by user, and calculates the total quantity of items for each one.
            string sql = @"
            SELECT 
                u.FirstName, 
                SUM(oi.Quantity) as TotalItemsOrdered
            FROM 
                Users u
            INNER JOIN 
                Orders o ON u.UserID = o.UserID
            INNER JOIN 
                OrderItems oi ON o.OrderID = oi.OrderID
            GROUP BY 
                u.FirstName
            ORDER BY 
                TotalItemsOrdered DESC";

            return connection.Query<UserOrderStatistic>(sql);
        }
    }
}
public class UserOrderStatistic
{
    public string FirstName { get; set; }
    public int TotalItemsOrdered { get; set; }
}