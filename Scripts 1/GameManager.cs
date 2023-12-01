using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMove player;
    public GameObject gameLabel; // 게임 상태 UI 오브젝트 변수
    Text gameText; //게임 상태 UI텍스트 컴포넌트 변수
    public static GameManager gm;
    public Text point;
    public int money = 100;
    public Text point_shop;
    ButtonAction ba;
    EnemyFSM eFSM;
    public Text timeText;
    private float time;

    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
        time = 120f;
    }
    public enum GameState //게임 상태 상수
    {
        Ready,
        Run,
        Pause,
        Shoping,
        GameOver
    }

    public enum GameWave
    {
        Wave1,
        Wave2,
        Wave3,
        Wave4, 
        Clear,
        Clear1,
        Clear2,
        Clear3,
        Start
    }
    public GameState gState; //현재 게임 상태 변수
    public GameWave sState; //현재 게임 상태 변수



    public GameObject gameOption; // 옵션 화면 UI오브젝트 변수

    // Start is called before the first frame update
    void Start()
    {
        sState = GameWave.Clear;
        gState = GameState.Ready; //초기 게임 상태는 준비 상태로 설정
        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "Ready...";
        gameText.color = new Color32(255, 185, 0, 255);
        StartCoroutine(ReadyToStart()); //게임 준비-> 게임 중 상태로 전환
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f); //2초간 대기
        gameText.text = "Go!"; // 상태 텍스트를 "Go!"
        yield return new WaitForSeconds(0.5f); //0.5초 대기
        gameLabel.SetActive(false);//상태 텍스트를 비활성화
        gState = GameState.Run;//게임 상태를 '게임 중' 상태로 변경
    }
    // Update is called once per frame
    void Update()
    {
        if(sState == GameWave.Wave1 || sState == GameWave.Wave2 || sState == GameWave.Wave3)
        {
            if (time > 0)
                time -= Time.deltaTime;

            timeText.text = Mathf.Ceil(time).ToString();
            if (time == 0)
            {
                if(sState == GameWave.Wave1)
                {
                    sState = GameWave.Clear1;
                }
                else if (sState == GameWave.Wave2)
                {
                    sState = GameWave.Clear2;
                }else if (sState == GameWave.Wave3)
                {
                    sState = GameWave.Wave4;
                    timeText.text = "CLEAR";
                }
                
                    
                time = 120f;
            }
        }
        

        point_shop.text = money.ToString();
        point.text = money.ToString();
        if (player.hp <= 0) //플레이어가 죽었다면
        {
            //이동->대기 애니메이션 실행
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            gameLabel.SetActive(true); // 상태 텍스트 활성화
            gameText.text = "Game Over"; // 상태 텍스트를 Game Over로.
            gameText.color = new Color32(255, 0, 0, 255); //붉은색으로

            //상태 텍스트의 자식 오브잭트의 트랜스 폼 정보를 얻어옴
            Transform buttons = gameText.transform.GetChild(0);
            //버튼 오브잭트를 활성화
            buttons.gameObject.SetActive(true); 


            gState = GameState.GameOver; //게임 상태를 게임오버 상태로 변경

        }
    }
    
    public void OpenOptionWindow() //옵션 화면 켜기
    {
        gameObject.SetActive(true); // 옵션창 활성화
        Time.timeScale = 0f; // 게임속도 0배속(일시정지)
        gState= GameState.Pause; // 게임 상태 일시정지로 변경
    }

    public void CloseOptionWindow() //계속 하기 옵션
    {
        gameObject.SetActive(false); 
        Time.timeScale = 1f; 
        gState = GameState.Run; 
    }

    public void RestartOptionWindow() //다시 하기 옵션
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1); // LoadingScene을  실행
    }

    public void QuitGame() //종료 하기 옵션
    {
        Application.Quit();
    }
    public void StartShoping()
    {
        gState = GameState.Shoping;
        Cursor.visible = true;
        // Cursor.visible = false; 숨기기
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked; //마우스 커서 잠금
    }
    public void EndShoping()
    {
        gState = GameState.Run;
        Cursor.visible = false;
        // Cursor.visible = false; 숨기기
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.Locked; //마우스 커서 잠금
    }
    public void StageClear() {
        sState = GameWave.Clear;
    }
    public void StageStart()
    {
        sState = GameWave.Start;
    }


    public void Wave1()
    {
        sState = GameWave.Wave1;
    }
    public void Wave2()
    {
        sState = GameWave.Wave2;
    }
    public void Wave3()
    {
        sState = GameWave.Wave3;
    }
    public void Wave4()
    {
        sState = GameWave.Wave4;
    }
    public void NextWave()
    {
        print("go");
        if(sState == GameWave.Clear)
        {
            print("go2");
            sState = GameWave.Wave2;
        }
        else if (sState == GameWave.Clear1)
        {
            sState = GameWave.Wave3;
        }
        else if (sState == GameWave.Clear2)
        {
            sState = GameWave.Wave4;
        }
    }
}
