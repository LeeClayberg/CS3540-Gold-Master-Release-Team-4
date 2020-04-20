using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NewCharacterController.shieldUp = true;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = transform.GetChild(0).transform.GetComponent<Renderer>().material.color;
        transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        transform.Rotate(new Vector3(0, 4, 0));
        if (transform.localScale.y > 0.6f)
        {
            if (color.a > 0)
            {
                transform.GetChild(0).transform.GetComponent<Renderer>().material.color =
                    new Color(color.r, color.g, color.b, color.a - 0.18f);
            } else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>().currentState = EnemyAI.FSMStates.Dead;
        }
    }

    private void OnDestroy()
    {
        NewCharacterController.shieldUp = false;
    }
}
