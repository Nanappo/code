using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class Popup_Evaluation : PopupControl
{
    #region UI
    public GameObject EvaluationForm;
    public GameObject EvaluationForm_Popup;
    public GameObject EvaluationForm_PopupComplete;
    public TMP_InputField FormCategory_txt;
    public TMP_InputField FormPopup_txt;
    public TMP_Text EvaluationForm_DropdownLabel;
    public TMP_Dropdown EvaluationForm_Dropdown;
    public GameObject FormDropdown_warning;

    public GameObject EvaluationForm_Deep;
    public GameObject Finalcount_btn;
    public GameObject Save_btn;
    public GameObject Nocount_btn;
    public GameObject Finalcount_Popup;
    public GameObject FinalcountReturn_Popup;
    public GameObject Save_Popup;
    public GameObject Nocount_Popup;
    public GameObject NocountRetrun_Popup;
    public GameObject AlreadyCount_Popup;
    public Button Finalcount_componentbtn;
    public TMP_InputField TrainerOpinion;
    public TMP_Text TrainerOpinion_Placeholder;
    public List<TMP_Text> Actor = new List<TMP_Text>();
    public List<TMP_Text> EvForm = new List<TMP_Text>();
    public List<TMP_Text> Scenario = new List<TMP_Text>();
    public List<TMP_Text> Training = new List<TMP_Text>();
    public List<TMP_Text> Trainingitem = new List<TMP_Text>();
    public List<TMP_Text> Trainingitemdeep = new List<TMP_Text>();
    public List<TMP_Text> AssessmentAll = new List<TMP_Text>();
    public List<TMP_InputField> Assessment = new List<TMP_InputField>();

    #endregion

    private PopupManager popupManager;
    private List<string> dropdownList = new List<string>();
    private List<UID> CurrentUIDs = new List<UID>();
    private int currentEVUid;
    private bool isAssessment = false; //최종제출 버튼 활성화 유무를 위해 추가

    public struct UID
    {
        public int trs_uid, ac_uid, trt_uid, scn_uid;

        public UID(int _trs_uid, int _ac_uid, int _trt_uid, int _scn_uid)
        {
            this.trs_uid = _trs_uid;
            this.ac_uid = _ac_uid;
            this.trt_uid = _trt_uid;
            this.scn_uid = _scn_uid;
        }
    }
    public override void Open(bool _active)
    {
        base.Open(_active);

        popupManager = GameObject.FindObjectOfType<PopupManager>();
    }
    public override void Close()
    {
        CloseSetting();

        base.Close();
    }
    private void CloseSetting()
    {
        isAssessment = false;
        dropdownList.Clear();
        EvaluationForm_Dropdown.ClearOptions();
        CurrentUIDs.Clear();

        FormCategory_txt.text = "";
        FormPopup_txt.text = "";
        TrainerOpinion.text = "";
        TrainerOpinion_Placeholder.text = "추가 의견을 작성할 수 있습니다.";

        for (int i = 0; i < 5; i++)
        {
            Assessment[i].readOnly = false;
        }

        TrainerOpinion.readOnly = false;
        AssessmentAll[0].text = "0점";
        AssessmentAll[1].text = "0";

        EvaluationForm_Dropdown.value = 0;
        Finalcount_componentbtn.interactable = false;
        EvaluationForm.SetActive(false);
        EvaluationForm_Deep.SetActive(false);
        EvaluationForm_Popup.SetActive(false);
        EvaluationForm_PopupComplete.SetActive(false);
        FormDropdown_warning.SetActive(false);
        Finalcount_btn.SetActive(false);
        Nocount_btn.SetActive(false);
        Save_btn.SetActive(false);
        Finalcount_Popup.SetActive(false);
        FinalcountReturn_Popup.SetActive(false);
        Save_Popup.SetActive(false);
        Nocount_Popup.SetActive(false);
        NocountRetrun_Popup.SetActive(false);
        AlreadyCount_Popup.SetActive(false);
    }
    //isdeep : true -> 상세평가, false -> 평가서 불러오기 팝업
    //category : 0 -> 미평가, 1 -> 진행중 2 -> 제출완료
    //title : trs_uid
    private void DataSetting(bool _isDeep, int _category, string _title)
    {
        List<int> list = new List<int>();
        List<string> assessmentlist = new List<string>();
        list = popupManager.trSuidToUID(int.Parse(_title));
        CurrentUIDs.Add(new UID(list[0], list[1], list[2], list[3]));

        if (_isDeep)
        {
            currentEVUid = popupManager.GetEvUid(CurrentUIDs[0].trs_uid);
            int evsuid = popupManager.GetEvSUid(CurrentUIDs[0].trs_uid);
            assessmentlist = popupManager.GetEVScoreData(evsuid);
            EvaluationForm_Deep.SetActive(true);
            Nocount_btn.SetActive(true);

            DeepEvaluationSetting(_isDeep);

            for (int i = 0; i < 5; i++)
            {
                Assessment[i].text = assessmentlist[i];
                Assessment[i].readOnly = true;
            }
            AssessmentAll[0].text = assessmentlist[5] + "점";
            AssessmentAll[1].text = assessmentlist[5];

            if (assessmentlist[6] == "")
            {
                TrainerOpinion_Placeholder.text = "작성된 교관 의견이 없습니다.";
                TrainerOpinion.readOnly = true;
            }
            else
            {
                TrainerOpinion.text = assessmentlist[6];
                TrainerOpinion.readOnly = true;
            }
        }
        else
        {
            if (_category == 0)
            {
                dropdownList.Add("평가서를 선택해주세요.");
                dropdownList.AddRange(popupManager.GetEvaluationFormTitle(CurrentUIDs[0].trt_uid));
                dropdownList.Reverse(1, dropdownList.Count - 1);
                EvaluationForm.SetActive(true);
                FormCategory_txt.text = trTuidToString(CurrentUIDs[0].trt_uid);
                EvaluationForm_Dropdown.AddOptions(dropdownList);

                for (int i = 0; i < 5; i++)
                {
                    Assessment[i].text = "";
                }
                AssessmentAll[0].text = "0점";
                AssessmentAll[1].text = "0";
            }
            else
            {
                currentEVUid = popupManager.GetEvUid(CurrentUIDs[0].trs_uid);
                int evsuid = popupManager.GetEvSUid(CurrentUIDs[0].trs_uid);
                assessmentlist = popupManager.GetEVScoreData(evsuid);
                Finalcount_btn.SetActive(true);
                Save_btn.SetActive(true);

                DeepEvaluationSetting(_isDeep);

                for (int i = 0; i < 5; i++)
                {
                    if (assessmentlist[i] == "0")
                    {
                        Assessment[i].text = "";
                        continue;
                    }

                    Assessment[i].text = assessmentlist[i];
                }
                AssessmentAll[0].text = assessmentlist[5] + "점";
                AssessmentAll[1].text = assessmentlist[5];
                TrainerOpinion.text = assessmentlist[6];
            }
        }
    }
    public void UISetting(bool _isDeep, int _category, string _title)
    {
        DataSetting(_isDeep, _category, _title);
    }
    private void DeepEvaluationSetting(bool isDeep)
    {
        EvaluationForm_Deep.SetActive(true);
        isAssessment = true;

        SetActorData();
        SetScenarioData();
        SetEvFormData(isDeep);
        SetTrainingData();
        SetTrainingitemsData();
    }
    private void SetActorData()
    {
        List<string> list = new List<string>();

        list = popupManager.GetActorData(CurrentUIDs[0].ac_uid);
        for (int i = 0; i < list.Count; i++)
        {
            Actor[i].text = list[i];
        }
    }
    private void SetScenarioData()
    {
        List<string> list = new List<string>();

        list = popupManager.GetScenarioData(CurrentUIDs[0].scn_uid, 1);
        for (int i = 0; i < list.Count; i++)
        {
            Scenario[i].text = list[i];
        }
    }
    private void SetEvFormData(bool isDeep)
    {
        List<string> list = new List<string>();

        list = popupManager.GetEVData(currentEVUid);
        for (int i = 0; i < list.Count; i++)
        {
            EvForm[i].text = list[i];
        }

        if (isDeep)
        {
            int evs_uid = popupManager.GetEvSUid(CurrentUIDs[0].trs_uid);
            int evr_uid = popupManager.GetEVRUid(evs_uid);
            string ac_name = popupManager.UidToName(evr_uid);
            EvForm[2].text = ac_name;
        }
        else
        {
            EvForm[2].text = popupManager.EvToAcName_DBSearch(ManagerControl.Instance.ownerID);
        }
    }
    private void SetTrainingData()
    {
        List<string> list = new List<string>();

        list = popupManager.GetTrainingData(CurrentUIDs[0].trs_uid);
        for (int i = 0; i < list.Count; i++)
        {
            if (i == 0)
            {
                list[i] = StringToDate(list[i]);
            }

            Training[i].text = list[i];
        }
    }
    private void SetTrainingitemsData()
    {
        List<string> list = new List<string>();

        list = popupManager.GetEVDeepData(currentEVUid);

        for (int i = 0; i < Trainingitem.Count; i++)
        {
            Trainingitem[i].text = list[i];
            Trainingitemdeep[i].text = list[i + 5];
        }
    }
    private string trTuidToString(int _trtuid)
    {

        string res = "";
        switch (_trtuid)
        {
            case 1:
                res = "VR 훈련 평가서";
                break;
            case 2:
                res = "AR 훈련 평가서";
                break;
            case 3:
                res = "CBT 훈련 평가서";
                break;
        }

        return res;
    }
    private string DateTimeInsert()
    {
        string lastlogindate = DateTime.Now.ToString("yyyyMMdd");
        string lastlogintime = DateTime.Now.ToString("HHmmss");

        return lastlogindate + lastlogintime;
    }
    private string StringToDate(string _date)
    {
        DateTime date = DateTime.Parse(_date);
        return date.ToString("yyyy-MM-dd");
    }
    private bool InputAssessmentCheck(TMP_InputField _input)
    {
        string str = Regex.Replace(_input.text, @"[^0-9]", "");

        if (_input.text.Equals(str) == false)
        {
            _input.text = str;
            return false;
        }

        return true;
    }
    private bool FormDropdownCheck()
    {
        return EvaluationForm_Dropdown.value == 0 ? false : true;
    }
    private void Update()
    {
        if (isAssessment)
        {
            int cnt = 0;
            int sum = 0;

            for (int i = 0; i < Assessment.Count; i++)
            {
                if (Assessment[i].text != "" && InputAssessmentCheck(Assessment[i]))
                {
                    if (int.Parse(Assessment[i].text) > 20) Assessment[i].text = "20";
                    sum += int.Parse(Assessment[i].text);
                    cnt++;
                }
                else
                {
                    Finalcount_componentbtn.interactable = false;
                }
            }

            if (cnt == Assessment.Count)
            {
                AssessmentAll[0].text = sum.ToString() + "점";
                AssessmentAll[1].text = sum.ToString();
                Finalcount_componentbtn.interactable = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Assessment[0].isFocused)
                Assessment[1].ActivateInputField();

            else if (Assessment[1].isFocused)
                Assessment[2].ActivateInputField();

            else if (Assessment[2].isFocused)
                Assessment[3].ActivateInputField();

            else if (Assessment[3].isFocused)
                Assessment[4].ActivateInputField();

            else if (Assessment[4].isFocused)
                TrainerOpinion.ActivateInputField();

            else
                Assessment[0].ActivateInputField();
        }
    }
    #region FormButton
    public void Cancle_Btn()
    {
        popupManager.Close(ControlState.List);
    }
    public void FormPopupCancle_Btn()
    {
        EvaluationForm_Popup.SetActive(false);
    }
    public void EvaluationFormPopup_Btn()
    {
        if (!FormDropdownCheck())
        {
            FormDropdown_warning.SetActive(true);
            return;
        }

        FormDropdown_warning.SetActive(false);
        EvaluationForm_Popup.SetActive(true);
        FormPopup_txt.text = EvaluationForm_DropdownLabel.text;
    }
    public void EvaluationFormPopupComplete_Btn()
    {
        EvaluationForm_PopupComplete.SetActive(true);
        currentEVUid = popupManager.EVTitleToUid(FormPopup_txt.text);
    }
    public void MessagePopup_Complete()
    {
        EvaluationForm.SetActive(false);
        EvaluationForm_Popup.SetActive(false);
        EvaluationForm_PopupComplete.SetActive(false);

        DeepEvaluationSetting(false);
        Finalcount_btn.SetActive(true);
        Save_btn.SetActive(true);
    }
    #endregion
    #region FormDeepButton
    public void Save_Btn()
    {
        //이미시리즈 팝업
        int evs_uid = 0;
        int situation = popupManager.GetEvSituation(CurrentUIDs[0].trs_uid);
        int evr_uid = popupManager.IDToUid(ManagerControl.Instance.ownerID);
        string date = DateTimeInsert();
        int sum = 0;

        for (int i = 0; i < Assessment.Count; i++)
        {
            if (Assessment[i].text == "")
            {
                Assessment[i].text = "0";
                sum += 0;
                continue;
            }

            sum += int.Parse(Assessment[i].text);
        }

        AssessmentAll[0].text = sum.ToString() + "점";
        AssessmentAll[1].text = sum.ToString();
        TrainerOpinion.text = TrainerOpinion.text.Replace("\\", string.Empty);

        if (situation == 0)
        {
            evs_uid = popupManager.EvScoreUidMax_DBSearch() + 1;
            popupManager.SetAssessment_DBInsert(evs_uid, CurrentUIDs[0].ac_uid, evr_uid, currentEVUid, int.Parse(Assessment[0].text), int.Parse(Assessment[1].text),
                int.Parse(Assessment[2].text), int.Parse(Assessment[3].text), int.Parse(Assessment[4].text), sum, TrainerOpinion.text, date);
        }
        else if (situation == 1)
        {
            AlreadyCount_Popup.SetActive(true);
            return;
        }
        else if (situation == 2)
        {
            evs_uid = popupManager.GetEvSUid(CurrentUIDs[0].trs_uid);

            popupManager.SetAssessment_DBUpdate(evs_uid, int.Parse(Assessment[0].text), int.Parse(Assessment[1].text),
                int.Parse(Assessment[2].text), int.Parse(Assessment[3].text), int.Parse(Assessment[4].text), sum, TrainerOpinion.text, date, evr_uid);
        }

        popupManager.SetAssessment_DBUpdate(CurrentUIDs[0].trs_uid, 2, evs_uid);
        Save_Popup.SetActive(true);
    }
    public void NoCount_Btn()
    {
        Nocount_Popup.SetActive(true);
    }
    public void NoCountPopup_Btn()
    {
        popupManager.SetScoreYN_DBUpdate(CurrentUIDs[0].trs_uid, 0);
        NocountRetrun_Popup.SetActive(true);
    }
    public void NoCountPopupCancle_Btn()
    {
        Nocount_Popup.SetActive(false);
    }
    public void FinalCount_Btn()
    {
        int situation = popupManager.GetEvSituation(CurrentUIDs[0].trs_uid);

        if (situation == 1)
        {
            AlreadyCount_Popup.SetActive(true);
            return;
        }

        Finalcount_Popup.SetActive(true);
    }
    public void FinalCountPopupCancle_Btn()
    {
        Finalcount_Popup.SetActive(false);
    }
    public void FinalCountPopup_Btn()
    {
        int situation = popupManager.GetEvSituation(CurrentUIDs[0].trs_uid);
        string date = DateTimeInsert();
        int sum = 0;

        for (int i = 0; i < Assessment.Count; i++)
        {
            if (Assessment[i].text == "")
            {
                sum += 0;
                continue;
            }

            sum += int.Parse(Assessment[i].text);
        }

        TrainerOpinion.text = TrainerOpinion.text.Replace("\\", string.Empty);

        if (situation == 0)
        {
            int evr_uid = popupManager.IDToUid(ManagerControl.Instance.ownerID);
            int evs_uid = popupManager.EvScoreUidMax_DBSearch() + 1;

            popupManager.SetAssessment_DBInsert(evs_uid, CurrentUIDs[0].ac_uid, evr_uid, currentEVUid, int.Parse(Assessment[0].text), int.Parse(Assessment[1].text),
                int.Parse(Assessment[2].text), int.Parse(Assessment[3].text), int.Parse(Assessment[4].text), sum, TrainerOpinion.text, date);

            popupManager.SetAssessment_DBUpdate(CurrentUIDs[0].trs_uid, 1, evs_uid);
        }
        else if (situation == 2)
        {
            int evr_uid = popupManager.IDToUid(ManagerControl.Instance.ownerID);
            int evs_uid = popupManager.GetEvSUid(CurrentUIDs[0].trs_uid);

            popupManager.SetAssessment_DBUpdate(evs_uid, int.Parse(Assessment[0].text), int.Parse(Assessment[1].text),
                int.Parse(Assessment[2].text), int.Parse(Assessment[3].text), int.Parse(Assessment[4].text), sum, TrainerOpinion.text, date, evr_uid);

            popupManager.SetAssessment_DBUpdate(CurrentUIDs[0].trs_uid, 1, evs_uid);
        }

        AssessmentAll[0].text = sum.ToString() + "점";
        AssessmentAll[1].text = sum.ToString();
        FinalcountReturn_Popup.SetActive(true);
    }
    #endregion
}