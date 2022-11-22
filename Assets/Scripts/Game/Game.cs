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
    public Counter GetClosestCounterToInput(int _ID, Vector3 inp){
        var nClosest = counters.Where(t => (t.gameObject.GetComponent<Counter>().GetID() == _ID))
            .OrderBy(t => (t.position - inp).sqrMagnitude)
            .FirstOrDefault();
        return nClosest.gameObject.GetComponent<Counter>();
    }

    /// <summary>
    /// Place a counter!
    /// </summary>
    public void PlaceCounter(int _ID){
        Counter counter = Instantiate(counterPrefab, Controls.Mouse.GetPosition(), Quaternion.identity).GetComponent<Counter>().Initialize(_ID);
        counters.Add(counter.gameObject.transform);

        // Todo: Change to mesh
        /*SpriteRenderer sp = counter.GetComponent<SpriteRenderer>();

        switch (_ID){
            case 1: // Red
                sp.color = new Color(1, 0, 0, 0.25f);
                break;
            case 2: // Blue
                sp.color = new Color(0, 0, 1, 0.25f);
                break;
            case 3: // Black
                sp.color = new Color(0, 0, 0, 0.25f);
                break;
            case 4: // White
                sp.color = new Color(1, 1, 1, 0.25f);
                break;
            default:
                sp.color = Color.cyan;
                break;
        }*/
    }

    /// <summary>
    /// Move to the next player
    /// </summary>
    public void NextPlayer(){
        // Reset the mouse to avoid instant clicks when swapping players
        Controls.Mouse.Reset();

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
        if (newPlayer.GetState() == PlayerStates.MovingCoin) Coin.Instance.Activate();
    }

    /// <summary>
    /// Get the current active player's ID
    /// </summary>
    public Player GetCurrentPlayer(){
        return turn.Peek();
    }

    /// <summary>
    /// Create an instance of a player
    /// </summary>
    void CreatePlayer(int _ID){
        // Create
        GameObject obj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        // Reference
        Player ply = obj.GetComponent<Player>();
        // Initialize
        ply.Initialize(_ID);
        // Enqueue
        turn.Enqueue(ply);
    }

    void Start(){
        for (int i = 0; i < players; i++){
            CreatePlayer(i + 1);
        }
    }
}
