using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingNextScene : MonoBehaviour
{
    public int sceneNumber = 2; // 진행할 씬 번호
    public Slider loadingBar; // 로딩 슬라이더 바
    public Text loadingText; // 로딩 진행 텍스트

    private void Start()
    {
        StartCoroutine(TransitionNextScene(sceneNumber));
    }
    IEnumerator TransitionNextScene(int num)
    {   
        // 지정된 씬을 비동기 형식으로 로드한다
        AsyncOperation ao = SceneManager.LoadSceneAsync(num);
        // 
        ao.allowSceneActivation = false;
        while (!ao.isDone) // 로딩이 완료될때까지 진행과정을 표시한다
        {
            //현재 진행률을 슬라이더 바와 텍스트로 표시
            loadingBar.value = ao.progress;
            loadingText.text = 
                (ao.progress * 100f).ToString() +"%";
            //진행률이 90프로 이상 되었다면
            if(ao.progress >= 0.9f)
            {
                //로드된 씬을 화면에 보이게 함
                ao.allowSceneActivation= true;
            }
            yield return null; //다음 프레임이 될때까지 기다림
        }
    }
    
}
