using UnityEngine;
using DG.Tweening;

// For Doors, Vents, Windows, etc.
public interface IOpenable{
    bool IsOpen {get; set;}
    void Toggle();
    void Open();
    void Close();
}

public class Door : MonoBehaviour, IOpenable{

    [SerializeField] private Ease easeType;
    [SerializeField] private float tweenTime = 0.4f;

    private float posY;

    private float moveAmount;
    void Start(){
        moveAmount = transform.localScale.y;
        posY = transform.localPosition.y;
    }

    #region IOpenable
    public bool IsOpen {get;set;}
    public void Toggle(){
        IsOpen = !IsOpen;
        if(IsOpen) Open();
        else Close();
    }
    public virtual void Open(){ 
        transform.DOLocalMoveY(posY + moveAmount, tweenTime).SetEase(easeType);
        GetComponent<Collider2D>().enabled = false;
    }
    public virtual void Close(){ 
        transform.DOLocalMoveY(posY, tweenTime).SetEase(easeType);
        GetComponent<Collider2D>().enabled = true;
    }
    #endregion
}