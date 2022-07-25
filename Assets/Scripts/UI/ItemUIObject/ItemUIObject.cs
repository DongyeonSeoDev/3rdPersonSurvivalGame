using UnityEngine;

public class ItemUIObject : MonoBehaviour // UI 오브젝트
{
    // 켜졌을때 이벤트
    public virtual void ActiveTrue()
    {
        gameObject.SetActive(true);
    }

    // 꺼졌을때 이벤트
    public virtual void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
