using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour // �Ǽ� Manager
{
    [HideInInspector]
    public ItemSO currentBuildItem; // ���� ����ִ� �Ǽ� ������
    
    // ��� ������ �Ǽ� ������Ʈ Ȯ��
    public Dictionary<ItemSO, bool> isUsableBuilding = new Dictionary<ItemSO, bool>();
    // Pool ������Ʈ Dictionary
    public Dictionary<ItemSO, Queue<GameObject>> poolObjectDictionary = new Dictionary<ItemSO, Queue<GameObject>>();

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

        // Key�� ���ٸ� ����
        if (!poolObjectDictionary.ContainsKey(currentBuildItem))
        {
            poolObjectDictionary.Add(currentBuildItem, new Queue<GameObject>());
        }

        GameObject buildObject;

        if (poolObjectDictionary[currentBuildItem].Count > 0)
        {
            // Pool Object���� ������
            buildObject = poolObjectDictionary[currentBuildItem].Dequeue();
        }
        else
        {
            // ������ ���� ����
            buildObject = Instantiate(currentBuildItem.buildObject);
        }

        // ��ġ�� ���� ����
        buildObject.transform.position = position;
        buildObject.transform.rotation = rotation;

        buildObject.SetActive(true);
    }
}