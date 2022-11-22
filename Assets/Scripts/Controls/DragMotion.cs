using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Records drag input from the user to calculate force to apply to the coin
/// Todo: Apply a "flick" detection
/// </summary>

public enum DragStates {
    Idle,
    Recording,
    Exported,
}

public class DragMotion : MonoBehaviour {
    [System.NonSerialized] public static DragMotion Instance;
    
    // rb
    Rigidbody2D rb;

    // Drag
    const float DragTime = 0.125f;
    const float DragMult = 2f;

    Vector2 startPos;
    public DragStates drag;
    float t;
    
    void Awake(){
        if (Instance == null){
            Instance = this;
        } else {
            Destroy(this);

            Debug.LogError("Instance of " + this.gameObject.name.ToString() + " already exists");
        }
    }

    void Start(){
        drag = DragStates.Idle;
    }

    public bool isDragActive(){
        return drag == DragStates.Recording;
    }

    public bool isDragIdle(){
        return drag == DragStates.Idle;
    }

    /// <summary>
    /// End the drag motion
    /// </summary>
    public void EndDrag(){
        drag = DragStates.Exported;

        rb.AddForce(
            (
                (Controls.Mouse.GetPosition() * Random.Range(1f, 1.15f)) - startPos) 
                * 
                (DragMult + Random.Range(-0.15f, 0.15f)
            ), 
            
            ForceMode2D.Impulse
        );
    }

    /// <summary>
    /// Begin recording
    /// </summary>
    public void Export(ref Rigidbody2D q){
        // Store ref
        rb = q;

        // Start the drag
        drag = DragStates.Recording;

        t = DragTime;
        startPos = Controls.Mouse.GetPosition();
    }

    void Update(){
        if (drag == DragStates.Recording){
            // Exit drag early
            if (!Controls.Mouse.GetHeld(0)) EndDrag();

            // If timer is finished then end the drag
            t -= Time.deltaTime;

            if (t <= 0f){
                EndDrag();
            }
        } else {
            if (rb != null){
                if (rb.velocity.magnitude < 0.5f){
                    drag = DragStates.Idle;
                }
            }
        }
    }
}
