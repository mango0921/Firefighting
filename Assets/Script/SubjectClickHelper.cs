using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using QuickOutline;
using UnityEngine;
using UnityEngine.Events;

public class SubjectClickHelper : MonoBehaviour
{
    private bool clicked = true;
    private bool isHighLight = false;
    private CancellationTokenSource cancellationTokenSource;
    void OnMouseDown()
    {
        if (!clicked)
        {
            clicked = true;
        }
    }
    public void OpenHighLight()
    {
        gameObject.OpenFlashHighLight();
        isHighLight = true;
    }
    public void CloseHightLight()
    {
        gameObject.CloseFlashHighLight();
        isHighLight = false;
    }
    public async UniTask WaitObjectClick()
    {
        OpenHighLight();
        clicked = false;
        var cancellationToken = this.GetCancellationTokenOnDestroy();
        cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        await UniTask.WaitUntil(() => clicked, cancellationToken: cancellationTokenSource.Token);
        CloseHightLight();
    }
    public void CancelWait()
    {
        if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
            clicked = true;
            CloseHightLight();
        }
    }

}