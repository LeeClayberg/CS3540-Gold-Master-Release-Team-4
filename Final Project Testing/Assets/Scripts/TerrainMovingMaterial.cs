using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMovingMaterial : MonoBehaviour
{
    public Material material;
    public float LavaSpeed = 1000.0f;

    // Start is called before the first frame update
    void Start()
    {
        material.EnableKeyword("_NORMALMAP");
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuBehavior.isGamePaused)
        {
            string textureName = "_MainTex";
            Vector2 offset = material.GetTextureOffset(textureName);
            Vector2 change = new Vector2(1, 1) / LavaSpeed;
            material.SetTextureOffset(textureName, offset + change);
        }
    }
}
