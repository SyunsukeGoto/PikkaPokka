//
// Outline.cs
// Actor: Tamamura Shuuki
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 輪郭を表示する
// スクリプトから適用する場合エラーが出ますが、一度実行すれば直ります
[ExecuteInEditMode]
public class Outline : MonoBehaviour
{

    [SerializeField]
    private Shader _outlineShader = null;      // アウトライン描画用

    [SerializeField]
    private Texture _texture = null;           // 通常レンダリング用のテクスチャ

    [SerializeField]
    private Color _color = Color.black;        // アウトラインの色

    public float _outlineSize;      // アウトラインの大きさ

    private Material _outlineMaterial;  // アウトラインマテリアル
    private Renderer _renderer;         // レンダラー


    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        _outlineMaterial = new Material(_outlineShader);
        _renderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        _outlineMaterial.SetTexture("_MainTex", _texture);
        _outlineMaterial.SetColor("_Color", _color);
        _outlineMaterial.SetFloat("_OutlineSize", _outlineSize);

        _renderer.material = _outlineMaterial;
    }
}
