using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentControl : MonoBehaviour
{
    public ControlState _myState;

    protected Action m_callback = null;

    //처음에 매니저에서 미리 입력시켜두는 값들
    public virtual void Init(ControlState _state)
    {
        _myState = _state;
        m_callback = Open;
    }

    //상태가 바뀌고 처음 호출되는 함수
    public virtual void Open()
    {
    }

    public virtual void Close(ControlState _state)
    {
        ManagerControl.Instance.ChangeState(_state);
        gameObject.SetActive(false);
    }

    public virtual void Push()
    {
    }

    public virtual void Search()
    {
    }

    public virtual void Callback()
    {
        if (m_callback == null) return;

        m_callback.Invoke();
    }

}
