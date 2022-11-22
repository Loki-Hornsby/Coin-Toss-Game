using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
    
/// <summary>
/// Controls the coin for the game
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

public class Coin : MonoBehaviour {
    [System.NonSerialized] public static Coin Instance;

    // Rigid body
    Rigidbody2D rb;

    // Dragging motion
    bool active;
    bool bounced;

    // Constants
    public const float DragDefault = 2.5f;
    public const float MinimumSpeed = 0.5f;
    
    void Awake(){
        if (Instance == null){
            Instance = this;
        } else {
            Destroy(this);

            Debug.LogError("Instance of " + this.gameObject.name.ToString() + " already exists");
        }
    }

    void Start(){
        // Drag setup
        active = false;

        // Rigidbody setup
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    /// <summary>
    /// Enables movement of the coin
    /// </summary>
    public void Activate(){
        active = true;
    }

    /// <summary>
    /// Returns wether the coin is moving or not
    /// </summary>
    public bool isMoving(){
        return (rb.velocity.magnitude > MinimumSpeed);
    }

    void Update(){
        if (active){
            // Apply drag motion to rb
            if (Controls.Mouse.GetClicked(0) && !isMoving()){
                DragMotion.Instance.Export(ref rb);

                active = false;
            }

            // Set Position to nearest mouse pos
            if (DragMotion.Instance.isDragIdle()){
                Vector3 target = Game.Instance.GetClosestCounterToInput(
                    Game.Instance.GetCurrentPlayer().GetID(),
                    Controls.Mouse.GetPosition()
                ).gameObject.transform.position;

                transform.position = target;
            }
        }
    }
}
