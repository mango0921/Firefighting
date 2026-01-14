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
    void OnMouseDown()
    {
        if (clicked)
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
        bool clicked = false;
        await UniTask.WaitUntil(() => clicked);
        CloseHightLight();
    }


}