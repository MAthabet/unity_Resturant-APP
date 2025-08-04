using System;
using SQLite4Unity3d;
public enum OrderStatus
{
    New,
    Viewed,
    Completed
}
public class Users
{
    [PrimaryKey, AutoIncrement]
    public int UserID { get; set; }
    [NotNull]
    public string Email { get; set; }
    [NotNull]
    public string PhoneNumber { get; set; }
    [NotNull]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [NotNull]
    public string Password { get; set; }
    [NotNull]
    public bool IsAdmin { get; set; }
}

public class Meals
{
    [PrimaryKey, AutoIncrement]
    public int MealID { get; set; }
    [NotNull]
    public string MealName { get; set; }
    [NotNull]
    public float Price { get; set; }
    [NotNull]
    public int CategoryID { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    [NotNull]
    public bool Available { get; set; }
}

public class Orders
{
    [PrimaryKey, AutoIncrement]
    public int OrderID { get; set; }

    public int UserID { get; set; }
    public float Discount { get; set; }
    public float TotalCost { get; set; }
    [NotNull]
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
}

public class OrderItems
{
    [PrimaryKey]
    public int OrderID { get; set; }
    [PrimaryKey]
    public int MealID { get; set; }
    [NotNull]
    public int Quantity { get; set; }
}

public class Reviews
{
    [PrimaryKey]
    public int UserID { get; set; }
    [PrimaryKey]
    public int MealID { get; set; }
    [NotNull]
    public int Rating { get; set; }
    public string Comment { get; set; }
}
public class GameScores
{
    [PrimaryKey, AutoIncrement]
    public int ScoreID { get; set; }
    [NotNull]
    public int UserID { get; set; }
    [NotNull]
    public int Score { get; set; }
    [NotNull]
    public DateTime ScoreTime { get; set; }
}

public class MealCategories
{
    [PrimaryKey, AutoIncrement]
    public int CategoryID { get; set; }
    [NotNull]
    public string CategoryName { get; set; }
}
