using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsScript : MonoBehaviour
{
    public Image TLogo;
    public Text TName, TAttack, THP, TCumcost, TAbility;
    public GameObject Prefab;
    // Start is called before the first frame update
    void Start()
    {
        ChangeDescription();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeDescription()
    {
       TLogo = Prefab.transform.GetChild(0).GetComponent<Image>();
       TName = Prefab.transform.GetChild(1).GetComponent<Text>();
       TAttack = Prefab.transform.GetChild(3).GetComponent<Text>();
       THP = Prefab.transform.GetChild(4).GetComponent<Text>();
       TCumcost = Prefab.transform.GetChild(5).GetComponent<Text>();
       TAbility = Prefab.transform.GetChild(6).GetComponent<Text>();
        Debug.Log("Работает");
    }


}

