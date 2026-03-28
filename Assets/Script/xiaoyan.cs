using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class xiaoyan : xiaofangbase
{
    public ParticleSystem dayan;
    [SerializeField]
    xiaoyanAudio audios;

    [SerializeField]
    private xiaoyanStart start;
    [SerializeField]
    private xiaoyanStep1Obj step1;
    [SerializeField]
    private xiaoyanStep2Obj step2;
    [SerializeField]
    private xiaoyanStep3Obj step3;
    [SerializeField]
    private xiaoyanStep4Obj step4;
    [SerializeField]
    private xiaoyanStep5Obj step5;
    [SerializeField]
    private xiaoyanStep6Obj step6;
    [SerializeField]
    private xiaoyanStep7Obj step7;
    [SerializeField]
    private xiaoyanStepEndObj end;
    protected override void Start()
    {
        base.Start();
    }
    protected override async UniTask StepStart()
    {
        PlayYuying(audios.xiaoyanstart);
        start.panel.SetActive(true);
        start.camera.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { start.btn });
        start.panel.SetActive(false);
        start.camera.gameObject.SetActive(false);
        await Step1();
    }
    protected override void Init()
    {
        yuyingAudio.Stop();

        start.panel.gameObject.SetActive(true);
        step1.panel.gameObject.SetActive(false);
        step2.panel1.gameObject.SetActive(false);
        step2.panel2.gameObject.SetActive(false);
        step3.panel.gameObject.SetActive(false);
        step4.panel.gameObject.SetActive(false);
        step5.panel.gameObject.SetActive(false);
        step6.panel.gameObject.SetActive(false);
        step7.panel.gameObject.SetActive(false);
        end.panel.gameObject.SetActive(false);

        start.camera.gameObject.SetActive(true);
        step1.camera.gameObject.SetActive(false);
        step2.camera.gameObject.SetActive(false);
        step4.camera.gameObject.SetActive(false);
        step6.camera.gameObject.SetActive(false);
        step7.camera.gameObject.SetActive(false);


        start.btn.gameObject.SetActive(true);
        step1.btn.gameObject.SetActive(true);
        step2.btn1.gameObject.SetActive(true);
        step2.btn2.gameObject.SetActive(true);
        step2.btn3.gameObject.SetActive(true);
        step3.btn.gameObject.SetActive(true);
        step4.yesbtn.gameObject.SetActive(true);
        step4.nobtn.gameObject.SetActive(true);
        step5.yesbtn.gameObject.SetActive(true);
        step5.nobtn.gameObject.SetActive(true);
        step6.btn.gameObject.SetActive(true);
        step7.btn.gameObject.SetActive(true);
        end.btn.gameObject.SetActive(true);

        dayan.Stop();
        dayan.Clear();

        step1.jiantou1.gameObject.SetActive(false);
        step1.jiantou2.gameObject.SetActive(false);
        step3.jiantou1.gameObject.SetActive(false);
        step3.jiantou2.gameObject.SetActive(false);
        step3.jiantou3.gameObject.SetActive(false);
        step7.jiantou1.gameObject.SetActive(false);
        step7.jiantou2.gameObject.SetActive(false);

        step4.human.SetActive(false);
        step5.human.SetActive(false);
        step6.human.SetActive(false);

    }
    async UniTask Step1()
    {
        PlayYuying(audios.xiaoyan1);
        step1.panel.SetActive(true);
        step1.camera.gameObject.SetActive(true);

        await WaitForAnyBtnClick(new Button[] { step1.btn });
        step1.btn.gameObject.SetActive(false);
        step1.jiantou1.gameObject.SetActive(true);
        step1.jiantou2.gameObject.SetActive(true);
        var _ = step1.jiantou1.WaitObjectClick();
        var _1 = step1.jiantou2.WaitObjectClick();

        var firstCompletedTask = await UniTask.WhenAny(_, _1);
        if (firstCompletedTask == 0) // 表示第一个任务 `_` 先完成了
        {
            step1.jiantou2.CancelWait();

        }
        else if (firstCompletedTask == 1) // 表示第二个任务 `_1` 先完成了
        {
            step1.jiantou1.CancelWait();
            endText = "逃生应选择合理的逃生线路，应避免强行穿越浓烟，按照疏散指示灯的指引方向进行逃生。";
            return;
        }
        step1.jiantou1.gameObject.SetActive(false);
        step1.jiantou2.gameObject.SetActive(false);
        step1.panel.SetActive(false);
        step1.camera.gameObject.SetActive(false);
        await Step2();
    }
    async UniTask Step2()
    {
        PlayYuying(audios.xiaoyan21);
        step2.panel1.SetActive(true);
        step2.camera.gameObject.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step2.btn1 });
        PlayYuying(audios.xiaoyan22);
        dayan.Play();
        step2.panel1.SetActive(false);
        step2.panel2.SetActive(true);
        var _ = WaitForAnyBtnClick(new Button[] { step2.btn2 });
        var _1 = WaitForAnyBtnClick(new Button[] { step2.btn3 });
        var num = await UniTask.WhenAny(_, _1);
        step2.panel2.gameObject.SetActive(false);
        if (num == 0)
        {
            endText = "吸入毒烟，人员昏迷。";
        }
        else
        {
            await Step3();
        }
    }
    async UniTask Step3()
    {
        PlayYuying(audios.xiaoyan3);
        dayan.Stop();
        dayan.Clear();
        step3.panel.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step3.btn });
        step3.btn.gameObject.SetActive(false);
        step3.jiantou1.gameObject.SetActive(true);
        step3.jiantou2.gameObject.SetActive(true);
        step3.jiantou3.gameObject.SetActive(true);
        var _ = step3.jiantou1.WaitObjectClick();
        var _1 = step3.jiantou2.WaitObjectClick();
        var _2 = step3.jiantou3.WaitObjectClick();
        var firstCompletedTask = await UniTask.WhenAny(_, _1, _2);
        step3.panel.SetActive(false);
        step3.jiantou1.gameObject.SetActive(false);
        step3.jiantou2.gameObject.SetActive(false);
        step3.jiantou3.gameObject.SetActive(false);
        if (firstCompletedTask == 0)
        {
            step3.jiantou2.CancelWait();
            step3.jiantou3.CancelWait();
        }
        else if (firstCompletedTask == 1)
        {
            step3.jiantou1.CancelWait();
            step3.jiantou3.CancelWait();
            endText = "未选择合理路线，吸入毒烟，人员昏迷。";
            return;
        }
        else if (firstCompletedTask == 2)
        {
            step3.jiantou1.CancelWait();
            step3.jiantou2.CancelWait();
            endText = "未选择合理路线，吸入毒烟，人员昏迷。";
            return;
        }
        await Step4();
    }
    async UniTask Step4()
    {
        PlayYuying(audios.xiaoyan4);
        step2.camera.gameObject.SetActive(false);
        step4.camera.gameObject.SetActive(true);
        step3.panel.gameObject.SetActive(false);
        step4.panel.gameObject.SetActive(true);
        step4.human.SetActive(true);
        var _ = WaitForAnyBtnClick(new Button[] { step4.yesbtn });
        var _1 = WaitForAnyBtnClick(new Button[] { step4.nobtn });
        var firstCompletedTask = await UniTask.WhenAny(_, _1);
        step4.panel.SetActive(false);
        step4.human.SetActive(false);
        if (firstCompletedTask == 0)
        {
            endText = "错误，还应用湿毛巾、衣物等捂住口鼻。";
            return;
        }
        else if (firstCompletedTask == 1)
        {

        }
        await Step6();
    }
    async UniTask Step5()
    {
        PlayYuying(audios.xiaoyan4);
        step5.human.SetActive(true);
        step5.panel.SetActive(true);
        var _ = WaitForAnyBtnClick(new Button[] { step5.yesbtn });
        var _1 = WaitForAnyBtnClick(new Button[] { step5.nobtn });
        var firstCompletedTask = await UniTask.WhenAny(_, _1);
        step5.panel.SetActive(false);
        step5.human.SetActive(false);
        if (firstCompletedTask == 0)
        {

        }
        else if (firstCompletedTask == 1)
        {
            endText = "错误，穿越烟区逃生过程中，应低头弯腰，用湿毛巾、衣物等捂住口鼻。";
            return;
        }
        await Step6();
    }
    async UniTask Step6()
    {
        PlayYuying(audios.xiaoyan6);
        step6.human.SetActive(true);
        step4.camera.gameObject.SetActive(false);
        step6.camera.gameObject.SetActive(true);
        step6.panel.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step6.btn });
        step6.panel.SetActive(false);
        step6.human.SetActive(false);
        await Step7();
    }
    async UniTask Step7()
    {
        PlayYuying(audios.xiaoyan7);
        step6.camera.gameObject.SetActive(false);
        step7.camera.gameObject.SetActive(true);
        step7.panel.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { step7.btn });
        step7.btn.gameObject.SetActive(false);
        step7.jiantou1.gameObject.SetActive(true);
        step7.jiantou2.gameObject.SetActive(true);
        var _ = step7.jiantou1.WaitObjectClick();
        var _1 = step7.jiantou2.WaitObjectClick();
        var firstCompletedTask = await UniTask.WhenAny(_, _1);
        step7.panel.SetActive(false);
        step7.jiantou1.gameObject.SetActive(false);
        step7.jiantou2.gameObject.SetActive(false);
        if (firstCompletedTask == 0)
        {
            step7.jiantou2.CancelWait();
        }
        else if (firstCompletedTask == 1)
        {
            step7.jiantou1.CancelWait();
            endText = "应选择楼梯进行逃生，不得乘坐电梯。";
            return;
        }
        await StepEnd();
    }

    async UniTask StepEnd()
    {
        PlayYuying(audios.xiaoyanend);
        end.panel.SetActive(true);
        await WaitForAnyBtnClick(new Button[] { end.btn });
        end.btn.gameObject.SetActive(false);
        isend = true;
    }
}
[Serializable]
public class xiaoyanStart
{
    public GameObject panel;
    public Button btn;
    public Camera camera;

}
[Serializable]
public class xiaoyanStep1Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper jiantou1;
    public SubjectClickHelper jiantou2;
}
[Serializable]
public class xiaoyanStep2Obj
{
    public GameObject panel1;
    public Button btn1;
    public Camera camera;
    public GameObject panel2;
    public Button btn2;
    public Button btn3;
}
[Serializable]
public class xiaoyanStep3Obj
{
    public GameObject panel;
    public Button btn;
    public SubjectClickHelper jiantou1;
    public SubjectClickHelper jiantou2;
    public SubjectClickHelper jiantou3;
}
[Serializable]
public class xiaoyanStep4Obj
{
    public GameObject panel;
    public Button yesbtn;
    public Button nobtn;
    public Camera camera;
    public GameObject human;
}
[Serializable]
public class xiaoyanStep5Obj
{
    public GameObject panel;
    public Button yesbtn;
    public Button nobtn;
    public GameObject human;
}
[Serializable]
public class xiaoyanStep6Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public GameObject human;
}
[Serializable]
public class xiaoyanStep7Obj
{
    public GameObject panel;
    public Button btn;
    public Camera camera;
    public SubjectClickHelper jiantou1;
    public SubjectClickHelper jiantou2;
}
[Serializable]
public class xiaoyanStepEndObj
{
    public GameObject panel;
    public Button btn;
}
[Serializable]
public class xiaoyanAudio
{
    public AudioClip xiaoyanstart;
    public AudioClip xiaoyan1;
    public AudioClip xiaoyan21;
    public AudioClip xiaoyan22;
    public AudioClip xiaoyan3;
    public AudioClip xiaoyan4;
    public AudioClip xiaoyan6;
    public AudioClip xiaoyan7;
    public AudioClip xiaoyanend;
}