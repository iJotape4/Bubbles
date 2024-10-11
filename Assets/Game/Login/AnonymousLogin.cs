using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Firebase;

public class AnonymousLogin : MonoBehaviour
{
    FirebaseManager firebaseManager;

    [SerializeField] TMP_InputField userNameText;
    [SerializeField] Button loginButton;
    // Start is called before the first frame update
    void Start()
    {
        firebaseManager = FirebaseManager.Instance;
        loginButton.onClick.AddListener(SignIn);
        userNameText.onValueChanged.AddListener(EnablePlayButton);
    }

    void EnablePlayButton(string text)
    {
        if (string.IsNullOrEmpty(text))
            loginButton.interactable = false;
        else
            loginButton.interactable = true;
    }

    void SignIn()
    {
        firebaseManager.auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;

            // Set the display name
            FirebaseUser user = result.User;
            UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = userNameText.text
            };

            user.UpdateUserProfileAsync(profile).ContinueWith(updateProfileTask =>
            {
                if (updateProfileTask.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (updateProfileTask.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + updateProfileTask.Exception);
                    return;
                }

                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    user.DisplayName, user.UserId);
            });
        });

    }


}