using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    private List<Action> actions = new List<Action>();
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private LayerMask buttonLayer;


    private int ctr = -1;

    public void SetActions(List<Action> a){
        actions = a.ToList();
        ctr = actions.Count-1;
    }

    void Update(){
        if(ctr > -1){
            Action action = actions[ctr--];
            transform.position = action.position;

            if(action.action != ActionType.NONE){
                var cols = Physics2D.OverlapBoxAll(transform.position, boxSize, buttonLayer);
                foreach(var c in cols){
                    if(c == null) continue;
                    var b = c.GetComponent<Button>();
                    if(b != null){
                        if(action.action == ActionType.PRESS) b.Press();
                        if(action.action == ActionType.RELEASE) b.Release();
                    }
                }
            }

            if(ctr == 0) Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
