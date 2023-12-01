using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SpawnMonster : MonoBehaviour
{
    public GameObject Enermy1; // 폭발 이펙트 프리팹 변수
    public GameObject Enermy2;
    public GameObject Enermy3;

    public GameObject Spawn;
    public GameObject Ending;
    public GameObject Nomal;
    PlayerMove pm;
    PlayerFire pf;
    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.Find("Player").GetComponent<PlayerMove>();
        pf = GameObject.Find("Player").GetComponent<PlayerFire>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.sState == GameManager.GameWave.Wave1)
        {
            
            
            StartCoroutine(Wave1());
        }
        else if(GameManager.gm.sState == GameManager.GameWave.Wave2)
        {
          
            StartCoroutine(Wave3());
        }
        
        else if (GameManager.gm.sState == GameManager.GameWave.Wave3)
        {
           
            StartCoroutine(Wave4());
        }
        else if (GameManager.gm.sState.Equals("Clear"))
        {

            GameManager.gm.StageClear();
        }
        else if (GameManager.gm.sState.Equals("Start"))
        {
            GameManager.gm.StageStart();

        }

    }
    IEnumerator Wave1()
    {
        GameObject spw = Instantiate(Enermy1);
        spw.transform.position = Spawn.transform.position;
        Destroy(gameObject); //자신 자신을 제거한다. 
        yield return new WaitForSeconds(10);
    }
    IEnumerator Wave2()
    {
        GameObject spw = Instantiate(Enermy2);
        spw.transform.position = Spawn.transform.position;
        Destroy(gameObject); //자신 자신을 제거한다. 
        yield return new WaitForSeconds(10);
    }
    IEnumerator Wave3()
    {
        GameObject spw = Instantiate(Enermy3);
        spw.transform.position = Spawn.transform.position;
        Destroy(gameObject); //자신 자신을 제거한다. 
        yield return new WaitForSeconds(10);
    }
    IEnumerator Wave4()
    {   
        Nomal.SetActive(false);
        yield return new WaitForSeconds(10);
        Ending.SetActive(true);
    }

}

