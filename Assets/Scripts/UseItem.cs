using UnityEngine;

public class UseItem : MonoBehaviour
{
    public void AddHunger(float addHungerValue) // ����� �÷��ִ� ������ ���
    {
        PlayerStatus.Instance.AddHunger(addHungerValue);
    }
}
