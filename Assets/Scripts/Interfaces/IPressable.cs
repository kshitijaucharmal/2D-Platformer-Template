// For buttons, Pressure-pads, etc.
public interface IPressable {

    public bool Enabled {get; set;}         // Is Button Enabled?

    public bool CanPress { get; set; }      // Can Button Be Pressed (Object In Range?)

    public bool PressOnEnter { get; set; }  // Activate on Entering range
    public bool ReleaseOnExit { get; set; } // Release On Exitting range (For Pressure pads)

    public bool IsPressed { get; set; }     // Is Button Pressed?

    public float TimePressed { get; set; }  // Time for which button is held
    public float HoldTime { get; set;}      // TIme to hold the button down for
    public float ActiveTime { get; set; }   // Time ctr for which button should work
    public float ActiveThreshold { get; set; }   // Time for which button should work

    public bool IsOn { get; set; }

    public void Press();
    public void Release();

    // Enable/Disable Buttons
    public void Enable();
    public void Disable();

}