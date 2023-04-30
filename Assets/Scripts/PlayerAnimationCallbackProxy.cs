using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationCallbackProxy : MonoBehaviour
{
    public Player p;

    //Called By Animations
    //=======================================================
    public void OnPickup()
    {
        p.OnPickup();
    }
}
