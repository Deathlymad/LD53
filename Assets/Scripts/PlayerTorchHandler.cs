using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorchHandler : MonoBehaviour, TorchHandler.TorchHandlerProxy
{
    public UIController controller;

    public TorchHandler obj;

    public void OnLightTorch(TorchHandler handler)
    {
        controller.setTorchCount(handler.torches);
        obj = handler;
    }
    public void updateTorch(TorchHandler handler)
    {
        controller.setTorchLevel(((float)handler.torchAnim.GetInteger("Power")) / ((float)handler.maxPower) * (handler.torches > 0 ? 1:0));
    }
    public void PauseAnimation()
    {
        obj.torchAnim.speed = 0;
    }
    public void PlayAnimation()
    {
        obj.torchAnim.speed = 1;
    }
    public void ForceUpdate()
    {
        obj.updateState();
    }
}
