using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the placed counter
/// </summary>

public class Counter : MonoBehaviour {
    int ID;

    public int GetID(){
        return ID;
    }

    public Counter Initialize(int _ID){
        ID = _ID;
        return this;
    }
}
