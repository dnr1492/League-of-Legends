using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum Type { Damage, Speed }

    public struct ItemData
    {
        public Type type;
        public int price;
        public int damageAndSpeed;
    }
    public ItemData curItemData;

    private ShopPopup shopPopup;
    private Button btn;
    private Image img;
    private Text txtPrice, txtDamageAndSpeed;

    private void Awake()
    {
        btn = GetComponent<Button>();
        img = transform.Find("img_item").GetComponent<Image>();
        txtPrice = transform.Find("txt_price").GetComponent<Text>();
        txtDamageAndSpeed = transform.Find("txt_damage_and_speed").GetComponent<Text>();

        img.raycastTarget = false;
        txtPrice.raycastTarget = false;
        txtDamageAndSpeed.raycastTarget = false;
    }

    public void Init(ShopPopup shopPopup, int price, int increDamage, int increSpeed, Sprite spr)
    {
        this.shopPopup = shopPopup;

        gameObject.SetActive(true);

        btn.onClick.AddListener(()=> {
            Purchase(price, increDamage, increSpeed);
        });

        txtPrice.text = "가격 : " + price.ToString();
        if (increDamage != 0) txtDamageAndSpeed.text = "데미지 : " + increDamage.ToString();
        if (increSpeed != 0) txtDamageAndSpeed.text = "속도 : " + increSpeed.ToString();
        img.sprite = spr;
    }

    private void Purchase(int price, int increDamage, int increSpeed)
    {
        if (!shopPopup.EquiptItem(this, price, increDamage, increSpeed)) return;

        if (increDamage != 0) ChampionController.Champion.GetComponent<ChampionController>().Damage += increDamage;
        if (increSpeed != 0) ChampionController.Champion.GetComponent<ChampionController>().MoveSpeed += increSpeed;
    }

    #region 인벤토리에 아이템 추가
    public void AddInventory(int price, int increDamage, int increSpeed)
    {
        img.rectTransform.sizeDelta = new Vector2(50, 50);
        txtPrice.gameObject.SetActive(false);
        txtDamageAndSpeed.gameObject.SetActive(false);

        if (increDamage != 0) SetItemData(Type.Damage, price, increDamage);
        if (increSpeed != 0) SetItemData(Type.Speed, price, increSpeed);
    }

    private void SetItemData(Type type, int price, int DamageAndSpeed)
    {
        curItemData = new ItemData();
        curItemData.type = type;
        curItemData.price = price;
        curItemData.damageAndSpeed = DamageAndSpeed;
    }
    #endregion
}
