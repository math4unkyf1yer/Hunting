using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTransparency : MonoBehaviour
{

    // Stores the renderer of the player, which includes its material and colors
    private Renderer colorRenderer;


    private float alpha = 0f; // Transparency value
    private Color oldColor; // Saves the original colors
    // Start is called before the first frame update
    void Start()
    {
        // Get renderer's data along with the original color
        colorRenderer = GetComponent<Renderer>();
        oldColor = colorRenderer.material.color;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Use changeColor method below
        changeColor(colorRenderer.material);
        
        
    }
    private void changeColor(Material mat) {

        // When player holds down right click, the transparency will change
        if(Input.GetMouseButtonDown(1)) {
            // material.color = Color.red;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);

            // Changes shader values to make player transparent
            mat.SetColor("_Color", newColor);
            mat.SetInt("_Glossiness", 0);
        }
        else if(Input.GetMouseButtonUp(1)) {

            // Changes shader values back
            mat.SetColor("_Color", oldColor);
            mat.SetFloat("_Glossiness", 0.5f);
        }
    }
}
