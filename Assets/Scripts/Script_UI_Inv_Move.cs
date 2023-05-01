using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_UI_Inv_Move : MonoBehaviour
{
    public GameObject _Panel;
    private Vector3 _Current_Position;
    private bool _Display;
    private float _Move_Panel_Max = 350f;
    // Use this for initialization
    void Start()
    {
        //Starting Positon
        _Current_Position = _Panel.transform.position;
        _Display = false;
    }

    // Update is called once per frame
    void Update()
    {
        _Panel.transform.position = _Current_Position;
    }

    public void setState(bool display)
    {
        if (_Display != display)
        {
            Panel_Click();
        }
    }

    public void Panel_Click()
    {
        if (_Display)
        {
            //Hide Panel
            _Display = false;
            _Current_Position.x -= _Move_Panel_Max;
        }
        else
        {
            //Display Panel
            _Display = true;
            _Current_Position.x += _Move_Panel_Max;
        }
    }
}
