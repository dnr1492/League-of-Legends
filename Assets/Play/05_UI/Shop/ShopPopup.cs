using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPopup : MonoBehaviour
{
    [SerializeField] RectTransform content;
    [SerializeField] GameObject itemPrefab;
    private readonly int itemCount = 10;

    [SerializeField] int[] arrPrice;
    [SerializeField] int[] arrDamage;
    [SerializeField] int[] arrSpeed;
    [SerializeField] Sprite[] arrSprites;

    [SerializeField] Text txtOwnGold;
    [SerializeField] Transform inventoryTr;
    [SerializeField] Button btnSell;

    [SerializeField] GraphicRaycaster graphicRaycaster;
    [SerializeField] EventSystem eventSystem;
    private PointerEventData pointerEventData;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        SelectItem();
    }

    #region 상점 열기/닫기
    public void Open(bool isOpen)
    {
        gameObject.SetActive(isOpen);

        if (isOpen == false) return;

        for (int i = 0; i < itemCount; i++)
        {
            GameManager.GetInstance().DisplayGold(txtOwnGold);

            if (content.childCount - 1 >= arrSprites.Length) break;
            if (i >= arrSprites.Length) break;

            Item item = Instantiate(itemPrefab, content).GetComponent<Item>();
            item.Init(this, arrPrice[i], arrDamage[i], arrSpeed[i], arrSprites[i]);
        }
    }
    #endregion

    #region 아이템 장착
    public bool EquiptItem(Item item, int price, int increDamage, int increSpeed)
    {
        if (GameManager.GetInstance().GetGold() - price < 0 || inventoryTr.childCount >= 6) return false;
        else GameManager.GetInstance().DecreaseGold(price);

        GameObject itemGo = Instantiate(item.gameObject, inventoryTr);
        itemGo.GetComponent<Item>().AddInventory(price, increDamage, increSpeed);
        itemGo.layer = LayerMask.NameToLayer("Inventory_Slot");
        return true;
    }
    #endregion

    #region 아이템 판매
    private void SelectItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);

            Item item = null;
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.layer != LayerMask.NameToLayer("Inventory_Slot")) continue;
                item = result.gameObject.GetComponent<Item>();
                break;
            }

            if (item != null) SellItem(item);
        }
    }

    private void SellItem(Item item)
    {
        btnSell.onClick.RemoveAllListeners();
        btnSell.onClick.AddListener(() => {
            Item.ItemData itemData = item.curItemData;
            if (itemData.type == Item.Type.Damage) ChampionController.Champion.GetComponent<ChampionController>().Damage -= itemData.damageAndSpeed;
            if (itemData.type == Item.Type.Speed) ChampionController.Champion.GetComponent<ChampionController>().MoveSpeed -= itemData.damageAndSpeed;
            GameManager.GetInstance().IncreaseGold(itemData.price);
            Destroy(item.gameObject);
        });
    }
    #endregion
}
