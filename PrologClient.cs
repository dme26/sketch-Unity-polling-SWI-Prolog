using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

// The JsonUtility parsing built into Unity requires a class target
[System.Serializable]
public class PrologIntermediary
{
    public int id;
    public string name;
}

[System.Serializable]
public class PrologIntermediaries
{
    public PrologIntermediary[] intermediaries;
}


public class PrologClient : MonoBehaviour
{
    void Start()
    {
        // Prolog API server URL
        string url = "http://localhost:8080/api/intermediaries";
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
                PrologIntermediaries response = JsonUtility.FromJson<PrologIntermediaries>(responseText);
                Debug.Log("Count() intermediaries from Prolog: " + response.intermediaries.Count());
            }
        }
    }
}
