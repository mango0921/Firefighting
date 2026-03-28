using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class dayan : xiaofangbase
{

    [SerializeField]
    dayanAudio audios;

    [SerializeField]
    private dayanStart start;
    [SerializeField]
    private dayanStep1Obj step1;
    [SerializeField]
    private dayanStep2Obj step2;
    [SerializeField]
    private dayanStep3Obj step3;
    [SerializeField]
    private dayanStep4Obj step4;
    [SerializeField]
    private dayanStep5Obj step5;
    [SerializeField]
    private dayanStep6Obj step6;
    [SerializeField]
    private dayanStepEndObj end;

    protected override void Start()
    {
        base.Start();
    }

    protected override async UniTask StepStart()
    {
        PlayYuying(audios.dayanstart);
        fireAudio.Play();
        start.panel.gameObject.SetActive(true);
        start.camera.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { start.btn });
        start.panel.gameObject.SetActive(false);

        await Step1();
    }
    async UniTask Step1()
    {
        PlayYuying(audios.dayan1);
        step1.panel.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step1.btn });
        step1.panel.gameObject.SetActive(false);
        await Step2();
    }
    async UniTask Step2()
    {
        PlayYuying(audios.dayan2);
        step2.panel.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step2.btn });
        step2.panel.gameObject.SetActive(false);
        await Step3();
    }
    async UniTask Step3()
    {
        PlayYuying(audios.dayan3);
        start.camera.gameObject.SetActive(false);
        step3.panel.gameObject.SetActive(true);
        step3.camera.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step3.btn });
        step3.gq1.gameObject.SetActive(true);
        step3.gq2.gameObject.SetActive(true);
        step3.gq3.gameObject.SetActive(true);
        step3.btn.gameObject.SetActive(false);
        var _ = step3.gq1.WaitObjectClick();
        var _1 = step3.gq2.WaitObjectClick();
        var _2 = step3.gq3.WaitObjectClick();
        var firstCompletedTask = await UniTask.WhenAny(_, _1, _2);

        if (firstCompletedTask == 0) // 表示第一个任务 `_` 先完成了
        {
            step3.gq2.CancelWait();
            step3.gq3.CancelWait();
            step3.gq1.gameObject.SetActive(false);
            step3.gq2.gameObject.SetActive(false);
            step3.gq3.gameObject.SetActive(false);
            step3.panel.gameObject.SetActive(false);
            step3.camera.gameObject.SetActive(false);
            await Step4();
        }
        else if (firstCompletedTask == 1) // 表示第二个任务 `_1` 先完成了
        {
            step3.gq1.CancelWait();
            step3.gq3.CancelWait();
            endText = "错误，应尽量选择有水源、有外窗的房间。";
        }
        else if (firstCompletedTask == 2) // 表示第三个任务 `_2` 先完成了
        {
            step3.gq1.CancelWait();
            step3.gq2.CancelWait();
            endText = "错误，应尽量选择有水源、有外窗的房间。";
        }
    }
    async UniTask Step4()
    {
        PlayYuying(audios.dayan4);
        step4.panel.gameObject.SetActive(true);
        step4.camera.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step4.btn });
        step4.btn.gameObject.SetActive(false);
        var _1 = AttachIndex(step4.men.WaitObjectClick(), 1);
        var _2 = AttachIndex(step4.chuang.WaitObjectClick(), 2);
        var xfs = new List<SubjectClickHelper> { step4.men, step4.chuang };
        var _ = new List<UniTask<int>> { _1, _2 };
        await foreach (var completedIndex in UniTask.WhenEach(_))
        {
            if (completedIndex.GetResult() == 1)
            {
                step4.men.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
            }
            else if (completedIndex.GetResult() == 2)
            {
                step4.chuang.transform.DOLocalMove(new Vector3(3.03123832f, 2.87936664f, 6.52600002f), 1f);
            }
        }
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        step4.panel.gameObject.SetActive(false);
        step4.camera.gameObject.SetActive(false);
        await Step5();
    }
    async UniTask Step5()
    {
        PlayYuying(audios.dayan5);
        step5.panel.gameObject.SetActive(true);
        step5.camera.gameObject.SetActive(true);
        step5.yanwu.Play();
        await WaitForAnyBtnClick(new Button[] { step5.btn });
        step5.btn.gameObject.SetActive(false);
        step5.maojing.gameObject.SetActive(true);
        await step5.maojing.WaitObjectClick();

        step5.maojingObj.SetActive(true);
        step5.maojing.gameObject.SetActive(false);
        step5.yanwu.Stop();
        step5.panel.gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        step5.camera.gameObject.SetActive(false);
        await Step6();
    }
    async UniTask Step6()
    {
        PlayYuying(audios.dayan6);
        step6.lianpen.gameObject.SetActive(true);
        step6.panel.gameObject.SetActive(true);
        step6.camera.gameObject.SetActive(true);
        step6.lianpen.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step6.btn });
        step6.btn.gameObject.SetActive(false);
        await step6.lianpen.WaitObjectClick();
        step6.lianpenAnim.Play("poshui");
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        step6.shui.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        step6.panel.gameObject.SetActive(false);
        await StepEnd();
    }
    async UniTask StepEnd()
    {
        PlayYuying(audios.dayanend);
        end.panel.gameObject.SetActive(true);
        end.camera.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { end.btn });
        end.btn.gameObject.SetActive(false);
        await end.chuang.WaitObjectClick();
        await end.chuang.transform.DOLocalMove(new Vector3(-0.0142754931f, 0.702550709f, 0.42899999f), 1f);
        isend = true;
    }
    protected override void Init()
    {
        yuyingAudio.Stop();

        step3.gq1.gameObject.SetActive(false);
        step3.gq2.gameObject.SetActive(false);
        step3.gq3.gameObject.SetActive(false);

        start.panel.gameObject.SetActive(true);
        step1.panel.gameObject.SetActive(false);
        step2.panel.gameObject.SetActive(false);
        step3.panel.gameObject.SetActive(false);
        step4.panel.gameObject.SetActive(false);
        step5.panel.gameObject.SetActive(false);
        step6.panel.gameObject.SetActive(false);
        end.panel.gameObject.SetActive(false);

        start.camera.gameObject.SetActive(true);
        step3.camera.gameObject.SetActive(false);
        step4.camera.gameObject.SetActive(false);
        step5.camera.gameObject.SetActive(false);
        step6.camera.gameObject.SetActive(false);
        end.camera.gameObject.SetActive(false);

        start.btn.gameObject.SetActive(true);
        step1.btn.gameObject.SetActive(true);
        step2.btn.gameObject.SetActive(true);
        step3.btn.gameObject.SetActive(true);
        step4.btn.gameObject.SetActive(true);
        step5.btn.gameObject.SetActive(true);
        step6.btn.gameObject.SetActive(true);
        end.btn.gameObject.SetActive(true);

        step4.men.transform.localRotation = Quaternion.Euler(0, -95, 0);
        step4.chuang.transform.localPosition = new Vector3(3.03123832f, 2.87936664f, 7.40700006f);

        step5.maojing.gameObject.SetActive(false);
        step5.maojingObj.SetActive(false);
        step5.yanwu.Stop();

        step6.lianpen.gameObject.SetActive(false);

        end.chuang.transform.localPosition = new Vector3(-0.0142754931f, 0.702550709f, -0.437000006f);
    }
}
[Serializable]
public class dayanStart
{
    public GameObject panel;
    public Button btn;
    public Camera camera;

}
[Serializable]
public class dayanStep1Obj
{
    public GameObject panel;
    public Button btn;
}
[Serializable]
public class dayanStep2Obj
{
    public GameObject panel;
    public Button btn;
}
[Serializable]
public class dayanStep3Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper gq1;
    public SubjectClickHelper gq2;
    public SubjectClickHelper gq3;
}
[Serializable]
public class dayanStep4Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper men;
    public SubjectClickHelper chuang;
}
[Serializable]
public class dayanStep5Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper maojing;
    public ParticleSystem yanwu;
    public GameObject maojingObj;
}
[Serializable]
public class dayanStep6Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper lianpen;
    public Animator lianpenAnim;
    public ParticleSystem shui;
}
[Serializable]
public class dayanStepEndObj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper chuang;
}
[Serializable]
public class dayanAudio
{
    public AudioClip dayanstart;
    public AudioClip dayan1;
    public AudioClip dayan2;
    public AudioClip dayan3;
    public AudioClip dayan4;
    public AudioClip dayan5;
    public AudioClip dayan6;
    public AudioClip dayanend;
}