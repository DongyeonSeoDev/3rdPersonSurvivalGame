using UnityEngine;

public class UseItem : MonoBehaviour
{
    public void AddHunger(float addHungerValue) // ����� �÷��ִ� ������ ���
    {
        PlayerStats.Instance.AddHunger(addHungerValue);
    }
}
