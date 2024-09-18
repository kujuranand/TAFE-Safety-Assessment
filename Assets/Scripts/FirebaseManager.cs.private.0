using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Linq;

public class FirebaseManager : MonoBehaviour {
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;

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

    //User Data variables
    [Header("UserData")]
    public TMP_InputField usernameField;
    public TMP_InputField hazardsField;
    public TMP_InputField permitsField;
    public TMP_InputField ppesField;
    public TMP_InputField scoreField;
    public GameObject scoreElement;
    public Transform scoreboardContent;

    void Awake() {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void ClearLoginFields() {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }

    public void ClearRegisterFields() {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }

    private bool ValidateEmail(string email) {
        // Use a regular expression to validate the email format
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }

    //Function for the login button
    public void LoginButton() {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    
    //Function for the register button
    public void RegisterButton() {
        string email = emailRegisterField.text;
        if (!ValidateEmail(email)) {
            warningRegisterText.text = "Invalid Email Format!";
            StartCoroutine(ClearWarningAfterDelay(warningRegisterText));
            return;
        }

        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    //Function for the sign out button
    public void SignOutButton() {
        auth.SignOut();
        UIManager.instance.LoginScreen();
        ClearRegisterFields();
        ClearLoginFields();
    }

    //Function for the save button
    public void SaveDataButton() {
        StartCoroutine(UpdateUsernameAuth(usernameField.text));
        StartCoroutine(UpdateUsernameDatabase(usernameField.text));

        StartCoroutine(UpdateHazards(int.Parse(hazardsField.text)));
        StartCoroutine(UpdatePermits(int.Parse(permitsField.text)));
        StartCoroutine(UpdatePPEs(int.Parse(ppesField.text)));
        StartCoroutine(UpdateScore(int.Parse(scoreField.text)));
    }

    //Function for the scoreboard button
    public void ScoreboardButton()
    {        
        StartCoroutine(LoadScoreboardData());
    }

    // Add a coroutine to clear warning texts after a delay
    private IEnumerator ClearWarningAfterDelay(TMP_Text warningText) {
        yield return new WaitForSeconds(1);
        warningText.text = "";
    }

    private IEnumerator Login(string _email, string _password) {
        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null) {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode) {
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
            //Hide the message after few seconds
            yield return new WaitForSeconds(1);
            warningLoginText.text = "";
        }
        else {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            StartCoroutine(LoadUserData());
            
            // Hide the message after few seconds
            yield return new WaitForSeconds(1);
            
            usernameField.text = User.DisplayName;
            UIManager.instance.UserDataScreen();
            confirmLoginText.text = "";
            ClearLoginFields();
            ClearRegisterFields();
        }
    }

    private IEnumerator Register(string _email, string _password, string _username) {
        if (_username == "") {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
            // Hide the message after few seconds
            yield return new WaitForSeconds(1);
            warningRegisterText.text = "";
        }
        else if(passwordRegisterField.text != passwordRegisterVerifyField.text) {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
            // Hide the message after few seconds
            yield return new WaitForSeconds(1);
            warningRegisterText.text = "";
        }
        else {
            //Call the Firebase auth signin function passing the email and password
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null) {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode) {
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
                // Hide the message after few seconds
                yield return new WaitForSeconds(1);
                warningRegisterText.text = "";
            }
            else {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result.User;

                if (User != null) {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    Task ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null) {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                        // Hide the message after few seconds
                        yield return new WaitForSeconds(1);
                        warningRegisterText.text = "";
                    }
                    else {
                        //Username is now set
                        //Now return to login screen
                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                        ClearLoginFields();
                        ClearRegisterFields();
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username) {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        Task ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else {
            //Auth username is now updated
        }        
    }

    private IEnumerator UpdateUsernameDatabase(string _username) {
        //Set the currently logged in user username in the database
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else {
            //Database username is now updated
        }
    }

    private IEnumerator UpdateHazards(int _hazards) {
        //Set the currently logged in user hazards
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("hazards").SetValueAsync(_hazards);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else {
            //Hazards are now updated
        }
    }

    private IEnumerator UpdatePermits(int _permits) {
        //Set the currently logged in user permits
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("permits").SetValueAsync(_permits);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else {
            //Permits are now updated
        }
    }

    private IEnumerator UpdatePPEs(int _ppes) {
        //Set the currently logged in user ppes
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("ppes").SetValueAsync(_ppes);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else {
            //PPEs are now updated
        }
    }

    private IEnumerator UpdateScore(int _score) {
        //Set the currently logged in user score
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("score").SetValueAsync(_score);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else {
            //Score is now updated
        }
    }

    private IEnumerator LoadUserData() {
        //Get the currently logged in user data
        Task<DataSnapshot> DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null) {
            //No data exists yet
            hazardsField.text = "0";
            permitsField.text = "0";
            ppesField.text = "0";
            scoreField.text = "0";
        }
        else {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            hazardsField.text = snapshot.Child("hazards").Value.ToString();
            permitsField.text = snapshot.Child("permits").Value.ToString();
            ppesField.text = snapshot.Child("ppes").Value.ToString();
            scoreField.text = snapshot.Child("score").Value.ToString();
        }
    }

    private IEnumerator LoadScoreboardData() {
        //Get all the users data ordered by kills amount
        Task<DataSnapshot> DBTask = DBreference.Child("users").OrderByChild("score").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform) {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>()) {
                string username = childSnapshot.Child("username").Value.ToString();
                int hazards = int.Parse(childSnapshot.Child("hazards").Value.ToString());
                int permits = int.Parse(childSnapshot.Child("permits").Value.ToString());
                int ppes = int.Parse(childSnapshot.Child("ppes").Value.ToString());
                int score = int.Parse(childSnapshot.Child("score").Value.ToString());

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, hazards, permits, ppes, score);
            }

            //Go to scoreboard screen
            UIManager.instance.ScoreboardScreen();
        }
    }
}