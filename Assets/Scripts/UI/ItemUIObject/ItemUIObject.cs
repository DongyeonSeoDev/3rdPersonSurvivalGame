using UnityEngine;

public class ItemUIObject : MonoBehaviour
{
    public virtual void ActiveTrue()
    {
        gameObject.SetActive(true);
    }

    public virtual void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
