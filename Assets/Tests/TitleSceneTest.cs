using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using NCMB;

public class TitleSceneTest : MonoBehaviour
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
    public IEnumerator testClickNewTheme()
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

        var drawingPanelGameObject = GameObject.Find("DrawingPanel");
        Assert.IsNotNull(drawingPanelGameObject, "Missing `DrawingPanel` from scene `draw`");
    }

    [UnityTest]
    public IEnumerator testClickTodayThemes()
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

        var btnTodayThemesGameObj = GameObject.Find("themes");
        Assert.IsNotNull(btnTodayThemesGameObj, "Missing `Today's Themes` button from `title` scene");
        var btnTodayThemes = btnTodayThemesGameObj.GetComponent<Button>();
        btnTodayThemes.onClick.Invoke();

        yield return new WaitForSeconds(3);

        var titleTodayTheme = GameObject.Find("ThemeImage");
        Assert.IsNotNull(titleTodayTheme, "Missing `Today's Themes` title from scene `themes`");
    }
}
