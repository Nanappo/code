using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PopupControl : MonoBehaviour
{
    public PopupState _myState;

    protected delegate void callback(bool _active);
    protected callback _callback;
    protected bool isAdmin = false;

    public virtual void Init(PopupState _state)
    {
        _myState = _state;
        _callback = Open;
    }

    public virtual void Open(bool _active)
    {
        isAdmin = _active;
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void TransferData(string[] _data)
    {
    }
    public virtual void Callback(bool _active)
    {
        if (_callback == null) return;

        _callback.Invoke(_active);
    }
}
