using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ItemSO")]
public class ItemSO : ScriptableObject // 아이템 데이터
{
    public Sprite itemSprite; // 아이템 이미지
    public int attackPower; // 무기라면 공격력 설정
    public bool isUsable; // 사용 가능 확인

    public UnityEvent itemUseEvent; // 아이템을 사용했을때 발생하는 이벤트
}
