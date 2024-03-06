using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public bool Visible;
    private GameObject objectInScene;
    public abstract void Initialize();
    public virtual void Hide()
    {
        Destroy(objectInScene);
        Visible = false;
    }
    public virtual void Show(Transform parent = null)
    {
        objectInScene = Instantiate(gameObject, parent);
        Visible = true;
    }
}
