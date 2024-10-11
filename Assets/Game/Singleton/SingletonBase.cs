using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
    public static T Instance { private set; get; }

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = (T)this;
        else
            if (Instance != this)
            Destroy(gameObject);
    }
}