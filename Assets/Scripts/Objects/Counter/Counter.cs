using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the placed counter
/// </summary>

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Counter : MonoBehaviour {
    // Mesh Setup
    private Vector3[] vertices;

    // General Behaviour
    CircleCollider2D col; // Albeit a mesh we can still use a 2D collider
    int BelongsTo;
    Player entered;

    public int GetID(){
        return BelongsTo;
    }

    /// <summary>
    /// Tries to destroy a section of this counter
    /// </summary>
    void DestroySection(){
        
    }

    private void OnDrawGizmos () {
        if (vertices == null) {
			return;
		}

		Gizmos.color = Color.black;
		for (int i = 0; i < vertices.Length; i++) {
			Gizmos.DrawSphere(vertices[i], 0.1f);
		}
	}

    /// <summary>
    /// Generate the mesh
    /// https://catlikecoding.com/unity/tutorials/mesh-basics/
    /// Literally just copying the code and reading how it works ~ 
    ///     there's not much point for me to try to write this from scratch since i'll end up with the same code more or less
    ///         Also that would take way too long when i can understand it in a much easier fashion
    /// </summary>
    void Generate(){
        // Size params
        int X = 10;
        int Y = 10;

        // Create mesh
        Mesh GenMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = GenMesh;
		GenMesh.name = "Procedural Grid";

        // Initialize vertices
        vertices = new Vector3[(X + 1) * (Y + 1)];

        // Generate positions
        for (int i = 0, y = 0; y <= Y; y++) {
			for (int x = 0; x <= X; x++, i++) {
				vertices[i] = new Vector3(x, y);
			}
		}

        // Apply mesh
        GenMesh.vertices = vertices;

        // Generate triangles
        int[] triangles = new int[X * Y * 6];
		for (int ti = 0, vi = 0, y = 0; y < Y; y++, vi++) { // I did not know you could stack conditions in a for loop!
            for (int x = 0; x < X; x++, ti += 6, vi++) {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + X + 1;
                triangles[ti + 5] = vi + X + 2;
            }
		}

        // Apply triangles
        GenMesh.triangles = triangles;
    }

    public Counter Initialize(int _ID){
        // Generate the mesh
        Generate();

        // General
        col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;

        entered = null;
        BelongsTo = _ID;

        return this;
    }

    void RecordEntity(Collider2D v, bool e){
        try {
            if (v.tag == "Coin" && Game.Instance.GetCurrentPlayer().GetID() != BelongsTo){
                if (e == true) {
                    entered = Game.Instance.GetCurrentPlayer();
                    Debug.Log(entered.GetID());
                } else { 
                    entered = null;
                }
            }
        } catch (Exception ex){
            Debug.Log(ex.ToString());
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        RecordEntity(other, true);
    }

    void OnTriggerExit2D(Collider2D other) {
        RecordEntity(other, false);
    }

    void Update(){
        if (entered != null){
            if (entered.GetState() == PlayerStates.MovingCoin && DragMotion.Instance.isDragIdle()){
                DestroySection();

                entered = null;
            }
        }
    }
}
