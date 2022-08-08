using UnityEngine;

public abstract class Badge : ScriptableObject
{
    [Header("Name")] public string name;
    [Tooltip("This describes what the badge does")]
    [Header("Description")] public string description;
    


}