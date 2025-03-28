using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{
    private SpriteController spriteController;

    private List<Button> btns = new List<Button>();
    [SerializeField] GameObject btnPrefab;
    [SerializeField] Button btnMain;
    private bool isOn = false;
    [SerializeField] RectTransform listRt;
    private List<DropdownButton> dropDownButtonList;
    private DropdownButton curDropdownButton;
    private Image imgSelect;
    private Text txtSelect;

    private Color selectColor = new Color(0.858f, 0.858f, 0.858f, 1);
    private int mainIndex = 0;
    private bool isActive = true;

    private void Awake()
    {
        spriteController = FindObjectOfType<SpriteController>();

        btnMain.onClick.AddListener(OnClickMainButton);

        imgSelect = btnMain.transform.Find("imgSelect").GetComponent<Image>();
        txtSelect = btnMain.transform.Find("txtSelect").GetComponent<Text>();

        ExchangeDropdown();
    }

    private void OnClickMainButton()
    {
        isOn = !isOn;
        listRt.gameObject.SetActive(isOn);
    }

    private void ExchangeDropdown()
    {
        isOn = false;
        dropDownButtonList = new List<DropdownButton>();

        int count = 0;
        int enumCount = System.Enum.GetValues(typeof(ChampionController.ChampionType)).Length;
        for (int i = 0; i < enumCount; i++)
        {
            GameObject btnGo = Instantiate(btnPrefab, listRt);
            btns.Add(btnGo.GetComponent<Button>());
        }
        for (int i = 0; i < btns.Count; i++)
        {
            SetDropdown(i, ((ChampionController.ChampionType)i).ToString(), mainIndex, isActive, ref count);
        }

        imgSelect.sprite = curDropdownButton.img.sprite;
        txtSelect.text = ((ChampionController.ChampionType)mainIndex).ToString();

        btnMain.gameObject.SetActive(true);
        listRt.gameObject.SetActive(false);
        SelectDropdownButton(mainIndex, EventAction);
    }

    private void SetDropdown(int index, string name, int mainIndex, bool isActive, ref int count)
    {
        if (isActive)
        {
            DropdownButton cdb = new DropdownButton();
            cdb.btn = btns[index];
            cdb.img = btns[index].transform.Find("img_icon").GetComponent<Image>();
            cdb.img.sprite = spriteController.Get(name);
            cdb.checkGo = btns[index].transform.Find("bg").Find("img_check").gameObject;
            cdb.txt = btns[index].transform.Find("txt").GetComponent<Text>();
            cdb.txt.text = name;
            cdb.index = count;

            int buttonIndex = count;
            btns[index].onClick.RemoveAllListeners();
            btns[index].onClick.AddListener(() => {
                SelectDropdownButton(buttonIndex, EventAction);
            });

            if (dropDownButtonList.Count == mainIndex)
            {
                curDropdownButton = cdb;
                cdb.btn.image.color = selectColor;
                cdb.checkGo.SetActive(true);
            }
            else
            {
                cdb.btn.image.color = Color.white;
                cdb.checkGo.SetActive(false);
            }

            count++;
            dropDownButtonList.Add(cdb);
        }

        btns[index].gameObject.SetActive(isActive);
    }

    private void SelectDropdownButton(int index, UnityAction eventAction)
    {
        Debug.Log("Select Dropdown Button Index : " + index);

        if (curDropdownButton != null)
        {
            curDropdownButton.btn.image.color = Color.white;
            curDropdownButton.checkGo.SetActive(false);
        }

        curDropdownButton = dropDownButtonList[index];
        curDropdownButton.btn.image.color = selectColor;
        curDropdownButton.checkGo.SetActive(true);

        imgSelect.sprite = curDropdownButton.img.sprite;
        txtSelect.text = ((ChampionController.ChampionType)index).ToString();

        isOn = false;
        listRt.gameObject.SetActive(isOn);

        eventAction();
    }

    private void EventAction()
    {
        ChampionController.SetChampion(curDropdownButton.txt.text);
    }
}

[System.Serializable]
public class DropdownButton
{
    public Button btn;
    public Image img;
    public GameObject checkGo;
    public Text txt;
    public int index;
}