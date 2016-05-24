using UnityEngine;
using System.Collections;

public class WebcamTestBehaviourScript : MonoBehaviour {


    IEnumerator Start()
    {
        //Show Authrizatton dialog box.
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        //許可が出ればWebCamTextureを使用する
        if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
            WebCamTexture w = new WebCamTexture();
            //Materialにテクスチャを貼り付け
            GetComponent<Renderer>().material.mainTexture = w;
            //再生
            w.Play();
        }
    }
}
