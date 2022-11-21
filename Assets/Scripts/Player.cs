using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a player
/// </summary>

public enum States {
    WaitingToPlace,
    PlacingCounters,
    WaitingToMove,
    MovingCoin
}

public class Player : MonoBehaviour {
    int ID;

    public States state;
    int counters;

    public int GetID(){
        return ID;
    }

    /// <summary>
    /// End the players turn
    /// </summary>
    public void EndTurn(){
        Debug.Log("Player " + ID + " ended their turn.");

        state = States.WaitingToMove;
    }

    /// <summary>
    /// Begin the players turn
    /// </summary>
    public void BeginTurn(){
        Debug.Log("Player " + ID + " started their turn.");

        if (counters > 0){
            state = States.PlacingCounters;
        } else {
            state = States.MovingCoin;
        }

        // We reset the click since it carries over when changing player
        Controls.Mouse.click = false;
    }

    /// <summary>
    /// Setup the player
    /// </summary>
    public void Initialize(int _ID) {
        counters = 3;
        ID = _ID;

        if (ID == 1){
            state = States.PlacingCounters;
        } else {
            state = States.WaitingToPlace;
        }

        Debug.Log("Player " + ID + " initialized");
    }

    void Update() {
        switch (state){
            case States.PlacingCounters:
                if (Controls.Mouse.click){
                    if (counters > 0){
                        counters--;
                        Game.Instance.PlaceCounter(ID);
                    }
                           
                    if (counters == 0) Game.Instance.NextPlayer();
                }

                break;

            case States.MovingCoin:
                if (!DragMotion.Instance.isDragActive()){
                    Game.Instance.NextPlayer();
                }

                break;
        }
    }
}
