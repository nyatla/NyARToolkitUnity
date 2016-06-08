[English here!](README.EN.md "")

# NyARToolkit for Unity
version 5.0.8

Copyright (C)2008-2012 Ryo Iizuka

http://nyatla.jp/nyartoolkit/  
airmail(at)ebony.plala.or.jp  
wm(at)nyatla.jp

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/h_p89B1i6u0/0.jpg)](https://www.youtube.com/watch?v=h_p89B1i6u0&feature=youtu.be)

## About NyARToolkit
* NyARToolkitUnityは、NyARToolKitCSのUnity向けの実装です。
* Unity5.3以降に対応しています。
* 画像データから、ターゲット(ARマーカ、IDマーカ、NFTマーカ)の位置と姿勢を計算することができます。
* 純粋なC#による実装です。


ARToolKitについては、下記のURLをご覧ください。  
http://www.hitl.washington.edu/artoolkit/



## サンプルの概要

### Unityアプリケーション
sampleディレクトリ以下にあります。

##### Sample/SimpleLite
ARマーカの上に立方体を表示するサンプルです。単一マーカのサンプルです。
##### Sample/SimpleNFT
NFT（自然特徴点マーカ）を使うサンプルです。
##### Sample/ImagePickup
マーカ平面から画像を取得するサンプルです。
##### Sample/JpegFile
静止画からマーカを検出するサンプルです。
##### Sample/MarkerPlane
マーカ平面とスクリーン座標を変換するサンプルです。
##### Sample/SimpleLiteM
複数のARマーカの上に立方体を表示するサンプルです。


### webプレイヤー

sample.webplayerディレクトリ以下にあります。

##### SimpleLiteWeb
単一マーカのサンプルです。 Webプレイヤーで再生できます。

##### WebCamTest
WebプレイヤーでWebカメラを使用するサンプルです。

## ライセンス

NyARToolkitAS3は、商用ライセンスとLGPLv3のデュアルライセンスを採用しています。

LGPLv3を承諾された場合には、商用、非商用にかかわらず、無償でご利用になれます。 LGPLv3を承諾できない場合には、商用ライセンスの購入をご検討ください。

* LGPLv3  
LGPLv3については、COPYING.txtをお読みください。
* 商用ライセンス  
商用ライセンスについては、ARToolWorks社に管理を委託しております。http://www.artoolworks.com/Home.html


## SimpleNFTのカスタマイズ

SimpleNFTが使う画像特徴点ファイル(nftdataset)は、パッケージのData/toolsにある、NftFileGeneratorを使って作成できます。
NftFileGeneratorで特徴点を計算後、Exportメニューの"Save NYARTK nft dataset"で.datasetを保存してください。
保存したファイルの拡張子を.bytesに変更して、Resourceディレクトリにコピーすることで、Unityからアクセスできるようになります。

## チュートリアル
* http://nyatla.jp/nyartoolkit/wp/?p=1778

