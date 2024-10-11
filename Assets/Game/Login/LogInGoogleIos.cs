//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using Google;
//using Systems.Firebase;
//using Firebase.Auth;
//using System.Threading.Tasks;
//using Firebase.Extensions;
//using System.Collections;

//public class LogInGoogleIos : MonoBehaviour
//{
//    [Header("UIButtonComponents")]
//    [SerializeField] TextMeshProUGUI loginWithText;
//    [SerializeField] Image loginImage;
//    [SerializeField] Sprite iosSprite, googleSprite;
//    Button loginButton;
//    readonly string loginTextAndroid = "\r\n\r\nEnter with\r\nGoogle", loginTextIos = "\r\n\r\nEnter with\r\nApple iOS"; //TODO : LocalizeString

//    [Space]
//    [Header("Welcome PopUp Components")]
//    [SerializeField] TextMeshProUGUI usernameTxt;
//    [SerializeField] Image userProfilePic;
//    [SerializeField] string imageUrl;
//    [SerializeField] GameObject welcomePopUp;

//    [Header("Authentication Fields")]
//    FirebaseManager firebaseManager;
//    private GoogleSignInConfiguration configuration;

//    void Start()
//    {
//        loginButton = GetComponent<Button>();
//        loginButton.onClick.AddListener(PlatformLogin);
//        firebaseManager = FirebaseManager.Instance;
//#if PLATFORM_ANDROID
//        SetGoogleSignInConfiguration();
//        SetAndroidButton();
//#elif PLATFORM_IOS
//        SetIosButton();
//#endif

//    }
//    void SetAndroidButton() => SetLoginButton(loginTextAndroid, googleSprite);
//    void SetIosButton() => SetLoginButton(loginTextIos, iosSprite);
//    void SetLoginButton(string text, Sprite icon)
//    {
//        loginWithText.text = text;
//        loginImage.sprite = icon;
//    }

//    void PlatformLogin()
//    {
//#if PLATFORM_ANDROID
//        GoogleSignInClick();
//#endif
//    }

//    public void SetGoogleSignInConfiguration()
//    {
//        configuration = new GoogleSignInConfiguration
//        {
//            WebClientId = firebaseManager.GoogleWebAPI, 
//            RequestIdToken = true
//        };
//    }

//    public void GoogleSignInClick()
//    {
//        if (firebaseManager.auth.CurrentUser != null)
//        {
//            // AddData();
//            return;
//        }

//        GoogleSignIn.Configuration = configuration;
//        GoogleSignIn.Configuration.UseGameSignIn = false;
//        GoogleSignIn.Configuration.RequestIdToken = true;
//        GoogleSignIn.Configuration.RequestEmail = true;

//        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
//    }   

//    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
//    {
//        if (task.IsFaulted) Debug.LogError("Fault");
//        else if (task.IsCanceled) Debug.LogError("Login Cancel");
//        else
//        {
//            Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
//            firebaseManager.auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
//            {
//                if (task.IsCanceled)
//                {
//                    Debug.LogError("SignInWithCredentialAsync was canceled");
//                    return;
//                }
//                if (task.IsFaulted)
//                {
//                    Debug.LogError("SignInWithCredentialAsync encountered an error:" + task.Exception);
//                    return;
//                }

//                DisplayWelcomeMessage(firebaseManager.GetUser());
//            });
//        }
//    }

//    void DisplayWelcomeMessage(FirebaseUser user)
//    {
//        usernameTxt.text = "Welcome : " + user.DisplayName;
//        StartCoroutine(LoadImage(CheckIamgeUrl(user.PhotoUrl.ToString())));
//        welcomePopUp.SetActive(true);
//    }

//    private string CheckIamgeUrl(string url)
//    {
//        if (!string.IsNullOrEmpty(url)) return url;
//        return imageUrl;
//    }

//    IEnumerator LoadImage(string imageUrl)
//    {
//        WWW www = new WWW(imageUrl);
//        yield return www;
//        userProfilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
//    }
//}