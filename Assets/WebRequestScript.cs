using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WebRequestScript : MonoBehaviour
{
    [SerializeField] string url;
    [SerializeField] AudioClip myClip;
    [SerializeField] Text placeHolder;
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    public void OnClickButton(string name)
    {
        StartCoroutine(Upload(name));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            Debug.Log(webRequest.result);
        }
    }

    IEnumerator Upload(string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("system_response", "sr_init");
        form.AddField("engagement_id", "NzQ3MTBjNTItNDJhNS0zZTY1LWIxZjAtMmRjMzllYmU0MmMyZXhoaWJpdF9hbGRv");
        form.AddField("customer_state", name);
        // PostData postData = new PostData();
        // postData.system_response = "sr_init";
        // postData.engagement_id = "NzQ3MTBjNTItNDJhNS0zZTY1LWIxZjAtMmRjMzllYmU0MmMyZXhoaWJpdF9hbGRv";
        // postData.customer_state = "cs_men_products";

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.result);
            Debug.Log(www.downloadHandler.text);
            // placeHolder.text = www.downloadHandler.text.ToString();
            // SaveHTML(www.downloadHandler.text.ToString());
            GetData getData = JsonUtility.FromJson<GetData>(www.downloadHandler.text.ToString());
            placeHolder.text = getData.placeholder.ToString();
            StartCoroutine(GetAudioClip(getData.response_channels.voice));
        }
    }

    IEnumerator GetAudioClip(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                myClip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = myClip;
                audioSource.gameObject.SetActive(true);
            }
        }
    }
}

[System.Serializable]
public class PostData
{
    public string system_response;
    public string engagement_id;
    public string customer_state;
}


[System.Serializable]
public class Data
{
    public List<Slideshow> slideshow ;
}

[System.Serializable]
public class PlaceholderAliases
{
}

[System.Serializable]
public class ResponseChannels
{
    public string voice ;
    public string frames ;
    public string shapes ;
}

[System.Serializable]
public class GetData
{
    public bool hide_in_customer_history ;
    public object registered_entities ;
    public string whiteboard_template ;
    public string customer_state ;
    public PlaceholderAliases placeholder_aliases ;
    public bool show_feedback ;
    public ToStateFunction to_state_function ;
    public string placeholder ;
    public bool show_in_history ;
    public Data data ;
    public bool overwrite_whiteboard ;
    public string start_timestamp ;
    public string console ;
    public string name ;
    public string title ;
    public ResponseChannels response_channels ;
    public StateOptions state_options ;
    public string response_id ;
    public string whiteboard_title ;
    public string timestamp ;
    public bool maintain_whiteboard ;
    public int wait ;
    public string type ;
    public object options ;
    public string engagement_id ;
}

[System.Serializable]
public class Slideshow
{
    public string image ;
    public string caption ;
}

[System.Serializable]
public class StateOptions
{
    public string cs_top_three ;
    public string cs_must_have ;
    public string cs_enquiry ;
    public string cs_mt1 ;
    public string cs_mt2 ;
    public string cs_mt3 ;
}

[System.Serializable]
public class ToStateFunction
{
    public string function ;
}
