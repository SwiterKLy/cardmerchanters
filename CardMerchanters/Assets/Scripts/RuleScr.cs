using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleScr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField]
    private GameObject rules; // ���� �����
    [SerializeField]
    private GameObject page1; // ���� �����
    [SerializeField]
    private GameObject page2; // ���� �����
    [SerializeField]
    private GameObject page3; // ���� �����
    [SerializeField]
    private GameObject buttonF; // ���� �����
    [SerializeField]
    private GameObject buttonB; // ���� �����

    public void BuyACard()
    {
        if (rules.active == false)
        {
            rules.SetActive(true);
        }
        else { rules.SetActive(false); };
    }

    public void GoForward()
    {
        if (page1.active == true)
        {
            page1.SetActive(false);
            page2.SetActive(true);
            buttonB.SetActive(true);
        }
        else if (page2.active == true)
        {
            page2.SetActive(false);
            page3.SetActive(true);
            buttonF.SetActive(false);
        }
        else { };
    }
    public void GoBackward()
    {
        if (page3.active == true)
        {
            page3.SetActive(false);
            page2.SetActive(true);
            buttonF.SetActive(true);
        }
        else if (page2.active == true)
        {
            page2.SetActive(false);
            page1.SetActive(true);
            buttonB.SetActive(false);
        }
        else { };
    }
}
