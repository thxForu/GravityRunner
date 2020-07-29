using Termway.Helper;

using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Termway.TimeframeUI
{
    /// <summary>
    /// UI that show a grid and graph with timeframe performance in milliseconds (ms) and frame per seconds (fps).
    /// Drag and drop the file on a gameObject and rebuild the ui or add it from the menu GameObject > UI > Timeframe.
    /// </summary>
    [HelpURL("https://gitlab.com/TermWay/timeframeui")]
    [AddComponentMenu("UI/Timeframe")]
    [DisallowMultipleComponent]
    public class TimeframeUI : MonoBehaviour
    {
        public enum StatsFormat
        {
            Legend,
            Ms,
            Fps
        }

        /// <summary>
        /// Max value displayed by the grid.
        /// </summary>
        const uint MAX_GRID_VALUE = 9999;

        [Tooltip("Short key to toggle the UI visibility. Visibility does not stop the recording of the time frame values.")]
        public KeyCode toggleUiKey = KeyCode.F2;

        [Tooltip("Default visibility of the UI which is the visibility on the editor and the visibility on the first frame of the application.")]
        public bool defaultVisibility = true;

        [Tooltip("Latest frame number used to compute statistics on.")]
        public uint statsFrameRange = 2048;

        [Tooltip("Force the frame rate value. The zero value means that there is no forced frame rate.")]
        public uint targetFramerate = 0;

        [Tooltip(@"Anchor to snap the UI at one corner. Will rebuild the UI.
    - (0,0) is bottom left,
    - (0,1) is bottom right,
    - (1,0) is top left,
    - (1,1) is top right.")]
        [Vector2IntRange(0, 1)]
        public Vector2Int uiAnchorPosition = new Vector2Int(0, 1);

        [Tooltip("UI absolute global offset in pixel according to the anchor position. Will rebuild the UI.")]
        [Vector2IntRange(-8192, 8192)]
        public Vector2Int uiOffset;


        [Header("Grid")]
        [Tooltip("Delay in second between each value update of the grid.")]
        public float gridUpdateDelaySeconds = 1;

        [Tooltip("Visibility of the legend row and column. Will rebuild the UI.")]
        public bool showLegend = true;

        [Tooltip("Visibility of the time frame (millisecond) column. Will rebuild the UI.")]
        public bool showMs = true;

        [Tooltip("Visibility of the frame rate (frame per second) column. Will rebuild the UI.")]
        public bool showFps = true;

        [Tooltip("Cell size of the grid in pixel. Will resize the grid. Will rebuild the UI.")]
        [Vector2Range(7, 200, 5, 200)]
        public Vector2 gridCellSize = new Vector2(35, 17);

        [Tooltip("Statistics to include in the grid display in the given order. Each data is a row in the grid.")]
        public GridRow[] gridRowsData;


        [Header("Graph")]
        [Tooltip("Visibility of the graph. Will rebuild the UI.")]
        public bool showGraph = true;

        [Tooltip("Automatic resize of the width when the resolution changed. Will rebuild the graph.")]
        public bool graphWidthAutoResize = false;

        [Tooltip("Width of the graph. If the auto resize option is activated and the value set to zero the graph will take all the available width space. Will rebuild the graph.")]
        public uint graphWidth = 256;

        [Tooltip("texture size of the graph. Each column on the X axis has a unique statistic value. Should be below Stats Frame Range value. Will recreate the graph texture.")]
        [Vector2IntRange(2, 8192)]
        public Vector2Int graphTextureSize = new Vector2Int(256, 64);

        [Tooltip("Upper limit of Y axis of the graph in millisecond. The zero value means it's computed as twice the current median. Will recreate the graph texture.")]
        public uint graphMsUpperLimit = 0;

        [Tooltip("Tint color of the graph used to contrast new override painted value. Tint is multiplied to all the color painted.")]
        public Color graphColorAlternateTint = Color.white - Color.black * 0.3f;


        [SerializeField] GameObject canvasGameObject;
        [SerializeField] Image gridImage;
        [SerializeField] Image graphImage;
        [SerializeField] GridLayoutGroup gridLayoutGroup;
        [SerializeField] Texture2D texture;
        Color[] pixelsColor;

        Color clearColor = Color.clear; //Because static Color are not cached. 

        /// <summary>
        /// Reduce string allocation at runtime.
        /// [0, MAX_VALUE.9] => 10 * <see cref="MAX_GRID_VALUE"/> values. 
        /// </summary>
        string[] cachedFloatString;

        uint graphCurrentColumn;
        float timeSinceLastGridUpdate;

        Stats stats;


        /// <summary>
        /// Columns count of the grid ui. +1 for the legend.
        /// </summary>
        int GridColumnCount { get { return (showLegend ? 1 : 0) + (showMs ? 1 : 0) + (showFps ? 1 : 0); } }

        /// <summary>
        /// Rows count of the grid ui. +1 for the legend.
        /// </summary>
        int RowCount { get { return gridRowsData.Length + (showLegend ? 1 : 0); } }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/UI/Timeframe")]
        static void CreateAndAddUI()
        {
            GameObject timeframeGameObject = new GameObject(typeof(TimeframeUI).Name);
            UnityEditor.Undo.RegisterCreatedObjectUndo(timeframeGameObject, "TimeframeUI");
            TimeframeUI timeframeUI = UnityEditor.Undo.AddComponent<TimeframeUI>(timeframeGameObject);
            timeframeUI.BuildUI();
        }
#endif

        [ContextMenu("Default Stats", false, 1001)]
        void DefaultStatsThenBuildUI()
        {
            gridRowsData = new GridRow[]
            {
                new GridRow(GridRow.StatsType.Current, 50, GridRow.GraphRepresentation.BarFromBottom),
                new GridRow(GridRow.StatsType.Average),
                new GridRow(GridRow.StatsType.Percentile, 50),
                new GridRow(GridRow.StatsType.Percentile, 10),
                new GridRow(GridRow.StatsType.Percentile, 01),
            };
            BuildUI();
        }


        [ContextMenu("Full Stats with color", false, 1002)]
        void FullStatsThenBuildUI()
        {
            gridRowsData = new GridRow[]
            {
                new GridRow(GridRow.StatsType.Current, 50, GridRow.GraphRepresentation.BarFromBottom, Color.black * 0.7f),
                new GridRow(GridRow.StatsType.Average, 0, GridRow.GraphRepresentation.Point, new Color(0, 81f / 255, 188f / 255), true,  GridRow.ColorOperation.Blend),
                new GridRow(GridRow.StatsType.Min),
                new GridRow(GridRow.StatsType.Percentile, 99, GridRow.GraphRepresentation.BarFromBottom, new Color(128f / 255, 0, 0), true, GridRow.ColorOperation.Blend),
                new GridRow(GridRow.StatsType.Percentile, 90),
                new GridRow(GridRow.StatsType.Percentile, 50),
                new GridRow(GridRow.StatsType.Percentile, 10),
                new GridRow(GridRow.StatsType.Percentile, 01),
                new GridRow(GridRow.StatsType.Max)
            };
            BuildUI();
        }

        [ContextMenu("Minimal stats", false, 1003)]
        void MinimalStatsThenBuildUI()
        {
            gridRowsData = new GridRow[]
            {
                new GridRow(GridRow.StatsType.Current, 50, GridRow.GraphRepresentation.BarFromBottom),
                new GridRow(GridRow.StatsType.Average, 0, GridRow.GraphRepresentation.Point),
                new GridRow(GridRow.StatsType.Percentile, 01, GridRow.GraphRepresentation.Point),
            };
            BuildUI();
        }

        /// <summary>
        /// Destroy all children and rebuild ui.
        /// </summary>
        [ContextMenu("Rebuild UI", false, 1021)]
        internal void BuildUI()
        {
            //Remove canvas child game object.
            if (canvasGameObject)
                GameObject.DestroyImmediate(canvasGameObject);

            Transform canvasTransform = transform.Find("Canvas_" + typeof(TimeframeUI).Name);
            if (canvasTransform != null && canvasTransform.gameObject.GetComponent<Canvas>() != null)         
                GameObject.DestroyImmediate(canvasTransform.gameObject);

            foreach (GridRow displayedEntry in gridRowsData)
                displayedEntry.Texts.Clear();

            canvasGameObject = new GameObject("Canvas_" + typeof(TimeframeUI).Name);
            canvasGameObject.transform.parent = gameObject.transform;
            Canvas canvas = canvasGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject gridImageGameObject = new GameObject("GridImage");
            gridImageGameObject.transform.parent = canvasGameObject.transform;
            gridImage = gridImageGameObject.AddComponent<Image>();
            gridImage.rectTransform.anchorMin = uiAnchorPosition;
            gridImage.rectTransform.anchorMax = uiAnchorPosition;
            gridImage.color = new Color(0.2f, 0.2f, 0.2f, 0.2f);

            if (showGraph)
            {
                GameObject graphImageGameObject = new GameObject("GraphImage");
                graphImageGameObject.transform.parent = canvasGameObject.transform;
                graphImage = graphImageGameObject.AddComponent<Image>();
                graphImage.rectTransform.anchorMin = uiAnchorPosition;
                graphImage.rectTransform.anchorMax = uiAnchorPosition;
            }

            if (GridColumnCount > 0)
            {
                gridLayoutGroup = gridImageGameObject.AddComponent<GridLayoutGroup>();
                gridLayoutGroup.cellSize = gridCellSize;
                gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                gridLayoutGroup.constraintCount = RowCount;
            }

            if (showLegend)
                CreateTexts(gridImage, StatsFormat.Legend, FontStyle.Bold);

            if (showMs)
                CreateTexts(gridImage, StatsFormat.Ms);

            if (showFps)
                CreateTexts(gridImage, StatsFormat.Fps);

            ResizeGridUI();
            SetUiState(defaultVisibility);
            InitTexture();

            if (Application.isPlaying)
            {
                UpdateGrid();
                UpdateAllTextureColumns();
            }
        }

        [ContextMenu("Resize grid UI", false, 1022)]
        internal void ResizeGridUI()
        {
            if (GridColumnCount > 0)
                gridLayoutGroup.cellSize = gridCellSize;

            gridImage.rectTransform.sizeDelta = new Vector2(gridCellSize.x * GridColumnCount, gridCellSize.y * RowCount);
            int signX = uiAnchorPosition.x == 0 ? 1 : -1;
            int signY = uiAnchorPosition.y == 0 ? 1 : -1;
            gridImage.rectTransform.anchoredPosition = new Vector2(
                signX * gridImage.rectTransform.sizeDelta.x / 2 + uiOffset.x,
                signY * gridImage.rectTransform.sizeDelta.y / 2 - uiOffset.y);

            if (showGraph)
                ResizeGraphUI();
        }

        [ContextMenu("Resize graph UI", false, 1023)]
        internal void ResizeGraphUI()
        {
            if (showGraph)
            {
                int negIfAnchorIsTop = uiAnchorPosition.y == 1 ? -1 : 1;
                int negIfAnchorIsRight = uiAnchorPosition.x == 1 ? -1 : 1;

                // Graph size is the given graph width and the computed grid height.
                if (graphWidthAutoResize)
                {
                    graphImage.rectTransform.anchorMin = new Vector2(0, uiAnchorPosition.y);
                    graphImage.rectTransform.anchorMax = new Vector2(1, uiAnchorPosition.y);

                    float offsetFromBorderGridSide = gridImage.rectTransform.sizeDelta.x + uiOffset.x * negIfAnchorIsRight;
                    float offsetFromOtherBorder = graphWidth <= 0 ? 0 : GameWindowResolution.x - gridImage.rectTransform.sizeDelta.x - graphWidth - uiOffset.x * negIfAnchorIsRight;
                    graphImage.rectTransform.Left(uiAnchorPosition.x == 0 ? offsetFromBorderGridSide : offsetFromOtherBorder);
                    graphImage.rectTransform.Right(uiAnchorPosition.x == 0 ? offsetFromOtherBorder : offsetFromBorderGridSide);
                    graphImage.rectTransform.Height(gridImage.rectTransform.sizeDelta.y);
                    graphImage.rectTransform.PosY(negIfAnchorIsTop * gridImage.rectTransform.sizeDelta.y / 2 - uiOffset.y);
                }
                else
                {
                    int subWidthIfAnchorIsRight = uiAnchorPosition.x == 1 ? -(int)graphWidth : 0;

                    graphImage.rectTransform.anchoredPosition = new Vector2(
                        subWidthIfAnchorIsRight + negIfAnchorIsRight * gridImage.rectTransform.sizeDelta.x + graphWidth / 2 + uiOffset.x,
                        negIfAnchorIsTop * gridImage.rectTransform.sizeDelta.y / 2 - uiOffset.y);
                    graphImage.rectTransform.sizeDelta = new Vector2(graphWidth, gridImage.rectTransform.sizeDelta.y);
                }
            }
        }

        void CreateTexts(Image image, StatsFormat statsFormat, FontStyle fontStyle = FontStyle.Italic)
        {
            string prefixName = statsFormat.ToString();
            if (showLegend)
            {
                GameObject textGameObject = new GameObject(prefixName + "_" + statsFormat);
                textGameObject.transform.parent = image.transform;
                Text text = textGameObject.AddComponent<Text>();
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.text = GetLegendFromStatFormat(statsFormat);
                text.color = Color.black;
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = 1;
                text.alignment = TextAnchor.MiddleCenter;
            }

            foreach (GridRow gridRow in gridRowsData)
            {
                GameObject textGameObject = new GameObject(prefixName + "_" + gridRow.statsType + gridRow.Value);
                textGameObject.transform.parent = image.transform;
                Text text = textGameObject.AddComponent<Text>();
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.color = gridRow.ApplyColorToText ? gridRow.Color : Color.black;
                text.fontStyle = fontStyle;
                text.text = statsFormat == StatsFormat.Legend ? GetDisplayEntryLegend(gridRow) : "###";
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = 4;
                text.alignment = statsFormat == StatsFormat.Legend ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;
                gridRow.Texts.Add(text);
            }
        }

        string GetLegendFromStatFormat(StatsFormat statFormat)
        {
            switch (statFormat)
            {
                case StatsFormat.Fps:
                    return "fps";
                case StatsFormat.Ms:
                    return "ms";
                default:
                    return "";
            }
        }

        string GetDisplayEntryLegend(GridRow entry)
        {
            switch (entry.statsType)
            {
                case GridRow.StatsType.Current:
                    return "cur";
                case GridRow.StatsType.Average:
                    return "avg";
                case GridRow.StatsType.Percentile:
                    return entry.Value + "%";
                case GridRow.StatsType.Min:
                    return "min";
                case GridRow.StatsType.Max:
                    return "max";
                default:
                    return "";
            }
        }

        void Reset()
        {
            //Recover canvasGameObject lost by the reset.
            Canvas[] canvas = GetComponentsInChildren<Canvas>();
            foreach (Canvas canva in canvas)
                if (canva.gameObject.name == "Canvas_" + typeof(TimeframeUI).Name)
                {
                    if (canvasGameObject != null) // Remove possible duplicate
                        DestroyImmediate(canvasGameObject);
                    canvasGameObject = canva.gameObject;
                }
            DefaultStatsThenBuildUI();
        }

        void Start()
        {
            if (statsFrameRange < graphTextureSize.x)
                Debug.LogWarningFormat("{0} is below {1} x size and performance is wasted. " +
                    "You can either reduce the {1} x or increase the {0}. {0} < {1}.",
                    Name.Of(() => statsFrameRange), Name.Of(() => graphTextureSize));

            if(targetFramerate != 0)
                Application.targetFrameRate = (int) targetFramerate;

            if (canvasGameObject == null)
                Debug.LogError("The reference of the canvas GameObject is lost. Please Rebuild the UI.");

            InitStats();
            InitTexture();
            InitPixelColors();
            CreateCachedFloatString();
            CorrectGridUpdateDelay();
            UpdateGrid();
        }

        internal void CorrectGridUpdateDelay()
        {
            gridUpdateDelaySeconds = Math.Max(0.01f, gridUpdateDelaySeconds); //Min attribute is buggy.
        }

        internal void InitStats()
        {
            if (Application.isPlaying)
                stats = new Stats(Math.Max(1, statsFrameRange));
        }

        void InitPixelColors()
        {
            pixelsColor = new Color[graphTextureSize.x * graphTextureSize.y];
            for (int c = 0; c < pixelsColor.Length; c++)
                pixelsColor[c] = Color.clear;
        }

        internal void InitTexture()
        {
            InitPixelColors();
            texture = new Texture2D(graphTextureSize.x, graphTextureSize.y, TextureFormat.RGBA32, false); //Mipmap needs to be disable to avoid blur.
            texture.filterMode = FilterMode.Point; //Give sharp pixel aspect. Remove to smooth the graph renderer.

            if (showGraph)
                graphImage.sprite = Sprite.Create(texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0f, 0f), 10f);

            //Create initial line color for graph part.
            int start = graphTextureSize.x * (graphTextureSize.y / 2);
            int end = start + graphTextureSize.x;
            for (int c = start; c < end; c++)
                pixelsColor[c] = Color.black * graphColorAlternateTint;
            texture.SetPixels(pixelsColor, 0);
            texture.Apply();
        }

        void CreateCachedFloatString()
        {
            uint totalSize = (MAX_GRID_VALUE + 1) * 10;
            cachedFloatString = new string[totalSize];
            for (int i = 0; i < totalSize; i++)
                cachedFloatString[i] = (i / 10f).ToString("R1");
        }

        void Update()
        {
            UpdateTimeframe();

            if (Input.GetKeyDown(toggleUiKey))
                ToggleUI();

            UpdateGraph();

            if (Time.time - timeSinceLastGridUpdate > gridUpdateDelaySeconds)
                UpdateGrid();
        }

        void UpdateTimeframe()
        {
            stats.AddNext(Time.unscaledDeltaTime * 1000f); // s -> ms
        }

        internal void UpdateGrid()
        {
            if (canvasGameObject.activeSelf)
            {
                int msIndex = showLegend ? 1 : 0;
                int fpsIndex = msIndex + (showMs ? 1 : 0);

                foreach (GridRow displayedEntry in gridRowsData)
                {
                    if (showMs)
                        displayedEntry.Texts[msIndex].text = FloatToString(ComputeDisplayStatValue(displayedEntry));

                    if (showFps)
                        displayedEntry.Texts[fpsIndex].text = FloatToString(1000f / ComputeDisplayStatValue(displayedEntry), false);
                }
                timeSinceLastGridUpdate = Time.time;
            }
        }

        string FloatToString(float f, bool hasDigit = true)
        {
            if (f > MAX_GRID_VALUE)
                return cachedFloatString.Last();
            //1 digit 
            double d = Math.Round((double)f, hasDigit ? 1 : 0);
            int index = (int)(d * 10);
            return cachedFloatString[index];
        }

        void ToggleUI()
        {
            canvasGameObject.SetActive(!canvasGameObject.activeSelf);
        }

        internal void SetUiState(bool state)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(state);
        }

        void UpdateGraph()
        {
            if (showGraph && canvasGameObject.activeSelf)
            {
                graphCurrentColumn++;
                if (graphCurrentColumn / graphTextureSize.x >= 2)
                    graphCurrentColumn = 0;

                ClearTextureColumn(GetGraphColumnIndex);
                UpdateTextureColumnPixels(GetGraphColumnIndex);

                texture.SetPixels(pixelsColor, 0);
                texture.Apply();
            }
        }

        internal void RecreateTexture()
        {
            InitTexture();
            UpdateAllTextureColumns();
        }

        internal void UpdateAllTextureColumns()
        {
            if (Application.isPlaying)
            {
                for (uint x = 0; x < graphTextureSize.x; x++)
                {
                    ClearTextureColumn(x);
                    UpdateTextureColumnPixels(x);
                }
            }
        }

        void ClearTextureColumn(uint x)
        {
            for (int y = 0; y < graphTextureSize.y; y++)
            {
                int index = ((int)x % graphTextureSize.x) + y * graphTextureSize.x;
                pixelsColor[index] = clearColor;
            }
        }

        void UpdateTextureColumnPixels(uint x, bool ShouldCurrentStatsOnlyTakeRealTimeframeValueAndNotAverageOne = true)
        {
            foreach (GridRow gridRow in gridRowsData)
            {
                float value = ComputeDisplayStatValue(gridRow);

                if (ShouldCurrentStatsOnlyTakeRealTimeframeValueAndNotAverageOne && gridRow.statsType == GridRow.StatsType.Current)
                {
                    uint lastNValue = x > GetGraphColumnIndex ? GetGraphColumnIndex + (uint)graphTextureSize.x - x : GetGraphColumnIndex - x;
                    value = stats.Last(lastNValue);
                }


                switch (gridRow.graphRepresentation)
                {
                    case GridRow.GraphRepresentation.BarFromBottom:
                    case GridRow.GraphRepresentation.BarFromTop:
                        UpdateTextureColumnPixelsBar(x, value, gridRow);
                        break;
                    case GridRow.GraphRepresentation.Point:
                        UpdateTextureColumnPixelsPoint(x, value, gridRow);
                        break;
                    default:
                        break;
                }
            }
        }

        uint GetGraphColumnIndex
        {
            get
            {
                return (uint)(graphCurrentColumn % graphTextureSize.x);
            }
        }


        void UpdateTextureColumnPixelsBar(uint x, float value, GridRow gridRow)
        {
            Color graphColor = gridRow.Color * GetTintColor(graphCurrentColumn, x);
            // FIXME? Variable upper limit make the graph wrong for previous values.
            float max = CurrentUpperLimit;
            int y = Mathf.Min(graphTextureSize.y - 1, (int)(value * graphTextureSize.y / max));
            if (gridRow.graphRepresentation == GridRow.GraphRepresentation.BarFromBottom)
                for (int p = 0; p <= y; p++)
                    SetTexturePixel(x, p, gridRow, graphColor);

            else
                for (int p = graphTextureSize.y - 1; p >= y; p--)
                    SetTexturePixel(x, p, gridRow, graphColor);
        }

        void SetTexturePixel(uint x, int y, GridRow gridRow, Color graphColor)
        {
            int index = ((int)x % graphTextureSize.x) + y * graphTextureSize.x;
            pixelsColor[index] = gridRow.PerformColorOperation(pixelsColor[index], graphColor);
        }

        void UpdateTextureColumnPixelsPoint(uint x, float value, GridRow gridRow)
        {
            Color graphColor = gridRow.Color * GetTintColor(graphCurrentColumn, x);
            int y = Mathf.Min(graphTextureSize.y - 1, (int)(value * graphTextureSize.y / CurrentUpperLimit));
            int index = ((int)x % graphTextureSize.x) + y * graphTextureSize.x;
            pixelsColor[index] = gridRow.PerformColorOperation(pixelsColor[index], graphColor);
        }

        Color GetTintColor(uint currentColumn, uint columnToUpdate)
        {
            if ((currentColumn / graphTextureSize.x) % 2 == 0)
                return currentColumn % graphTextureSize.x < columnToUpdate ? graphColorAlternateTint : Color.white;
            else
                return currentColumn % graphTextureSize.x < columnToUpdate ? Color.white : graphColorAlternateTint;
        }

        float CurrentUpperLimit
        {
            get
            {
                return graphMsUpperLimit == 0 ? stats.Percentile(50) * 2 : graphMsUpperLimit;
            }
        }

        float ComputeDisplayStatValue(GridRow displayStat)
        {
            switch (displayStat.statsType)
            {
                case GridRow.StatsType.Current:
                    return displayStat.Value == 0 ? stats.LastAdded : stats.Average((uint)displayStat.Value);
                case GridRow.StatsType.Average:
                    return stats.Average((uint)displayStat.Value);
                case GridRow.StatsType.Percentile:
                    return stats.Percentile(displayStat.Value);
                case GridRow.StatsType.Min:
                    return stats.Min;
                case GridRow.StatsType.Max:
                    return stats.Max;
                default:
                    return 0;
            }
        }

        Vector2 GameWindowResolution
        {
            get
            {
#if UNITY_EDITOR
                return UnityEditor.Handles.GetMainGameViewSize();
#else
                return new Vector2(Screen.width, Screen.height);
#endif
            }
        }

        [Serializable]
        public class GridRow
        {
            public enum StatsType
            {
                Current,
                Average,
                Percentile,
                Min,
                Max
            }

            public enum GraphRepresentation
            {
                None,
                BarFromBottom,
                BarFromTop,
                Point
            }

            public enum ColorOperation
            {
                Replace,
                Add,
                Sub,
                Mul,
                Min,
                Max,
                Blend
            }

            [Tooltip(@"Statistics type to be computed among min, max, current, average and percentile.
        ▪ Average: compute the average from the last recorded values specified. The zero value force all recorded values to be used.
        ▪ Current: compute the average from the last recorded values specified. The zero value force the current time frame to be used.
        ▪ Min: get the minimal time frame recorded.
        ▪ Max: get the maximal time frame recorded.
        ▪ Percentile: compute the percentile given value of all recorded values.")]
            public StatsType statsType;

            [Tooltip("Statistic parameter.")]
            public float Value;

            [Tooltip("Point (line) or bar (histogram) rendering. None disables the graph representation.")]
            public GraphRepresentation graphRepresentation;

            [Tooltip("If the color is also applied to the grid.")]
            public bool ApplyColorToText;

            [Tooltip("Color used on the graph for this statistic.")]
            public Color Color;

            [Tooltip("How color is painted over previous statistics on the graph.")]
            public ColorOperation colorOperation;

            [SerializeField, HideInInspector]
            internal List<Text> Texts;

            public GridRow(StatsType statsType,
                float value = 0,
                GraphRepresentation graphRepresentation = GraphRepresentation.None,
                Color? color = null, //default(Color) is 0,0,0,0 which is not practical.
                bool applyColorToText = false,
                ColorOperation colorOperation = ColorOperation.Replace)
            {
                this.statsType = statsType;
                Value = value;
                this.graphRepresentation = graphRepresentation;
                ApplyColorToText = applyColorToText;
                this.colorOperation = colorOperation;
                Color = color.HasValue ? color.Value : Color.black;
                Texts = new List<Text>();
            }

            public Color PerformColorOperation(Color fg, Color bg)
            {
                switch (colorOperation)
                {
                    case ColorOperation.Add: return bg + fg;
                    case ColorOperation.Sub: return bg - fg;
                    case ColorOperation.Mul: return bg * fg;

                    case ColorOperation.Min:
                        return new Color(
                           Mathf.Min(bg.r, fg.r),
                           Mathf.Min(bg.g, fg.g),
                           Mathf.Min(bg.b, fg.b),
                           Mathf.Min(bg.a, fg.a));

                    case ColorOperation.Max:
                        return new Color(
                           Mathf.Max(bg.r, fg.r),
                           Mathf.Max(bg.g, fg.g),
                           Mathf.Max(bg.b, fg.b),
                           Mathf.Max(bg.a, fg.a));

                    //https://stackoverflow.com/a/727339/985714
                    /*  newColor => foreground , currentColor => background
                        r.R = fg.R * fg.A / r.A + bg.R * bg.A * (1 - fg.A) / r.A; 
                        r.G = fg.G * fg.A / r.A + bg.G * bg.A * (1 - fg.A) / r.A; 
                        r.B = fg.B * fg.A / r.A + bg.B * bg.A * (1 - fg.A) / r.A;
                        r.A = 1 - (1 - fg.A) * (1 - bg.A);
                        */
                    case ColorOperation.Blend:
                        float rA = 1 - (1 - fg.a) * (1 - bg.a);
                        return new Color(
                            fg.r * fg.a / rA + bg.r * bg.a * (1 - fg.a) / rA,
                            fg.g * fg.a / rA + bg.g * bg.a * (1 - fg.a) / rA,
                            fg.b * fg.a / rA + bg.b * bg.a * (1 - fg.a) / rA,
                            rA);

                    case ColorOperation.Replace:
                    default:
                        return bg;
                }

            }
        }
    }
}
