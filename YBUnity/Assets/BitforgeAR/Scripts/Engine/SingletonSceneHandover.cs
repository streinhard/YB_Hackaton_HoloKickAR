using UnityEngine;


/// <summary>
/// A MonoBehaviour Singleton
/// </summary>
public class SingletonSceneHandover<T> : SingletonApp<T> where T : MonoBehaviour
{
    private static bool _otherSingletonWasShutdown = true;  // initial value is true, so that this singleton don't get shut down immidiatly

    public override void DestroyThisCopy()
    {
        base.DestroyThisCopy();
        _otherSingletonWasShutdown = true;
    }

    public virtual void OnLevelWasLoaded(int level)
    {
        //TODO is never called, don't know why??, correct it
        //Debug.Log("OnLevelWasLoaded");

        var instances = FindObjectsOfType(typeof(T));

        // destroy this instance if it is the last one and no other singleton was shut down
        if(instances.Length == 1 && !_otherSingletonWasShutdown) 
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }
}