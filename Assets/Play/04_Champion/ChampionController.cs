using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChampionController : Stats
{
    public enum ChampionType { Galio, Garen, Gragas, Kaisa, MissFortune, AurelionSol }

    public static BuffController buffController;
    private StatusController statusController;

    public static Transform Champion { get => selectionChampion; }
    private static Transform selectionChampion;
    private static Transform championsTr;
    private static Transform blueRespawnTr;
    public static readonly string CHAMPION = "Champion";

    [SerializeField] Joystick joystick;
    private Vector3 moveDirection;
    public static Vector3 movePos;
    public static bool usingSkill = false;
    
    public static Camera quarterViewCam;

    public static Animator animator;
    public static readonly string aniMove = "Move";
    public static readonly string aniSpellQ = "SpellQ";
    public static readonly string aniSpellW = "SpellW";
    public static readonly string aniSpellE = "SpellE";
    public static readonly string aniSpellEHit = "SpellE_Hit";
    public static readonly string aniSpellR = "SpellR";

    protected override void Awake()
    {
        buffController = GameObject.Find("BuffController").GetComponent<BuffController>();
        statusController = GameObject.Find("StatusController").GetComponent<StatusController>();
        quarterViewCam = GameObject.Find("QuarterViewCam").GetComponent<Camera>();
        blueRespawnTr = GameObject.Find("Blue_Respawn").GetComponent<Transform>();
    }

    protected override void Start()
    {
        movePos = championsTr.position = blueRespawnTr.position;
    }

    protected override void Update()
    {
        UpdateMoveSetting();

        statusController.SetStatus(Damage, MoveSpeed);
    }

    #region 챔피언
    public static void SetChampion(string name)
    {
        championsTr = GameObject.Find("Champions").GetComponent<Transform>();
        for (int i = 0; i < championsTr.childCount; i++)
        {
            championsTr.GetChild(i).gameObject.SetActive(false);
        }

        int enumCount = Enum.GetValues(typeof(ChampionType)).Length;
        for (int i = 0; i < enumCount; i++)
        {
            string curName = ((ChampionType)i).ToString();
            if (name == curName)
            {
                selectionChampion = championsTr.Find(curName).transform;
                selectionChampion.gameObject.SetActive(true);
                movePos = selectionChampion.position;
                animator = selectionChampion.GetComponent<Animator>();
                Debug.Log("현재 선택한 챔피언 : " + selectionChampion.name);
            }
        }
    }
    #endregion

    #region 마우스를 목표 위치에 클릭하여 이동
    private void UpdateMoveSetting()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (usingSkill) return;

            Plane zeroPlane = new Plane(Vector3.up, Vector3.zero);  //Y가 0인 평면을 가져옴. 즉, 높이가 0이 아니게 하려면 Vector3.zero를 바꿔주면 될 듯
            Ray ray = quarterViewCam.ScreenPointToRay(Input.mousePosition);
            if (zeroPlane.Raycast(ray, out float distance))
            {
                movePos = ray.origin + ray.direction * distance;
                movePos.y = selectionChampion.position.y;
                moveDirection = (movePos - selectionChampion.position).normalized;
                selectionChampion.forward = moveDirection;
            }
        }

        if (Vector3.Distance(movePos, selectionChampion.position) >= 0.05f) selectionChampion.position += MoveSpeed * Time.deltaTime * moveDirection;  //selectionChampion.Translate(MoveSpeed * Time.deltaTime * Vector3.forward);
        animator.SetBool(aniMove, Vector3.Distance(movePos, selectionChampion.position) >= 0.05f);
#else
        Vector3 right = new Vector3(quarterViewCam.transform.right.x, 0, quarterViewCam.transform.right.z).normalized;
        Vector3 forward = new Vector3(quarterViewCam.transform.forward.x, 0, quarterViewCam.transform.forward.z).normalized;

        Vector3 rightMovement = right * MoveSpeed * Time.deltaTime * joystick.Horizontal;
        Vector3 forwardMovement = forward * MoveSpeed * Time.deltaTime * joystick.Vertical;

        selectionChampion.position += forwardMovement;
        selectionChampion.position += rightMovement;
        selectionChampion.LookAt(selectionChampion.position + forwardMovement + rightMovement);

        if (joystick.Horizontal < 0 || joystick.Horizontal > 0 || joystick.Vertical < 0 || joystick.Vertical > 0) animator.SetBool(aniMove, true);
        else animator.SetBool(aniMove, false);
#endif
    }
    #endregion
}