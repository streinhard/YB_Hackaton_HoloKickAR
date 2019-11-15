using UnityEngine;

/// <summary>
/// A MonoBehaviour Singleton which exists inside a scene
/// </summary>
public class SingletonScene<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool _applicationIsQuitting = false;
    private static object _lock = new object();
    protected static T _instance = null;

    public virtual void Awake() {
        var instances = FindObjectsOfType(typeof(T));
        if (instances.Length > 1)
        {
            Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.");
        }
        else if(instances.Length == 1 && _instance == null)
        {
            InitSingletonInstance(instances);
        }
    }
	public static T Instance
	{
		get
		{
			if (_applicationIsQuitting) {
				//Debug.LogWarning("[Singleton] Instance '"+ typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
				return null;
			}

            if (_instance == null)
            {
                InitSingletonInstance();
			}

            // return instance
            return _instance;
        }
	}
    private static void InitSingletonInstance()
    {
        InitSingletonInstance(FindObjectsOfType(typeof(T)));
    }
    protected static void InitSingletonInstance(Object[] instances)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                // find existing instances
                if (instances == null || instances.Length <= 0 || (instances.Length == 1 && instances[0] == null))
                {
                    // no instance exists, create a new one
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();

                    #if UNITY_EDITOR
                    singleton.name = "(singleton) " + typeof(T).ToString();
                    #endif

                    // !! init singleton (if you get an exception here, you need to derivate singlton from SingletonScene<T> instead of just T
                    (singleton as SingletonScene<T>).InitSingleton();
                }
                else if (instances.Length == 1)
                {
                    // reuse existing instance
                    _instance = (T)instances[0];

                    // init singleton
                    if (_instance is SingletonScene<T>)
                    {
                        (_instance as SingletonScene<T>).InitSingleton();
                    }
                }
                else
                {
                    // mulitple objects found in scene, something went wrong
                    Debug.LogErrorFormat(instances[0], "Something went really wrong - there should never be more than 1 singleton({0})! Reopening the scene might fix it.", typeof(T).Name);
                }
            }
        }
    }


    /// <summary>
    /// Used for setting DontDestroyOnLoad in AppSingleton
    /// </summary>
    public virtual void InitSingleton() {}

    /// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, it will create a buggy ghost object that will stay on the Editor scene even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
    public virtual void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
        _instance = null;
    }
}