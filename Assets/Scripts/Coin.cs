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
    public void Activate(int _ID){
        transform.position = Game.Instance.GetClosestCounterToMouse(_ID).gameObject.transform.position;
        active = true;
    }

    void Update(){
        // Apply drag motion to rb
        if (Controls.Mouse.click && active) {
            DragMotion.Instance.Export(ref rb);

            Game.Instance.NextPlayer();

            active = false;
        } 

        // Bounces off edges of screen - needs improvement - mirroring isn't taken into consideration 
        /*bool inScreen = MainCamera.Instance.IsInBounds(this.transform);

        if (inScreen && bounced){
            bounced = false;
        } else if (!inScreen && !bounced) {
            Vector2 vel = -rb.velocity;

            rb.velocity = vel;
        
            bounced = true;
        }*/
    }
}
