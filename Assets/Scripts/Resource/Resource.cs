using UnityEngine;
/// <summary> Represents a resource with a name, amount, and capacity. </summary>
public class Resource 
{
    /// <summary> The name of the resource. </summary>
    public string Name { get; private set; }
    /// <summary> The current amount of the resource. </summary>
    private int Amount { get; set; }
    /// <summary> The maximum capacity of the resource. (Storage capacity)</summary>
    public int Capacity { get; set; }
    /// <summary> Initializes a new instance of the Resource with a specified name, amount, and capacity. </summary>
    /// <param name="name">The name of the resource.</param>
    /// <param name="amount">The initial amount of the resource.</param>
    /// <param name="capacity">The maximum capacity of the resource. (Storage capacity)</param>
    public Resource(string name, int amount, int capacity)
    {
        Name = name;
        Amount = amount;
        Capacity = capacity;
    }
    /// <summary> Adds a specified value to the resource amount, ensuring it does not exceed capacity. </summary>
    /// <param name="value">The value to add to the resource amount. Overflow will be voided. Accepts only positive values.</param>
    public void Add(int value = 1)
    {
        if (value < 0)
        {
            Debug.LogWarning($"[{Name}] Attempted to add a negative or 0 value: {value}. Operation ignored.");
            return;
        }
        Amount = Mathf.Clamp(Amount + value, 0, Capacity);
        Debug.Log($"[{Name}] New amount: {Amount}/{Capacity}");
    }
    /// <summary> Sets the resource amount to a specified value, ensuring it does not exceed capacity. </summary>
    /// <param name="value">The value to set the resource amount to. Overflow will be voided. Accepts only positive values.</param>
    public void Set(int value)
    {
        if (value < 0)
        {
            Debug.LogWarning($"[{Name}] Attempted to set a negative or 0 value: {value}. Operation ignored.");
            return;
        }
        Amount = Mathf.Clamp(value, 0, Capacity);
    }
    /// <summary> Gets the current amount of the resource. </summary>
    public int Get()
    {
        return Amount;
    }
}
