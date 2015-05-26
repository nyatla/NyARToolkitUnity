# NyARToolkit for Unity
version 4.1.1

Copyright (C)2008-2012 Ryo Iizuka

http://nyatla.jp/nyartoolkit/  
airmail(at)ebony.plala.or.jp  
wm(at)nyatla.jp


## About NyARToolkit
* NyARToolkitは、NyARToolKit 4.1.1のAPIを基盤としたARアプリケーション向けのクラスライブラリです。
* Unity Player 4.0以上で動作します。
* ARToolKitの基本機能と、NyARToolKitオリジナルの拡張機能、フレームワークで構成しています。
* sampleモジュールは、いくつかの動作チェックプログラムと、RPFを使ったサンプルアプリケーションがあります。


ARToolKitについては、下記のURLをご覧ください。  
http://www.hitl.washington.edu/artoolkit/

## 特徴

NyARToolkit for Unityの特徴を紹介します。
* Unity標準のWebCameraオブジェクト、テクスチャ形式の入力が可能です。
* 純粋なC#で実装されています。プラットフォームに依存しません。WebPlayerでも使用できます。
* NyId規格のIDマーカが使用できます。
* MarkerSystemが使用できます。MarkerSystemと組み合わせることで、コンパクトな実装ができます。




## サンプルの概要

### Unityアプリケーション
sampleディレクトリ以下にあります。

##### Sample/ImagePickup
マーカ平面から画像を取得するサンプルです。
##### Sample/JpegFile
静止画からマーカを検出するサンプルです。
##### Sample/MarkerPlane
マーカ平面とスクリーン座標を変換するサンプルです。
##### Sample/SimpleLite
ARマーカの上に立方体を表示するサンプルです。単一マーカのサンプルです。
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
