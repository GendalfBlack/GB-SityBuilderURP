using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Data;

/// <summary> A simple Unity MonoBehaviour script that demonstrates how 
/// to use the InteractionBuilder and ResourceBuilder classes. </summary>
public class UIButtonInteraction : MonoBehaviour
{
    /// <summary> The button that will trigger the interaction when clicked. </summary>
    private Button button;
    /// <summary> The resource that will be modified when the button is clicked. </summary>
    private Resource resource;
    /// <summary> The interaction that will be triggered by the button click. </summary>
    private InteractionBuilder interaction;
    
    /// <summary> Gets or sets the button that will trigger the interaction when clicked. </summary>
    public Button Button { get => button; set { if (button != value) button = value; } }
    /// <summary> Gets the name of the resource. If the resource is null, returns "No Resource". </summary>
    public string Name => resource != null ? resource.Name : "No Resource";
    /// <summary> Gets the current amount of the resource. Resource amount is 0 if resource is null. </summary>
    public int Get() => resource != null ? resource.Get() : 0;
    /// <summary> Creates a new resource with the specified name, amount, and capacity. </summary>
    /// <param name="name">The name of the new resource. Cannot be empty string.</param>
    /// <param name="amount">The initial amount of the new resource.</param>
    /// <param name="capacity">The maximum capacity of the new resource. (Storage capacity)</param>
    /// <returns>Returns true if the resource was null and was created, false if it already exists with the same properties.</returns>
    public bool CreateResourse(string name, int amount, int capacity)
    {
        if (resource == null)
        {
            resource = new ResourceBuilder()
                .WithName(name)                     // Set the name of the resource
                .WithAmount(amount)                 // Set the initial amount of the resource
                .WithCapacity(capacity)             // Set the maximum capacity of the resource
                .Build();                           // Create a new Resource instance with the specified properties
            return true;                            // Resource was created or updated
        }
        return false;                               // Resource already exists with the same properties
    }

    public bool UpdateResource(string name, int amount, int capacity)
    {
        if (resource != null && resource.Name == name)
        {
            resource.Set(amount);                 // Update the amount of the existing resource
            resource.Capacity = capacity;         // Update the capacity of the existing resource
            return true;                          // Resource was updated
        }
        return false;                             // Resource does not exist or has a different name
    }

    // <summary> Creates an interaction that adds a specified amount to the resource when the button is clicked. </summary>
    /// <param name="increment_amount">The amount to add to the resource when the button is clicked.</param>
    /// <returns>Returns true if the interaction was created successfully, false if it already exists. Only one interaction can be created per button.</returns>
    public bool CreateInteraction(int increment_amount)
    {
        if (interaction == null)
        {
            interaction = new InteractionBuilder()
                .SetSource(button)                  // Set the source of the interaction to the button
                .SetReceiver(resource)              // Set the receiver of the interaction to the resource
                .Subscribe(() => resource.Add(increment_amount));  // Subscribe to the interaction with an action that adds 5 to the resource
            button.onClick.RemoveAllListeners();    // Clear any existing listeners to avoid duplicates
            button.onClick.AddListener(interaction.InvokeOnce); // Add the interaction to the button's onClick event
            return true;                            // Interaction was created
        }
        return false;                               // Interaction already exists
    }
}

/// <summary> Custom editor for UIButtonInteraction that allows you to set up the button and resource in the inspector. </summary>
[CustomEditor(typeof(UIButtonInteraction))]
public class UIButtonInteractionEditor : Editor
{
    string Resource_name = EditorGUI.TextField(EditorGUILayout.GetControlRect(), "Resource Name", "");
    int Resource_amount = EditorGUILayout.IntField("Initial Amount", 10);
    int Resource_capacity = EditorGUILayout.IntField("Capacity", 100);
    int Resource_amount_increment = EditorGUILayout.IntField("Increment Amount", 5);

    public void OnEnable()
    {
        // Initialize default values for resource settings
        Resource_name = "New Resource";
        Resource_amount = 10;
        Resource_capacity = 100;
        Resource_amount_increment = 5;
    }

    public override void OnInspectorGUI()
    {
        UIButtonInteraction script = (UIButtonInteraction)target;

        // Check for required Button component
        Button button = script.GetComponent<Button>();

        if (button == null)
        {
            EditorGUILayout.HelpBox("This component requires a Button component attached to the same GameObject.", MessageType.Error);
            return;
        }

        // Assign the button if not set
        if (script.Button == null)
        {
            script.Button = button;
            EditorUtility.SetDirty(script);
        }
        
        #region Inspector Fields
        
        EditorGUILayout.Space();

        // Get name, amount and capacity from inspector and create a new ResourceBuilder instance
        EditorGUILayout.LabelField("Resource Settings", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();

        // Input fields for resource name, amount, and capacity
        Resource_name = EditorGUILayout.TextField("Resource Name", Resource_name);
        Resource_amount = EditorGUILayout.IntField("Initial Amount", Resource_amount);
        Resource_capacity = EditorGUILayout.IntField("Capacity", Resource_capacity);
        Resource_amount_increment = EditorGUILayout.IntField("Increment Amount", Resource_amount_increment);

        // Button to create or update the resource
        if (GUILayout.Button("Create or Update Resource"))
        {
            if (!script.CreateResourse(Resource_name, Resource_amount, Resource_capacity))
            {
                script.UpdateResource(Resource_name, Resource_amount, Resource_capacity);

                script.CreateInteraction(Resource_amount_increment);
            }
            else
            {
                EditorGUILayout.HelpBox("Resource created or updated successfully.", MessageType.Info);
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("UIButton Interaction", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();

        // Display the link to the source button component for inspector view
        var f = EditorGUILayout.ObjectField("Button Component:", script.Button, typeof(Button), false);
        // Disable field after assignment
        if (f != script.Button)
        {
            script.Button = f as Button;
            EditorUtility.SetDirty(script);
        }
        
        EditorGUILayout.Space();

        // Display the interaction details
        EditorGUILayout.LabelField("Resource Display", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        if (EditorApplication.isPlaying)
        {
            // Display current resource amount in play mode
            EditorGUILayout.LabelField($"Current {script.Name} Amount:", script.Get().ToString());
        }
        else
        {
            // Display initial resource amount in edit mode
            EditorGUILayout.LabelField($"Initial {script.Name} Amount:", script.Get().ToString());
        }
        #endregion

        // Draw default inspector for other serialized fields
        DrawDefaultInspector();
    }
}
