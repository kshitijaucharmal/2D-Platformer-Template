using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour, IOpenable{

    [SerializeField] private Ease easeType;
    [SerializeField] private float tweenTime = 0.4f;
    [SerializeField] private int requiredKeys = 1;
    [SerializeField] private Vector2 moveTo;

    private Vector2 pos;

    void Start(){
        pos = transform.position;

        RequiredKeys = requiredKeys;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(moveTo, 0.3f);
        Gizmos.DrawLine(transform.position, moveTo);
    }

    #region IOpenable
    public int RequiredKeys { get; set; }
    public int Keys { get; set; }
    public bool IsOpen {get;set;}

    public void Toggle(){
        IsOpen = !IsOpen;
        if(IsOpen) Open();
        else Close();
    }

    public virtual void AddKey(int n=1){
        Keys += n;
        if(Keys >= RequiredKeys){
            Open();
        }
    }
    public virtual void RemoveKey(int n=1){
        Keys -= n;
        if(Keys < RequiredKeys) Close();
        if(Keys <= 0){ 
            Keys = 0;
            Close();
        }
    }

    public virtual void Open(){ 
        transform.DOMove(moveTo, tweenTime).SetEase(easeType);
        // GetComponent<Collider2D>().enabled = false;
    }
    public virtual void Close(){ 
        transform.DOMove(pos, tweenTime).SetEase(easeType);
        // GetComponent<Collider2D>().enabled = true;
    }
    #endregion
}