
using UnityEngine;

public class SandBox : MonoBehaviour
{
    [SerializeField] private PlayerModel player;
    // Start is called before the first frame update
    void Start()
    {
        player.SetAllAbilitiesSand();
    }

}
