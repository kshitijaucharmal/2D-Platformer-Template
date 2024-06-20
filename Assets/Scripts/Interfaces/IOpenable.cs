// For Doors, Vents, Windows, etc.
public interface IOpenable{
    bool IsOpen {get; set;}

    int Keys { get; set; }
    int RequiredKeys { get; set; }

    void AddKey(int n);
    void RemoveKey(int n);
    void Toggle();
    void Open();
    void Close();
}