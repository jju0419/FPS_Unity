using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingNextScene : MonoBehaviour
{
    public int sceneNumber = 2; // ������ �� ��ȣ
    public Slider loadingBar; // �ε� �����̴� ��
    public Text loadingText; // �ε� ���� �ؽ�Ʈ

    private void Start()
    {
        StartCoroutine(TransitionNextScene(sceneNumber));
    }
    IEnumerator TransitionNextScene(int num)
    {   
        // ������ ���� �񵿱� �������� �ε��Ѵ�
        AsyncOperation ao = SceneManager.LoadSceneAsync(num);
        // 
        ao.allowSceneActivation = false;
        while (!ao.isDone) // �ε��� �Ϸ�ɶ����� ��������� ǥ���Ѵ�
        {
            //���� ������� �����̴� �ٿ� �ؽ�Ʈ�� ǥ��
            loadingBar.value = ao.progress;
            loadingText.text = 
                (ao.progress * 100f).ToString() +"%";
            //������� 90���� �̻� �Ǿ��ٸ�
            if(ao.progress >= 0.9f)
            {
                //�ε�� ���� ȭ�鿡 ���̰� ��
                ao.allowSceneActivation= true;
            }
            yield return null; //���� �������� �ɶ����� ��ٸ�
        }
    }
    
}
