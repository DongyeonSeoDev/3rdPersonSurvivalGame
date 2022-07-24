using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItem : MonoBehaviour
{
    public List<ItemSO> needItemList = new List<ItemSO>();
    public List<int> needItemCountList = new List<int>();
    public List<int> currentItemCountList = new List<int>();
    public List<Text> needItemTextList = new List<Text>();
    public ItemSO craftingItem;
    public ItemSO needBuilding;
    public Image buttonImage;
    public bool isCrafting;
}
