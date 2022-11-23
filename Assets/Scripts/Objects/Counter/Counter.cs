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
    // Mesh behaviour
    Vector3[] vertices;
    int[] triangles;

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

    /// <summary>
    /// https://stackoverflow.com/questions/13708395/how-can-i-draw-a-circle-in-unity3d/31767755#31767755 
    /// https://catlikecoding.com/unity/tutorials/mesh-basics/
    /// https://stackoverflow.com/questions/53406534/procedural-circle-mesh-with-uniform-faces/53422022#53422022 
    /// Swiper no swiping? ~ i rewrote most of the code 'cus although it worked i had no clue how!
    /// <summary>
    public Mesh GenerateCircle(int res) {
        /*
        float d = 1f / res;

        var vtc = new List<Vector3>();
        vtc.Add(Vector3.zero); // Start with only center point
        var tris = new List<int>();

        // First pass => build vertices
        for (int circ = 0; circ < res; ++circ) {
            float angleStep = (Mathf.PI * 2f) / ((circ + 1) * 6);
            for (int point = 0; point < (circ + 1) * 6; ++point) {
                vtc.Add(new Vector2(
                    Mathf.Cos(angleStep * point),
                    Mathf.Sin(angleStep * point)) * d * (circ + 1));
            }
        }

        // Second pass => connect vertices into triangles
        for (int circ = 0; circ < res; ++circ) {
            for (int point = 0, other = 0; point < (circ + 1) * 6; ++point) {
                if (point % (circ + 1) != 0) {
                    // Create 2 triangles
                    tris.Add(GetPointIndex(circ - 1, other + 1));
                    tris.Add(GetPointIndex(circ - 1, other));
                    tris.Add(GetPointIndex(circ, point));
                    tris.Add(GetPointIndex(circ, point));
                    tris.Add(GetPointIndex(circ, point + 1));
                    tris.Add(GetPointIndex(circ - 1, other + 1));
                    ++other;
                } else {
                    // Create 1 inverse triange
                    tris.Add(GetPointIndex(circ, point));
                    tris.Add(GetPointIndex(circ, point + 1));
                    tris.Add(GetPointIndex(circ - 1, other));
                    // Do not move to the next point in the smaller circle
                }
            }
        }*/

        // Circle parameters
        float Theta = 0f;
        float ThetaScale = 0.01f;
        int Size = (int)((1f / ThetaScale) + 1f);
        float radius = 2f;
        int Rings = 4;

        // Mesh calc
        vertices = new Vector3[Size * Rings];
        triangles = new int[Size/3];

        // Circle
        for (int i = 0; i < Size; i++) {
            for (int v = 0; v < Rings; v++){
                // Position
                Theta += (2.0f * Mathf.PI * ThetaScale);
                float x = (radius / (v + 1)) * Mathf.Cos(Theta);
                float y = (radius / (v + 1)) * Mathf.Sin(Theta);
                
                // Define Triangles
                /*if (i % 3 == 0 && i > 3 && i < triangles.Length - 3){
                    for (int v = 0; v < 3; v++){
                        triangles[(i - 3) + v] = i + v;
                    }
                }*/
                
                // Define Vertices
                vertices[i * (v + 1)] = new Vector3(x, y, 0);

                if ((i * (v + 1)) > 0) Debug.DrawLine(vertices[(i * (v + 1)) - 1], vertices[i * (v + 1)], Color.green, 15f);
            }
        }

        // Create the mesh
        var m = new Mesh();
        m.SetVertices(vertices);
        m.SetTriangles(triangles, 0);
        m.RecalculateNormals();
        m.UploadMeshData(true);

        // Throw it back
        return m;
    }


    /// <summary>
    /// Generate the mesh
    /// </summary>
    void Generate(){
        GetComponent<MeshFilter>().mesh = GenerateCircle(6);
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
