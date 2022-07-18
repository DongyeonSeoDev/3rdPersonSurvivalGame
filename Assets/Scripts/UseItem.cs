using UnityEngine;

public class UseItem : MonoBehaviour
{
    public void AddHunger(float addHungerValue) // 배고픔 올려주는 아이템 사용
    {
        PlayerStats.Instance.AddHunger(addHungerValue);
    }
}
