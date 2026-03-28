using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class xiaohuo : xiaofangbase
{

    public ParticleSystem fire;
    public ParticleSystem miehuoqi;
    public ParticleSystem smoke;
    [SerializeField]
    xiaohuoAudio audios;
    [SerializeField]
    xiaohuoStart start;
    [SerializeField]
    xiaohuoStep1Obj step1;
    [SerializeField]
    xiaohuoStep2Obj step2;
    [SerializeField]
    xiaohuoStep3Obj step3;
    [SerializeField]
    xiaohuoStep4Obj step4;
    [SerializeField]
    xiaohuoStep5Obj step5;
    [SerializeField]
    xiaohuoStepEndObj end;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Init()
    {
        yuyingAudio.Stop();

        start.panel.gameObject.SetActive(true);
        step1.panel.gameObject.SetActive(false);
        step2.panel.gameObject.SetActive(false);
        step3.panel.gameObject.SetActive(false);
        step4.panel.gameObject.SetActive(false);
        step5.panel.gameObject.SetActive(false);
        end.panel.gameObject.SetActive(false);

        start.camera.gameObject.SetActive(true);
        step1.camera.gameObject.SetActive(false);
        step2.camera.gameObject.SetActive(false);
        step3.camera.gameObject.SetActive(false);
        step4.camera.gameObject.SetActive(false);
        step5.camera.gameObject.SetActive(false);
        end.camera.gameObject.SetActive(false);

        start.btn.gameObject.SetActive(true);
        step1.btn.gameObject.SetActive(true);
        step2.btn.gameObject.SetActive(true);
        step3.btn.gameObject.SetActive(true);
        step4.btn.gameObject.SetActive(true);
        step5.btn.gameObject.SetActive(true);
        end.btn.gameObject.SetActive(true);

        step2.mhq1.gameObject.SetActive(false);
        step2.mhq2.gameObject.SetActive(false);
        step3.human.gameObject.SetActive(false);
        step4.human.gameObject.SetActive(false);
        step5.jiantou1.gameObject.SetActive(false);
        step5.jiantou2.gameObject.SetActive(false);
        step5.jiantou3.gameObject.SetActive(false);

        step3.gq1.gameObject.SetActive(false);
        step3.gq2.gameObject.SetActive(false);
        step3.gq3.gameObject.SetActive(false);

        fire.Stop();
        fire.Clear();
        fireAudio.Stop();
        fireAudio.volume = 0.5f;
        miehuoqi.Stop();
        miehuoqi.Clear();
        smoke.Stop();
        smoke.Clear();
        fire.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    protected override async UniTask StepStart()
    {
        PlayYuying(audios.xiaohuostart);
        start.panel.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { start.btn });
        start.panel.gameObject.SetActive(false);
        fire.Play();
        fireAudio.Play();
        smoke.Play();
        await Step1();
    }
    async UniTask Step1()
    {
        PlayYuying(audios.xiaohuo1);
        step1.panel.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step1.btn });
        step1.btn.gameObject.SetActive(false);
        start.camera.gameObject.SetActive(false);
        step1.camera.gameObject.SetActive(true);
        var _ = step1.xfs1.WaitObjectClick();
        var _1 = step1.xfs2.WaitObjectClick();
        var firstCompletedTask = await UniTask.WhenAny(_, _1);
        if (firstCompletedTask == 0) // 表示第一个任务 `_` 先完成了
        {
            step1.xfs2.CancelWait();
            await Step2();
        }
        else if (firstCompletedTask == 1) // 表示第二个任务 `_1` 先完成了
        {
            step1.xfs1.CancelWait();
            endText = "没有就近找寻灭火器，火势变大，任务失败。";
            //todo 大火重开
        }
    }
    async UniTask Step2()
    {
        PlayYuying(audios.xiaohuo2);
        step2.mhq1.gameObject.SetActive(true);
        step2.mhq2.gameObject.SetActive(true);
        step1.camera.gameObject.SetActive(false);
        step2.camera.gameObject.SetActive(true);
        step1.panel.gameObject.SetActive(false);
        step2.panel.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step2.btn });
        step2.btn.gameObject.SetActive(false);

        var _ = step2.mhq1.WaitObjectClick();
        var _1 = step2.mhq2.WaitObjectClick();
        var firstCompletedTask = await UniTask.WhenAny(_, _1);
        if (firstCompletedTask == 0) // 表示第一个任务 `_` 先完成了
        {
            step2.mhq2.CancelWait();
            await Step3();
        }
        else if (firstCompletedTask == 1) // 表示第二个任务 `_1` 先完成了
        {
            step2.mhq1.CancelWait();
            endText = "选择了错误的灭火器，任务失败。";
        }
    }
    async UniTask Step3()
    {
        PlayYuying(audios.xiaohuo3);
        step3.human.gameObject.SetActive(true);
        step2.panel.gameObject.SetActive(false);
        step3.panel.gameObject.SetActive(true);
        step2.camera.gameObject.SetActive(false);
        step3.camera.gameObject.SetActive(true);
        step3.gq1.gameObject.SetActive(true);
        step3.gq2.gameObject.SetActive(true);
        step3.gq3.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step3.btn });
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
            await Step4();
        }
        else if (firstCompletedTask == 1) // 表示第二个任务 `_1` 先完成了
        {
            step3.gq1.CancelWait();
            step3.gq3.CancelWait();
            endText = "站立位置不当，任务失败。";
        }
        else if (firstCompletedTask == 2) // 表示第三个任务 `_2` 先完成了
        {
            step3.gq1.CancelWait();
            step3.gq2.CancelWait();
            endText = "站立位置不当，任务失败。";
        }
    }
    async UniTask Step4()
    {
        PlayYuying(audios.xiaohuo4);
        step3.human.gameObject.SetActive(false);
        step4.human.gameObject.SetActive(true);
        step3.panel.gameObject.SetActive(false);
        step4.panel.gameObject.SetActive(true);
        step3.camera.gameObject.SetActive(false);
        step4.camera.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step4.btn });

        if (step4.toggles[0].isOn)
        {
            step4.panel.gameObject.SetActive(false);
            step4.humanAnim.Play("Scene");
            await UniTask.Delay(TimeSpan.FromSeconds(4));
            await Step5();
        }
        else
        {
            endText = "选择动作错误，任务失败。";
        }
    }
    async UniTask Step5()
    {
        PlayYuying(audios.xiaohuo5);
        step5.panel.gameObject.SetActive(true);
        step4.camera.gameObject.SetActive(false);
        step5.camera.gameObject.SetActive(true);
        step5.jiantou1.gameObject.SetActive(true);
        step5.jiantou2.gameObject.SetActive(true);
        step5.jiantou3.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step5.btn });
        step5.btn.gameObject.SetActive(false);
        var _ = step5.jiantou1.WaitObjectClick();
        var _1 = step5.jiantou2.WaitObjectClick();
        var _2 = step5.jiantou3.WaitObjectClick();
        var firstCompletedTask = await UniTask.WhenAny(_, _1, _2);
        step5.panel.gameObject.SetActive(false);
        step5.jiantou1.gameObject.SetActive(false);
        step5.jiantou2.gameObject.SetActive(false);
        step5.jiantou3.gameObject.SetActive(false);
        if (firstCompletedTask == 0) // 表示第一个任务 `_` 先完成了
        {
            step5.jiantou2.CancelWait();
            step5.jiantou3.CancelWait();
            miehuoqi.Play();
            miehuoqi.GetComponent<AudioSource>().Play();
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            DOTween.To(() => fireAudio.volume, x => fireAudio.volume = x, 0f, 2f);
            await fire.transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 2f);
            fire.Stop();
            fireAudio.Stop();
            miehuoqi.Stop();
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            //todo 结束
            await StepEnd();
        }
        else if (firstCompletedTask == 1) // 表示第二个任务 `_1` 先完成了
        {
            step5.jiantou1.CancelWait();
            step5.jiantou3.CancelWait();
            endText = "灭火器瞄准位置不当，任务失败。";
        }
        else if (firstCompletedTask == 2) // 表示第三个任务 `_2` 先完成了
        {
            step5.jiantou1.CancelWait();
            step5.jiantou2.CancelWait();
            endText = "灭火器瞄准位置不当，任务失败。";
        }
    }
    async UniTask StepEnd()
    {
        PlayYuying(audios.xiaohuoend1);
        end.panel.gameObject.SetActive(true);
        step5.camera.gameObject.SetActive(false);
        end.camera.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { end.btn });
        end.btn.gameObject.SetActive(false);
        end.text.text = "还需要做什么？";
        PlayYuying(audios.xiaohuoend2);
        await end.chuanghu.WaitObjectClick();
        await end.chuanghu.transform.DOLocalMove(new Vector3(-0.0142754931f, 0.702550709f, 0.488999993f), 1f);
        smoke.Stop();
        PlayYuying(audios.xiaohuoend3);
        end.text.text = "恭喜你，完成了消防小知识的学习！\n记住，预防火灾，人人有责！";
        isend = true;
    }

}
[Serializable]
public class xiaohuoStart
{
    public GameObject panel;
    public TextMeshProUGUI text;
    public Button btn;
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

[Serializable]
public class xiaohuoStep2Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper mhq1;
    public SubjectClickHelper mhq2;
}
[Serializable]
public class xiaohuoStep3Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper gq1;
    public SubjectClickHelper gq2;
    public SubjectClickHelper gq3;
    public Transform human;
}
[Serializable]
public class xiaohuoStep4Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public Transform human;
    public Animator humanAnim;
    public List<Toggle> toggles;
}
[Serializable]
public class xiaohuoStep5Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper jiantou1;
    public SubjectClickHelper jiantou2;
    public SubjectClickHelper jiantou3;
}
[Serializable]
public class xiaohuoStepEndObj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper chuanghu;
    public TextMeshProUGUI text;
}
[Serializable]
public class xiaohuoAudio
{
    public AudioClip xiaohuostart;
    public AudioClip xiaohuo1;
    public AudioClip xiaohuo2;
    public AudioClip xiaohuo3;
    public AudioClip xiaohuo4;
    public AudioClip xiaohuo5;
    public AudioClip xiaohuoend1;
    public AudioClip xiaohuoend2;
    public AudioClip xiaohuoend3;
}