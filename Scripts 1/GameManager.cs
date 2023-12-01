using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerMove player;
    public GameObject gameLabel; // ���� ���� UI ������Ʈ ����
    Text gameText; //���� ���� UI�ؽ�Ʈ ������Ʈ ����
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
    public enum GameState //���� ���� ���
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
    public GameState gState; //���� ���� ���� ����
    public GameWave sState; //���� ���� ���� ����



    public GameObject gameOption; // �ɼ� ȭ�� UI������Ʈ ����

    // Start is called before the first frame update
    void Start()
    {
        sState = GameWave.Clear;
        gState = GameState.Ready; //�ʱ� ���� ���´� �غ� ���·� ����
        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "Ready...";
        gameText.color = new Color32(255, 185, 0, 255);
        StartCoroutine(ReadyToStart()); //���� �غ�-> ���� �� ���·� ��ȯ
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f); //2�ʰ� ���
        gameText.text = "Go!"; // ���� �ؽ�Ʈ�� "Go!"
        yield return new WaitForSeconds(0.5f); //0.5�� ���
        gameLabel.SetActive(false);//���� �ؽ�Ʈ�� ��Ȱ��ȭ
        gState = GameState.Run;//���� ���¸� '���� ��' ���·� ����
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
        if (player.hp <= 0) //�÷��̾ �׾��ٸ�
        {
            //�̵�->��� �ִϸ��̼� ����
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            gameLabel.SetActive(true); // ���� �ؽ�Ʈ Ȱ��ȭ
            gameText.text = "Game Over"; // ���� �ؽ�Ʈ�� Game Over��.
            gameText.color = new Color32(255, 0, 0, 255); //����������

            //���� �ؽ�Ʈ�� �ڽ� ������Ʈ�� Ʈ���� �� ������ ����
            Transform buttons = gameText.transform.GetChild(0);
            //��ư ������Ʈ�� Ȱ��ȭ
            buttons.gameObject.SetActive(true); 


            gState = GameState.GameOver; //���� ���¸� ���ӿ��� ���·� ����

        }
    }
    
    public void OpenOptionWindow() //�ɼ� ȭ�� �ѱ�
    {
        gameObject.SetActive(true); // �ɼ�â Ȱ��ȭ
        Time.timeScale = 0f; // ���Ӽӵ� 0���(�Ͻ�����)
        gState= GameState.Pause; // ���� ���� �Ͻ������� ����
    }

    public void CloseOptionWindow() //��� �ϱ� �ɼ�
    {
        gameObject.SetActive(false); 
        Time.timeScale = 1f; 
        gState = GameState.Run; 
    }

    public void RestartOptionWindow() //�ٽ� �ϱ� �ɼ�
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1); // LoadingScene��  ����
    }

    public void QuitGame() //���� �ϱ� �ɼ�
    {
        Application.Quit();
    }
    public void StartShoping()
    {
        gState = GameState.Shoping;
        Cursor.visible = true;
        // Cursor.visible = false; �����
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked; //���콺 Ŀ�� ���
    }
    public void EndShoping()
    {
        gState = GameState.Run;
        Cursor.visible = false;
        // Cursor.visible = false; �����
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.Locked; //���콺 Ŀ�� ���
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
