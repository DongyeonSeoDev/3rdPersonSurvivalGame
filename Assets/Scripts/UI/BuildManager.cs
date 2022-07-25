using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour // 건설 Manager
{
    [HideInInspector]
    public ItemSO currentBuildItem; // 현재 들고있는 건설 아이템
    
    // 사용 가능한 건설 오브젝트 확인
    public Dictionary<ItemSO, bool> isUsableBuilding = new Dictionary<ItemSO, bool>();

    // 싱글톤 패턴
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
    
    // 현재 들고있는 오브젝트 설정
    public void SetBuildObject(ItemSO buildItem)
    {
        if (currentBuildItem != buildItem)
        {
            currentBuildItem = buildItem;
        }
    }

    public void Build()
    {
        // 오브젝트를 플레이어 방향으로 회전
        Quaternion rotation = Quaternion.Euler(0f, GameManager.player.transform.rotation.eulerAngles.y, 0f);
        // 오브젝트를 플레이어 앞쪽에 배치하도록 설정
        Vector3 position = GameManager.player.position + rotation * currentBuildItem.buildPosition;
        // 오브젝트 생성
        Instantiate(currentBuildItem.buildObject, position, rotation);
    }
}