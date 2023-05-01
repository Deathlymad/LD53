using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public GameObject cameraCenter;
    public GameObject player;
    public GameObject monsterPrefab;
    private Player playerComponent;

    public float turnFactor = 0.5f;
    public float moveSpeed = 1f;
    public float rotSpeed = 20f;
    public float shakeAmount = 0.03f;
    public bool shaking = false;

    private Vector3 cameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //move to static state
        playerComponent = player.GetComponent<Player>();
        cameraPosition = cameraCenter.transform.GetChild(0).localPosition;


        HandleMovementSpeed[] foos = playerComponent.playerAnim.GetBehaviours<HandleMovementSpeed>();
        foreach (var v in foos)
        {
            v.control = this;
        }
    }

    void rotatePlayerToCamera()
    {
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, cameraCenter.transform.rotation, Time.deltaTime * rotSpeed * turnFactor);

    }

    void shakeCamera()
    {
        if (shaking)
        {
            cameraCenter.transform.GetChild(0).localPosition = cameraPosition + UnityEngine.Random.insideUnitSphere * shakeAmount;
        }
        else
        {
            cameraCenter.transform.GetChild(0).localPosition = cameraPosition;
        }
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
        if (Input.GetKeyUp("p"))
        {
            shaking = !shaking;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if ( Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Confined;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        if (isDown)
            playerComponent.Walk();
        else
            playerComponent.Stop();
        shakeCamera();
    }
}
