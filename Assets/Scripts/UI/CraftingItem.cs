using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItem : MonoBehaviour // ������ ���� ������
{
    public List<ItemSO> needItemList = new List<ItemSO>(); // �ʿ��� ������ ����Ʈ
    public List<int> needItemCountList = new List<int>(); // �ʿ��� ������ ���� ����Ʈ
    public List<int> currentItemCountList = new List<int>(); // ���� ������ �ִ� ������ ����Ʈ
    public List<Text> needItemTextList = new List<Text>(); // �ʿ��� �������� �ؽ�Ʈ ����Ʈ
    public ItemSO craftingItem; // �����ϴ� ������
    public ItemSO needBuilding; // �ʿ��� ���๰
    public Image buttonImage; // ���� ��ư �̹���
    public bool isCrafting; // ���� �������� Ȯ��
}
