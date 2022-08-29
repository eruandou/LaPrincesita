using InteractableObjects;
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

    [MenuItem("GameObject/Princesita/CreateEquipableInteractable")]
    public static InteractableEquipableItem CreateEquipableInteractable()
    {
        var itemSpawned = CreateInteractableObject("ContinuedInteractable");
        var interactable = itemSpawned.AddComponent<InteractableEquipableItem>();
        interactable.SetDefaults();
        return interactable;
    }


    [MenuItem("GameObject/Princesita/CreateContinuedInteractable")]
    public static void CreateContinuedInteractable()
    {
        var itemSpawned = CreateInteractableObject("ContinuedInteractable");
        var interactable = itemSpawned.AddComponent<ContinuedInteractionObject>();
        interactable.SetDefaults();
    }

    private static GameObject CreateInteractableObject(string objectName)
    {
        var itemSpawned = new GameObject(objectName)
        {
            layer = LayerMask.NameToLayer("Interactable")
        };

        var visuals = new GameObject("Visuals")
        {
            transform =
            {
                parent = itemSpawned.transform
            }
        };

        visuals.AddComponent<SpriteRenderer>();
        itemSpawned.AddComponent<BoxCollider2D>();
        Selection.activeGameObject = itemSpawned;

        return itemSpawned;
    }
}