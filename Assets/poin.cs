using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class poin : MonoBehaviour
{
    public TextMeshProUGUI text;
    private float time = 0;
    DateTime dateTime;
    public URLParameters URLParams;

    void Start()
    {
        Debug.Log("链接为：" + Application.absoluteURL);
        dateTime = DateTime.Now;

        string url = Application.absoluteURL;
        // 解析URL并获取JSON
        Uri uri = new Uri(url);
        string query = uri.Query;

        var queryParams = System.Web.HttpUtility.ParseQueryString(query);
        URLParams = new URLParameters
        {
            id = queryParams.Get("id"),
            name = queryParams.Get("name"),
            path = queryParams.Get("path"),
            token = queryParams.Get("token"),
            platform = queryParams.Get("platform"),
            debug = queryParams.Get("debug") == "1"
        };
        text.gameObject.SetActive(URLParams.debug);

        QualitySettings.SetQualityLevel(URLParams.platform == "pc" ? 2 : 0);
    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            time -= 1;
            text.text = (1 / Time.deltaTime).ToString();
        }
    }
    public async UniTask UpdateScore(int _score)
    {

        string apiUrl = $"https://aqkc.jxnu.edu.cn/api/app-api/edu/app/vr-exam/score/submit";
        SubmitData submitData = new SubmitData
        {
            vrExamType = URLParams.id,
            examName = URLParams.name,
            score = _score,
            totalScore = 100,
            duration = (int)(DateTime.Now - dateTime).TotalSeconds,
            status = "completed"
        };
        string jsonData = JsonConvert.SerializeObject(submitData);
        await SendRequestWithTokenAsync(apiUrl, "POST", jsonData, URLParams.token);
    }
    private async UniTask<string> SendRequestWithTokenAsync(string url, string method, string jsonData, string token)
    {
        try
        {
            using (UnityWebRequest webRequest = new UnityWebRequest(url, method))
            {
                if (!string.IsNullOrEmpty(jsonData))
                {
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                    webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    webRequest.SetRequestHeader("Content-Type", "application/json");
                }

                webRequest.downloadHandler = new DownloadHandlerBuffer();

                // 添加 token 到请求头
                webRequest.SetRequestHeader("Authorization", "Bearer " + token);

                UnityWebRequestAsyncOperation operation = webRequest.SendWebRequest();
                await operation;

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"{method} Error: " + webRequest.error);
                    return null;
                }
                else
                {
                    Debug.Log($"{method} Success: " + webRequest.downloadHandler.text);
                    return webRequest.downloadHandler.text;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"{method} Exception: " + ex.Message);
            return null;
        }
    }
}
[Serializable]
public class URLParameters
{
    public string id;
    public string name;
    public string path;
    public string token;
    public string platform = "mobile";
    public bool debug = false;
}

public class SubmitData
{
    public string vrExamType;
    public string examName;
    public float score;
    public int totalScore;
    public int duration;
    public string status;
}