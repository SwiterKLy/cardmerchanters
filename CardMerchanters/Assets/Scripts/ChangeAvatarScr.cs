using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAvatarScr : MonoBehaviour
{
    public int numb;
    public Image avtr;
    public Sprite img1;
    public Sprite img2;
    public Sprite img3;
    // Start is called before the first frame update
    void Start()
    {
        numb = PlayerPrefs.GetInt("Img");
    }

    public void forw()
    {
        if (avtr.sprite == img1)
        {
            PlayerPrefs.SetInt("Img", 2);
            avtr.sprite = img2;
        }
        else
              if (avtr.sprite == img2)
        {
            PlayerPrefs.SetInt("Img", 3);
            avtr.sprite = img3;
        }

    }
    public void backw()
    {
        if (avtr.sprite == img3)
        {
            PlayerPrefs.SetInt("Img", 2);
            avtr.sprite = img2;
        }
        else
               if (avtr.sprite == img2)
        {
            PlayerPrefs.SetInt("Img", 1);
            avtr.sprite = img1;
        }
    }


    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetInt("Img", 1);
            avtr.sprite = img1;
        }
    }
}
