using System.Collections;
using Managers;
using UnityEngine;

public class ReachedDemoEnd : MonoBehaviour
{
    private void Start()
    {
        //Reached demo end
        StartCoroutine(WaitToReset());
    }

    private static IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(8);
        GameManager.Instance.DataSaver.ResetSaveData();
        GameManager.Instance.CustomSceneManager.LoadMenu();
    }
}