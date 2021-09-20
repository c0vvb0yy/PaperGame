using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlipAlways : MonoBehaviour
{
    public List<Transform> Visuals; 
    public List<Transform> xVisuals;
    public List<Transform> zVisuals;
    private List<float> xVisBasePos;
    private List<float> zVisBasePos;
    private float CurrentXRotation;
    private float CurrentZRotation;


    private bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        xVisBasePos = xVisuals.Select(value => value.localPosition.x).ToList();
        zVisBasePos = zVisuals.Select(value => value.localPosition.z).ToList();
    }

    private void Update() 
    {
        FlipSpriteAlways();
    }

    public void FlipSpriteAlways()
    {
        foreach (var currentVisual in Visuals)
        {
            Vector3 rotation = currentVisual.eulerAngles;
            rotation.y = Mathf.MoveTowards(rotation.y, facingRight ? 0 : 360, 360 * Time.deltaTime); 
            currentVisual.eulerAngles = rotation;
            
            if(rotation.y == 0 || rotation.y == 360)
            {
                facingRight = !facingRight;
            }
        }
    }
}