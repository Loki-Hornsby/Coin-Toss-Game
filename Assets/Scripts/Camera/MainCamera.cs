using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the camera for the game
/// </summary>

public class MainCamera : MonoBehaviour {
    [System.NonSerialized] public static MainCamera Instance;
    [System.NonSerialized] public Camera camera;

    void Awake(){
        if (Instance == null){
            Instance = this;
        } else {
            Destroy(this);

            Debug.LogError("Instance of " + this.gameObject.name.ToString() + " already exists");
        }
    }

    void Start(){
        camera = GetComponent<Camera>();
        camera.orthographic = true;
    }
}
