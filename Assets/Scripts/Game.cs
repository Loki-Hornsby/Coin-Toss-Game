using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Controls the game
/// </summary>

[RequireComponent(typeof(DragMotion))]

public class Game : MonoBehaviour {
    [System.NonSerialized] public static Game Instance;

    [Header("Counter")]
    public GameObject counterPrefab;
    List<Transform> counters = new List<Transform>();

    [Header("Players")]
    public GameObject playerPrefab;
    public int players;
    Queue<Player> turn = new Queue<Player>();

    void Awake(){
        if (Instance == null){
            Instance = this;
        } else {
            Destroy(this);

            Debug.LogError("Instance of " + this.gameObject.name.ToString() + " already exists");
        }
    }

    /// <summary>
    /// https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    /// </summary>
    public Counter GetClosestCounterToMouse(int _ID){
        var nClosest = counters.Where(t => (t.gameObject.GetComponent<Counter>().GetID() == _ID))
            .OrderBy(t => ((Vector2)t.position - Controls.Mouse.position).sqrMagnitude)
            .FirstOrDefault();
        return nClosest.gameObject.GetComponent<Counter>();
    }

    /// <summary>
    /// Place a counter!
    /// </summary>
    public void PlaceCounter(int ID){
        Counter counter = Instantiate(counterPrefab, Controls.Mouse.position, Quaternion.identity).GetComponent<Counter>().Initialize(ID);
        counters.Add(counter.gameObject.transform);
    }

    /// <summary>
    /// Move to the next player
    /// </summary>
    public void NextPlayer(){
        // Assign old player
        Player lastPlayer = turn.Dequeue();

        // end old players turn
        lastPlayer.EndTurn();

        // Put old player at end of queue
        turn.Enqueue(lastPlayer);

        // Assign new player
        Player newPlayer = turn.Peek();

        // Begin new players turn
        newPlayer.BeginTurn();

        // Activate coin
        if (newPlayer.state == States.MovingCoin) Coin.Instance.Activate(newPlayer.GetID());
    }

    /// <summary>
    /// Create an instance of a player
    /// </summary>
    void CreatePlayer(int ID){
        // Create
        GameObject obj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        // Reference
        Player ply = obj.GetComponent<Player>();
        // Initialize
        ply.Initialize(ID);
        // Enqueue
        turn.Enqueue(ply);
    }

    void Start(){
        for (int i = 0; i < players; i++){
            CreatePlayer(i + 1);
        }
    }
}
