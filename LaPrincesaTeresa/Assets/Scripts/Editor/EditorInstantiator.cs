using InteractableObjects;
using NPC;
using UnityEditor;
using UnityEngine;

public class EditorInstantiator
{
    [MenuItem("GameObject/Princesita/CreateNPC")]
    public static void CreateNPCCcharacter()
    {
        var itemSpawned = new GameObject("NewNPC")
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

        itemSpawned.AddComponent<BoxCollider2D>();
        itemSpawned.AddComponent<NPCController>();
        visuals.AddComponent<SpriteRenderer>();
        SetObjectAsFinal(itemSpawned);
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
        SetObjectAsFinal(itemSpawned);
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
        return itemSpawned;
    }

    [MenuItem("GameObject/Princesita/CreateTriggerDialogueArea")]
    public static void CreateTriggerDialogueArea()
    {
        var itemSpawned = new GameObject("New Trigger Dialogue Area")
        {
            layer = LayerMask.NameToLayer("Triggerer")
        };
        itemSpawned.AddComponent<DiegeticDialogueTrigger>();
        itemSpawned.AddComponent<BoxCollider2D>();

        SetObjectAsFinal(itemSpawned);
    }

    private static void SetObjectAsFinal(GameObject goToModify)
    {
        Selection.activeGameObject = goToModify;
        var sceneCam = SceneView.lastActiveSceneView;
        goToModify.transform.position = sceneCam.camera.transform.position;
    }
}