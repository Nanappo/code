using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

#region DataStruct
public struct AccountSelect
{
    public int arid, uid, gender;
    public string name, id, password, birthday;
    public DateTime lastlogin, firstlogin;

    public AccountSelect(string _name, string _id, DateTime _lastlogin, DateTime _firstlogin, int _arid, string _password, int _uid, int _gender, string _birthday)
    {
        this.id = _id;
        this.name = _name;
        this.lastlogin = _lastlogin;
        this.firstlogin = _firstlogin;
        this.arid = _arid;
        this.password = _password;
        this.uid = _uid;
        this.gender = _gender;
        this.birthday = _birthday;
    }
}
public struct DeleteSelect
{
    public int arid, gender, uid;
    public string name, id, password, deletereason, deepreason, birthday;
    public DateTime deletetime, firstlogin;

    public DeleteSelect(string _name, string _id, DateTime _deletetime, DateTime _firstlogin, int _arid, string _password, string _deletereason, string _deepreason, int _gender, string _birthday, int _uid)
    {
        this.id = _id;
        this.name = _name;
        this.deletetime = _deletetime;
        this.firstlogin = _firstlogin;
        this.arid = _arid;
        this.password = _password;
        this.deletereason = _deletereason;
        this.deepreason = _deepreason;
        this.gender = _gender;
        this.birthday = _birthday;
        this.uid = _uid;
    }
}
public struct OptionSelect
{
    public int use, op_uid;
    public string category;
    public DateTime changedate;

    public OptionSelect(string _category, int _use, DateTime _changedate, int _op_uid)
    {
        this.category = _category;
        this.use = _use;
        this.changedate = _changedate;
        this.op_uid = _op_uid;
    }
}
public struct GraphSelect
{
    public int trainingcase, traininggrade, assessment, trSuid, evsituation, quizcount;
    public string scenario, name, id;
    public DateTime date;

    public GraphSelect(int _trainingcase, string _scenario, DateTime _date, int _assessment, int _traininggrade, int _trsuid, string _name, string _id, int _evsituation, int _quizcount)
    {
        this.trainingcase = _trainingcase;
        this.scenario = _scenario;
        this.date = _date;
        this.assessment = _assessment;
        this.traininggrade = _traininggrade;
        this.trSuid = _trsuid;
        this.id = _id;
        this.name = _name;
        this.evsituation = _evsituation;
        this.quizcount = _quizcount;
    }
}
public struct EvaluationSelect
{
    public int truid, usecategory;
    public string title, id, name;
    public DateTime date;

    public EvaluationSelect(int _truid, string _title, string _name, string _id, DateTime _date, int _usecategory)
    {
        this.truid = _truid;
        this.title = _title;
        this.name = _name;
        this.id = _id;
        this.date = _date;
        this.usecategory = _usecategory;
    }
}
public struct EvaluationChangeSelect
{
    public int truid;
    public string title, item1, item2, item3, item4, item5, itemdeep1, itemdeep2, itemdeep3, itemdeep4, itemdeep5;

    public EvaluationChangeSelect(int _truid, string _title, string _item1, string _item2, string _item3, string _item4, string _item5,
        string _itemdeep1, string _itemdeep2, string _itemdeep3, string _itemdeep4, string _itemdeep5)
    {
        this.truid = _truid;
        this.title = _title;
        this.item1 = _item1;
        this.item2 = _item2;
        this.item3 = _item3;
        this.item4 = _item4;
        this.item5 = _item5;
        this.itemdeep1 = _itemdeep1;
        this.itemdeep2 = _itemdeep2;
        this.itemdeep3 = _itemdeep3;
        this.itemdeep4 = _itemdeep4;
        this.itemdeep5 = _itemdeep5;
    }
}
public struct ScenarioSelect
{
    public int trtuid, scnuid, person;
    public string scenarioname, area;
    public DateTime trainingdate, gradedate;

    public ScenarioSelect(int _trtuid, int _scnuid, int _person, string _scenarioname, string _area, DateTime _trainingdate, DateTime _gradedate)
    {
        this.trtuid = _trtuid;
        this.scnuid = _scnuid;
        this.person = _person;
        this.scenarioname = _scenarioname;
        this.area = _area;
        this.trainingdate = _trainingdate;
        this.gradedate = _gradedate;
    }
}
public struct RoomSelect
{
    public string roomname, roomarea, scenario, time, trainername, del_reason, deep_reason;
    public int person, roomstate, r_uid, ship;
    public DateTime startdate, enddate;

    public RoomSelect(string _roomname, string _roomarea, string _scenario, string _time, string _trainername, int _person, int _roomstate, DateTime _startdate, int _r_uid, int _ship, DateTime _enddate, string _del_reason, string _deep_reason)
    {
        this.roomname = _roomname;
        this.roomarea = _roomarea;
        this.scenario = _scenario;
        this.time = _time;
        this.trainername = _trainername;
        this.person = _person;
        this.roomstate = _roomstate;
        this.startdate = _startdate;
        this.r_uid = _r_uid;
        this.ship = _ship;
        this.enddate = _enddate;
        this.del_reason = _del_reason;
        this.deep_reason = _deep_reason;
    }
}
public struct QuizBankSelect
{
    public string question;
    public int quizuid, trtuid, scnuid, answer;
    public DateTime regdate;

    public QuizBankSelect(int _quizuid, int _trtuid, int _scnuid, string _question, int _answer, DateTime _regdate)
    {
        this.quizuid = _quizuid;
        this.trtuid = _trtuid;
        this.scnuid = _scnuid;
        this.question = _question;
        this.answer = _answer;
        this.regdate = _regdate;
    }
}
#endregion

public class DBManager : ComponentControl
{
    private MySqlConnection con;
    private bool isConnet = false;
    private bool insertResult = false;
    private ReaderINI iniReader;

    public override void Init(ControlState _state)
    {
        base.Init(_state);
        iniReader = GetComponent<ReaderINI>();
        ConnectionDB();
    }
    //DB연동
    public void ConnectionDB()
    {
        isConnet = true;

        try
        {
            string conStr = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};", $"{iniReader.iniData.GameServerIP}", $"{iniReader.iniData.DBSchemaName}", $"{iniReader.iniData.DBLoginId}", $"{iniReader.iniData.DBLoginPwd}");
            //string conStr = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};", "192.168.50.158", "test_tms", "root", "root");
            //string conStr = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};", "192.168.50.199", "sys", "root", "root");
            con = new MySqlConnection(conStr);

            con.Open();
            Debug.Log(isConnet);
        }
        catch (Exception ex)
        {
            isConnet = false;
            Debug.Log("false / e.ConnectionTest: " + enabled.ToString());
        }
    }
    //DB 데이터를 저장 및 갱신
    public void Insert_UpdateDB(string _str)
    {
        insertResult = false;
        try
        {
            MySqlCommand cmd = new MySqlCommand(_str, con);
            cmd.Connection = con;

            if (cmd.ExecuteNonQuery() == 1)
            {
                insertResult = true;
                Debug.Log("성공");
            }
            else
            {
                insertResult = false;
                Debug.Log("실패");
            }
        }
        catch (Exception ex)
        {
            insertResult = false;
            Debug.Log("ex.insert_updatedb: " + ex.ToString());
        }
    }
    //DB 데이터를 삭제
    public void DeleteDB(string _str)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(_str, con);
            cmd.Connection = con;

            if (cmd.ExecuteNonQuery() == 1)
            {
                Debug.Log("성공");
            }
            else
            {
                Debug.Log("실패");
            }
        }
        catch (Exception ex)
        {
            Debug.Log("ex.insert_updatedb: " + ex.ToString());
        }
    }
    //DB 데이터를 가져옴
    public DataSet SelectDB(string _str)
    {
        DataSet dataset = new DataSet();

        try
        {
            MySqlDataAdapter adpt = new MySqlDataAdapter(_str, con);
            adpt.Fill(dataset);

            return dataset;
        }
        catch (Exception ex)
        {
            Debug.Log("ex.selectdb: " + ex.ToString());
            return null;
        }
    }
    //DB 데이터를 가져옴
    public MySqlDataReader SelectDBRead(string _str)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(_str, con);
            MySqlDataReader table = cmd.ExecuteReader();

            return table;
        }
        catch (Exception ex)
        {
            Debug.Log("ex.selecttest: " + ex.ToString());
            return null;
        }
    }
    //DB 컬럼명을 변경
    public void Columns_UpdateDB(string _str)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(_str, con);
            cmd.Connection = con;

            cmd.ExecuteNonQuery();
            Debug.Log("Column Change");
        }
        catch (Exception ex)
        {
            Debug.Log("ex.insert_updatedb: " + ex.ToString());
        }
    }
    //검색할 테이블의 총 개수
    public int allRowCount(string _countstr)
    {
        return SelectDB(_countstr).Tables[0].Rows.Count;
    }
    //DB에 데이터가 제대로 저장 됐는지 확인
    public bool InsertResult()
    {
        return insertResult;
    }
    public void DBClose()
    {
        con.Close();
        isConnet = false;
    }
    private void OnDestroy()
    {
        DBClose();
    }
}
