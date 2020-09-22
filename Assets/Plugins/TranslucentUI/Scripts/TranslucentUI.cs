using UnityEngine;
using UnityEngine.UI;

namespace TranslucentUI
{
    [AddComponentMenu("TranslucentUI/TranslucentUI")]
    //[RequireComponent(typeof(Canvas))]
    public class TranslucentUI : MonoBehaviour
    {
        public bool ApplyOnChildren;

        public BlurOption blurOption = BlurOption.BlurBehindUI;

        [Range(-1f, 1f)] public float Brightness;

        [Range(0, 4)] public int DownSample = 2;

        [Range(0f, 1f)] public float GreyScale;

        [Range(0, 4)] public int Iterations = 2;

        public BlurKernelSize kernalSize = BlurKernelSize.Medium;
        //[Range(0f, 1f)]
        //public float Transparency = 0.5f;

        [SerializeField] public Camera mainCamera;

        public bool MobileDevice;

        private Shader shader;
        private Material translucencyMat;
        private TranslucentUICamera translucentUICamera;
        private TranslucentUICameraMobile translucentUICameraMobile;
        private Image uiImage;

        private Image[] uiImages;

        [Range(0, 60)] public int UpdateFrameRate = 60;

        // Use this for initialization
        private void Start()
        {
        }

        public void AddTranslucencyComponent()
        {
            if (shader == null)
                shader = Shader.Find("Custom/Translucency");

            if (translucencyMat)
                translucencyMat = new Material(shader);


            if (ApplyOnChildren)
            {
                AddTranslucencyComponentOnChildren();
            }
            else
            {
                uiImage = GetComponent<Image>();
                if (uiImage != null)
                {
                    var translucency = uiImage.gameObject.GetComponent<Translucency>();
                    if (translucency == null)
                    {
                        translucency = uiImage.gameObject.AddComponent<Translucency>();
                        translucency.SetTranslucencyMaterial(translucencyMat);
                        translucency.SetGreyScale(GreyScale);
                        translucency.SetBrightness(Brightness);
                    }
                }
            }
        }

        public void AddTranslucentUICamera()
        {
            if (mainCamera)
            {
                translucentUICamera = mainCamera.gameObject.GetComponent<TranslucentUICamera>();
                if (translucentUICamera == null) mainCamera.gameObject.AddComponent<TranslucentUICamera>();
            }
        }

        public void RemoveTranslucentUICamera()
        {
            if (mainCamera)
            {
                var tuc = mainCamera.gameObject.GetComponent<TranslucentUICamera>();
                if (tuc != null)
                {
                    DestroyImmediate(tuc);
                    translucentUICamera = null;
                }
            }
        }

        public void AddTranslucentUICameraMobile()
        {
            if (mainCamera)
            {
                translucentUICameraMobile = mainCamera.gameObject.GetComponent<TranslucentUICameraMobile>();
                if (translucentUICameraMobile == null) mainCamera.gameObject.AddComponent<TranslucentUICameraMobile>();
            }
        }

        public void RemoveTranslucentUICameraMobile()
        {
            if (mainCamera)
            {
                var tuc = mainCamera.gameObject.GetComponent<TranslucentUICameraMobile>();
                if (tuc != null)
                {
                    DestroyImmediate(tuc);
                    translucentUICameraMobile = null;
                }
            }
        }

        public void AddTranslucencyComponentOnChildren()
        {
            uiImages = GetComponentsInChildren<Image>();
            for (var i = 0; i < uiImages.Length; i++)
            {
                var translucency = uiImages[i].gameObject.GetComponent<Translucency>();
                if (translucency == null)
                {
                    translucency = uiImages[i].gameObject.AddComponent<Translucency>();
                    translucency.SetTranslucencyMaterial(translucencyMat);
                    translucency.SetGreyScale(GreyScale);
                    translucency.SetBrightness(Brightness);
                }
            }
        }

        public void RemoveTranslucentUI()
        {
            foreach (var image in GetComponentsInChildren<Image>())
            {
                var translucency = image.GetComponent<Translucency>();
                if (translucency != null) DestroyImmediate(translucency);
            }

            RemoveTranslucentUICamera();
            RemoveTranslucentUICameraMobile();

            var tui = GetComponent<TranslucentUI>();
            if (tui != null) DestroyImmediate(tui);
        }

        /*
        public void RemoveTranslucencyComponentFromChildren()
        {
            foreach (var image in GetComponentsInChildren<Image>())
            {
                var translucency = image.GetComponent<Translucency>();
                var translucentUI = image.GetComponent<TranslucentUI>();
                if (translucency != null && translucentUI == null) DestroyImmediate(translucency);
            }

            uiImages = GetComponentsInChildren<Image>();
        }*/

        public void ApplyCameraProperties()
        {
            if (MobileDevice)
            {
                if (translucentUICameraMobile != null)
                {
                    translucentUICameraMobile.blurOption = blurOption;
                    translucentUICameraMobile.DownSample = DownSample;
                    translucentUICameraMobile.Iterations = Iterations;
                    translucentUICameraMobile.UpdateFrameRate = UpdateFrameRate;
                }
            }
            else
            {
                if (translucentUICamera != null)
                {
                    translucentUICamera.blurOption = blurOption;
                    translucentUICamera.DownSample = DownSample;
                    translucentUICamera.Iterations = Iterations;
                    translucentUICamera.kernalSize = kernalSize;
                    translucentUICamera.UpdateFrameRate = UpdateFrameRate;
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (ApplyOnChildren)
            {/*
                for (var i = 0; i < uiImages.Length; i++)
                {
                    var translucency = uiImages[i].gameObject.GetComponent<Translucency>();
                    translucency.SetGreyScale(GreyScale);
                    translucency.SetBrightness(Brightness);
                }
                */
                
            }
            else
            {
                if (uiImage != null)
                {
                    var translucency = uiImage.gameObject.GetComponent<Translucency>();
                    translucency.SetGreyScale(GreyScale);
                    translucency.SetBrightness(Brightness);
                }
            }


            if (blurOption == BlurOption.BlurBackground)
            {
                if (MobileDevice)
                {
                    if (translucentUICameraMobile != null)
                    {
                        translucentUICameraMobile.Brightness = Brightness;
                        translucentUICameraMobile.GreyScale = GreyScale;
                    }
                }
                else
                {
                    if (translucentUICamera != null)
                    {
                        translucentUICamera.Brightness = Brightness;
                        translucentUICamera.GreyScale = GreyScale;
                    }
                }
            }
            if (ApplyOnChildren)
            {/*
            
                for (var i = 0; i < uiImages.Length; i++)
                {
                    var translucency = uiImages[i].gameObject.GetComponent<Translucency>();
                    translucency.SetGreyScale(GreyScale);
                    translucency.SetBrightness(Brightness);
                }*/
                
            }
            else
            {
                if (uiImage != null)
                {
                    var translucency = uiImage.gameObject.GetComponent<Translucency>();
                    translucency.SetGreyScale(GreyScale);
                    translucency.SetBrightness(Brightness);
                }
            }


            if (blurOption == BlurOption.BlurBackground)
            {
                if (MobileDevice)
                {
                    if (translucentUICameraMobile != null)
                    {
                        translucentUICameraMobile.Brightness = Brightness;
                        translucentUICameraMobile.GreyScale = GreyScale;
                    }
                }
                else
                {
                    if (translucentUICamera != null)
                    {
                        translucentUICamera.Brightness = Brightness;
                        translucentUICamera.GreyScale = GreyScale;
                    }
                }
            }
        }
    }
}