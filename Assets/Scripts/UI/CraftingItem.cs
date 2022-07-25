using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItem : MonoBehaviour // 아이템 제작 데이터
{
    public List<ItemSO> needItemList = new List<ItemSO>(); // 필요한 아이템 리스트
    public List<int> needItemCountList = new List<int>(); // 필요한 아이템 갯수 리스트
    public List<int> currentItemCountList = new List<int>(); // 현재 가지고 있는 아이템 리스트
    public List<Text> needItemTextList = new List<Text>(); // 필요한 아이템의 텍스트 리스트
    public ItemSO craftingItem; // 제작하는 아이템
    public ItemSO needBuilding; // 필요한 건축물
    public Image buttonImage; // 제작 버튼 이미지
    public bool isCrafting; // 제작 가능한지 확인
}
