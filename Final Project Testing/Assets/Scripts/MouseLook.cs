using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{

    public GameObject player;
    public Transform playerBody;
    public NewCharacterController ncc;
    public Slider pauseMenuSensitivity;
    public GameObject head;
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuSensitivity.value = GameController.mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        //chnage camera distance
        if (!GameController.isGameOver)
        {
            RaycastHit hitWall = new RaycastHit();
            Vector3 cameraDirection = transform.position - head.transform.position;
            if (Physics.Raycast(head.transform.position, cameraDirection.normalized, out hitWall))
            {
                float zDist = player.GetComponent<CharacterController>().height * 2;
                if (hitWall.distance < zDist)
                {
                    float yChange = hitWall.distance / 4 * 1.5f;
                    if (ncc.GetComponent<NewCharacterController>().shrunken)
                    {
                        transform.localPosition = new Vector3(0, yChange * 4, -hitWall.distance * 4);
                    }
                    else
                    {
                        transform.localPosition = new Vector3(0, yChange, -hitWall.distance);
                    }
                }
                else
                {
                    transform.localPosition = new Vector3(0, 0.75f, -2);
                }
            }
        }

        float flipped = ncc.GetComponent<NewCharacterController>().flippedNegator;
        float moveX = Input.GetAxis("Mouse X") * GameController.mouseSensitivity * flipped * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * GameController.mouseSensitivity * Time.deltaTime;

        Debug.Log(moveY);

        xRotation -= moveY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerBody.Rotate(Vector3.up * moveX);
        head.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
