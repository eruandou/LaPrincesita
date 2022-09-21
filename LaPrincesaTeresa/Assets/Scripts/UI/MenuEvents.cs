using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuEvents : MonoBehaviour
{
    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Totorial part1");
    }

}
