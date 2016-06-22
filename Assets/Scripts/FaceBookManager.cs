using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FaceBookManager : MonoBehaviour {

	private static FaceBookManager _instance;

	public static FaceBookManager Instance{
		get {
			if (_instance == null) {
				GameObject fbm = new GameObject ("FBManager");
				fbm.AddComponent<FaceBookManager> ();
			}
			return _instance;
		}
	}

	public bool IsLoggedIn { get; set; }
	public string ProfileName { get; set; }
	public Sprite ProfilePic { get; set; }
	public string AppLinkURL { get; set; }

	void Awake() {
		DontDestroyOnLoad (this.gameObject);
		_instance = this;

		IsLoggedIn = true;
	}

	public void InitFB() {
		if (!FB.IsInitialized) {
			FB.Init (SetInit, OnHideUnity);
		} else {
			IsLoggedIn = FB.IsLoggedIn;
		}
	}

	void SetInit()
	{
		if (FB.IsLoggedIn) {
			Debug.Log ("FB is logged in");
			GetProfile ();
		} else {
			Debug.Log ("FB is not logged in");
		}
		IsLoggedIn = FB.IsLoggedIn;
	}

	void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	public void GetProfile() {
		FB.API ("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
		FB.API ("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);

		FB.GetAppLink (DealWithAppLink);
	}

	void DisplayUsername(IResult result)
	{
		if (result.Error == null) {
			ProfileName = "" + result.ResultDictionary ["first_name"];
		} else {
			Debug.Log (result.Error);
		}
	}

	void DisplayProfilePic(IGraphResult result)
	{
		if (result.Texture != null) {
			ProfilePic = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 ());
		}
	}

	void DealWithAppLink(IAppLinkResult result) {
		//NOTE: if there are any issues it may be that the app is in development mode
		if (!string.IsNullOrEmpty (result.Url)) {
			AppLinkURL = "" + result.Url + "";
			Debug.Log (AppLinkURL);
		} else {
			AppLinkURL = "http://google.com";
		}
	}

	public void Share() {
		//TODO on share success award some gems
		FB.FeedShare (
			string.Empty,
			new Uri(AppLinkURL),
			"Hello this is the title",
			"This is the caption",
			"Check out this game",
			new Uri("https://i.ytimg.com/vi/NtgtMQwr3Ko/maxresdefault.jpg"),
			string.Empty,
			ShareCallback
		);
	}

	void ShareCallback(IResult result) {
		if (result.Cancelled) {
			Debug.Log ("Share cancelled");
		} else if (!string.IsNullOrEmpty (result.Error)) {
			Debug.Log ("Error on share!");
		} else if (!string.IsNullOrEmpty (result.RawResult)) {
			Debug.Log ("Success on share");
		}
	}

	public void Invite() {
		//TODO on share success award some gems
		FB.Mobile.AppInvite (
			new Uri(AppLinkURL),
			new Uri("https://i.ytimg.com/vi/NtgtMQwr3Ko/maxresdefault.jpg"),
			InviteCallBack
		);
	}

	void InviteCallBack(IResult result) {
		if (result.Cancelled) {
			Debug.Log ("Invite cancelled");
		} else if (!string.IsNullOrEmpty (result.Error)) {
			Debug.Log ("Error on invite!");
		} else if (!string.IsNullOrEmpty (result.RawResult)) {
			Debug.Log ("Success on invite");
		}
	}

	public void ShareWithUsers() {
		FB.AppRequest (
			"Come and try to beat me!",
			null,
			new List<object> () { "app_users" },
			null,
			null,
			null,
			null,
			ShareWithUsersCallback
		);
	}

	void ShareWithUsersCallback(IAppRequestResult result) {
		if (result.Cancelled) {
			Debug.Log ("Challenge cancelled");
		} else if (!string.IsNullOrEmpty (result.Error)) {
			Debug.Log ("Error on challenge!");
		} else if (!string.IsNullOrEmpty (result.RawResult)) {
			Debug.Log ("Success on challenge");
		}
	}

}
