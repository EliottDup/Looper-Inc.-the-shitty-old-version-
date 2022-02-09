using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class clockTicksystem : MonoBehaviour
{
    public class OntickEventsArgs : EventArgs{
        public int tick;
    }
    public static event EventHandler<OntickEventsArgs> OnTick;
    private const float TICK_TIMER_MAX = 1f;
    public int tick;
    private float tickTimer;

    private void awake(){
        tick = 0;
        Debug.Log(tick);
    }
    
    private void Update(){
        tickTimer += Time.deltaTime;
        if (tickTimer >= TICK_TIMER_MAX){
            tickTimer -= TICK_TIMER_MAX;
            tick++;
            if (OnTick != null) OnTick(this, new OntickEventsArgs {tick = tick});
            Debug.Log("tick: " + tick);
        }
    }
}
