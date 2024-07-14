using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lengthX, startposX;
    private float lengthY, startposY;
    public GameObject cam;

    public float parallaxEffectX;
    public float parallaxEffectY;
    public float offsetY; // Public variable to adjust the y offset

    // Start is called before the first frame update
    void Start()
    {
        startposX = transform.position.x;
        startposY = transform.position.y;
        lengthX = 128; // Assuming the length for the x axis is 128 units
        lengthY = 128; // Assuming the length for the y axis is 128 units
    }

    // Update is called once per frame
    void Update()
    {
        float tempX = (cam.transform.position.x * (1 - parallaxEffectX));
        float distX = (cam.transform.position.x * parallaxEffectX);
        
        float tempY = (cam.transform.position.y * (1 - parallaxEffectY));
        float distY = (cam.transform.position.y * parallaxEffectY);

        // Applying the offsetY to the calculated y position
        transform.position = new Vector3(startposX + distX, startposY + distY + offsetY, transform.position.z);

        if (tempX > startposX + lengthX) startposX += lengthX;
        else if (tempX < startposX - lengthX) startposX -= lengthX;

        if (tempY > startposY + lengthY) startposY += lengthY;
        else if (tempY < startposY - lengthY) startposY -= lengthY;
    }
}
