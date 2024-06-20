using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {
    NONE,
    PRESS,
    RELEASE
}

public struct Action{
    public Vector2 position;
    public ActionType action;

    public Action(Vector2 position, ActionType action){
        this.position = position;
        this.action = action;
    }
}

public class PlayerActionStore : MonoBehaviour {
    
    #region Singleton
    public PlayerActionStore Instance {get; private set;}

    void Awake(){
    // If an instance already exists and it's not this, destroy this instance.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public bool isRecording = false;
    [SerializeField] private GameObject playerClone;

    public List<Action> actions = new List<Action>();

    void Update(){
        if(!isRecording) return;

        // Create new action
        var action = new Action(
            transform.position,
            ActionType.NONE
        );

        // Handle Interactions
        if(Input.GetButtonDown("Interact")){
            action.action = ActionType.RELEASE;
        }
        if(Input.GetButtonUp("Interact")){
            action.action = ActionType.PRESS;
        }

        // Add to list
        actions.Add(action);
    }

    void CreateClone(Vector2 pos){
        var pc = Instantiate(playerClone, pos, Quaternion.identity, GameManager.Instance.transform).GetComponent<PlayerClone>();
        pc.SetActions(actions);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Checkpoint")){
            isRecording = !isRecording;

            if(!isRecording && actions.Count != 0){ 
                StopAllCoroutines();
                var c = SpawnClones(other.transform.position);
                StartCoroutine(c);
            }

            // Checkpoint
            if(isRecording){
                StopAllCoroutines();
                GameManager.Instance.ActivateCP();
                actions.Clear();
            } 
            else {
                GameManager.Instance.DeactivateCP();
                GameManager.Instance.ResetPlayer();
            }
        }
    }

    IEnumerator SpawnClones(Vector2 pos){
        while(true){
            CreateClone(pos);
            yield return new WaitForSeconds(2);
        }
    }
}