using UnityEngine;

/// <summary>
/// A MonoBehaviour Singleton
/// </summary>
public class SingletonApp<T> : SingletonScene<T> where T : MonoBehaviour
{
    public override void Awake()
    {
        var instances = FindObjectsOfType(typeof(T));
        if (instances.Length > 1)
        {
            DestroyThisCopy();
            Debug.LogFormat(this, "New singlton of type {0} arrived - and it is shut down immediatly :)", this.name);
        }
        else if (instances.Length == 1 && _instance == null)
        {
            InitSingletonInstance(instances);
        }
    }
    public override void InitSingleton()
    {
        DontDestroyOnLoad(_instance.gameObject);
    }

    public virtual void DestroyThisCopy()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
