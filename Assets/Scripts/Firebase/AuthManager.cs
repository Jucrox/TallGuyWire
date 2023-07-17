using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;

public class FirebaseAuthManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result.User;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        //UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                    }
                }
            }
        }
    }
    //protected Firebase.Auth.FirebaseAuth auth;
    //protected Firebase.Auth.FirebaseUser user;
    //private string displayName;

    //public TMP_InputField inputFieldRegisterEmail;
    //public TMP_InputField inputFieldRegisterPassword;

    //public TMP_InputField inputFieldLoginEmail;
    //public TMP_InputField inputFieldLoginPassword;

    //public GameObject panelLogin;
    //public GameObject panelRegister;

    //private bool signedIn;

    //// Start is called before the first frame update
    void Start()
    {
        //inputFieldRegisterEmail.text = "";
        //inputFieldRegisterPassword.text = "";
        //inputFieldLoginEmail.text = "";
        //inputFieldLoginPassword.text = "";
        //InitializeFirebase();
    }

    //// Update is called once per frame
    void Update()
    {
        //if (signedIn)
        //{
        //    //SceneManager.LoadScene(1);
        //}
    }

    //void InitializeFirebase()
    //{
    //    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    //    auth.StateChanged += AuthStateChanged;
    //    AuthStateChanged(this, null);
    //}

    //void AuthStateChanged(object sender, System.EventArgs eventArgs)
    //{
    //    if (auth.CurrentUser != user)
    //    {
    //        bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
    //            && auth.CurrentUser.IsValid();
    //        if (!signedIn && user != null)
    //        {
    //            Debug.Log("Signed out " + user.UserId);
    //        }
    //        user = auth.CurrentUser;
    //        if (signedIn)
    //        {
    //            Debug.Log("Signed in " + user.UserId);
    //            displayName = user.DisplayName ?? "";
    //            panelRegister.SetActive(false);
    //            panelLogin.SetActive(true);
    //            //emailAddress = user.Email ?? "";
    //            //photoUrl = user.PhotoUrl ?? "";
    //        }
    //    }
    //}

    //public void CreateUserByEmail() 
    //{
    //    string email = inputFieldRegisterEmail.text;
    //    string password = inputFieldRegisterPassword.text;

    //    auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
    //        if (task.IsCanceled)
    //        {
    //            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            //Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    //            Debug.Log("Ha ocurrido un error, intentelo de nuevo");
    //            panelRegister.SetActive(true);
    //            panelLogin.SetActive(false);
    //            return;
    //        }

    //        // Firebase user has been created.

    //        Firebase.Auth.AuthResult result = task.Result;
    //        Debug.LogFormat("Firebase user created successfully: {0} ({1})",
    //            result.User.DisplayName, result.User.UserId);
    //    });
    //    //panelRegister.SetActive(false);
    //    //panelLogin.SetActive(true);
    //}

    //public void SessionActivated() 
    //{
    //    Firebase.Auth.FirebaseAuth auth;
    //    Firebase.Auth.FirebaseUser user;

    //    // Handle initialization of the necessary firebase modules:
    //    void InitializeFirebase()
    //    {
    //        Debug.Log("Setting up Firebase Auth");
    //        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    //        auth.StateChanged += AuthStateChanged;
    //        AuthStateChanged(this, null);
    //    }

    //    // Track state changes of the auth object.
    //    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    //    {
    //        if (auth.CurrentUser != user)
    //        {
    //            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
    //            if (!signedIn && user != null)
    //            {
    //                Debug.Log("Signed out " + user.UserId);
    //            }
    //            user = auth.CurrentUser;
    //            if (signedIn)
    //            {
    //                Debug.Log("Signed in " + user.UserId);
    //                SceneManager.LoadScene(1);
    //            }
    //        }
    //    }

    //    // Handle removing subscription and reference to the Auth instance.
    //    // Automatically called by a Monobehaviour after Destroy is called on it.
    //    void OnDestroy()
    //    {
    //        auth.StateChanged -= AuthStateChanged;
    //        auth = null;
    //    }
    //}

    //public void GetUserProfile() 
    //{
    //    Firebase.Auth.FirebaseUser user = auth.CurrentUser;
    //    if (user != null)
    //    {
    //        string name = user.DisplayName;
    //        string email = user.Email;
    //        System.Uri photo_url = user.PhotoUrl;
    //        // The user's Id, unique to the Firebase project.
    //        // Do NOT use this value to authenticate with your backend server, if you
    //        // have one; use User.TokenAsync() instead.
    //        string uid = user.UserId;
    //    }
    //}

    //public void UserAccess() 
    //{
    //    string email = inputFieldLoginEmail.text;
    //    string password = inputFieldLoginPassword.text;

    //    auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
    //        if (task.IsCanceled)
    //        {
    //            Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            //Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    //            Debug.Log("Usuario no encontrado");
    //            return;
    //        }

    //        Firebase.Auth.AuthResult result = task.Result;
    //        Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            result.User.DisplayName, result.User.UserId);
    //        //SceneManager.LoadScene(1);
    //    });
    //}

    //void OnDestroy()
    //{
    //    auth.StateChanged -= AuthStateChanged;
    //    auth = null;
    //}
}
