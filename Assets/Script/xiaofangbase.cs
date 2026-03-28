using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class xiaofangbase : MonoBehaviour
{
    [SerializeField]
    protected LogPanelObj logPanel;
    [SerializeField]
    protected AudioSource fireAudio;
    [SerializeField]
    protected AudioSource yuyingAudio;
    protected bool isend = false;
    protected string endText = "";
    protected int score = 100;
    protected poin poin;
    void Awake()
    {
        poin = GetComponent<poin>();
    }
    protected virtual async void Start()
    {
        Init();
        await StepStart();
        while (!isend)
        {
            await LogPanel();
            Init();
            score -= 20;
            score = math.clamp(score, 0, 100);
            await StepStart();
        }
        endText = $"恭喜你完成了全部的任务！\n最终得分：{score}";
        await LogPanel();
        await poin.UpdateScore(score);
    }
    protected virtual void Init()
    {

    }
    protected virtual async UniTask StepStart()
    {

    }
    protected void PlayYuying(AudioClip clip)
    {
        yuyingAudio.clip = clip;
        yuyingAudio.Play();
    }
    async UniTask LogPanel()
    {
        logPanel.panel.gameObject.SetActive(true);
        logPanel.text.text = endText;
        logPanel.btn.GetComponentInChildren<TextMeshProUGUI>().text = isend ? "提交成绩" : "重新开始";
        await WaitForAnyBtnClick(new Button[] { logPanel.btn });
        logPanel.panel.gameObject.SetActive(false);
    }
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
    protected async UniTask<int> AttachIndex(UniTask task, int index)
    {
        await task;
        return index;
    }
}
[Serializable]
public class LogPanelObj
{
    public GameObject panel;
    public Button btn;
    public TextMeshProUGUI text;
}