using System.Collections;
using System.Collections.Generic;
using System.Text;
using GraphQL;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour {
	public delegate void OnSuccess();
	public delegate void OnError();

	private string _url;
	private Dictionary<string, string> _headers;

	public static ApiClient Create(string url, Dictionary<string, string> headers) {
		GameObject go = new GameObject("Api Client");
		ApiClient client = go.AddComponent<ApiClient>();
		DontDestroyOnLoad(go);

		client.Init(url, headers);
		return client;
	}

	public void Init(string url, Dictionary<string, string> headers) {
		_url = url;
		_headers = headers;
	}

	public void Query(GeneratedQuery queryObject, OnSuccess onSuccess, OnError onError) {
		StartCoroutine(PerformQuery(queryObject, onSuccess, onError));
	}

	private IEnumerator PerformQuery(GeneratedQuery queryObject, OnSuccess onSuccess, OnError onError) {
		string json = "{ \"query\" : \"" + queryObject.QueryContent().Replace('\t', ' ').Replace('\n', ' ') + "\" }";

		UnityWebRequest www = new UnityWebRequest(_url, method: UnityWebRequest.kHttpVerbPOST);
		www.uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(json));
		www.uploadHandler.contentType = "application/json";

		www.downloadHandler = new DownloadHandlerBuffer();

		foreach (var header in _headers) {
			www.SetRequestHeader(header.Key, header.Value);
		}

		Debug.Log(Encoding.Default.GetString(www.uploadHandler.data));

		yield return www.Send();

		if (!string.IsNullOrEmpty(www.error)) {
			Debug.LogError("GraphQL Request Error! " + www.error);
			onError();
			yield break;
		}

		Debug.Log("GraphQL Good! " + Encoding.UTF8.GetString(www.downloadHandler.data));
	}
}
