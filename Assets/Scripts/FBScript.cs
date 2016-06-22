using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FBScript : MonoBehaviour {

    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject DialogUsername;
    public GameObject DialogProfilePic;
	public GameObject networkManager;

    void Awake()
    {
		//creates instance of FBManager and Init FB
		FaceBookManager.Instance.InitFB ();
		DealWithFBMenus (FB.IsLoggedIn);
    }

    public void FBlogin()
    {
        List<string> permissions = new List<string> ();
        permissions.Add ("public_profile");

        FB.LogInWithReadPermissions (permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result)
    {
        if (result.Error != null) {
            Debug.Log (result.Error);
        } else {
            if (FB.IsLoggedIn) {
				FaceBookManager.Instance.IsLoggedIn = true;
				FaceBookManager.Instance.GetProfile ();
				networkManager.SetActive (true);
                Debug.Log ("FB is logged in");
            } else {
                Debug.Log ("FB is not logged in");
            }
            DealWithFBMenus (FB.IsLoggedIn);
        }
    }

	//hides/shows FB login menus based on whether you are logged in or not
    void DealWithFBMenus(bool isLoggedIn)
    {
        if (isLoggedIn) {
            DialogLoggedIn.SetActive (true);
            DialogLoggedOut.SetActive (false);

			//get profile name
			if (FaceBookManager.Instance.ProfileName != null) {
				Text userName = DialogUsername.GetComponent<Text> ();
				userName.text = "Hi " + FaceBookManager.Instance.ProfileName;
			} else {
				StartCoroutine ("WaitForProfileName");
			}
        } else {
            DialogLoggedIn.SetActive (false);
            DialogLoggedOut.SetActive (true);
        }

		//get profile pic
		if (FaceBookManager.Instance.ProfilePic != null) {
			Image profilePic = DialogProfilePic.GetComponent<Image> ();
			profilePic.sprite = FaceBookManager.Instance.ProfilePic;
			Color alphaColor = profilePic.color;
			alphaColor.a = 255;
			profilePic.color = alphaColor;
		} else {
			StartCoroutine ("WaitForProfilePic");
		}
    }

	IEnumerator WaitForProfileName() {
		while (FaceBookManager.Instance.ProfileName == null) {
			yield return null;
		}

		DealWithFBMenus (FB.IsLoggedIn);
	}

	IEnumerator WaitForProfilePic() {
		while (FaceBookManager.Instance.ProfilePic == null) {
			yield return null;
		}

		DealWithFBMenus (FB.IsLoggedIn);
	}

	public void Share() {
		FaceBookManager.Instance.Share ();
	}

	public void Invite() {
		FaceBookManager.Instance.Invite ();
	}

	public void ShareWithUsers() {
		FaceBookManager.Instance.ShareWithUsers ();
	}
}