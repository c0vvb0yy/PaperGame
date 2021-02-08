using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public List<Transform> Visuals; 
    public List<Transform> xVisuals;
    public List<Transform> zVisuals;
    private List<float> xVisBasePos;
    private List<float> zVisBasePos;
    private float CurrentXRotation;
    private float CurrentZRotation;


    // Start is called before the first frame update
    void Start()
    {
        xVisBasePos = xVisuals.Select(value => value.localPosition.x).ToList();
        zVisBasePos = zVisuals.Select(value => value.localPosition.z).ToList();
    }

    public void FlipSprite(bool FacingRight, bool FacingFront)
    {
        foreach (var currentVisual in Visuals)
        {
            Vector3 rotation = currentVisual.eulerAngles;
            rotation.y = Mathf.MoveTowards(rotation.y, FacingRight ? 0 : 180, 360 * Time.deltaTime); 
            currentVisual.eulerAngles = rotation;
        }

        CurrentXRotation = Mathf.MoveTowards(CurrentXRotation, FacingRight ? 0 : 180, 360 * Time.deltaTime);
        
        foreach((var currentXVis, var basePos) in xVisuals.Zip(xVisBasePos, (x, y) => (x, y) ))
        {
            var pos = currentXVis.localPosition;
            pos.x = Mathf.Cos(CurrentXRotation * Mathf.Deg2Rad) * basePos;
            currentXVis.localPosition = pos; 
        }
        
        CurrentZRotation = Mathf.MoveTowards(CurrentZRotation, FacingFront ? 0 : 180, 360 * Time.deltaTime);

        foreach((var currentZVis, var basePos) in zVisuals.Zip(zVisBasePos, (x, y) => (x, y) ))
        {
            var pos = currentZVis.localPosition;
            pos.z = Mathf.Cos(CurrentZRotation * Mathf.Deg2Rad) * basePos;
            currentZVis.localPosition = pos; 
        }
    }
}