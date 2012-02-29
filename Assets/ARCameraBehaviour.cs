using UnityEngine;
using System;
using System.Collections;
using jp.nyatla.nyartoolkit.cs.markersystem;
using jp.nyatla.nyartoolkit.cs.core;
using NyARUnityUtils;
using System.IO;

/// <summary>
/// AR camera behaviour.
/// This sample shows simpleLite demo.
/// 1.Connect webcam to your computer.
/// 2.Start sample program
/// 3.Take a "HIRO" marker on capture image
/// 
/// * Structure of scene
/// object tree
/// -Unity
///  +-camera (add ARCameraBehaviour)
///  +-MarkerObject as Gameobject (for transform matrix)
///    +-Cube as Cube
/// </summary>
public class ARCameraBehaviour : MonoBehaviour
{
	private NyARUnityMarkerSystem _ms;
	private NyARUnityWebCam _ss;
	private int mid;//marker id
	private GameObject _bg_panel;
	void Awake()
	{
		NyARMarkerSystemConfig config = new NyARMarkerSystemConfig(320,240);
		this._ms=new NyARUnityMarkerSystem(config);
		mid=this._ms.addARMarker("./Assets/Data/patt.hiro",16,25,80);

		//setup unity webcam
		WebCamDevice[] devices= WebCamTexture.devices;
		WebCamTexture w;
		if (devices.Length > 0){
			w=new WebCamTexture(320, 240, 15);
			this._ss=new NyARUnityWebCam(w);
			//setup background
			this._bg_panel=GameObject.Find("Plane");
			this._bg_panel.renderer.material.mainTexture=w;
			this._ms.setARBackgroundTransform(this._bg_panel.transform);
			
			//setup camera projection
			this._ms.setARCameraProjection(this.camera);
			
		}else{
			Debug.LogError("No Webcam.");
		}
	}
	// Use this for initialization
	void Start ()
	{
		//start sensor
		this._ss.start();
	}
	// Update is called once per frame
	void Update ()
	{
		//Update SensourSystem
		this._ss.update();
		//Update marker system by ss
		this._ms.update(this._ss);
		//update Gameobject transform
		if(this._ms.isExistMarker(mid)){
			this._ms.setUnityMarkerTransform(mid,GameObject.Find("MarkerObject").transform);
		}else{
			// hide Game object
			GameObject.Find("MarkerObject").transform.localPosition=new Vector3(0,0,-100);
		}
	}
}
