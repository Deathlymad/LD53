using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject cameraCenter;
    public GameObject player;
    private Player playerComponent;

    public float turnFactor = 0.5f;
    public float moveSpeed = 1f;
    public float rotSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        playerComponent = player.GetComponent<Player>();
    }

    void rotatePlayerToCamera()
    {
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, cameraCenter.transform.rotation, Time.deltaTime * rotSpeed * turnFactor);

    }

    // Update is called once per frame
    void Update()
    {
        bool isDown = false;
        if (playerComponent.IsMovable())
        {
            if (Input.GetAxis("Mouse X") > 0.01 || Input.GetAxis("Mouse X") < -0.01)
            {
                cameraCenter.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, Vector3.up);
            }
            if (Input.GetKey("w"))
            {
                rotatePlayerToCamera();
                transform.position += player.transform.forward * moveSpeed * Time.deltaTime;
                isDown |= true;
            }
            if (Input.GetKey("a"))
            {
                rotatePlayerToCamera();
                transform.position -= player.transform.right * moveSpeed * Time.deltaTime;
                isDown |= true;
            }
            if (Input.GetKey("s"))
            {
                rotatePlayerToCamera();
                transform.position -= player.transform.forward * moveSpeed * Time.deltaTime;
                isDown |= true;
            }
            if (Input.GetKey("d"))
            {
                rotatePlayerToCamera();
                transform.position += player.transform.right * moveSpeed * Time.deltaTime;
                isDown |= true;
            }
        }
        if (isDown)
            playerComponent.Walk();
        else
            playerComponent.Stop();
    }
}
