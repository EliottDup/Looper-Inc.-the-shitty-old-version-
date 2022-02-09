using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    private float GameTime;
    [SerializeField] GameObject Canvas;

    void Update(){
        string test = GameTime.ToString("F2");
        TimerText.SetText(test.ToString());
        if (!IsReversed.Instance.isReversed)GameTime += Time.deltaTime;
        else GameTime -= Time.deltaTime;
        if (GameTime < 0) {
            GameTime = 0;
            Canvas.GetComponent<DeathMenu>().IsDed = true;
        }
    }
}
