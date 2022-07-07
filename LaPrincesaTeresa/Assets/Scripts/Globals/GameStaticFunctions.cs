using UnityEngine;

public static class GameStaticFunctions
{
    public static bool IsGoInLayerMask(GameObject go, LayerMask layerMask)
    {
        return (layerMask | (1 << go.layer)) != 0;
    }
}