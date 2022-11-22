using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the... controls
/// </summary>

public static class Controls {
    /// <summary>
    /// Literally only the mouse is used so this static class isn't much use
    /// However in larger games this a super duper helpful structure (atleast for me)
    /// </summary>
    public static class Mouse {
        static int reset;

        static bool ResetCheck(){
            if (reset > 0){
                reset--;

                return true;
            } else {
                return false;
            }
        }

        public static void Reset(){
            reset = 1;
        }

        public static bool GetHeld(int type){
            if (ResetCheck()) return false;

            return Input.GetMouseButton(type);
        }

        public static bool GetClicked(int type){
            if (ResetCheck()) return false;

            return Input.GetMouseButtonDown(type);
        }

        public static bool GetUp(int type){
            if (ResetCheck()) return false;

            return Input.GetMouseButtonUp(type);
        }

        public static Vector2 GetPosition(){
            return MainCamera.Instance.camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
