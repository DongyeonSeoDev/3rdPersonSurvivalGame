using UnityEngine;

public class ItemUIObject : MonoBehaviour // UI ������Ʈ
{
    // �������� �̺�Ʈ
    public virtual void ActiveTrue()
    {
        gameObject.SetActive(true);
    }

    // �������� �̺�Ʈ
    public virtual void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
