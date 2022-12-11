using UnityEditor;
using UnityEngine;

public static class GameStaticFunctions
{
    public static bool IsGoInLayerMask(GameObject go, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << go.layer));
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#else
        Application.Quit();
#endif
    }
}