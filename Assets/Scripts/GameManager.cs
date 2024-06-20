using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Singleton
    public static GameManager Instance;
    void Awake(){
    // If an instance already exists and it's not this, destroy this instance.
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    [Header("Checkpoints")]
    private CheckPoint[] checkPoints;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color activeColor;
    public void ActivateCP(){
        foreach(var c in checkPoints){
            c.GetComponent<SpriteRenderer>().color = activeColor;
        }
    }

    public void DeactivateCP(){
        foreach(var c in checkPoints){
            c.GetComponent<SpriteRenderer>().color = normalColor;
        }
    }


    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;

    private Transform player;
    public void ResetPlayer(){
        player.position = playerSpawnPoint.position;
    }

    // Start is called before the first frame update
    void Start() {
        checkPoints = FindObjectsOfType<CheckPoint>();
        player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity).transform;
    }

    // Update is called once per frame
    void Update() {

    }
}
