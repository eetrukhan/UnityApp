using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class DoubleShift : MonoBehaviour
{
    public Sprite pic;
    public GameObject panel;
    private Image im;
    void Start()
    {
        im = panel.GetComponent<Image>();
    }
    public void doubleShift()
    {
        im.sprite = pic;
    }
}
