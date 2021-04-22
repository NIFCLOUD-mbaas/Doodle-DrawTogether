using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using NCMB;

public class DrawSceneTest : MonoBehaviour
{
    private static string username = "testuser";
    private static string password = "123456";

    [SetUp]
    public void init()
    {
        SceneManager.LoadScene("login");
        UITestSettings.CallbackFlag = false;
    }
    

    [UnityTest]
    public IEnumerator testSubmitNewTheme()
    {
        NCMBUser.LogInAsync (username, password, (NCMBException e1) => {    
            if (e1 != null) {     
                UITestSettings.CallbackFlag = true;                   
            } else {
                // delete if the user exists
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

        var btnNewThemGameObject = GameObject.Find("newTheme");
        Assert.IsNotNull(btnNewThemGameObject, "Missing `New Theme` button from `title` scene");
        var btnNewThem = btnNewThemGameObject.GetComponent<Button>();
        btnNewThem.onClick.Invoke();

        yield return new WaitForSeconds(3);

        var btnSubmitGameObject = GameObject.Find("Submit");
        Assert.IsNotNull(btnSubmitGameObject, "Missing `Submit` button from scene `draw`");
        var btnSubmit = btnSubmitGameObject.GetComponent<Button>();
        btnSubmit.onClick.Invoke();

        yield return new WaitForSeconds(5);
        var titleTodayTheme = GameObject.Find("ThemeImage");
        Assert.IsNotNull(titleTodayTheme, "Missing `Today's Themes` title from scene `themes`");
    }

    [UnityTest]
    public IEnumerator testClickGoBackFromDrawScene()
    {
        NCMBUser.LogInAsync (username, password, (NCMBException e1) => {    
            if (e1 != null) {     
                UITestSettings.CallbackFlag = true;                   
            } else {
                // delete if the user exists
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

        var btnNewThemGameObject = GameObject.Find("newTheme");
        Assert.IsNotNull(btnNewThemGameObject, "Missing `New Theme` button from `title` scene");
        var btnNewThem = btnNewThemGameObject.GetComponent<Button>();
        btnNewThem.onClick.Invoke();

        yield return new WaitForSeconds(3);

        var btnBackGameObject = GameObject.Find("Back");
        Assert.IsNotNull(btnBackGameObject, "Missing `Back` button from scene `draw`");
        var btnBack = btnBackGameObject.GetComponent<Button>();
        btnBack.onClick.Invoke();

        yield return new WaitForSeconds(1);
        // back to title scene
        var bestTextGameObject = GameObject.Find("BestText");
        Assert.IsNotNull(bestTextGameObject, "Missing \"Today's Best\" from `title` scene");
    }
}
