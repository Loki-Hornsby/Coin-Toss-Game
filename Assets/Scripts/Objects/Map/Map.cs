using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the map for the game
/// </summary>

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class Map : MonoBehaviour{
    public Sprite BGSprite;

    PolygonCollider2D col;
    SpriteRenderer sr;

    void Start(){
        col = GetComponent<PolygonCollider2D>();
        col.isTrigger = true;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = BGSprite;
    }

    private void OnTriggerExit2D(Collider2D other) {
        try {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = -rb.velocity;
            rb.drag = Coin.DragDefault*2f;
        } catch (Exception ex) {
            Debug.LogError(ex.ToString());
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        try {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            rb.drag = Coin.DragDefault;
        } catch (Exception ex) {
            Debug.LogError(ex.ToString());
        }
    }
}
