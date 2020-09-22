using UnityEngine;
using UnityEngine.UI;

namespace TranslucentUI
{
    [AddComponentMenu("TranslucentUI/Translucency")]
    [RequireComponent(typeof(Image))]
    public class Translucency : MonoBehaviour
    {
        private int _BlurTexID;
        private int _BrightnessID;
        private int _GreyScaleID;

        [Range(-1f, 1f)] public float Brightness;

        [Range(0f, 1f)] public float GreyScale;

        private Image image;
        private Material translucencyMat;
        private TranslucentUICamera translucentUICamera;
        private TranslucentUICameraMobile translucentUICameraMobile;

        [Range(0f, 1f)] public float Transparency = 0.5f;

        private void Start()
        {
            translucentUICamera = FindObjectOfType<TranslucentUICamera>();
            if (translucentUICamera != null)
            {
                image = GetComponent<Image>();
                if (translucentUICamera.blurOption == BlurOption.BlurBehindUI)
                {
                    if (translucencyMat != null)
                    {
                        image.material = translucencyMat;
                    }
                    else
                    {
                        var translucentImage = Shader.Find("Custom/Translucency");
                        var translucentMat = new Material(translucentImage);
                        image.material = translucentMat;
                    }

                    _BlurTexID = Shader.PropertyToID("_BlurTex");
                    _GreyScaleID = Shader.PropertyToID("_GreyScale");
                    _BrightnessID = Shader.PropertyToID("_Brightness");
                }
            }
            else
            {
                translucentUICameraMobile = FindObjectOfType<TranslucentUICameraMobile>();
                if (translucentUICameraMobile != null)
                {
                    image = GetComponent<Image>();
                    if (translucentUICameraMobile.blurOption == BlurOption.BlurBehindUI)
                    {
                        if (translucencyMat != null)
                        {
                            image.material = translucencyMat;
                        }
                        else
                        {
                            var translucentImage = Shader.Find("Custom/Translucency");
                            var translucentMat = new Material(translucentImage);
                            image.material = translucentMat;
                        }

                        _BlurTexID = Shader.PropertyToID("_BlurTex");
                        _GreyScaleID = Shader.PropertyToID("_GreyScale");
                        _BrightnessID = Shader.PropertyToID("_Brightness");
                    }
                }
            }

            if (image)
            {
                var color = image.color;
                Transparency = 1 - color.a;
            }
        }

        public void SetTranslucencyMaterial(Material mat)
        {
            translucencyMat = mat;
        }

        /*public void SetTransparency(float transparency)
        {
            Transparency = transparency;
        }*/

        public void SetGreyScale(float greyScale)
        {
            GreyScale = greyScale;
        }

        public void SetBrightness(float brightness)
        {
            Brightness = brightness;
        }

        private void LateUpdate()
        {
            if (translucentUICamera && translucentUICamera.blurOption == BlurOption.BlurBehindUI)
            {
                if (translucentUICamera.BlurRT != null)
                {
                    image.materialForRendering.SetTexture(_BlurTexID, translucentUICamera.BlurRT);
                    //image.material.SetTexture (_BlurTexID, translucentUICamera.BlurRT);
                    image.material.SetFloat(_GreyScaleID, GreyScale);
                    image.material.SetFloat(_BrightnessID, Brightness);
                }
            }
            else if (translucentUICameraMobile && translucentUICameraMobile.blurOption == BlurOption.BlurBehindUI)
            {
                if (translucentUICameraMobile.BlurRT != null)
                {
                    image.materialForRendering.SetTexture(_BlurTexID, translucentUICameraMobile.BlurRT);
                    //image.material.SetTexture (_BlurTexID, translucentUICameraMobile.BlurRT);
                    image.material.SetFloat(_GreyScaleID, GreyScale);
                    image.material.SetFloat(_BrightnessID, Brightness);
                }
            }

            if (image)
            {
                var color = image.color;
                color.a = 1.0f - Transparency;
                image.color = color;
            }
        }
    }
}