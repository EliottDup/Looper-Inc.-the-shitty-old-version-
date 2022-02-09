using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{    // Start is called before the first frame update
    public Transform Seconds, Minutes, Hours;
    private int Startpos;

    void Awake(){
        Startpos = Random.Range(1, 43200);
        Seconds.rotation = Quaternion.Euler(0, 0, (Startpos) * 6);
        Minutes.rotation = Quaternion.Euler(0, 0, (Startpos) * 0.1f);
        Hours.rotation = Quaternion.Euler(0, 0, (Startpos) * 43200);

    }
    void Start()
    {
        
        clockTicksystem.OnTick += delegate(object sender, clockTicksystem.OntickEventsArgs e){
            Seconds.rotation = Quaternion.Euler(0, 0, (e.tick + Startpos) * 6);
            Minutes.rotation = Quaternion.Euler(0, 0, (e.tick + Startpos) * 0.1f);
            Hours.rotation = Quaternion.Euler(0, 0, (e.tick + Startpos) * (1/43200));

        };
    }

    // Update is called once per frame
    void Update()
    {
    }
}
