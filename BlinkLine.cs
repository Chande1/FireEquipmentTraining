using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkLine : MonoBehaviour
{
    protected SpriteRenderer renderer;
    [SerializeField] float rendalpa;
    [SerializeField] float blinkspeed;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rendalpa = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(rendalpa>=1)
        {
            blinkspeed *= -1; 
        }
        else
        {
            rendalpa += blinkspeed * Time.deltaTime;
        }
        
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, rendalpa);
    }
}
