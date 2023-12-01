using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    public ShopManager ShopManager;
    public Text number1, number2, number3, number4,
               price1, price2, price3, price4;
    public int maxnum = 10;
    public int value = 100;
    public GameObject nm;
    private void Start()
    {
    }
    private void Update()
    {
    }

    public void OnClick()
    {
        nm.SetActive(false);
        ShopManager.EndShoping();
        GameManager.gm.NextWave();
     
    }
    public void OnClick1()
    {
        if (maxnum > 0)
        {
            if(GameManager.gm.money >= value)
            {
                ShopManager.BuyAttack();
                maxnum = maxnum - 1;
                number1.text = maxnum + "/10";
                GameManager.gm.money -= value;
                value += (int)((float)value * 0.2f);
            }
            else
            {
                nm.SetActive(true);
            }
        }
        return;
    }
    public void OnClick2()
    {
        if (maxnum > 0)
        {
            if (GameManager.gm.money >= value)
            {
                ShopManager.BuyHealth();
                maxnum = maxnum - 1;
                number2.text = maxnum + "/10";
                GameManager.gm.money -= value;
                value += (int)((float)value * 0.2f);

            }
            else
            {
                nm.SetActive(true);
            }
        }
        return;


    }
    public void OnClick3()
    {
        if (maxnum > 0)
        {
            if (GameManager.gm.money >= value)
            {
                ShopManager.BuySpeed();
                maxnum = maxnum - 1;
                number3.text = maxnum + "/10";
                GameManager.gm.money -= value;
                value += (int)((float)value * 0.2f);

            }
            else
            {
                nm.SetActive(true);
            }
        }
        return;

    }
    public void OnClick4()
    {
        if (maxnum > 0)
        {
            if (GameManager.gm.money >= value)
            {
                maxnum = maxnum - 1;
                number4.text = maxnum + "/10";
                GameManager.gm.money -= value;
                value += (int)((float)value * 0.2f);

            }
            else
            {
                nm.SetActive(true);
            }
        }
        return;
    }
}
