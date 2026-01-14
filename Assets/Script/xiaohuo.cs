using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public class xiaohuo : MonoBehaviour
{
    public ParticleSystem fire;
    [SerializeField]
    xiaohuoStart start;
    [SerializeField]
    xiaohuoStep1Obj step1;
    async void Start()
    {
        start.panel.gameObject.SetActive(true);
        start.text.text = "";
        await WaitForAnyBtnClick(new Button[] { start.button });
        start.panel.gameObject.SetActive(false);
        fire.Play();
        await Step1();
    }

    async UniTask Step1()
    {
        step1.panel.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step1.btn });
        step1.panel.gameObject.SetActive(false);
        start.camera.gameObject.SetActive(false);
        step1.camera.gameObject.SetActive(true);
        
    }
    async UniTask Step2() { }
    async UniTask Step3() { }
    async UniTask Step4() { }
    async UniTask Step5() { }

    protected async UniTask WaitForAnyBtnClick(Button[] btns, Action<Button> onClick = null)
    {
        if (btns == null || btns.Length == 0)
            return;

        var tasks = new List<UniTask>();
        var cancellationToken = this.GetCancellationTokenOnDestroy();

        foreach (var btn in btns)
        {
            // 使用 UniTask 的异步事件等待
            var task = btn.onClick.GetAsyncEventHandler(cancellationToken).OnInvokeAsync();
            tasks.Add(task.ContinueWith(() => onClick?.Invoke(btn)));
        }

        await UniTask.WhenAny(tasks);
    }
}
[Serializable]
public class xiaohuoStart
{
    public GameObject panel;
    public TextMeshProUGUI text;
    public Button button;
    public TextMeshProUGUI btnText;
    public Camera camera;
}
[Serializable]
public class xiaohuoStep1Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper xfs1;
    public SubjectClickHelper xfs2;
}