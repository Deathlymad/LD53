using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchHandler : MonoBehaviour
{
    public interface TorchHandlerProxy
    {
        public void OnLightTorch(TorchHandler obj);
        public void updateTorch(TorchHandler obj);
    }

    public Animator torchAnim;
    public GameObject proxy;
    private TorchHandlerProxy p;

    public int torches = 3;
    public int maxPower = 3;

    public float burnTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        torchAnim.SetBool("HasNewTorch", torches > 0);
        if (proxy != null)
            p = proxy.GetComponent<TorchHandlerProxy>();
    }

    public void updateState()
    {
        torchAnim.SetBool("HasNewTorch", torches > 0);
    }

    protected void OnLightTorch()
    {
        Invoke("updateTorch", burnTime);
        torchAnim.SetBool("HasNewTorch", torches > 0);
        torches -= 1;
        torchAnim.SetBool("HasNewTorch", torches > 0);
        torchAnim.SetInteger("Power", maxPower);
        if (p != null)
            p.OnLightTorch(this);
    }
    protected void updateTorch()
    {
        torchAnim.SetInteger("Power", torchAnim.GetInteger("Power") - 1);

        torchAnim.SetTrigger("Flicker");
        Invoke("updateTorch", burnTime);
        if (p != null)
            p.updateTorch(this);
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
