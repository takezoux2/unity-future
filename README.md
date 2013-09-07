# UnityFuture

Future(,Promise or deferred) をUnity向けに提供するライブラリです。
他のライブラリと組み合わせて非同期処理を楽に記述することを目的としています。

## 使い方

Assets/Outputディレクトリの中のファイルを自分のプロジェクトにコピーしてください。

## サンプルコード

    using GeishaTokyo.Concurrent.Factory;
    using UnityEngine;
    ...
    void Hoge(){
        Future<string> f = FutureFactory.NewFuture<string>();
        
	
	f.OnComplete( result => {
            if(result.Success){
                Debug.Log("Result = " + result.Result);
            }else{
                Debug.Log("Error = " + result.Error);
            }
        });

        f.SetResult("ok");

    }



