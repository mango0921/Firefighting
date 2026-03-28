using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using QuickOutline;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class dahuo : xiaofangbase
{
    public ParticleSystem fire;
    public ParticleSystem water;
    [SerializeField]
    dahuoAudio audios;
    [SerializeField]
    private dahuoStart start;
    [SerializeField]
    private dahuoStep1Obj step1;
    [SerializeField]
    private dahuoStep2Obj step2;
    [SerializeField]
    private dahuoStep3Obj step3;
    [SerializeField]
    private dahuoStep4Obj step4;
    [SerializeField]
    private dahuoStepEndObj end;
    protected override async void Start()
    {
        base.Start();
    }
    protected override void Init()
    {
        fire.transform.localScale = new Vector3(1f, 1f, 1f);
        water.Stop();
        fire.Stop();
        fireAudio.Stop();
        fireAudio.volume = 0.5f;
        yuyingAudio.Stop();

        step1.plane1.SetActive(false);
        step1.plane2.SetActive(false);

        start.panel.gameObject.SetActive(true);
        step1.panel.gameObject.SetActive(false);
        step2.panel.gameObject.SetActive(false);
        step3.panel.gameObject.SetActive(false);
        step4.panel.gameObject.SetActive(false);
        end.panel.gameObject.SetActive(false);

        start.camera.gameObject.SetActive(true);
        step1.camera.gameObject.SetActive(false);
        step2.camera.gameObject.SetActive(false);
        step3.camera.gameObject.SetActive(false);
        step4.camera.gameObject.SetActive(false);
        end.camera.gameObject.SetActive(false);

        start.btn.gameObject.SetActive(true);
        step1.yesbtn.gameObject.SetActive(true);
        step1.nobtn.gameObject.SetActive(true);
        step2.btn.gameObject.SetActive(true);
        step3.btn.gameObject.SetActive(true);
        step4.btn.gameObject.SetActive(true);
        end.btn.gameObject.SetActive(true);

        step3.xfs1.gameObject.SetActive(true);
        step3.xfs2.gameObject.SetActive(true);
        step4.jiantou1.gameObject.SetActive(false);
        step4.jiantou2.gameObject.SetActive(false);
        step4.jiantou3.gameObject.SetActive(false);
        step4.jiantou4.gameObject.SetActive(false);
        step4.shuiguan.gameObject.SetActive(false);
        end.shuiguan.gameObject.SetActive(false);

        step2.xfs1.GetComponent<Collider>().enabled = true;
        step1.kqkg.transform.localEulerAngles = Vector3.zero;
        step3.men.transform.localEulerAngles = new Vector3(0, -180, 0);
    }
    protected override async UniTask StepStart()
    {
        PlayYuying(audios.dahuostart);
        start.panel.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { start.btn });
        start.panel.gameObject.SetActive(false);
        fire.Play();
        fireAudio.Play();
        await Step1();
    }
    async UniTask Step1()
    {
        PlayYuying(audios.dahuo1);
        step1.panel.gameObject.SetActive(true);
        var _ = WaitForAnyBtnClick(new Button[] { step1.yesbtn });
        var _1 = WaitForAnyBtnClick(new Button[] { step1.nobtn });
        var num = await UniTask.WhenAny(_, _1);
        step1.panel.gameObject.SetActive(false);
        if (num == 0)
        {
            endText = "选择错误，应先断开电源！";
        }
        else
        {
            start.camera.gameObject.SetActive(false);
            step1.camera.gameObject.SetActive(true);
            await step1.kqkg.WaitObjectClick();
            await step1.kqkg.transform.DORotate(new Vector3(0, 0, -70), 1f);
            step1.plane1.SetActive(true);
            step1.plane2.SetActive(true);
            await Step2();
        }
    }
    async UniTask Step2()
    {
        PlayYuying(audios.dahuo2);
        step1.panel.gameObject.SetActive(false);
        step2.panel.gameObject.SetActive(true);
        //step2.camera.enabled = true;

        await WaitForAnyBtnClick(new Button[] { step2.btn });
        step2.btn.gameObject.SetActive(false);
        step1.camera.gameObject.SetActive(false);
        step2.camera.gameObject.SetActive(true);
        var _ = step2.xfs1.WaitObjectClick();
        var _2 = step2.xfs2.WaitObjectClick();
        var firstCompletedTask = await UniTask.WhenAny(_, _2);
        if (firstCompletedTask == 0) // 表示第一个任务 `_` 先完成了
        {
            step2.xfs2.CancelWait();
            await Step3();
        }
        else if (firstCompletedTask == 1) // 表示第二个任务 `_1` 先完成了
        {
            step2.xfs1.CancelWait();
            endText = "没有就近找寻消防栓，火势变大，任务失败。";
            //todo 大火重开
        }
    }
    async UniTask Step3()
    {
        PlayYuying(audios.dahuo3);
        step2.xfs1.GetComponent<Collider>().enabled = false;
        step2.panel.gameObject.SetActive(false);
        step3.panel.gameObject.SetActive(true);
        step2.camera.gameObject.SetActive(false);
        step3.camera.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step3.btn });
        step3.btn.gameObject.SetActive(false);
        await step3.men.transform.DORotate(new Vector3(0, -90, 0), 1f);
        var _1 = AttachIndex(step3.xfs1.WaitObjectClick(), 1);
        var _2 = AttachIndex(step3.xfs2.WaitObjectClick(), 2);
        var _3 = AttachIndex(step3.xfs3.WaitObjectClick(), 3);
        var _4 = AttachIndex(step3.xfs4.WaitObjectClick(), 4);
        var xfs = new List<SubjectClickHelper> { step3.xfs1, step3.xfs2, step3.xfs3, step3.xfs4 };
        var _ = new List<UniTask<int>> { _1, _2, _3, _4 };
        int expectedOrder = 1;
        await foreach (var completedIndex in UniTask.WhenEach(_))
        {
            if (completedIndex.GetResult() != expectedOrder)
            {
                endText = "安装顺序错误！";
                for (int i = 0; i < _.Count; i++)
                {
                    if (i != completedIndex.GetResult() - 1)
                    {
                        xfs[i].CancelWait();
                    }
                }
                return;
            }
            expectedOrder++;
        }
        await Step4();
    }
    async UniTask Step4()
    {
        PlayYuying(audios.dahuo4);
        step3.camera.gameObject.SetActive(false);
        step4.camera.gameObject.SetActive(true);
        step3.xfs1.gameObject.SetActive(false);
        step3.xfs2.gameObject.SetActive(false);
        step4.shuiguan.gameObject.SetActive(true);
        step3.panel.gameObject.SetActive(false);
        step4.panel.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step4.btn });
        step4.btn.gameObject.SetActive(false);
        step4.jiantou1.gameObject.SetActive(true);
        step4.jiantou2.gameObject.SetActive(true);
        step4.jiekou1.OpenFlashHighLight();
        var _ = step4.jiantou1.WaitObjectClick();
        var _2 = step4.jiantou2.WaitObjectClick();
        var firstCompletedTask = await UniTask.WhenAny(_, _2);
        step4.jiekou1.CloseFlashHighLight();
        if (firstCompletedTask == 0) // 表示第一个任务 `_` 先完成了
        {
            step4.jiantou2.CancelWait();
        }
        else if (firstCompletedTask == 1) // 表示第二个任务 `_1` 先完成了
        {
            step4.jiantou1.CancelWait();
            endText = "旋转方向错误！";
            return;
        }
        step4.jiantou1.gameObject.SetActive(false);
        step4.jiantou2.gameObject.SetActive(false);
        step4.jiantou3.gameObject.SetActive(true);
        step4.jiantou4.gameObject.SetActive(true);
        step4.jiekou2.OpenFlashHighLight();
        var _3 = step4.jiantou3.WaitObjectClick();
        var _4 = step4.jiantou4.WaitObjectClick();
        firstCompletedTask = await UniTask.WhenAny(_3, _4);
        step4.jiekou2.CloseFlashHighLight();
        if (firstCompletedTask == 0) // 表示第一个任务 `_` 先完成了
        {
            step4.jiantou3.CancelWait();

        }
        else if (firstCompletedTask == 1) // 表示第二个任务 `_1` 先完成了
        {
            step4.jiantou4.CancelWait();
            endText = "旋转方向错误！";
            return;
        }
        await StepEnd();
    }
    async UniTask StepEnd()
    {

        step4.panel.gameObject.SetActive(false);
        end.camera.gameObject.SetActive(true);
        end.shuiguan.gameObject.SetActive(true);
        water.Play();
        water.GetComponent<AudioSource>().Play();
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        DOTween.To(() => fireAudio.volume, x => fireAudio.volume = x, 0f, 2f);
        await fire.transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 2f);
        fire.Stop();
        fireAudio.Stop();
        end.panel.gameObject.SetActive(true);
        PlayYuying(audios.dahuoend);
        await WaitForAnyBtnClick(new Button[] { end.btn });
        end.btn.gameObject.SetActive(false);
        water.GetComponent<AudioSource>().Stop();
        water.Stop();
        isend = true;
    }
}
[Serializable]
public class dahuoStart
{
    public GameObject panel;
    public TextMeshProUGUI text;
    public Button btn;
    public TextMeshProUGUI btnText;
    public Camera camera;
}
[Serializable]
public class dahuoStep1Obj
{
    public GameObject panel;
    public Button yesbtn;
    public Button nobtn;
    public Camera camera;
    public SubjectClickHelper kqkg;
    public GameObject plane1;
    public GameObject plane2;
}

[Serializable]
public class dahuoStep2Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper xfs1;
    public SubjectClickHelper xfs2;
}
[Serializable]
public class dahuoStep3Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public GameObject men;
    public SubjectClickHelper xfs1;
    public SubjectClickHelper xfs2;
    public SubjectClickHelper xfs3;
    public SubjectClickHelper xfs4;
}
[Serializable]
public class dahuoStep4Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public GameObject shuiguan;
    public SubjectClickHelper jiantou1;
    public SubjectClickHelper jiantou2;
    public SubjectClickHelper jiantou3;
    public SubjectClickHelper jiantou4;
    public GameObject jiekou1;
    public GameObject jiekou2;
}
[Serializable]
public class dahuoStepEndObj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public GameObject shuiguan;
}
[Serializable]
public class dahuoAudio
{
    public AudioClip dahuostart;
    public AudioClip dahuo1;
    public AudioClip dahuo2;
    public AudioClip dahuo3;
    public AudioClip dahuo4;
    public AudioClip dahuoend;
}