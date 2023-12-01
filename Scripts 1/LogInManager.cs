using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInManager : MonoBehaviour
{
    public InputField id;// ����� ���̵�
    public InputField password;// ����� �н�����
    public Text notify; // �˻� �ؽ�Ʈ ����


    // Start is called before the first frame update
    void Start()
    {
        notify.text = "";
    }
    public void SaveUserData()
    {
        if (!PlayerPrefs.HasKey(id.text))
        {
            // ������� ���̵�� Ű(Key)�� �н������ ��(value)�� ����
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "���̵� ������ �Ϸ�Ǿ����ϴ�.";
        }
        else
        {
            notify.text = "�̹� �����ϴ� ���̵��Դϴ�.";

        }
    }
    bool CheckInput(string id, string pwd)
    {
        if (id == "" || pwd == "") //�Է�ĭ�� ���ִ°��
        {
            notify.text = "���̵� �Ǵ� �н����带 �Է��ϼ���.";
            return false;
        }
        else return true;
    }
    public void CheckUserData()
    {
        if(!CheckInput(id.text, password.text)) { return; }
        string pass = PlayerPrefs.GetString(id.text);   
        if(password.text == pass)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            notify.text = "�Է��Ͻ� ���̵�� �н����尡" +
                "��ġ���� �ʽ��ϴ�.";
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
