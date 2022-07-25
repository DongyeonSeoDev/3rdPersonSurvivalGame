using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ItemSO")]
public class ItemSO : ScriptableObject // ������ ������
{
    public Sprite itemSprite; // ������ �̹���
    public int attackPower; // ������ ���ݷ� ����
    public bool isUsable; // ��� ���� Ȯ��
    public bool isBuildable; // ��ġ�� �������� Ȯ��

    public UnityEvent itemUseEvent; // �������� ��������� �߻��ϴ� �̺�Ʈ
    public GameObject buildObject; // ��ġ�� ������Ʈ
    public Vector3 buildPosition; // ��ġ�� ��ġ
}
