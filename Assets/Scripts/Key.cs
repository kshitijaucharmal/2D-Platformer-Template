using UnityEngine;

public class Key : MonoBehaviour {

    [SerializeField] private Door doorToOpen;

    void OnTriggerEnter2D(Collider2D other){
        doorToOpen.AddKey();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, doorToOpen.transform.position);
    }
}
