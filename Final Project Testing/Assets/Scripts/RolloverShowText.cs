using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RolloverShowText : MonoBehaviour
{
    public GameObject showText;

    public void OnMouseEnter()
    {
        showText.SetActive(true);
    }

    public void OnMouseExit()
    {
        showText.SetActive(false);
    }
}
