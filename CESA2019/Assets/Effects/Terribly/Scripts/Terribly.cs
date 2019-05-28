//
// Terribly.cs
// Actor: Tamamura Shuuki
//

using System;
using UnityEngine;


// どきどきする
//[ExecuteInEditMode] 
public class Terribly : MonoBehaviour
{

    [SerializeField]
    private Material _material = null;

    [SerializeField, Range(0, 30)]
    private int _iteration = 1;

    [SerializeField, Range(0, 1)]
    private float _intensity = 0.1f;

    private RenderTexture[] _renderTextures = new RenderTexture[30];

    public float _beatInterval;    // 鼓動間隔
    private float _beatCount;      // 次の鼓動までのカウント
    private bool _calmDown;        // どきどきフラグ

    private Func<bool> _terribly;   // どきどき関数


    private void Start()
    {
        _intensity = 1;

        _calmDown = false;
        _iteration = 0;

        _terribly = (() => {return true;});
    }

    private void Update()
    {
#if DEBUG == true
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Play();
        }
#endif

        _terribly();    // ドキドキする
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        var width = source.width;
        var height = source.height;
        var currentSource = source;

        var i = 0;
        RenderTexture currentDest = null;


        // ダウンサンプリング
        for (; i < _iteration; i++)
        {
            width /= 2;
            height /= 2;
            if (width < 2 || height < 2)
            {
                break;
            }
            currentDest = _renderTextures[i] = RenderTexture.GetTemporary(width, height, 0, source.format);

            // Blit時にマテリアルとパスを指定する
            Graphics.Blit(currentSource, currentDest, _material, 0);

            currentSource = currentDest;
        }

        // アップサンプリング
        for (i -= 2; i >= 0; i--)
        {
            currentDest = _renderTextures[i];

            // Blit時にマテリアルとパスを指定する
            Graphics.Blit(currentSource, currentDest, _material, 1);

            _renderTextures[i] = null;
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDest;
        }

        // 最後にdestにBlit
        Graphics.Blit(currentSource, destination, _material, 1);
        _material.SetFloat("_Intensity", _intensity);
        Graphics.Blit(currentSource, destination, _material, 2);

        if (_iteration > 0)
        {
            RenderTexture.ReleaseTemporary(currentSource);
        }
    }

    // ---------------------------------------------------
    // どきどきエフェクトを再生する
    // ---------------------------------------------------
    public void Play()
    {
        // ドキドキ関数を合成する
        _terribly = (() => {

            _beatCount += Time.deltaTime;

            // ドキドキ中
            if (_calmDown == true)
            {
                _iteration = 0;
                if (_iteration <= 0)
                {
                    _calmDown = false;
                }
            }
            // 落着き中
            else
            {
                if (_beatCount >= _beatInterval)
                {
                    _iteration = 3;
                    _beatCount = 0;
                    _calmDown = true;
                }
            }
            _iteration = Mathf.Max(_iteration, 0);

            _intensity = Mathf.Lerp(_intensity, 0.3f, Time.deltaTime);

            return true;
        });
    }

    // -------------------------------------------------
    // ドキドキを停止する
    // -------------------------------------------------
    public void Stop()
    {
        // ドキドキ関数を空にする
        _terribly = (() => {
            _calmDown = false;
            _iteration = 0;
            _intensity = Mathf.Lerp(_intensity, 1, Time.deltaTime);
            return true;
        });
    }
}
