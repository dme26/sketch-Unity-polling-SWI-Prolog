using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using TMPro;

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
    // Prolog API server URL
    private string apiURL = "http://localhost:8080/api/intermediaries";
    private float requestInterval = 1.0f; // in seconds

    public TextMeshProUGUI countLabel;


    void Start()
    {
        StartCoroutine(RepeatedRequestCoroutine());
    }

    IEnumerator RepeatedRequestCoroutine()
    {
        while (true) // Infinite loop to keep making requests
        {
            yield return StartCoroutine(GetDataAboutIntermediaries(apiURL));
            yield return new WaitForSeconds(requestInterval);
        }
    }

    IEnumerator GetDataAboutIntermediaries(string uri)
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
                //Debug.Log("Received: " + responseText);

                // Use JsonUtility to deserialize the JSON string
                PrologIntermediaries response = JsonUtility.FromJson<PrologIntermediaries>(responseText);
		int intermediariesCount = response.intermediaries.Count();
                Debug.Log("Count() intermediaries from Prolog: " + intermediariesCount);
                countLabel.text = "Count is " + intermediariesCount;
            }
        }
    }
}
