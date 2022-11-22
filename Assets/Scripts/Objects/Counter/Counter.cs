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
    public int xSize, ySize;
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
    /// https://catlikecoding.com/unity/tutorials/procedural-grid/
    /// </summary>
    IEnumerator Generate(){
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices[i] = new Vector3(x, y);

                yield return null;
			}
		}
    }

    public Counter Initialize(int _ID){
        // Generate the mesh
        StartCoroutine(Generate());

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
