using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Singleton { get; private set; }

    Users currentUser;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetCurrentUser(Users user)
    {
        currentUser = user;
    }

    public void ClearCurrentUser()
    {
        currentUser = null;
    }

    public int GetCurrntUserID()
    {
        return currentUser.UserID;
    }
    public bool IsCurrentUserAdmin()
    {
        if (currentUser != null && currentUser.IsAdmin)
            return true;
        return false;
    }
}
