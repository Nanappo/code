using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupState
{
    None = 0,
    Account = 1,
    Delete = 2,
    DeleteChange = 3,
    Option = 4,
    Password = 5,
    Evaluation = 6,
    TrainingRoom = 7,
    EvaluationForm = 8,
    EvaluationFormChange = 9,
    QuizBank = 10,
    QuizBankChange = 11,
    QuizBankDelete = 12,
    ApplicationQuit = 13,
}

public class PopupManager : ComponentControl
{
    public List<PopupControl> containers = new List<PopupControl>();
    public bool isAdmin = false;
    public PopupState currentPopupState => _currentPopupState;

    private PopupDBControl popupDBControl;
    private PopupState _currentPopupState = PopupState.None;

    public override void Init(ControlState _state)
    {
        base.Init(_state);

        popupDBControl = GetComponent<PopupDBControl>();
        containers.AddRange(gameObject.GetComponentsInChildren<PopupControl>(true));

        for (int i = 0; i < containers.Count; i++)
        {
            containers[i].Init(PopupState.Account + i);
        }
    }
    public override void Open()
    {
        base.Open();

        isAdmin = ManagerControl.Instance.admin;
        popupDBControl.Init();
    }

    public override void Close(ControlState _state)
    {
        _currentPopupState = PopupState.None;
        foreach (PopupControl popup in containers)
        {
            popup.Close();
        }

        base.Close(_state);
    }

    public void ReturnTitle_Popup(bool _isMain, PopupState _state, int _category, string _title, string _newname)
    {
        var targetState = containers.Find(x => x._myState == _state);
        if (targetState == null) return;

        switch (_state)
        {
            case PopupState.None:
            case PopupState.Account:
            case PopupState.Password:
            case PopupState.TrainingRoom:
            case PopupState.EvaluationForm:
            case PopupState.EvaluationFormChange:
            case PopupState.QuizBank:
            case PopupState.QuizBankChange:
            case PopupState.QuizBankDelete:
            case PopupState.ApplicationQuit:
                break;
            case PopupState.DeleteChange:
                Popup_DeleteChange deleteChange = targetState as Popup_DeleteChange;
                deleteChange.UISetting(_isMain);
                break;
            case PopupState.Delete:
                Popup_Delete delete = targetState as Popup_Delete;
                delete.UISetting(_isMain);
                break;
            case PopupState.Option:
                Popup_Option option = targetState as Popup_Option;
                option.InputTitle(_isMain, _category, _title, _newname);
                break;
            case PopupState.Evaluation:
                Popup_Evaluation evaluation = targetState as Popup_Evaluation;
                evaluation.UISetting(_isMain, _category, _title);
                break;
            default:
                Debug.LogError("Null PopupState");
                break;
        }
    }

    public void SelectPopup_Open(PopupState _state)
    {
        if (_state != PopupState.ApplicationQuit) _currentPopupState = _state;

        var targetState = containers.Find(x => x._myState == _state);

        switch (_state)
        {
            case PopupState.None:
            case PopupState.Account:
            case PopupState.Delete:
            case PopupState.DeleteChange:
            case PopupState.Option:
            case PopupState.Password:
            case PopupState.Evaluation:
            case PopupState.TrainingRoom:
            case PopupState.EvaluationForm:
            case PopupState.EvaluationFormChange:
            case PopupState.QuizBank:
            case PopupState.QuizBankChange:
            case PopupState.QuizBankDelete:
            case PopupState.ApplicationQuit:
                targetState.gameObject.SetActive(true);
                targetState.Callback(isAdmin);
                break;
            default:
                Debug.LogError("Null PopupState");
                break;
        }
    }

    public void Transfer_Data(string[] _data, PopupState _state)
    {
        var targetState = containers.Find(x => x._myState == _state);

        switch (_state)
        {
            case PopupState.None:
            case PopupState.Account:
            case PopupState.Delete:
            case PopupState.DeleteChange:
            case PopupState.Option:
            case PopupState.Password:
            case PopupState.Evaluation:
            case PopupState.TrainingRoom:
            case PopupState.EvaluationForm:
            case PopupState.EvaluationFormChange:
            case PopupState.QuizBank:
            case PopupState.QuizBankChange:
            case PopupState.QuizBankDelete:
            case PopupState.ApplicationQuit:
                targetState.TransferData(_data);
                break;
            default:
                Debug.LogError("Null PopupState");
                break;
        }
    }
    #region DB
    public int TrainingID_DBSearch()
    {
        return popupDBControl.SearchTrainingID();
    }
    public string StudentIDCheck(string _trainingid)
    {
        return popupDBControl.StudentIDCheck(_trainingid);
    }
    public int Loginuid_DBSearch()
    {
        return popupDBControl.Search_uid();
    }
    public int Masteruid_DBSearch(string _id)
    {
        return popupDBControl.Search_Masteruid(_id);
    }
    public int Optionuid_DBSearch(string _optionname, int _type)
    {
        return popupDBControl.Search_Optionuid(_optionname, _type);
    }
    public string EvToAcName_DBSearch(string _id)
    {
        return popupDBControl.SearchForm_trainerName(_id);
    }
    public bool ID_DBSearch(string _id)
    {
        return popupDBControl.SearchID(_id);
    }
    public bool FormName_DBSearch(string _name)
    {
        return popupDBControl.SearchFormName(_name);
    }
    public bool DBInsertResult()
    {
        return popupDBControl.InsertComplete_Result();
    }
    public void Account_DBInsert(string _name, string _id, string _password, bool _admin, string _firstlogin, int _type, int _arid = -1, int _birthday = 0, int _gender = 1, int _masteruid = -1)
    {
        popupDBControl.MakeID_InDB(_name, _id, _password, _admin, _firstlogin, _type, _arid, _birthday, _gender, _masteruid);
    }
    public void Evaluation_DBInsert(string _name, string _id, int _truid, string _title, string _regdate, string _item1, string _item2,
        string _item3, string _item4, string _item5, string _item_d1, string _item_d2, string _item_d3, string _item_d4, string _item_d5)
    {
        popupDBControl.MakeForm_InDB(_name, _id, _truid, _title, _regdate, _item1, _item2,
        _item3, _item4, _item5, _item_d1, _item_d2, _item_d3, _item_d4, _item_d5);
    }
    public List<AccountSelect> AccountData_DBSearch(string _data)
    {
        return popupDBControl.SearchData_Account(_data);
    }
    public List<DeleteSelect> DeleteData_DBSearch(string _data)
    {
        return popupDBControl.SearchData_Delete(_data);
    }
    public List<string> OptionData_DBSearch(int _num)
    {
        return popupDBControl.SearchData_Option(_num);
    }
    public List<EvaluationChangeSelect> Evaluation_DBSearch(string _title)
    {
        return popupDBControl.SearchData_Evaluation(_title);
    }
    public void DeleteData_DBInset(string _id, string _deldate, int _deletereason_uid, string _deepreason)
    {
        popupDBControl.SetData_Delete(_id, _deldate, _deletereason_uid, _deepreason);
    }
    public void Data_DBTransfer(int _arid, string _name, string _id, string _password, string _firstlogin, string _lastlogin, string _deletetime, string _dropreason, string _deepreason)
    {
        popupDBControl.Transfer_DBData(_arid, _name, _id, _password, _firstlogin, _lastlogin, _deletetime, _dropreason, _deepreason);
    }
    public void Data_DBDelete(string _data)
    {
        popupDBControl.Delete_DBData(_data);
    }
    public void DeleteData_DBUpdate(string _reason, string _deepreason, string _input, int _type)
    {
        popupDBControl.DeleteReason_DBUpdate(_reason, _deepreason, _input, _type);
    }
    public void Option_DBChange(int _optionuid, string _changename, string _changedate)
    {
        popupDBControl.OptionName_Change(_optionuid, _changename, _changedate);
    }
    public void Option_DBAdd(int _num, string _category, int _use, string _changedate)
    {
        popupDBControl.OptionName_Add(_num, _category, _use, _changedate);
    }
    public void ChangePassword_DBUpdate(string _password, string _id)
    {
        popupDBControl.ChangePassword(_password, _id);
    }
    public void EvaluationChange_DBUpdate(string _title, int _usecategory)
    {
        popupDBControl.UpdateForm_InDB(_title, _usecategory);
    }
    public List<int> trSuidToUID(int _trsuid)
    {
        return popupDBControl.trSuidToUID(_trsuid);
    }
    public List<string> GetEvaluationFormTitle(int _trtuid)
    {
        return popupDBControl.GetEvaluationFormTitle(_trtuid);
    }
    public int EVTitleToUid(string _title)
    {
        return popupDBControl.EVTitleToUid(_title);
    }
    public List<string> GetActorData(int _acuid)
    {
        return popupDBControl.GetActorData(_acuid);
    }
    //_type -> 1 : 교육내역조회 훈련상세평가
    //_type -> 2 : 훈련방 등록
    public List<string> GetScenarioData(int _scnuid, int _type)
    {
        return popupDBControl.GetScenarioData(_scnuid, _type);
    }
    public List<string> GetEVData(int _evuid)
    {
        return popupDBControl.GetEVData(_evuid);
    }
    public List<string> GetEVDeepData(int _evuid)
    {
        return popupDBControl.GetEVDeepData(_evuid);
    }
    public List<string> GetTrainingData(int _trsuid)
    {
        return popupDBControl.GetTrainingData(_trsuid);
    }
    public List<string> GetEVScoreData(int _evsuid)
    {
        return popupDBControl.GetEVScoreData(_evsuid);
    }
    public int IDToUid(string _id)
    {
        return popupDBControl.IDToUid(_id);
    }
    public string UidToName(int _uid)
    {
        return popupDBControl.UidToName(_uid);
    }
    public int EvScoreUidMax_DBSearch()
    {
        return popupDBControl.Search_evuid();
    }
    public int GetEvUid(int _trs_uid)
    {
        return popupDBControl.GetEvUid(_trs_uid);
    }
    public int GetEvSUid(int _trs_uid)
    {
        return popupDBControl.GetEvSUid(_trs_uid);
    }
    public int GetEVRUid(int _evs_uid)
    {
        return popupDBControl.GetEVRUid(_evs_uid);
    }
    public void SetAssessment_DBInsert(int _evs_uid, int _evd_uid, int _evr_uid, int _ev_uid, int _grade1, int _grade2, int _grade3, int _grade4, int _grade5, int _gradeall, string _memo, string _reg_date)
    {
        popupDBControl.SetAssessment_DBInsert(_evs_uid, _evd_uid, _evr_uid, _ev_uid, _grade1, _grade2, _grade3, _grade4, _grade5, _gradeall, _memo, _reg_date);
    }
    public void SetAssessment_DBUpdate(int _evs_uid, int _grade1, int _grade2, int _grade3, int _grade4, int _grade5, int _gradeall, string _memo, string _reg_date, int _evr_uid)
    {
        popupDBControl.SetAssessment_DBUpdate(_evs_uid, _grade1, _grade2, _grade3, _grade4, _grade5, _gradeall, _memo, _reg_date, _evr_uid);
    }
    public void SetAssessment_DBUpdate(int _trs_uid, int _state, int _evs_uid)
    {
        popupDBControl.SetAssessment_DBUpdate(_trs_uid, _state, _evs_uid);
    }
    public void SetScoreYN_DBUpdate(int _trs_uid, int _yn)
    {
        popupDBControl.SetScoreYN_DBUpdate(_trs_uid, _yn);
    }
    public int GetEvSituation(int _trs_uid)
    {
        return popupDBControl.GetEvSituation(_trs_uid);
    }
    //type -> trT_uid
    //1 -> VR
    //2 -> AR
    public List<string> GetScenarioTitleList(int _type)
    {
        return popupDBControl.GetScenarioTitleList(_type);
    }
    public int GetScenarioUID(string _title)
    {
        return popupDBControl.GetScenarioUID(_title);
    }
    public string GetScenarioTitle(int _scnuid)
    {
        return popupDBControl.GetScenarioTitle(_scnuid);
    }
    public void SetRoom_DBInsert(int _acuid, int _scnuid, string _regdate, string _title)
    {
        popupDBControl.SetRoom_DBInsert(_acuid, _scnuid, _regdate, _title);
    }
    public void SetRoomData_TrainingDelete(int _uid, string _deldate, int _deletereason_uid, string _deepreason)
    {
        popupDBControl.SetRoomData_TrainingDelete(_uid, _deldate, _deletereason_uid, _deepreason);
    }
    public List<string> GetRoomDeleteReason(int _uid)
    {
        return popupDBControl.GetRoomDeleteReason(_uid);
    }
    public void SetRoomDelete_DBUpdate(string _reason, string _deepreason, int _r_uid, int _type)
    {
        popupDBControl.SetRoomDelete_DBUpdate(_reason, _deepreason, _r_uid, _type);
    }
    public List<string> GetRoomData()
    {
        return popupDBControl.GetRoomData();
    }
    public void QuizBank_DBInsert(int _trtuid, int _scnuid, string _quiztxt, int _quizox, string _date)
    {
        popupDBControl.QuizBank_DBInsert(_trtuid, _scnuid, _quiztxt, _quizox, _date);
    }
    public void SetQuizBank_DBUpdate(int _quizuid, string _quiztxt, int _quizox)
    {
        popupDBControl.SetQuizBank_DBUpdate(_quizuid, _quiztxt, _quizox);
    }
    public List<QuizBankSelect> GetQuizBankData(int _quizuid)
    {
        return popupDBControl.GetQuizBankData(_quizuid);
    }
    public void QuizBankDelete_DBData(int _quizuid)
    {
        popupDBControl.QuizBankDelete_DBData(_quizuid);
    }
    #endregion
    private void OnDestroy()
    {
        _currentPopupState = PopupState.None;
        Close(ControlState.None);
    }
}
