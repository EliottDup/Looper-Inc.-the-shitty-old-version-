using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Transform Player, PlayerRev;
    [SerializeField] bool isReversed;

    void Awake()
    {
        // Player = PlayerManager.instance.player.transform.Find("Head").transform;
        // PlayerRev = PlayerManager.instance.playerRev.transform.Find("Head").transform;
    }  

    void Update() {
        if (!IsReversed.Instance.isReversed) transform.position = Player.transform.position;
        else transform.position = PlayerRev.transform.position;
    }
}
