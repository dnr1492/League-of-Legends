using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    [SerializeField] ShopPopup shopPopup;
    [SerializeField] Button btnShop;
    private bool isOpen = false;

    private void Awake()
    {
        btnShop.onClick.AddListener(() => {
            isOpen = !isOpen;
            shopPopup.Open(isOpen);
        });
    }
}
