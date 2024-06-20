using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPressable {

    [Header("Start Variables")]
    // Define at start
    [SerializeField] bool isEnabled = false;
    // Input to interact with button
    [SerializeField] string inputButton = "Interact";
    // Press on enter
    [SerializeField] bool activateOnEnter = false;
    [SerializeField] bool releaseOnExit = false;
    // time to hold button for
    [SerializeField] float ButtonHoldTime = 2f;
    // Time button works for
    [SerializeField] float activeTimeThreshold = 2f;
    // Holding indicator
    [SerializeField] Slider timeIndicator;

    [Header("Connections")]
    [SerializeField] Door[] connectedTo;

    #region IPressable
    public bool PressOnEnter {get; set;}
    public bool ReleaseOnExit {get; set;}
    public bool Enabled {get; set;}
    public bool CanPress{ get; set; }
    public bool IsPressed{ get; set; }
    public bool IsOn{ get; set; }

    public float HoldTime { get; set; }
    public float ActiveTime { get; set; }
    public float ActiveThreshold { get; set; }

    public float TimePressed { 
        get { return _TimePressed; }
        set{
            _TimePressed = value;
            
            // Update Slider if it exists
            if(timeIndicator != null) timeIndicator.value = value;

            // Run Release function when done pressing
            if (value >= HoldTime){
                OnRelease();
                SetState(!IsOn);
                ActiveTime = 0;
            }
        }
    }
    private float _TimePressed = 0f;

    // Events
    public delegate void VoidFunc();

    public event VoidFunc OnPress;
    public event VoidFunc OnRelease;

    // Execute when pressed
    public virtual void Press(){
        IsPressed = true;
        Debug.Log(gameObject.name + " pressed");
    }
    // Execute when released
    public virtual void Release(){
        IsPressed = false;
        TimePressed = 0f; // Reset time to 0 on release
        Debug.Log(gameObject.name + " released");
    }

    // Don't think these need to be virtual
    public void Enable() { Enabled = true;}
    public void Disable() { Enabled = false; }
    #endregion

    void Start(){ 
        Enabled = isEnabled;
        PressOnEnter = activateOnEnter;
        ReleaseOnExit = releaseOnExit;
        HoldTime = ButtonHoldTime;
        ActiveThreshold = activeTimeThreshold;

        // Set slider
        if(timeIndicator != null){
            timeIndicator.maxValue = HoldTime;
            timeIndicator.gameObject.SetActive(false);
        }

        OnPress += Press;
        OnRelease += Release;
    }

    void SetState(bool on){
        IsOn = on;
        foreach(var c in connectedTo){
            if(c != null){
                if(IsOn) c.AddKey();
                else c.RemoveKey();
            }
        }
        // if(!IsOn){
        //     OnReset();
        // }
    }

    void Update(){
        if(IsOn && !PressOnEnter){
            ActiveTime += Time.deltaTime;
            if(ActiveTime > ActiveThreshold){
                SetState(false);
                ActiveTime = 0;
            }
        }

        // Return if not enabled, not in range or no input specified
        if(PressOnEnter || !Enabled || !CanPress || inputButton == "") return;

        if(IsPressed) TimePressed += Time.deltaTime;

        if(Input.GetButtonDown(inputButton)){ OnPress(); }
        if(Input.GetButtonUp(inputButton)){ OnRelease(); }
    }

    // This works better than OnTriggerStay
    void OnTriggerEnter2D(){ 
        if(PressOnEnter){ 
            OnPress();
            SetState(!IsOn);
        }
        CanPress = true;
        timeIndicator?.gameObject.SetActive(true);
    }
    void OnTriggerExit2D(){ 
        if(PressOnEnter && ReleaseOnExit){ SetState(false); }
        
        // Release if exit
        OnRelease();
        CanPress = false;
        timeIndicator?.gameObject.SetActive(false);
    }

    // Debugging
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        foreach(var c in connectedTo){
            if(c != null) Gizmos.DrawLine(transform.position, c.transform.position);
        }
    }
}