using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseManager : SingletonBase<FirebaseManager>
{
    private FirebaseApp app;
    public FirebaseAuth auth { get; private set; }
    public readonly string GoogleWebAPI = "771193471068-j2vl9kkg7819vc539bgaol2gp2m8gc23.apps.googleusercontent.com";
    public readonly string webClientSecret = "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";

    void Start() => InitFirebase();
    private void InitFirebase()
    {
        CheckGooglePlayServicesRequirements();
        auth = FirebaseAuth.DefaultInstance;
    }

    public FirebaseUser GetUser() => auth.CurrentUser;

    private void CheckGooglePlayServicesRequirements()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
}