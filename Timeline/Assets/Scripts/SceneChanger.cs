using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    [SerializeField] Image loadingImage;
    [SerializeField] Slider loadingBar;
    Coroutine loadingRoutine;
    public static SceneChanger Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(string sceneName)
    {
        Debug.Log("버튼 클릭됨");
        if (loadingRoutine != null)
            return;

        loadingRoutine = StartCoroutine(LoadingRoutine(sceneName));
    }

    

    IEnumerator LoadingRoutine(string sceneName)
    {
        Debug.Log("코루틴 실행됨");
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        oper.allowSceneActivation = true;
        loadingImage.gameObject.SetActive(true);

        while(oper.isDone == false)
        {
            if(oper.progress < 0.9f)
            {
                Debug.Log($" 현재 로딩 : {oper.progress}");
                loadingBar.value = oper.progress;
            }
            else
            {
                Debug.Log("로딩 완료");
                loadingBar.value = oper.progress;
                //oper.allowSceneActivation = true;

            }
            yield return null;
        }
    }
    
}
