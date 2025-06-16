using System;
using UnityEngine;
/// <summary> Builder class for creating instances of the Interaction class. </summary>
public class InteractionBuilder
{
    /// <summary> The source object that initiates the interaction. </summary>
    private object source;
    /// <summary> The receiver object that responds to the interaction. </summary>
    private object receiver;
    /// <summary> The action to be performed during the interaction. </summary>
    private Action action;

    /// <summary> Initializes source object that initiates the interaction. </summary>
    /// <param name="src">The source object that initiates the interaction. Can be null. Defaults to null.</param>
    /// <returns>Returns this builder instance for chaining.</returns>
    public InteractionBuilder SetSource(object src = null)
    {
        source = src;
        return this;
    }
    /// <summary> Initializes receiver object that must responds to the interaction. </summary>
    /// <param name="recv">The receiver object that responds to the interaction. Cannot be null. </param>
    /// <returns>Returns this builder instance for chaining.</returns>
    public InteractionBuilder SetReceiver(object recv)
    {
        if (recv == null)
        {
            throw new ArgumentNullException(nameof(recv), "Receiver cannot be null.");
        }
        receiver = recv;
        return this;
    }
    /// <summary> Initializes the action to be performed during the interaction. </summary>
    /// <param name="method">The action to be performed during the interaction. Cannot be null.</param>
    /// <returns>Returns this builder instance for chaining.</returns>
    public InteractionBuilder Subscribe(Action method)
    {
        if (method == null)
        {
            throw new ArgumentNullException(nameof(method), "Action cannot be null.");
        }
        action = method;
        return this;
    }
    /// <summary> Calls subscribed action once. </summary>
    public void InvokeOnce()
    {
        Debug.Log($"[Interaction] Invoked from {source?.ToString() ?? "unknown"} to {receiver?.ToString() ?? "unknown"}.");
        action?.Invoke();
    }
}
