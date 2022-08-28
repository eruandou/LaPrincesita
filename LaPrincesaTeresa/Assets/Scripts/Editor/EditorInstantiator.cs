using NPC;
using UnityEditor;
using UnityEngine;

public class EditorInstantiator
{
    [MenuItem("GameObject/Princesita/CreateNPC")]
    public static void CreateNPCCcharacter()
    {
        var itemSpawnObj = new GameObject("NewNPC")
        {
            layer = LayerMask.NameToLayer("Interactable")
        };

        var visuals = new GameObject("Visuals")
        {
            transform =
            {
                parent = itemSpawnObj.transform
            }
        };

        itemSpawnObj.AddComponent<BoxCollider2D>();
        itemSpawnObj.AddComponent<NPCController>();
        visuals.AddComponent<SpriteRenderer>();

        Selection.activeObject = itemSpawnObj;
    }
}