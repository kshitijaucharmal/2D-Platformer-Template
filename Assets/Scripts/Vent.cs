using UnityEngine;

public class Vent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ventFront;
    [SerializeField] private SpriteRenderer ventBack;
    [SerializeField] private Button ventDoorButton;

    // Start is called before the first frame update
    void Start()
    {
        SetInvisible();
    }

    void SetVisible(){
        ventFront.enabled = false;
        ventBack.enabled = true;
    }

    void SetInvisible(){
        ventFront.enabled = true;
        ventBack.enabled = false;
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            SetVisible();
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            SetInvisible();
        }
    }
}
