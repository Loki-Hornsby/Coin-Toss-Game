using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the... controls
/// </summary>

public class Controls : MonoBehaviour {
    [System.NonSerialized] public static Controls Instance;

    void Awake(){
        if (Instance == null){
            Instance = this;
        } else {
            Destroy(this);

            Debug.LogError("Instance of " + this.gameObject.name.ToString() + " already exists");
        }
    }

    /// <summary>
    /// Literally only the mouse is used so this static class isn't much use
    /// However in larger games this a super duper helpful structure (atleast for me)
    /// </summary>
    public static class Mouse {
        public static Vector2 position;
        public static bool held;
        public static bool click;

        public static void Refresh(){
            position = MainCamera.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
            held = Input.GetMouseButton(0);
            click = Input.GetMouseButtonDown(0);
        }
    }

    void Update(){
        Mouse.Refresh();
    }
}
