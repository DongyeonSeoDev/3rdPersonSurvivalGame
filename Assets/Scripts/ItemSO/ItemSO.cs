using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ItemSO")]
public class ItemSO : ScriptableObject // ������ ������
{
    public Sprite itemSprite; // ������ �̹���
    public int attackPower; // ������ ���ݷ� ����
    public bool isUsable; // ��� ���� Ȯ��

    public UnityEvent itemUseEvent; // �������� ��������� �߻��ϴ� �̺�Ʈ
}
