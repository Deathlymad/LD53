using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchHandler : MonoBehaviour
{
    public Animator torchAnim;

    public int torches = 3;

    public float burnTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        torchAnim.SetBool("HasNewTorch", torches > 0);
    }


    void OnLightTorch()
    {
        Invoke("updateTorch", burnTime);
        torchAnim.SetBool("HasNewTorch", torches > 0);
        torches -= 1;
        torchAnim.SetBool("HasNewTorch", torches > 0);
        torchAnim.SetInteger("Power", 3);
    }
    void updateTorch()
    {
        torchAnim.SetInteger("Power", torchAnim.GetInteger("Power") - 1);

        torchAnim.SetTrigger("Flicker");
        Invoke("updateTorch", burnTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Random.Range(0, 1000) > 867)
        {
            torchAnim.SetTrigger("Flicker");
        }
    }
}
