using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour // �Ǽ� Manager
{
    [HideInInspector]
    public ItemSO currentBuildItem; // ���� ����ִ� �Ǽ� ������
    
    // ��� ������ �Ǽ� ������Ʈ Ȯ��
    public Dictionary<ItemSO, bool> isUsableBuilding = new Dictionary<ItemSO, bool>();

    // �̱��� ����
    private static BuildManager instance;
    public static BuildManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);

            return;
        }

        instance = this;
    }
    
    // ���� ����ִ� ������Ʈ ����
    public void SetBuildObject(ItemSO buildItem)
    {
        if (currentBuildItem != buildItem)
        {
            currentBuildItem = buildItem;
        }
    }

    public void Build()
    {
        // ������Ʈ�� �÷��̾� �������� ȸ��
        Quaternion rotation = Quaternion.Euler(0f, GameManager.player.transform.rotation.eulerAngles.y, 0f);
        // ������Ʈ�� �÷��̾� ���ʿ� ��ġ�ϵ��� ����
        Vector3 position = GameManager.player.position + rotation * currentBuildItem.buildPosition;
        // ������Ʈ ����
        Instantiate(currentBuildItem.buildObject, position, rotation);
    }
}