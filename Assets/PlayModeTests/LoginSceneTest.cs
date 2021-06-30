using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using NCMB;

public class LoginSceneTest : MonoBehaviour
{
    private static string authenError = "ログインに失敗: ";
    private static string duplicationError = "新規登録に失敗: userName is duplication";
    private static string username = "testuser";
    private static string password = "123456";

    [SetUp]
    public void init()
    {
        SceneManager.LoadScene("login");
        UITestSettings.CallbackFlag = false;
    }
    
    [UnityTest]
    public IEnumerator testValidateInputField()
    {
        var errMsg = "名前とパスワードを輸入してください。";

        var ifUsernameGameObj = GameObject.Find("Name");
        var ifUsername = ifUsernameGameObj.GetComponent<InputField>();

        var ifPasswordGameObj = GameObject.Find("Password");
        var ifPassword = ifPasswordGameObj.GetComponent<InputField>();

        ifUsername.text = "abc";
        ifPassword.text = "";

        var btnLogInGameObject = GameObject.Find("Login");
        var btnLogIn = btnLogInGameObject.GetComponent<Button>();
        btnLogIn.onClick.Invoke();

        var txtNotationGameObj = GameObject.Find("Notation");
        var txtNotation = txtNotationGameObj.GetComponent<Text>();
        Assert.IsNotNull(txtNotation);
        Assert.True(txtNotation.text.Contains(errMsg));

        var btnSignUpGameObject = GameObject.Find("SignUp");
        var btnSignUp = btnSignUpGameObject.GetComponent<Button>();
        btnSignUp.onClick.Invoke();

        Assert.IsNotNull(txtNotation);
        Assert.True(txtNotation.text.Contains(errMsg));
        yield return null;
    }

    [UnityTest]
    public IEnumerator testLogin()
    {
        NCMBUser user = new NCMBUser();
        user.UserName = username;
        user.Password = password;
        user.SignUpAsync((NCMBException e) => {
            UITestSettings.CallbackFlag = true;
        });
        yield return UITestSettings.AwaitAsync();

        var ifUsernameGameObj = GameObject.Find("Name");
        var ifUsername = ifUsernameGameObj.GetComponent<InputField>();

        var ifPasswordGameObj = GameObject.Find("Password");
        var ifPassword = ifPasswordGameObj.GetComponent<InputField>();

        ifUsername.text = username;
        ifPassword.text = password;

        var btnLogInGameObject = GameObject.Find("Login");
        var btnLogIn = btnLogInGameObject.GetComponent<Button>();
        btnLogIn.onClick.Invoke();

        yield return new WaitForSeconds(3);

        var bestTextGameObject = GameObject.Find("BestText");
        Assert.IsNotNull(bestTextGameObject, "Missing \"Today's Best\" from `title` scene");
    }

    [UnityTest]
    public IEnumerator testLoginAuthenticationError()
    {
        var ifUsernameGameObj = GameObject.Find("Name");
        var ifUsername = ifUsernameGameObj.GetComponent<InputField>();

        var ifPasswordGameObj = GameObject.Find("Password");
        var ifPassword = ifPasswordGameObj.GetComponent<InputField>();

		ifUsername.text = username;
        ifPassword.text = "wrongpasswd";

        var btnLogInGameObject = GameObject.Find("Login");
        var btnLogIn = btnLogInGameObject.GetComponent<Button>();
        btnLogIn.onClick.Invoke();

        yield return new WaitForSeconds(3);

        var txtNotationGameObj = GameObject.Find("Notation");
        var txtNotation = txtNotationGameObj.GetComponent<Text>();
        Assert.IsNotNull(txtNotation);
        Assert.True(txtNotation.text.Contains(authenError));
    }

    [UnityTest]
    public IEnumerator testSignUp()
    {
        NCMBUser.LogInAsync (username, password, (NCMBException e1) => {    
            if (e1 != null) {     
                UITestSettings.CallbackFlag = true;                   
            } else {
                NCMBUser.CurrentUser.DeleteAsync((NCMBException e2) => {
                    UITestSettings.CallbackFlag = true;
                });
            }
        });
        yield return UITestSettings.AwaitAsync();

        var ifUsernameGameObj = GameObject.Find("Name");
        var ifUsername = ifUsernameGameObj.GetComponent<InputField>();

        var ifPasswordGameObj = GameObject.Find("Password");
        var ifPassword = ifPasswordGameObj.GetComponent<InputField>();

		ifUsername.text = username;
        ifPassword.text = password;

        var btnSignUpGameObject = GameObject.Find("SignUp");
        var btnSignUp = btnSignUpGameObject.GetComponent<Button>();
        btnSignUp.onClick.Invoke();

        yield return new WaitForSeconds(3);

        var bestTextGameObject = GameObject.Find("BestText");
        Assert.IsNotNull(bestTextGameObject, "Missing \"Today's Best\" from `title` scene");
    }

    [UnityTest]
    public IEnumerator testSignUpError()
    {
        NCMBUser user = new NCMBUser();
        user.UserName = username;
        user.Password = password;
        user.SignUpAsync((NCMBException e) => {
            UITestSettings.CallbackFlag = true;
        });
        yield return UITestSettings.AwaitAsync();

        var ifUsernameGameObj = GameObject.Find("Name");
        var ifUsername = ifUsernameGameObj.GetComponent<InputField>();

        var ifPasswordGameObj = GameObject.Find("Password");
        var ifPassword = ifPasswordGameObj.GetComponent<InputField>();

        ifUsername.text = username;
        ifPassword.text = password;

        var btnSignUpGameObject = GameObject.Find("SignUp");
        var btnSignUp = btnSignUpGameObject.GetComponent<Button>();
        btnSignUp.onClick.Invoke();

        yield return new WaitForSeconds(3);

        var txtNotationGameObj = GameObject.Find("Notation");
        var txtNotation = txtNotationGameObj.GetComponent<Text>();
        Assert.IsNotNull(txtNotation);
        Assert.True(txtNotation.text.Contains(duplicationError));
    }

    [TearDown]
    public void TearDown()
    {

    }
}
