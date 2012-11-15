using UnityEngine;
using System.Collections;

public class WebcamTestBehaviourScript : MonoBehaviour {


    IEnumerator Start ()
    {
            //許可ダイアログが出る。ユーザーが反応を起こすまで待つ
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            //許可が出ればWebCamTextureを使用する
            if (Application.HasUserAuthorization (UserAuthorization.WebCam)) {
                    WebCamTexture w = new WebCamTexture ();
                    //Materialにテクスチャを貼り付け
                    renderer.material.mainTexture = w;
                    //再生
                    w.Play ();
            }
    }
}
