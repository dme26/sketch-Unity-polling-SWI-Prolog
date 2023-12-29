using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// The JsonUtility parsing built into Unity requires a class target
[System.Serializable]
public class PrologResponse
{
    public string message;
    public int[] numbers;
}

public class PrologClient : MonoBehaviour
{
    void Start()
    {
        // URL of your Prolog server
        string url = "http://localhost:8080/api/data";

        // Start the coroutine to get data
        StartCoroutine(GetData(url));
    }

    IEnumerator GetData(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // Parse the response
                string responseText = webRequest.downloadHandler.text;
                Debug.Log("Received: " + responseText);

                // Use JsonUtility to deserialize the JSON string
                PrologResponse response = JsonUtility.FromJson<PrologResponse>(responseText);
                Debug.Log("Message from Prolog: " + response.message);
            }
        }
    }
}
