using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class ShopManager : MonoBehaviour
{
    public GameObject ShopEnter;
    public float ActiveDistance; //인식 가능 범위
    public GameObject PlayerUI;
    public GameObject ShopUI;
    Transform player; // 플레이어 트랜스폼

    GameManager gameManager;

    PlayerMove pm;
    PlayerFire pf;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pm = GameObject.Find("Player").GetComponent<PlayerMove>();
        pf = GameObject.Find("Player").GetComponent<PlayerFire>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < ActiveDistance)
        {
            ShopEnter.gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.F))

            {
                ShopUI.gameObject.SetActive(true);
                PlayerUI.gameObject.SetActive(false);
                ShopEnter.gameObject.SetActive(false);


                gameManager.StartShoping();

            }
        }
        else
        {
            ShopEnter.gameObject.SetActive(false);

        }


    }
    public void EndShoping()
    {
        PlayerUI.gameObject.SetActive(true);
        ShopUI.gameObject.SetActive(false);
        ShopEnter.gameObject.SetActive(true);

        gameManager.EndShoping();

    }
    public void BuyAttack()//돈 추가 개수 차감 미구현
    {
        //pf.weaponPower += 2;

    }
    public void BuyHealth()
    {
        pm.maxHp += 10;
        pm.hp += 10;

    }
    public void BuySpeed()
    {
        pm.moveSpeed += 0.3f;

    }

}
