using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using UnityEngine;

public class PopupDBControl : MonoBehaviour
{
    private PopupManager popupManager;
    private DBManager ManagerDB;

    public void Init()
    {
        popupManager = GetComponent<PopupManager>();
        ManagerDB = ManagerControl.Instance.dbManager;
    }

    #region Account
    public void MakeID_InDB(string _name, string _id, string _password, bool _admin, string _firstlogin, int _type, int _arid, int _birthday, int _gender, int _master_uid)
    {
        int _tut_yn = 0;
        int _del_yn = 1;
        string makeid_str = "";

        if (_admin)
        {
            //makeid_str = "INSERT INTO login_table(ac_uid,id,pwd,lastlogin,name,firstlogin,type) VALUES(" + _uid + ",'" + _id + "','" + _password + "','" + _firstlogin + "','" + _name + "','" + _firstlogin + "'," + _type + ")";
            makeid_str = "INSERT INTO account(id,password,logout_date,name,reg_date,type,tut_yn,del_yn) VALUES('" + _id + "','" + _password + "','" + _firstlogin + "','" + _name + "','" + _firstlogin + "'," + _type + "," + _tut_yn + "," + _del_yn + ")";
        }
        else
        {
            //makeid_str = "INSERT INTO login_table(ac_uid,id,pwd,lastlogin,name,type,arid,firstlogin,birthday,gender) VALUES(" + _uid + ",'" + _id + "','" + _password + "','" + _firstlogin + "','" + _name + "'," + _type + "," + _arid + ",'" + _firstlogin + "','" + _birthday + "'," + _gender + ")";
            makeid_str = "INSERT INTO account(id,password,logout_date,name,type,id_training,reg_date,birth_date,gender,tut_yn,master_uid,del_yn) VALUES('" + _id + "','" + _password + "','" + _firstlogin + "','" + _name + "'," + _type + "," + _arid + ",'" + _firstlogin + "'," + _birthday + "," + _gender + "," + _tut_yn + "," + _master_uid + "," + _del_yn + ")";
        }

        ManagerDB.Insert_UpdateDB(makeid_str);
    }

    public bool InsertComplete_Result()
    {
        return ManagerDB.InsertResult();
    }
    public bool SearchID(string _idtxt)
    {
        //string searchid_str = "SELECT * FROM login_table WHERE id = '" + _idtxt + "'";
        string searchid_str = "SELECT * FROM account WHERE id = '" + _idtxt + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchid_str);

        if (data.Tables[0].Rows.Count >= 1)
        {
            return false;
        }

        return true;
    }

    public int SearchTrainingID()
    {
        int cnt = 0;
        //string searcharid_str = "SELECT MAX(arid) FROM login_table";
        string searcharid_str = "SELECT MAX(id_training) FROM account";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searcharid_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            cnt = (int)r[0];
        }

        return cnt;
    }
    public int Search_uid()
    {
        int cnt = 0;
        //string searcharid_str = "SELECT MAX(ac_uid) FROM login_table";
        string searcharid_str = "SELECT MAX(ac_uid) FROM account";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searcharid_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            cnt = (int)r[0];
        }

        return cnt;
    }
    public int Search_Masteruid(string _id)
    {
        int cnt = 0;
        //string searcharid_str = "SELECT MAX(ac_uid) FROM login_table";
        string searcharid_str = "SELECT * FROM account WHERE id = '" + _id + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searcharid_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            cnt = (int)r[0];
        }

        return cnt;
    }
    public int Search_Optionuid(string _name, int _type)
    {
        int cnt = 0;
        string searchname_str = "SELECT * FROM option WHERE name = '" + _name + "' AND type=" + _type + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchname_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            cnt = (int)r[0];
        }

        return cnt;
    }
    public List<AccountSelect> SearchData_Account(string _data)
    {
        //string searchid_str = "SELECT * FROM login_table WHERE id = '" + _data + "'";
        string searchid_str = "SELECT * FROM account WHERE id = '" + _data + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchid_str);

        List<AccountSelect> list = new List<AccountSelect>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            //list.Add(new AccountSelect(r[4].ToString(), r[1].ToString(), (DateTime)r[3], (DateTime)r[6], (int)r[5], r[2].ToString(), (int)r[0], (int)r[8], r[7].ToString()));
            list.Add(new AccountSelect(r[2].ToString(), r[3].ToString(), (DateTime)r[9], (DateTime)r[7], (int)r[5], r[4].ToString(), (int)r[0], (int)r[10], r[15].ToString()));
        }

        return list;
    }
    public string StudentIDCheck(string _trainingid)
    {
        string data_str = "SELECT EXISTS(select id from account where id='" + _trainingid + "') as success";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(data_str);

        string res = "";

        foreach (DataRow r in data.Tables[0].Rows)
        {
            res = r[0].ToString();
        }

        return res;
    }
    #endregion
    #region Delete
    public void SetData_Delete(string _id, string _deldate, int _deletereason_uid, string _deepreason)
    {
        string setdata_str = "UPDATE account SET del_yn=0, del_date='" + _deldate + "', del_reason1=" + _deletereason_uid + ", del_reason2='" + _deepreason + "' WHERE id = '" + _id + "'";

        ManagerDB.Insert_UpdateDB(setdata_str);
    }
    public void Delete_DBData(string _data)
    {
        string delete_str = "DELETE FROM login_table WHERE id='" + _data + "'";

        ManagerDB.DeleteDB(delete_str);
    }
    public void Transfer_DBData(int _arid, string _name, string _id, string _password, string _firstlogin, string _lastlogin, string _deletetime, string _dropreason, string _deepreason)
    {
        string makeid_str =
            "INSERT INTO delete_table(id,pwd,lastlogin,name,arid,firstlogin,deletetime,deletereason,deepreason) VALUES('" + _id + "','" + _password + "','" + _lastlogin + "','" + _name + "'," + _arid + ",'" + _firstlogin + "','" + _deletetime + "','" + _dropreason + "','" + _deepreason + "');";

        ManagerDB.Insert_UpdateDB(makeid_str);
    }
    public void DeleteReason_DBUpdate(string _reason, string _deepreason, string _input, int _type)
    {
        int uid = Search_Optionuid(_reason, _type);

        //string update_str = "UPDATE delete_table SET deletereason='" + _reason + "', deepreason='" + _deepreason + "' WHERE id='" + _input + "'";
        string update_str = "UPDATE account SET del_reason1=" + uid + ", del_reason2='" + _deepreason + "' WHERE id='" + _input + "'";

        ManagerDB.Insert_UpdateDB(update_str);
    }
    public List<DeleteSelect> SearchData_Delete(string _data)
    {
        int optionuid = 0;
        string res = "";
        //string searchid_str = "SELECT * FROM delete_table WHERE id = '" + _data + "'";
        string searchid_str = "SELECT * FROM account WHERE id = '" + _data + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchid_str);

        List<DeleteSelect> list = new List<DeleteSelect>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            //list.Add(new DeleteSelect(r[2].ToString(), r[0].ToString(), (DateTime)r[4], (DateTime)r[3], (int)r[8], r[9].ToString(), r[1].ToString(), r[5].ToString(), r[6].ToString()));
            optionuid = (int)r[12];
            res = OptionuidToOptionName(optionuid);
            list.Add(new DeleteSelect(r[2].ToString(), r[3].ToString(), (DateTime)r[8], (DateTime)r[7], (int)r[5], r[4].ToString(), res, r[13].ToString(), (int)r[10], r[15].ToString(), (int)r[0]));
        }

        return list;
    }
    private string OptionuidToOptionName(int _uid)
    {
        string res = "";
        string data_str = "SELECT * FROM option WHERE op_uid=" + _uid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(data_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            res = r[2].ToString();
        }
        return res;
    }
    #endregion
    #region Option
    public List<string> SearchData_Option(int _num)
    {
        //string searchcategory_str = "SELECT * FROM option_table WHERE sidecase = " + _num + " AND usecategory=1";
        string searchcategory_str = "SELECT * FROM option WHERE type = " + _num + " AND useCategory=1";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchcategory_str);

        List<string> list = new List<string>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            //list.Add(r[1].ToString());
            list.Add(r[2].ToString());
        }

        return list;
    }
    public int SearchMax()
    {
        int cnt = 0;
        string opcount = GetOP_UidCount();

        if (opcount == "0")
        {
            return 0;
        }

        //string searchmax_str = "SELECT MAX(num) FROM option_table";
        string searchmax_str = "SELECT MAX(op_uid) FROM option";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchmax_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            cnt = (int)r[0];
        }

        return cnt;
    }
    private string GetOP_UidCount()
    {
        string cnt = "";
        string searchopid_str = "SELECT COUNT(op_uid) FROM option";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchopid_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            cnt = r[0].ToString();
        }
        return cnt;
    }
    public void ColumnName_Change(string _name, string _changename)
    {
        string namechange = "alter table option_table change " + _name + " " + _changename + " VARCHAR(45)";

        ManagerDB.Columns_UpdateDB(namechange);
    }
    public void ColumnName_Add(string _name)
    {
        string namechange = "alter table option_table add column " + _name + " VARCHAR(45)";

        ManagerDB.Columns_UpdateDB(namechange);
    }
    public void OptionName_Add(int _num, string _category, int _use, string _changedate)
    {
        int count = SearchMax() + 1;
        //string optionadd = "INSERT INTO option_table() VALUES(" + count + ",'" + _category + "'," + _use + ",'" + _changedate + "'," + _num + ")";
        string optionadd = "INSERT INTO option(op_uid,type, name, useCategory, edit_date) VALUES(" + count + "," + _num + ", '" + _category + "', " + _use + ", '" + _changedate + "')";

        ManagerDB.Insert_UpdateDB(optionadd);
    }
    public void OptionName_Change(int _optionuid, string _changename, string _changedate)
    {
        string optionchange = "UPDATE option SET name='" + _changename + "', edit_date='" + _changedate + "' WHERE op_uid=" + _optionuid + "";

        ManagerDB.Insert_UpdateDB(optionchange);
    }
    #endregion
    #region Password
    public void ChangePassword(string _password, string _id)
    {
        //string passwordchange = "UPDATE login_table SET pwd='" + _password + "' WHERE id='" + _id + "'";
        string passwordchange = "UPDATE account SET password='" + _password + "' WHERE id='" + _id + "'";

        ManagerDB.Insert_UpdateDB(passwordchange);
    }
    #endregion
    #region Evaluation Form
    public bool SearchFormName(string _nametxt)
    {
        //string searchid_str = "SELECT * FROM evaluation_table WHERE title = '" + _nametxt + "'";
        string searchid_str = "SELECT * FROM ev WHERE title = '" + _nametxt + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchid_str);

        if (data.Tables[0].Rows.Count >= 1)
        {
            return false;
        }

        return true;
    }
    public string SearchForm_trainerName(string _id)
    {
        string name = "";
        //string searchid_str = "SELECT * FROM login_table WHERE id = '" + _id + "'";
        string searchid_str = "SELECT * FROM account WHERE id = '" + _id + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchid_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            //name = r[4].ToString();
            name = r[2].ToString();
        }

        return name;
    }
    public void MakeForm_InDB(string _name, string _id, int _truid, string _title, string _regdate, string _item1, string _item2,
        string _item3, string _item4, string _item5, string _item_d1, string _item_d2, string _item_d3, string _item_d4, string _item_d5)
    {
        string makeform_str = "";
        int usecategory = 1;

        //makeform_str =
        //    "INSERT INTO evaluation_table() VALUES("+_evuid+",'"+_name+"','"+_id+"'," + _truid + ",'" + _title + "','" + _regdate + "'," + usecategory + ",'" + _item1 + "','" + _item2 + "','" + _item3 + "','" + _item4 + "','" + _item5 + "','" + _item_d1 + "','" + _item_d2 + "','" + _item_d3 + "','" + _item_d4 + "','" + _item_d5 + "')";
        makeform_str =
            "INSERT INTO ev(trT_uid,title,reg_date,useCategory,item1,item2,item3,item4,item5,item_d1,item_d2,item_d3,item_d4,item_d5,evr_name,evr_id) VALUES(" + _truid + ",'" + _title + "','" + _regdate + "'," + usecategory + ",'" + _item1 + "','" + _item2 + "','" + _item3 + "','" + _item4 + "','" + _item5 + "','" + _item_d1 + "','" + _item_d2 + "','" + _item_d3 + "','" + _item_d4 + "','" + _item_d5 + "','" + _name + "','" + _id + "')";

        ManagerDB.Insert_UpdateDB(makeform_str);
    }
    public void UpdateForm_InDB(string _title, int _usecategory)
    {
        //string updateform_str = "UPDATE evaluation_table SET usecategory=" + _usecategory + " WHERE title='" + _title + "'";
        string updateform_str = "UPDATE ev SET useCategory=" + _usecategory + " WHERE title='" + _title + "'";

        ManagerDB.Insert_UpdateDB(updateform_str);
    }
    public List<EvaluationChangeSelect> SearchData_Evaluation(string _title)
    {
        //string datastring = "SELECT * FROM evaluation_table WHERE title='" + _title + "'";
        string datastring = "SELECT * FROM ev WHERE title='" + _title + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<EvaluationChangeSelect> list = new List<EvaluationChangeSelect>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            //list.Add(new EvaluationChangeSelect((int)r[3], r[4].ToString(), r[7].ToString(), r[8].ToString(), r[9].ToString(), r[10].ToString(), r[11].ToString(),
            //    r[12].ToString(), r[13].ToString(), r[14].ToString(), r[15].ToString(), r[16].ToString()));
            list.Add(new EvaluationChangeSelect((int)r[1], r[2].ToString(), r[5].ToString(), r[6].ToString(), r[7].ToString(), r[8].ToString(), r[9].ToString(),
                r[10].ToString(), r[11].ToString(), r[12].ToString(), r[13].ToString(), r[14].ToString()));
        }

        return list;
    }
    #endregion
    #region Evaluation
    public List<int> trSuidToUID(int _trsuid)
    {
        //string datastring = "SELECT trT_uid FROM training_score WHERE trS_uid=" + _trsuid + "";
        string datastring = "SELECT * FROM training_score WHERE trS_uid=" + _trsuid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<int> list = new List<int>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            list.Add((int)r[0]);
            list.Add((int)r[1]);
            list.Add((int)r[2]);
            list.Add((int)r[3]);
        }

        return list;
    }
    public List<string> GetEvaluationFormTitle(int _trtuid)
    {
        string datastring = "SELECT title FROM ev WHERE trT_uid=" + _trtuid + " AND useCategory=1";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<string> list = new List<string>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            list.Add(r[0].ToString());
        }

        return list;
    }
    public int EVTitleToUid(string _title)
    {
        string datastring = "SELECT ev_uid FROM ev WHERE title='" + _title + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        int evuid = 0;

        foreach (DataRow r in data.Tables[0].Rows)
        {
            evuid = (int)r[0];
        }

        return evuid;
    }
    public List<string> GetActorData(int _acuid)
    {
        string datastring = "SELECT * FROM account WHERE ac_uid=" + _acuid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<string> list = new List<string>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            list.Add(r[2].ToString());
            list.Add(r[3].ToString());
            list.Add(r[15].ToString());
        }

        return list;
    }
    public List<string> GetScenarioData(int _scnuid, int _type)
    {
        string datastring = "SELECT * FROM scenario WHERE scn_uid=" + _scnuid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<string> list = new List<string>();

        if (_type == 1)
        {
            foreach (DataRow r in data.Tables[0].Rows)
            {
                list.Add(r[3].ToString());
                list.Add(r[5].ToString());
            }
        }
        else if (_type == 2)
        {
            foreach (DataRow r in data.Tables[0].Rows)
            {
                list.Add(r[4].ToString());
                list.Add(r[5].ToString());
                list.Add(r[6].ToString());
            }
        }

        return list;
    }
    public List<string> GetEVData(int _evuid)
    {
        string datastring = "SELECT * FROM ev WHERE ev_uid=" + _evuid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<string> list = new List<string>();
        string type = "";

        foreach (DataRow r in data.Tables[0].Rows)
        {
            type = r[1].ToString();
            if (type.Equals("1")) type = "VR";
            else if (type.Equals("2")) type = "AR";

            list.Add(r[2].ToString());
            list.Add(type);
            //list.Add(r[15].ToString());
        }

        return list;
    }
    public int GetEVRUid(int _evs_uid)
    {
        string datastring = "SELECT evr_uid FROM ev_score WHERE evS_uid=" + _evs_uid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        int ac_uid = 0;

        foreach (DataRow r in data.Tables[0].Rows)
        {
            ac_uid = (int)r[0];
        }

        return ac_uid;
    }
    public List<string> GetEVDeepData(int _evuid)
    {
        string datastring = "SELECT * FROM ev WHERE ev_uid=" + _evuid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<string> list = new List<string>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            for (int i = 5; i < 15; i++)
            {
                list.Add(r[i].ToString());
            }
        }

        return list;
    }
    public List<string> GetTrainingData(int _trsuid)
    {
        string datastring = "SELECT * FROM training_score WHERE trS_uid=" + _trsuid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<string> list = new List<string>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            list.Add(r[6].ToString());
            list.Add(r[11].ToString());
            list.Add(r[4].ToString());
        }

        return list;
    }
    public List<string> GetEVScoreData(int _evsuid)
    {
        string datastring = "SELECT * FROM ev_score WHERE evS_uid=" + _evsuid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<string> list = new List<string>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            for (int i = 4; i < 11; i++)
            {
                list.Add(r[i].ToString());
            }
        }

        return list;
    }
    public int IDToUid(string _id)
    {
        string datastring = "SELECT ac_uid FROM account WHERE id='" + _id + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        int acuid = 0;

        foreach (DataRow r in data.Tables[0].Rows)
        {
            acuid = (int)r[0];
        }

        return acuid;
    }
    public string UidToName(int _uid)
    {
        string datastring = "SELECT name FROM account WHERE ac_uid=" + _uid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        string name = "";

        foreach (DataRow r in data.Tables[0].Rows)
        {
            name = r[0].ToString();
        }

        return name;
    }
    public int Search_evuid()
    {
        int cnt = 0;
        string evcount = GetEV_UidCount();

        if (evcount == "0")
        {
            return 0;
        }

        string searchevid_str = "SELECT MAX(evS_uid) FROM ev_score";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchevid_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            cnt = (int)r[0];
        }

        return cnt;
    }
    private string GetEV_UidCount()
    {
        string cnt = "";
        string searchevid_str = "SELECT COUNT(evS_uid) FROM ev_score";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(searchevid_str);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            cnt = r[0].ToString();
        }
        return cnt;
    }
    public int GetEvUid(int _trs_uid)
    {
        int _evuid = 0;
        string datastr = "SELECT ev_score.ev_uid FROM ev_score join training_score using(evS_uid) WHERE trS_uid=" + _trs_uid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastr);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            _evuid = (int)r[0];
        }

        return _evuid;
    }
    public int GetEvSUid(int _trs_uid)
    {
        int _evsuid = 0;
        string datastr = "SELECT evS_uid FROM training_score WHERE trS_uid=" + _trs_uid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastr);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            _evsuid = (int)r[0];
        }

        return _evsuid;
    }
    public void SetAssessment_DBInsert(int _evs_uid, int _evd_uid, int _evr_uid, int _ev_uid, int _grade1, int _grade2, int _grade3, int _grade4, int _grade5, int _gradeall, string _memo, string _reg_date)
    {
        string setdata_str =
            "INSERT INTO ev_score() VALUES(" + _evs_uid + "," + _evd_uid + "," + _evr_uid + "," + _ev_uid + "," + _grade1 + "," + _grade2 + "," + _grade3 + "," + _grade4 + "," + _grade5 + "," + _gradeall + ",'" + _memo + "','" + _reg_date + "')";

        ManagerDB.Insert_UpdateDB(setdata_str);
    }
    public void SetAssessment_DBUpdate(int _evs_uid, int _grade1, int _grade2, int _grade3, int _grade4, int _grade5, int _gradeall, string _memo, string _reg_date, int _evr_uid)
    {
        string setdata_str =
            "UPDATE ev_score SET evr_uid=" + _evr_uid + ",score1=" + _grade1 + ",score2=" + _grade2 + ",score3=" + _grade3 + ",score4=" + _grade4 + ",score5=" + _grade5 + ",score_all=" + _gradeall + ",memo='" + _memo + "',reg_date='" + _reg_date + "' WHERE evS_uid=" + _evs_uid + "";

        ManagerDB.Insert_UpdateDB(setdata_str);
    }
    public void SetAssessment_DBUpdate(int _trs_uid, int _state, int _evs_uid)
    {
        string setdata_str = "UPDATE training_score SET ev_situation=" + _state + ", evS_uid=" + _evs_uid + " WHERE trS_uid=" + _trs_uid + "";

        ManagerDB.Insert_UpdateDB(setdata_str);
    }
    public void SetScoreYN_DBUpdate(int _trs_uid, int _yn)
    {
        string setdata_str = "UPDATE training_score SET score_yn=" + _yn + " WHERE trS_uid=" + _trs_uid + "";

        ManagerDB.Insert_UpdateDB(setdata_str);
    }
    public int GetEvSituation(int _trs_uid)
    {
        int _evsituation = 0;
        string datastr = "SELECT ev_situation FROM training_score WHERE trS_uid=" + _trs_uid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastr);

        foreach (DataRow r in data.Tables[0].Rows)
        {
            _evsituation = (int)r[0];
        }

        return _evsituation;
    }
    #endregion
    #region Training
    public List<string> GetScenarioTitleList(int _type)
    {
        string datastr = "SELECT title FROM scenario WHERE trT_uid=" + _type + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastr);

        List<string> list = new List<string>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            list.Add(r[0].ToString());
        }

        return list;
    }
    public string GetScenarioTitle(int _scnuid)
    {
        string datastr = "SELECT title FROM scenario WHERE scn_uid=" + _scnuid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastr);

        string title = "";

        foreach (DataRow r in data.Tables[0].Rows)
        {
            title = r[0].ToString();
        }

        return title;
    }
    public int GetScenarioUID(string _title)
    {
        string datastr = "SELECT scn_uid FROM scenario WHERE title='" + _title + "'";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastr);

        int uid = 0;

        foreach (DataRow r in data.Tables[0].Rows)
        {
            uid = (int)r[0];
        }

        return uid;
    }
    public void SetRoom_DBInsert(int _acuid, int _scnuid, string _regdate, string _title)
    {
        int _situation = 0;
        int _del_yn = 1;

        string setdata_str =
            "INSERT INTO room(ac_uid,scn_uid,title,situation,reg_date,del_yn) VALUES(" + _acuid + "," + _scnuid + ",'" + _title + "'," + _situation + ",'" + _regdate + "'," + _del_yn + ")";

        ManagerDB.Insert_UpdateDB(setdata_str);
    }
    public void SetRoomData_TrainingDelete(int _uid, string _deldate, int _deletereason_uid, string _deepreason)
    {
        string setdata_str = "UPDATE room SET del_yn=0, del_date='" + _deldate + "', del_reason1=" + _deletereason_uid + ", del_reason2='" + _deepreason + "' WHERE r_uid = " + _uid + "";

        ManagerDB.Insert_UpdateDB(setdata_str);
    }
    public void SetRoomDelete_DBUpdate(string _reason, string _deepreason, int _r_uid, int _type)
    {
        int uid = Search_Optionuid(_reason, _type);
        string update_str = "UPDATE room SET del_reason1=" + uid + ", del_reason2='" + _deepreason + "' WHERE r_uid=" + _r_uid + "";

        ManagerDB.Insert_UpdateDB(update_str);
    }
    public List<string> GetRoomDeleteReason(int _uid)
    {
        string datastr = "SELECT * FROM room WHERE r_uid=" + _uid + "";
        string optionname = "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastr);

        List<string> list = new List<string>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            optionname = OptionuidToOptionName((int)r[9]);
            list.Add(optionname);
            list.Add(r[10].ToString());
        }

        return list;
    }
    //훈련방 생성 후 패킷 보내기위함.
    //list[0] -> r_uid
    //list[1] -> room title
    //list[2] -> scn_uid
    public List<string> GetRoomData()
    {
        string datastring = "SELECT * FROM room WHERE r_uid=(SELECT MAX(r_uid) FROM room)";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<string> list = new List<string>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            list.Add(r[0].ToString());
            list.Add(r[4].ToString());
            list.Add(r[3].ToString());
        }

        return list;
    }
    #endregion
    #region QuizBank
    public void QuizBank_DBInsert(int _trtuid, int _scnuid, string _quiztxt, int _quizox, string _date)
    {
        string setdata_str =
            "INSERT INTO quiz_bank(trT_uid,scn_uid,quiz_text,quiz_ox,reg_date) VALUES(" + _trtuid + "," + _scnuid + ",'" + _quiztxt + "'," + _quizox + ",'" + _date + "')";

        ManagerDB.Insert_UpdateDB(setdata_str);
    }
    public void SetQuizBank_DBUpdate(int _quizuid, string _quiztxt, int _quizox)
    {
        string update_str = "UPDATE quiz_bank SET quiz_text='" + _quiztxt + "', quiz_ox=" + _quizox + " WHERE quiz_uid=" + _quizuid + "";

        ManagerDB.Insert_UpdateDB(update_str);
    }
    public void QuizBankDelete_DBData(int _quizuid)
    {
        string delete_str = "DELETE FROM quiz_bank WHERE quiz_uid=" + _quizuid + "";

        ManagerDB.DeleteDB(delete_str);
    }
    public List<QuizBankSelect> GetQuizBankData(int _quizuid)
    {
        string datastring = "SELECT * FROM quiz_bank WHERE quiz_uid=" + _quizuid + "";

        DataSet data = new DataSet();
        data.Clear();
        data = ManagerDB.SelectDB(datastring);

        List<QuizBankSelect> list = new List<QuizBankSelect>();

        foreach (DataRow r in data.Tables[0].Rows)
        {
            list.Add(new QuizBankSelect((int)r[0], (int)r[1], (int)r[2], r[3].ToString(), (int)r[4], (DateTime)r[5]));
        }

        return list;
    }
    #endregion
}
