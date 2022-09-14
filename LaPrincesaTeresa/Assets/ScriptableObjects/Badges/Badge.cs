using UnityEngine;

public abstract class Badge : ScriptableObject
{
    [Header("Name")] public string badgeName;

    [Tooltip("The UI description for this badge"), TextArea(1, 2), Header("Description")]
    public string description;
}