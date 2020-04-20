using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public bool isDiamond;
    public GameObject destroySystem;

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            var source = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SoundHandler>();
            source.PlayCollectableSound();
            if (!isDiamond)
            other.GetComponent<NewCharacterController>().currentMaxPlayer++;
            DestroyCollectable();
        }
    }

    void DestroyCollectable()
    {
        GameObject ps =
            Instantiate(destroySystem, transform.position,
            Quaternion.Euler(-transform.rotation.eulerAngles)) as GameObject;
        ps.gameObject.transform.localScale = transform.localScale * 2;
        ps.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<Renderer>().material.color;
        gameObject.SetActive(false);
        Destroy(ps, 5f);
        Destroy(gameObject, 0.5f);
    }
}
