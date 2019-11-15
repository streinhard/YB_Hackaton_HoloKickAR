/// <summary>
/// Singleton for model classes
/// </summary>
public class Singleton<T> where T : class, new()
{
    private static readonly T m_Instance = new T();
    public static T Instance
    {
        get
        {
            return m_Instance;
        }
    }
}