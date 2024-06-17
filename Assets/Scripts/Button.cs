using UnityEngine;
using UnityEngine.UI;

// For buttons, Pressure-pads, etc.
public interface IPressable {
    public bool PressOnEnter { get; set; }  // Activate on Enter
    public bool ReleaseOnExit { get; set; } // Release On Exit
    public bool Enabled {get; set;}         // Is Button Enabled
    public bool CanPress { get; set; }      // Can Button Be Pressed (Object In Range?)
    public bool IsPressed { get; set; }     // Is Button Pressed?
    public bool DonePressing { get; set; }  // If Button is done pressing
    public float TimePressed { get; set; }  // Time for which button is held
    public float HoldTime { get; set;}      // TIme to hold the button down for

    // Call when Starting to press or releasing
    public void OnPress();
    public void OnRelease();

    // Enable/Disable Buttons
    public void Enable();
    public void Disable();
}

public class Button : MonoBehaviour, IPressable {

    // Define at start
    [SerializeField] bool isEnabled = false;
    // Input to interact with button
    [SerializeField] string inputButton = "Interact";
    // Press on enter
    [SerializeField] bool activateOnEnter = false;
    [SerializeField] bool releaseOnExit = false;
    // time to hold button for
    [SerializeField] float ButtonHoldTime = 2f;
    // Holding indicator
    [SerializeField] Slider timeIndicator;

    [Header("Connections")]
    [SerializeField] Door[] connectedTo;

    #region IPressable
    public virtual bool PressOnEnter {get; set;}
    public virtual bool ReleaseOnExit {get; set;}
    public virtual bool Enabled {get; set;}
    public virtual bool CanPress{ get; set; }
    public virtual bool IsPressed{ get; set; }
    public virtual bool DonePressing{ get; set; }
    public virtual float HoldTime { get; set; }

    public virtual float TimePressed { 
        get { return _TimePressed; }
        set{
            _TimePressed = value;
            
            // Update Slider if it exists
            if(timeIndicator != null) timeIndicator.value = value;

            // Run Release function when done pressing
            if (value >= HoldTime){
                OnRelease();
                DonePressing = true;

                foreach(var c in connectedTo){
                    c.Toggle();
                }
            }
        }
    }
    private float _TimePressed = 0f;

    public virtual void OnPress(){
        IsPressed = true;
    }
    public virtual void OnRelease(){
        IsPressed = false;
        // Reset time to 0 on release
        TimePressed = 0f;
    }

    // Don't think these need to be virtual
    public void Enable() { Enabled = true;}
    public void Disable() { Enabled = false;}
    #endregion

    void Start(){ 
        PressOnEnter = activateOnEnter;
        ReleaseOnExit = releaseOnExit;
        Enabled = isEnabled;
        HoldTime = ButtonHoldTime;
        if(timeIndicator != null){
            timeIndicator.maxValue = HoldTime;
            timeIndicator.gameObject.SetActive(false);
        }
    }

    void Update(){
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
            DonePressing = true;
            foreach(var c in connectedTo){
                c.Toggle();
            }
        }
        CanPress = true;
        timeIndicator?.gameObject.SetActive(true);
    }
    void OnTriggerExit2D(){ 
        if(PressOnEnter && ReleaseOnExit){
            DonePressing = true;
            foreach(var c in connectedTo){
                c.Toggle();
            }
        }
        OnRelease();
        CanPress = false;
        timeIndicator?.gameObject.SetActive(false);
    }

    // Debugging
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        foreach(var c in connectedTo){
            Gizmos.DrawLine(transform.position, c.transform.position);
        }
    }
}