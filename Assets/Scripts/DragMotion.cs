using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMotion : MonoBehaviour {
    [System.NonSerialized] public static DragMotion Instance;
    
    // rb
    Rigidbody2D rb;

    // Drag
    const float DragTime = 0.125f * 1.25f;
    const float DragMult = 2f;
    Vector2 startPos;
    bool drag;
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
        drag = false;
    }

    /// <summary>
    /// Returns wether a "drag" (Recording of movement of the mouse) is being executed or not
    /// </summary>
    public bool isDragActive(){
        return drag;
    }

    /// <summary>
    /// Starts the drag motion
    /// </summary>
    public void StartDrag(){
        drag = true;

        t = DragTime;
        startPos = Controls.Mouse.position;
    }

    /// <summary>
    /// End the drag motion
    /// </summary>
    public void EndDrag(){
        drag = false;

        rb.AddForce(
            (
                (Controls.Mouse.position * Random.Range(1f, 1.15f)) - startPos) 
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
        rb = q;

        StartDrag();
    }

    void Update(){
        // End Drag
        if (drag){
            // Exit drag early
            if (!Controls.Mouse.held) EndDrag();

            // Start timer and once finished end drag
            t -= Time.deltaTime;

            if (t <= 0f){
                EndDrag();
            }
        }
    }
}
