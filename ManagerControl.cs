using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using E2C;
using System.Data;

public enum ControlState
{
    None = 0,
    Intro = 1,
    Login = 2,
    Popup = 3,
    List = 4,
    Data = 5,
    Graph = 6,
    Network = 7,
    CSV = 8,
}

public class ManagerControl : MonoBehaviour
{
    private static ManagerControl instance;
    public static ManagerControl Instance
    {
        get { return instance; }
    }

    private ControlState _currnetState = ControlState.None;

    public ControlState currentState => _currnetState;
    public List<ComponentControl> managers = new List<ComponentControl>();

    public bool admin = false; //admin인지 교관인지 구분짓기 위한 변수 true = admin // false = 교관
    public bool loginCheck = false;
    public E2Chart graphChart;
    [HideInInspector] public DBManager dbManager;
    [HideInInspector] public GraphManager graphManager;
    [HideInInspector] public TCPNetwork networkManager;
    [HideInInspector] public CSVManager csvManager;
    [HideInInspector] public DateTime ownerLoginTime;
    [HideInInspector] public string ownerID;
    [HideInInspector] public string ownerPassword;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        managers.AddRange(gameObject.GetComponentsInChildren<ComponentControl>(true));

        //Unity Hierarchy 창 순서 위에 Enum값대로 꼭 지키기!!
        for (int i = 0; i < managers.Count; i++)
        {
            managers[i].Init(ControlState.Intro + i);
        }

        dbManager = managers.Find(x => x._myState == ControlState.Data) as DBManager;
        graphManager = managers.Find(x => x._myState == ControlState.Graph) as GraphManager;
        networkManager = managers.Find(x => x._myState == ControlState.Network) as TCPNetwork;
        csvManager = managers.Find(x => x._myState == ControlState.CSV) as CSVManager;
        ChangeState(ControlState.Intro);
    }

    public void ChangeState(ControlState _state)
    {
        _currnetState = _state;
        var targetState = managers.Find(x => x._myState == _state);

        switch (_state)
        {
            case ControlState.None:
                break;
            case ControlState.Intro:
            case ControlState.Login:
            case ControlState.List:
                targetState.gameObject.SetActive(true);
                targetState.Callback();
                break;
            case ControlState.Popup:
                break;
            default:
                Debug.LogError("Null ControlState");
                break;
        }
    }
    public void ReturnLogin(ControlState _state)
    {
        var targetState = managers.Find(x => x._myState == _state);

        switch (_state)
        {
            case ControlState.None:
                break;
            case ControlState.Login:
            case ControlState.List:
            case ControlState.Popup:
                targetState.Close(_state);
                break;
            default:
                break;
        }
    }

    public void OpenPopup(PopupState _state, ControlState _controlState = ControlState.Popup)
    {
        var popupManager = managers.Find(x => x._myState == ControlState.Popup) as PopupManager;

        if (popupManager == null) return;

        _currnetState = ControlState.Popup;
        popupManager.gameObject.SetActive(true);
        popupManager.Callback();
        popupManager.SelectPopup_Open(_state);
    }

    //여러개의 계정을 선택 후 삭제를 하거나 데이터를 보내야 할 때 사용
    public void TransferData_Popup(string[] _data, PopupState _state)
    {
        //Debug.Log("_data : "+ _data[0]);
        var popupManager = managers.Find(x => x._myState == ControlState.Popup) as PopupManager;
        if (popupManager == null) return;

        popupManager.Transfer_Data(_data, _state);
    }

    //OpenPopup호출 후 이것도 호출로 Main여부, Title명이나 기존 옵션명, 바꿀 옵션명을 넘겨줘야함
    public void TitleReturn_Popup(bool _isMain, PopupState _state, int _category = 0, string _title = "", string _newname = "")
    {
        var popupManager = managers.Find(x => x._myState == ControlState.Popup) as PopupManager;
        if (popupManager == null) return;

        popupManager.ReturnTitle_Popup(_isMain, _state, _category, _title, _newname);
    }
    #region Graph
    //Graph tooltiptitle Setting
    public string GraphTooltipTitle()
    {
        return graphManager.TooltipTitle_txt();
    }
    //GraphSetting -> Graph열어줄 때 호출
    public void OnGraph(int _case, int _tap = 0, string _id = "")
    {
        graphManager.OnGraph(_case, _tap, _id);
    }
    public bool GetLegend()
    {
        return graphManager.GetLegend();
    }
    public void GetGraphData(int _scenariotype, int _num)
    {
        graphManager.OnButton(_scenariotype, _num);
    }
    #endregion
    #region Network
    public void RoomCreate_Req(string _rid, string _name, string _person, string _scnuid, string _situation)
    {
        networkManager.RoomCreate_Req(_rid, _name, _person, _scnuid, _situation);
    }
    public void RoomDelete_Req(string _rid)
    {
        networkManager.RoomDelete_Req(_rid);
    }
    public void StartGame_Req(string _rid)
    {
        networkManager.StartGame_Req(_rid);
    }
    public void EndGame_Req(string _rid)
    {
        networkManager.EndGame_Req(_rid);
    }
    #endregion
    #region CSV
    /// <summary>
    /// case 0: 관리자 계정조회
    /// case 1: 훈련생 계정조회
    /// case 2: 교육내역조회
    /// case 3: 문제 조회 및 등록
    /// </summary>
    public void UnityToExcelExport(int _case, List<ListDataBase> _list)
    {
        switch (_case)
        {
            case 0:
                csvManager.SetDataAccountTrainer_WriteCSV(_list);
                break;
            case 1:
                csvManager.SetDataAccountStudent_WriteCSV(_list);
                break;
            case 2:
                csvManager.SetDataEducationHistoryList_WriteCSV(_list);
                break;
            case 3:
                csvManager.SetDataQuizBank_WriteCSV(_list);
                break;
            default:
                break;
        }
    }
    #endregion
    //nhkim
    public void SendGraphDataToEducationInquiry(List<GraphSelect> _graphDataList, int _type = 2)
    {
        var listManager = managers.Find(x => x._myState == ControlState.List) as ListManager;
        if (listManager == null) return;

        listManager.SearchingDataForEducationInquiry(_graphDataList, _type);
    }
    //팝업 버튼 클릭 시 그에 맞는 페이지 전환
    public void ChangeListPage()
    {
        var listManager = managers.Find(x => x._myState == ControlState.List) as ListManager;
        if (listManager == null) return;

        if (admin) //관리자
        {
            if (listManager.adminUIManager != null)
            {
                int pageNum = 0;
                pageNum = (int)AdminState.Inquiry;
                listManager.adminUIManager.ChangePageState(pageNum);
            }
        }
        else //교관
        {
            if (listManager.trainerUIManager != null)
            {
                int pageNum = 0;
                pageNum = listManager.trainerUIManager.currentTrainer == TrainerState.TRInquiry_Detail ? (int)TrainerState.TRInquiry : (int)TrainerState.AccInquiry;
                if (listManager.trainerUIManager.currentTrainer == TrainerState.TRInquiry_Detail)
                {
                    pageNum = (int)TrainerState.TRInquiry;
                    listManager.trainerUIManager.trRoomContainer.checkAgain = true;
                    listManager.trainerUIManager.ChangePageState(pageNum);
                }
                else
                {
                    pageNum = (int)TrainerState.AccInquiry;
                    listManager.trainerUIManager.listContainer.checkAgain = true;
                    listManager.trainerUIManager.ChangePageState(pageNum);
                }
            }
        }
    }

    //계정, 훈련방, 문제, 옵션 등록 완료시 필터 & 페이지네이션 초기화
    public void ResetPages()
    {
        var listManager = managers.Find(x => x._myState == ControlState.List) as ListManager;
        if (listManager == null) return;

        if (admin) //관리자
        {
            listManager.adminUIManager.listContainer.ResetCurrentPage();
            listManager.adminUIManager.listContainer.SetSavePage();
            listManager.adminUIManager.filterContainer.ResetFilter();
        }
        else //교관
        {
            listManager.trainerUIManager.filterContainer.ResetFilter();
            if (listManager.trainerUIManager.currentTrainer == TrainerState.TRInquiry)
            {
                listManager.trainerUIManager.trRoomContainer.ResetCurrentPage();
            }
            else
            {
                listManager.trainerUIManager.listContainer.ResetCurrentPage();
                listManager.trainerUIManager.listContainer.SetSavePage();
                listManager.trainerUIManager.listContainer.checkAgain = false;
            }
        }
    }


    #region Login
    //로그인 중복체크
    private int LoginState_DBUpdate(string _id)
    {
        string setdata = "SELECT login_yn FROM account WHERE id='" + _id + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = dbManager.SelectDB(setdata);

        int login_yn = 0;

        foreach (DataRow r in data.Tables[0].Rows)
        {
            login_yn = (int)r[0];
        }

        return login_yn;
    }
    //로그인 중복체크를 위해 DB값 갱신
    private void LoginState_DBUpdate(string _id, int _state)
    {
        string setdata = "UPDATE account SET login_yn=" + _state + " WHERE id='" + _id + "'";
        dbManager.Insert_UpdateDB(setdata);
    }
    //로그아웃을 위한 초기화
    public void LogoutReset()
    {
        var loginManager = managers.Find(x => x._myState == ControlState.Login) as LoginManager;
        var listManager = managers.Find(x => x._myState == ControlState.List) as ListManager;
        var popupManager = managers.Find(x => x._myState == ControlState.Popup) as PopupManager;

        if (listManager == null || popupManager == null) return;

        listManager.OnClickLogout();
        popupManager.Close(ControlState.Login);
        loginManager.logoutcomplete_Popup.SetActive(true);
    }
    #endregion
    private void Update()
    {
        if (loginCheck && LoginState_DBUpdate(ownerID) == 0)
        {
            loginCheck = false;
            LogoutReset();
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Q))
        {
            OpenPopup(PopupState.ApplicationQuit);
        }

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //ResetPages();
        //ownerID = "qwe";
        //ownerPassword = "qwe";
        //string[] str = new string[1];
        //str[0] = "1";
        //OpenPopup(PopupState.QuizBankChange);
        //TransferData_Popup(str, PopupState.QuizBankChange);
        //TitleReturn_Popup(false, PopupState.DeleteChange);
        //OnGraph(2, 0, "aww");
        //ChangeListPage();
        //}
    }
    private void OnDestroy()
    {
        LoginState_DBUpdate(ownerID, 0);
    }
}
