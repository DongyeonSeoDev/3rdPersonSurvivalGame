using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [HideInInspector]
    public ItemSO currentBuildItem;
    
    public Dictionary<ItemSO, bool> isUsableBuilding = new Dictionary<ItemSO, bool>();

    // ΩÃ±€≈Ê ∆–≈œ
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

    public void SetBuildObject(ItemSO buildItem)
    {
        if (currentBuildItem != buildItem)
        {
            currentBuildItem = buildItem;
        }
    }

    public void Build()
    {
        Quaternion rotation = Quaternion.Euler(0f, GameManager.player.transform.rotation.eulerAngles.y, 0f);
        Vector3 position = GameManager.player.position + rotation * currentBuildItem.buildPosition;

        Instantiate(currentBuildItem.buildObject, position, rotation);
    }
}