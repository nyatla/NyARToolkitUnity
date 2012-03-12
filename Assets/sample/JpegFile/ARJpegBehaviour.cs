using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using jp.nyatla.nyartoolkit.cs.markersystem;
using jp.nyatla.nyartoolkit.cs.core;
using NyARUnityUtils;
using System.IO;

public class ARJpegBehaviour : MonoBehaviour
{
	private NyARUnityMarkerSystem _ms;
	private NyARUnitySensor _ss;
	private int mid;//marker id
	private GameObject _bg_panel;
	
	void Awake()
	{
		//setup texture
		Texture2D tex= (Texture2D)Resources.Load("320x240ABGR", typeof(Texture2D));
		this._ss=new NyARUnitySensor(tex.width,tex.height);
		this._ss.update(tex);
	
		NyARMarkerSystemConfig config = new NyARMarkerSystemConfig(tex.width,tex.height);
		this._ms=new NyARUnityMarkerSystem(config);
		this._ms.setConfidenceThreshold(0.1);
		mid=this._ms.addARMarker("./Assets/Data/patt.hiro",16,25,80);

		//setup background
		this._bg_panel=GameObject.Find("Plane");
		this._bg_panel.renderer.material.mainTexture=tex;
		this._ms.setARBackgroundTransform(this._bg_panel.transform);
		
		//setup camera projection
		this._ms.setARCameraProjection(this.camera);
	}
	// Use this for initialization
	void Start ()
	{
	}
	// Update is called once per frame
	void Update ()
	{
		//Update marker system by ss
		this._ms.update(this._ss);
		//update Gameobject transform
		if(this._ms.isExistMarker(mid)){
			Debug.Log(c+":"+this._ms.getConfidence(mid));
			this._ms.setMarkerTransform(mid,GameObject.Find("MarkerObject").transform);
		Debug.Log(this._ms.getConfidence(mid));
		}else{
			// hide Game object
			GameObject.Find("MarkerObject").transform.localPosition=new Vector3(0,0,-100);
		}
	}
	int c=0;
}
