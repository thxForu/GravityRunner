using UnityEngine;

namespace TranslucentUI
{
    [AddComponentMenu("TranslucentUI/TranslucentUICamera")]
    //[RequireComponent(typeof(Camera))]
    public class TranslucentUICamera : MonoBehaviour
    {
        private int __BrightnessID;

        private int __GreyScaleID;
        private Material blurBackgroundMat;

        public BlurOption blurOption = BlurOption.BlurBehindUI;

        //private Camera mainCamera = null;

        [Range(-1f, 1f)] public float Brightness;

        [Range(0, 4)] public int DownSample = 2;

        [Range(0f, 1f)] public float GreyScale;

        [Range(0, 4)] public int Iterations = 2;

        public BlurKernelSize kernalSize = BlurKernelSize.Medium;
        private float lastCameraUpdateTime;
        private float minCameraUpdateGap;
        private int screenHeight;
        private int screenWidth;

        [Range(0, 60)] public int UpdateFrameRate = 60;

        public RenderTexture BlurRT { get; private set; }

        private void Start()
        {
        }

        private void Awake()
        {
            blurBackgroundMat = new Material(Shader.Find("Custom/CameraBlur"));

            __GreyScaleID = Shader.PropertyToID("__GreyScale");
            __BrightnessID = Shader.PropertyToID("__Brightness");

            BlurRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);

            screenHeight = Screen.height;
            screenWidth = Screen.width;

            BlurRT.Create();
            lastCameraUpdateTime = 0.0f;
        }

        private void Update()
        {
            minCameraUpdateGap = 1.0f / UpdateFrameRate;
            if (blurOption == BlurOption.BlurBackground)
            {
                blurBackgroundMat.SetFloat(__GreyScaleID, GreyScale);
                blurBackgroundMat.SetFloat(__BrightnessID, Brightness);
            }
        }

        private void BlurBackground(RenderTexture source, RenderTexture destination)
        {
            var tempRT2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            var kernal = (int) kernalSize;
            for (var i = 0; i < Iterations; i++)
            {
                var _blurRange = 0.6f;
                var radius = i * _blurRange + _blurRange;
                blurBackgroundMat.SetFloat("_Radius", radius);
                Graphics.Blit(source, tempRT2, blurBackgroundMat, kernal + 1);
                source.DiscardContents();

                if (i == Iterations - 1)
                {
                    if (blurOption == BlurOption.BlurBehindUI) destination.DiscardContents();
                    Graphics.Blit(tempRT2, destination, blurBackgroundMat, kernal + 2);
                }
                else
                {
                    Graphics.Blit(tempRT2, source, blurBackgroundMat, kernal + 2);
                    tempRT2.DiscardContents();
                }
            }

            RenderTexture.ReleaseTemporary(tempRT2);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (screenWidth != Screen.width && screenHeight != Screen.height)
            {
                screenWidth = Screen.width;
                screenHeight = Screen.height;

                // orientation is changed
                BlurRT.height = Screen.height;
                BlurRT.width = Screen.width;
            }

            if (blurOption == BlurOption.BlurBackground)
            {
                if (Iterations == 0)
                {
                    Graphics.Blit(source, destination);
                    return;
                }

                var w = Screen.width >> DownSample;
                var h = Screen.height >> DownSample;

                var tempRT = RenderTexture.GetTemporary(w, h, 0, source.format);
                Graphics.Blit(source, tempRT);
                BlurBackground(tempRT, destination);
                RenderTexture.ReleaseTemporary(tempRT);
            }
            else if (blurOption == BlurOption.BlurBehindUI)
            {
                if (Iterations == 0)
                {
                    Graphics.Blit(source, BlurRT);
                    Graphics.Blit(source, destination);
                    return;
                }

                var now = Time.unscaledTime;
                if (now - lastCameraUpdateTime >= minCameraUpdateGap)
                {
                    var w = Screen.width >> DownSample;
                    var h = Screen.height >> DownSample;
                    var tempRT = RenderTexture.GetTemporary(w, h, 0, source.format);
                    Graphics.Blit(source, tempRT);
                    BlurBackground(tempRT, BlurRT);
                    RenderTexture.ReleaseTemporary(tempRT);
                    lastCameraUpdateTime = Time.unscaledTime;
                }

                Graphics.Blit(source, destination);
            }
        }
    }
}