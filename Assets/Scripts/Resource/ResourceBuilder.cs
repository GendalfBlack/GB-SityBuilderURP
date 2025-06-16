using System.Collections;
/// <summary> Builder class for creating instances of the Resource class. </summary>
public class ResourceBuilder
{
    /// <summary> The name of the resource. </summary>
    private string name;
    /// <summary> The current amount of the resource. </summary>
    private int amount;
    /// <summary> The maximum capacity of the resource. (Storage capacity) </summary>
    private int capacity;

    /// <summary> Initialize a new instance of the ResourceBuilder with default values. </summary>
    /// <param name="name">The name of the resource. Defaults to "Default".</param>
    /// <returns>Returns a new instance of ResourceBuilder.</returns>
    public ResourceBuilder WithName(string name = "Default")
    {
        this.name = name;
        return this;
    }

    /// <summary> Initialize a new instance of the ResourceBuilder with default values. </summary>
    /// <param name="amount">The initial amount of the resource. Defaults to 0.</param>
    /// <returns>Returns a new instance of ResourceBuilder.</returns>
    public ResourceBuilder WithAmount(int amount = 0)
    {
        this.amount = amount;
        return this;
    }

    /// <summary> Initialize a new instance of the ResourceBuilder with default values. </summary>
    /// <param name="capacity">The maximum capacity of the resource. Defaults to 100.</param>
    /// <returns>Returns a new instance of ResourceBuilder.</returns>
    public ResourceBuilder WithCapacity(int capacity = 100)
    {
        this.capacity = capacity;
        return this;
    }
    /// <summary> Builds and returns a new instance of the Resource class with the specified properties. </summary>
    public Resource Build()
    {
        return new Resource(name, amount, capacity);
    }
}
