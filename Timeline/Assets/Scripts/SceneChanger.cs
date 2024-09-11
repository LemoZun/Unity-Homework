using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ������ ���� �����͵� �ٽ� ���� ���鼭 �� �ڵ�� ���� ������ �ƴ� �ڵ�
// ������Ʈ�� ��� ��ư�� OnClick����  ChangeScene�� �����Ŵ
//oper.allowSceneActivation = true; true�� �ε��� ������ ���� ���� �� ����
// AsyncOperation ������ oper ������ oper.progress �� �����Ȳ�� Ȯ�� �� �� �ִ�.


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
        Debug.Log("��ư Ŭ����");
        if (loadingRoutine != null)
            return;

        loadingRoutine = StartCoroutine(LoadingRoutine(sceneName));
    }

    

    IEnumerator LoadingRoutine(string sceneName)
    {
        Debug.Log("�ڷ�ƾ �����");
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        oper.allowSceneActivation = true;
        loadingImage.gameObject.SetActive(true);

        while(oper.isDone == false)
        {
            if(oper.progress < 0.9f)
            {
                Debug.Log($" ���� �ε� : {oper.progress}");
                loadingBar.value = oper.progress;
            }
            else
            {
                Debug.Log("�ε� �Ϸ�");
                loadingBar.value = oper.progress;
                //oper.allowSceneActivation = true;

            }
            yield return null;
        }
    }
    
}
