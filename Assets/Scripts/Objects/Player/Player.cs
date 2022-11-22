using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a player
/// The player never controls themselves
/// </summary>

public enum PlayerStates {
    WaitingToPlace,
    PlacingCounters,
    WaitingToMove,
    MovingCoin,
}

public class Player : MonoBehaviour {
    int ID;

    PlayerStates state;
    int counters;

    public PlayerStates GetState(){
        return state;
    }

    public int GetID(){
        return ID;
    }

    /// <summary>
    /// End the players turn
    /// </summary>
    public void EndTurn(){
        // Debug.Log("Player " + ID + " ended their turn.");

        state = PlayerStates.WaitingToMove;
    }

    /// <summary>
    /// Begin the players turn
    /// </summary>
    public void BeginTurn(){
        // Debug.Log("Player " + ID + " started their turn.");

        if (counters > 0){
            state = PlayerStates.PlacingCounters;
        } else {
            state = PlayerStates.MovingCoin;
        }
    }

    /// <summary>
    /// Setup the player
    /// </summary>
    public void Initialize(int _ID) {
        counters = 3;
        ID = _ID;

        if (ID == 1){
            state = PlayerStates.PlacingCounters;
        } else {
            state = PlayerStates.WaitingToPlace;
        }

        // Debug.Log("Player " + ID + " initialized");
    }

    void Update() {
        // Counter placement
        if (state == PlayerStates.PlacingCounters){
            if (Controls.Mouse.GetClicked(0)){
                if (counters > 0){
                    counters--;
                    Game.Instance.PlaceCounter(ID);
                }
                        
                if (counters == 0) Game.Instance.NextPlayer();
            }
            
        // Coin movement
        } else if (state == PlayerStates.MovingCoin) {
            if (Controls.Mouse.GetUp(0)){
                if (DragMotion.Instance.isDragIdle()){
                    Game.Instance.NextPlayer();
                }
            }
        }
    }
}
