using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxUI : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField] float x;
    [SerializeField] float y;


    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) *  Time.deltaTime, image.uvRect.size);
    }
}
