using UnityEngine;
using System.Collections;
using jp.nyatla.nyartoolkit.cs.markersystem;
using jp.nyatla.nyartoolkit.cs.core;
using NyARUnityUtils;
using System.IO;




public class ARCameraBehaviour : MonoBehaviour
{
	private NyARUnityMarkerSystem _ms;
	private NyARUnityWebCam _ss;
	private int mid;
	void Awake()
	{
		NyARMarkerSystemConfig config = new NyARMarkerSystemConfig(320,240);
		this._ms=new NyARUnityMarkerSystem(config);
		mid=this._ms.addARMarker("../data/patt.hiro",16,25,80);
//		mid=this._ms.addNyIdMarker(55,80);
		WebCamDevice[] devices= WebCamTexture.devices;
		if (devices.Length > 0){
			WebCamTexture w=new WebCamTexture(320, 240, 15);
			
//			this._ss=new NyARUnityWebCam(new WebCamTexture(320, 240, 12));
			this._ss=new NyARUnityWebCam(w);
		}else{
			Debug.LogError("No Webcam.");
		}
		GameObject.Find("Plane").renderer.material.mainTexture=new Texture2D(320,240,TextureFormat.BGRA32,false);
	}
	// Use this for initialization
	void Start ()
	{
		//setup camera projection
//		this.camera.projectionMatrix=this._ms.getUnityProjectionMatrix();
		this.camera.nearClipPlane=10;
		this.camera.farClipPlane=10000;
		this.camera.transform.LookAt(new Vector3(0,0,0),new Vector3(1,0,0));
		GameObject bg=GameObject.Find("Plane");
		bg.transform.position=new Vector3(0,0,4000);
		bg.transform.localScale=new Vector3(-320,1f,240);
		
		this._ss.start();
	}
	static int r=0;
	// Update is called once per frame
	void Update ()
	{
		//Update SensourSystem
		this._ss.update();
		this._ms.update(this._ss);
		this._ss.dGetGsTex((Texture2D)(GameObject.Find("Plane").renderer.material.mainTexture));
		if(this._ms.isExistMarker(mid)){
			Transform t=GameObject.Find("Cube").transform;
			
			this._ms.setTransformFromMatrix(mid,t);
//			t.localPosition=this._ms.getUnityMarkerMatrix(mid).GetColumn(3);
//			.localToWorldMatrix.SetRow();
			//=this._ms.getUnityMarkerMatrix(mid);
//			Debug.Log("Y"+r++);
		}else{
//			Debug.Log("N"+r++);
		}
		//setup Gameobject?
	}
}
