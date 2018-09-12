using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using LiveCharts;
using LiveCharts.Wpf;


using AForge.Video;
using AForge.Video.DirectShow;

using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;
using System.IO.Ports;

using Microsoft.Kinect;
using Microsoft.Kinect.Face;
using System.Globalization;
using System.Speech.Synthesis;
using LiveCharts.Defaults;

// Ref: https://www.codeproject.com/Articles/285964/WPF-Webcam-Control

// Ref: https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread?f1url=https%3A%2F%2Fmsdn.microsoft.com%2Fquery%2Fdev15.query%3FappId%3DDev15IDEF1%26l%3DEN-US%26k%3Dk(System.Threading.Thread);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.6.1);k(DevLang-csharp)%26rd%3Dtrue%26f%3D255%26MSPPError%3D-2147217396&view=netframework-4.7.2

// Ref: https://lvcharts.net/App/examples/v1/wpf/Basic%20Line%20Chart

// Ref: https://stackoverflow.com/questions/2006055/implementing-a-webcam-on-a-wpf-app-using-aforge-net

// Ref: https://github.com/jrowberg/bglib


namespace WpfDashboardProject02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region LiveCharts declarations

        public SeriesCollection mySeriesCollection0 { get; set; }
        public SeriesCollection mySeriesCollection1 { get; set; }
        public SeriesCollection mySeriesCollection2 { get; set; }
        public SeriesCollection mySeriesCollection3 { get; set; }
        public SeriesCollection mySeriesCollection4 { get; set; }
        public SeriesCollection mySeriesCollection5 { get; set; }
        //public SeriesCollection mySeriesCollectionScatter0 { get; set; }

        public SeriesCollection mySeriesCollectionArduino { get; set; }

        public SeriesCollection mySeriesCollectionKinectV1 { get; set; }

        public SeriesCollection mySeriesCollectionKinematics0 { get; set; }
        public SeriesCollection mySeriesCollectionKinematics1 { get; set; }
        public SeriesCollection mySeriesCollectionKinematics2 { get; set; }
        public SeriesCollection mySeriesCollectionKinematics3 { get; set; }
        public SeriesCollection mySeriesCollectionKinematics4 { get; set; }
        public SeriesCollection mySeriesCollectionKinematics5 { get; set; }

        //public string[] Labels { get; set; }
        //public Func<double, string> YFormatter { get; set; }

        public ChartValues<double> myValues0 = null;
        public LineSeries myLineSeries0 = null;

        public ChartValues<double> myValues1 = null;
        public LineSeries myLineSeries1 = null;

        public ChartValues<double> myValues2 = null;
        public LineSeries myLineSeries2 = null;

        public ChartValues<double> myValues3 = null;
        public LineSeries myLineSeries3 = null;

        public ChartValues<double> myValues4 = null;
        public LineSeries myLineSeries4 = null;

        public ChartValues<double> myValues5 = null;
        public LineSeries myLineSeries5 = null;

        //public ChartValues<ObservablePoint> myValuesScatter0 = null;
        //public ScatterSeries myScatterSeries0 = null;

        public ChartValues<double> myValuesArduino = null;
        public LineSeries myLineSeriesArduino = null;

        public double[] freq = { .15, .3, .45, .6, .75, .9 };

        //public ChartValues<double> myValuesKinectV1 = null;
        //public LineSeries myLineSeriesKinectV1 = null;

        //public <double> myValuesKinematics0_1 = null;
        //public ChartValues<double> myValuesKinematics0_2 = null;
        //public LineSeries myLineSeriesKinematics0_1 = null;
        //public LineSeries myLineSeriesKinematics0_2 = null;

        //public double[] ccc = new double[6];

        // Kinematic chart 0 values
        public ChartValues<double> myValuesKinematics00 = null;
        public LineSeries myLineSeriesKinematics00 = null;
        public ChartValues<double> myValuesKinematics01 = null;
        public LineSeries myLineSeriesKinematics01 = null;
        public ChartValues<double> myValuesKinematics02 = null;
        public LineSeries myLineSeriesKinematics02 = null;

        // Kinematic chart 1 values
        public ChartValues<double> myValuesKinematics10 = null;
        public LineSeries myLineSeriesKinematics10 = null;
        public ChartValues<double> myValuesKinematics11 = null;
        public LineSeries myLineSeriesKinematics11 = null;
        public ChartValues<double> myValuesKinematics12 = null;
        public LineSeries myLineSeriesKinematics12 = null;

        // Kinematic chart 2 values
        public ChartValues<double> myValuesKinematics20 = null;
        public LineSeries myLineSeriesKinematics20 = null;
        public ChartValues<double> myValuesKinematics21 = null;
        public LineSeries myLineSeriesKinematics21 = null;
        public ChartValues<double> myValuesKinematics22 = null;
        public LineSeries myLineSeriesKinematics22 = null;

        // Kinematic chart 3 values
        public ChartValues<double> myValuesKinematics30 = null;
        public LineSeries myLineSeriesKinematics30 = null;
        public ChartValues<double> myValuesKinematics31 = null;
        public LineSeries myLineSeriesKinematics31 = null;
        public ChartValues<double> myValuesKinematics32 = null;
        public LineSeries myLineSeriesKinematics32 = null;

        // Kinematic chart 4 values
        public ChartValues<double> myValuesKinematics40 = null;
        public LineSeries myLineSeriesKinematics40 = null;
        public ChartValues<double> myValuesKinematics41 = null;
        public LineSeries myLineSeriesKinematics41 = null;
        public ChartValues<double> myValuesKinematics42 = null;
        public LineSeries myLineSeriesKinematics42 = null;

        // Kinematic chart 5 values
        public ChartValues<double> myValuesKinematics50 = null;
        public LineSeries myLineSeriesKinematics50 = null;
        public ChartValues<double> myValuesKinematics51 = null;
        public LineSeries myLineSeriesKinematics51 = null;
        public ChartValues<double> myValuesKinematics52 = null;
        public LineSeries myLineSeriesKinematics52 = null;

        #endregion

        #region Thread/Timer declarations

        int period = 1000;

        private System.Threading.Timer timer;

        private System.Threading.Timer timerKinematics;

        // Simulated signals stuff
        Thread chartThread = null;

        #endregion

        #region Webcam declarations

        // Webcam stuff
        VideoCaptureDevice LocalWebCam;
        public FilterInfoCollection LocalWebCamsCollection;

        //public Collection<EncoderDevice> VideoDevices { get; set; }
        //public Collection<EncoderDevice> AudioDevices { get; set; }

        #endregion

        #region Heartrate declarations

        // Heart Rate stuff
        public Bluegiga.BGLib bglib = new Bluegiga.BGLib();
        public Boolean isAttached = false;
        public Dictionary<string, string> portDict = new Dictionary<string, string>();

        public bool debug = false;

        SerialPort serialAPI = null;

        double heartRate = 0.0;

        #endregion

        #region Kinect V2 declarations

        // Active Kinect sensor
        private KinectSensor kinectSensor = null;

        // Coordinate mapper to map one type of point to another
        private CoordinateMapper coordinateMapper = null;

        //--------------------------------------------------------------------------------------------------
        // Color Image Items

        // Reader for color frames
        private ColorFrameReader colorFrameReader = null;

        // Bitmap to display
        private WriteableBitmap colorBitmap = null;

        //--------------------------------------------------------------------------------------------------
        // Depth Image Items

        // Map depth range to byte range
        private const int MapDepthToByte = 8000 / 256;

        // Reader for depth frames
        private DepthFrameReader depthFrameReader = null;

        // Description of the data contained in the depth frame
        private FrameDescription depthFrameDescription = null;

        // Bitmap to display
        private WriteableBitmap depthBitmap = null;

        // Intermediate storage for frame data converted to color
        private byte[] depthPixels = null;

        //--------------------------------------------------------------------------------------------------
        // Infrared Image Items

        // Maximum value (as a float) that can be returned by the InfraredFrame
        private const float InfraredSourceValueMaximum = (float)ushort.MaxValue;

        // The value by which the infrared source data will be scaled
        private const float InfraredSourceScale = 0.75f;

        // Smallest value to display when the infrared data is normalized
        private const float InfraredOutputValueMinimum = 0.01f;

        // Largest value to display when the infrared data is normalized
        private const float InfraredOutputValueMaximum = 1.0f;

        // Reader for infrared frames
        private InfraredFrameReader infraredFrameReader = null;

        // Description (width, height, etc) of the infrared frame data
        private FrameDescription infraredFrameDescription = null;

        // Bitmap to display infrared image
        private WriteableBitmap infraredBitmap = null;

        //--------------------------------------------------------------------------------------------------
        // Body Image

        // Radius of drawn hand circles
        private const double HandSize = 30;

        // Thickness of drawn joint lines
        private const double JointThickness = 15;

        // Thickness of clip edge rectangles
        private const double ClipBoundsThickness = 10;

        // Constant for clamping Z values of cameraImage space points from being negative
        private const float InferredZPositionClamp = 0.1f;

        // Brush used for drawing hands that are currently tracked as closed
        private readonly System.Windows.Media.Brush handClosedBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, 255, 0, 0));

        // Brush used for drawing hands that are currently tracked as opened
        private readonly System.Windows.Media.Brush handOpenBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, 0, 255, 0));

        // Brush used for drawing hands that are currently tracked as in lasso (pointer) position
        private readonly System.Windows.Media.Brush handLassoBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, 0, 0, 255));

        // Brush used for drawing joints that are currently tracked
        //private readonly System.Windows.Media.Brush trackedJointBrush = 
        //    new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 68, 192, 68));

        private readonly System.Windows.Media.Brush trackedJointBrush =
            new SolidColorBrush(System.Windows.Media.Colors.White);

        // Brush used for drawing joints that are currently inferred
        private readonly System.Windows.Media.Brush inferredJointBrush = System.Windows.Media.Brushes.Yellow;

        // Pen used for drawing bones that are currently inferred
        private readonly System.Windows.Media.Pen inferredBonePen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Gray, 8);

        // Drawing group for body rendering output
        private DrawingGroup bodyDrawingGroup;

        // Drawing image that we will display
        private DrawingImage bodyImageSource;

        // Reader for body frames
        private BodyFrameReader bodyFrameReader = null;

        // Array for the bodies
        private Body[] bodies = null;

        // Number of bodies tracked
        private int bodyCount;

        // Definition of bones
        private List<Tuple<JointType, JointType>> bones;

        string[] jointNames = { "None", "SpineMid", "Neck", "Head", // Valid indices are 1..24
            "ShoulderLeft", "ElbowLeft", "WristLeft", "HandLeft",
            "ShoulderRight", "ElbowRight", "WristRight", "HandRight",
            "HipLeft", "KneeLeft", "AnkleLeft", "FootLeft",
            "HipRight", "KneeRight", "AnkleRight", "FootRight",
            "SpineShoulder",
            "HandTipLeft", "ThumbLeft", "HandTipRight", "ThumbRight" };


        // Width of display (color or depth space)
        private int bodyDisplayWidth;

        // Height of display (depth space)
        private int bodyDisplayHeight;

        // List of colors for each body tracked
        private List<System.Windows.Media.Pen> bodyColors;

        //--------------------------------------------------------------------------------------------------
        // Basic Face Tracking Items

        // Thickness of face bounding box and face points
        private const double DrawFaceShapeThickness = 8;

        // Font size of face property text 
        private const double DrawTextFontSize = 30;

        // Radius of face point circle
        private const double FacePointRadius = 1.0;

        // Text layout offset in X axis
        private const float TextLayoutOffsetX = -0.1f;

        // Text layout offset in Y axis
        private const float TextLayoutOffsetY = -0.15f;

        // Face rotation display angle increment in degrees
        private const double FaceRotationIncrementInDegrees = 5.0;

        // Formatted text to indicate that there are no bodies/faces tracked in the FOV
        private FormattedText textFaceNotTracked = new FormattedText(
                        "No bodies or faces are tracked ...",
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        new Typeface("Georgia"),
                        DrawTextFontSize,
                        System.Windows.Media.Brushes.White);

        // Text layout for the no face tracked message
        private System.Windows.Point textLayoutFaceNotTracked = new System.Windows.Point(10.0, 10.0);

        // Drawing group for face rendering output
        private DrawingGroup faceDrawingGroup;

        // Drawing image that we will display
        private DrawingImage faceImageSource;

        // Face frame sources
        private FaceFrameSource[] faceFrameSources = null;

        // Face frame readers
        private FaceFrameReader[] faceFrameReaders = null;

        // Storage for face frame results
        private FaceFrameResult[] faceFrameResults = null;

        // Width of display (color space)
        private int faceDisplayWidth;

        // Height of display (color space)
        private int faceDisplayHeight;

        // Display rectangle
        private Rect faceDisplayRect;

        // List of brushes for each face tracked
        private List<System.Windows.Media.Brush> faceBrush;

        string[] facePropertyNames =
{
                "FaceProperty_Happy",
                "FaceProperty_Engaged",
                "FaceProperty_WearingGlasses",
                "FaceProperty_LeftEyeClosed",
                "FaceProperty_RightEyeClosed",
                "FaceProperty_MouthOpen",
                "FaceProperty_MouthMoved",
                "FaceProperty_LookingAway",
                "FaceProperty_Count",
        };


        //--------------------------------------------------------------------------------------------------
        // HD Face Tracking Items

        // Following model of Vangos Pterneas - CodeProject

        private HighDefinitionFaceFrameSource hdFaceFrameSource = null;

        private HighDefinitionFaceFrameReader hdFaceFrameReader = null;

        private FaceAlignment hdFaceAlignment = null;

        private FaceModel hdFaceModel = null;

        private List<Ellipse> hdFacePoints = new List<Ellipse>();

        private bool colorImageSelected = false;
        private bool depthImageSelected = false;
        private bool infraredImageSelected = false;
        private bool bodyImageSelected = false;
        private bool faceImageSelected = false;
        private bool hdFaceImageSelected = false;
        private bool nowRecording = false;

        bool frontImageSelected = false;
        bool sideImageSelected = false;
        bool topImageSelected = false;


        #endregion

        #region Arduino Bluetooth sensor declarations

        SerialPort serialPort = null;

        bool running = false;

        double arduinoValue = 0.0;

        #endregion

        #region Output File declarations

        StreamWriter writer = new StreamWriter("DebugWpfDashboard.txt", false);

        #endregion

        Random rnd = new Random(13579);

        int N = 100;
        int NKinematics = 100;

        Stopwatch stopwatchBiometrics = null;
        Stopwatch stopwatchKinematics = null;

        int sampleCounter = 0;
        int sampleCounterKinematics = 0;

        private SpeechSynthesizer synthesizer;

        private StreamWriter outFile;

        private double lastTime;

        public MainWindow()
        {
            InitializeComponent();

            #region Setup LiveChart components

            int strokeThickness = 3;

            System.Windows.Media.Color[] strokeColors = { Colors.Red, Colors.Green, Colors.Blue,
            Colors.Orange, Colors.Indigo, Colors.Violet };

            #region Setup Biometrics Charts
            // Setup Biometrics Chart 0-------------------------------------------------------------
            Axis myYAxis0 = new Axis();
            //myYAxis0.MinValue = 0.0;
            // myYAxis0.MaxValue = 150.0;
            myYAxis0.Title = "Heart Rate 0 (BPM)";
            myYAxis0.FontSize = 15;
            myYAxis0.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis0.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis0 = new Axis();
            myXAxis0.MinValue = 0;
            myXAxis0.MaxValue = (N + 1);
            myXAxis0.Title = "Sample";
            myXAxis0.FontSize = 15;
            myXAxis0.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis0.Foreground = System.Windows.Media.Brushes.Black;

            MyChart0.AxisY.Add(myYAxis0);
            MyChart0.AxisX.Add(myXAxis0);
            MyChart0.DisableAnimations = true;

            myValues0 = new ChartValues<double>();

            myLineSeries0 = new LineSeries();

            myLineSeries0.Title = "Series 0";
            myLineSeries0.Values = myValues0;
            myLineSeries0.StrokeThickness = strokeThickness;
            myLineSeries0.Stroke = new SolidColorBrush(strokeColors[0]);
            myLineSeries0.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries0.DataLabels = false;
            // myLineSeries0.PointGeometrySize = 0;
            myLineSeries0.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries0.PointGeometry = null;

            mySeriesCollection0 = new SeriesCollection();
            mySeriesCollection0.Add(myLineSeries0);

            // Setup Biometrics Chart 1-------------------------------------------------------------
            Axis myYAxis1 = new Axis();
            //myYAxis1.MinValue = 0.0;
            //myYAxis1.MaxValue = 150.0;
            //myYAxis1.Title = "Signal 1";
            myYAxis1.Title = "Heart Rate 1 (BPM)";
            myYAxis1.FontSize = 15;
            myYAxis1.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis1.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis1 = new Axis();
            myXAxis1.MinValue = 0;
            myXAxis1.MaxValue = (N + 1);
            myXAxis1.Title = "Sample";
            myXAxis1.FontSize = 15;
            myXAxis1.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis1.Foreground = System.Windows.Media.Brushes.Black;

            MyChart1.AxisY.Add(myYAxis1);
            MyChart1.AxisX.Add(myXAxis1);
            MyChart1.DisableAnimations = true;

            myValues1 = new ChartValues<double>();

            myLineSeries1 = new LineSeries();

            myLineSeries1.Title = "Series1 1";
            myLineSeries1.Values = myValues1;
            myLineSeries1.StrokeThickness = strokeThickness;
            myLineSeries1.Stroke = new SolidColorBrush(strokeColors[1]);
            myLineSeries1.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries1.DataLabels = false;
            //myLineSeries1.PointGeometrySize = 0;
            myLineSeries1.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries1.PointGeometry = null;

            mySeriesCollection1 = new SeriesCollection();
            mySeriesCollection1.Add(myLineSeries1);

            // Setup Biometrics Chart 2-------------------------------------------------------------
            Axis myYAxis2 = new Axis();
            //myYAxis2.MinValue = 0.0;
            //myYAxis2.MaxValue = 150.0;
            //myYAxis2.Title = "Signal 2";
            myYAxis2.Title = "Heart Rate 2 (BPM)";
            myYAxis2.FontSize = 15;
            myYAxis2.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis2.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis2 = new Axis();
            myXAxis2.MinValue = 0;
            myXAxis2.MaxValue = (N + 1);
            myXAxis2.Title = "Sample";
            myXAxis2.FontSize = 15;
            myXAxis2.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis2.Foreground = System.Windows.Media.Brushes.Black;

            MyChart2.AxisY.Add(myYAxis2);
            MyChart2.AxisX.Add(myXAxis2);
            MyChart2.DisableAnimations = true;

            myValues2 = new ChartValues<double>();

            myLineSeries2 = new LineSeries();

            myLineSeries2.Title = "Series 2";
            myLineSeries2.Values = myValues2;
            myLineSeries2.StrokeThickness = strokeThickness;
            myLineSeries2.Stroke = new SolidColorBrush(strokeColors[2]);
            myLineSeries2.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries2.DataLabels = false;
            //myLineSeries2.PointGeometrySize = 0;
            myLineSeries2.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries2.PointGeometry = null;

            mySeriesCollection2 = new SeriesCollection();
            mySeriesCollection2.Add(myLineSeries2);

            // Setup Biometrics Chart 3-------------------------------------------------------------
            Axis myYAxis3 = new Axis();
            //myYAxis3.MinValue = 0.0;
            //myYAxis3.MaxValue = 150.0;
            //myYAxis3.Title = "Signal 3";
            myYAxis3.Title = "Heart Rate 3 (BPM)";
            myYAxis3.FontSize = 15;
            myYAxis3.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis3.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis3 = new Axis();
            myXAxis3.MinValue = 0;
            myXAxis3.MaxValue = (N + 1);
            myXAxis3.Title = "Sample";
            myXAxis3.FontSize = 15;
            myXAxis3.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis3.Foreground = System.Windows.Media.Brushes.Black;

            MyChart3.AxisY.Add(myYAxis3);
            MyChart3.AxisX.Add(myXAxis3);
            MyChart3.DisableAnimations = true;

            myValues3 = new ChartValues<double>();

            myLineSeries3 = new LineSeries();

            myLineSeries3.Title = "Series 3";
            myLineSeries3.Values = myValues3;
            myLineSeries3.StrokeThickness = strokeThickness;
            myLineSeries3.Stroke = new SolidColorBrush(strokeColors[3]);
            myLineSeries3.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries3.DataLabels = false;
            //myLineSeries2.PointGeometrySize = 0;
            myLineSeries3.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries3.PointGeometry = null;

            mySeriesCollection3 = new SeriesCollection();
            mySeriesCollection3.Add(myLineSeries3);

            // Setup Biometrics Chart 4-------------------------------------------------------------
            Axis myYAxis4 = new Axis();
            //myYAxis4.MinValue = 0.0;
            //myYAxis4.MaxValue = 150.0;
            //myYAxis4.Title = "Signal 4";
            myYAxis4.Title = "Heart Rate 4 (BPM)";
            myYAxis4.FontSize = 15;
            myYAxis4.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis4.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis4 = new Axis();
            myXAxis4.MinValue = 0;
            myXAxis4.MaxValue = (N + 1);
            myXAxis4.Title = "Sample";
            myXAxis4.FontSize = 15;
            myXAxis4.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis4.Foreground = System.Windows.Media.Brushes.Black;

            MyChart4.AxisY.Add(myYAxis4);
            MyChart4.AxisX.Add(myXAxis4);
            MyChart4.DisableAnimations = true;

            myValues4 = new ChartValues<double>();

            myLineSeries4 = new LineSeries();

            myLineSeries4.Title = "Series 4";
            myLineSeries4.Values = myValues4;
            myLineSeries4.StrokeThickness = strokeThickness;
            myLineSeries4.Stroke = new SolidColorBrush(strokeColors[4]);
            myLineSeries4.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries4.DataLabels = false;
            //myLineSeries4.PointGeometrySize = 0;
            myLineSeries4.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries4.PointGeometry = null;

            mySeriesCollection4 = new SeriesCollection();
            mySeriesCollection4.Add(myLineSeries4);

            // Setup Biometrics  5-------------------------------------------------------------
            Axis myYAxis5 = new Axis();
            //myYAxis5.MinValue = 0.0;
            //myYAxis5.MaxValue = 150.0;
            //myYAxis5.Title = "Signal 5";
            myYAxis5.Title = "Heart Rate 5 (BPM)";
            myYAxis5.FontSize = 15;
            myYAxis5.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis5.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis5 = new Axis();
            myXAxis5.MinValue = 0;
            myXAxis5.MaxValue = (N + 1);
            myXAxis5.Title = "Sample";
            myXAxis5.FontSize = 15;
            myXAxis5.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis5.Foreground = System.Windows.Media.Brushes.Black;

            MyChart5.AxisY.Add(myYAxis5);
            MyChart5.AxisX.Add(myXAxis5);
            MyChart5.DisableAnimations = true;

            myValues5 = new ChartValues<double>();

            myLineSeries5 = new LineSeries();

            myLineSeries5.Title = "Series 5";
            myLineSeries5.Values = myValues5;
            myLineSeries5.StrokeThickness = strokeThickness;
            myLineSeries5.Stroke = new SolidColorBrush(strokeColors[5]);
            myLineSeries5.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries5.DataLabels = false;
            myLineSeries5.PointGeometrySize = 5;
            myLineSeries5.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries5.PointGeometry = null;

            mySeriesCollection5 = new SeriesCollection();
            mySeriesCollection5.Add(myLineSeries5);
            #endregion

            /*
            // Setup Biometrics  Scatter Plot -------------------------------------------------------------            
            Axis myYAxisScatter0 = new Axis();
            myYAxisScatter0.MinValue = 0.0;
            myYAxisScatter0.MaxValue = 150.0;
            myYAxisScatter0.Title = "Correlate 1";
            myYAxisScatter0.FontSize = 15;
            myYAxisScatter0.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisScatter0.Separator.Step = 50;
            myYAxisScatter0.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxisScatter0 = new Axis();
            myXAxisScatter0.MinValue = 0.0;
            myXAxisScatter0.MaxValue = 150.0;
            myXAxisScatter0.Title = "Correlate 2";
            myXAxisScatter0.FontSize = 15;
            myXAxisScatter0.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisScatter0.Separator.Step = 50;
            myXAxisScatter0.Foreground = System.Windows.Media.Brushes.Black;

            MyScatterPlot0.AxisY.Add(myYAxisScatter0);
            MyScatterPlot0.AxisX.Add(myXAxisScatter0);
            MyScatterPlot0.DisableAnimations = true;

            myValuesScatter0 = new ChartValues<ObservablePoint>();

            myScatterSeries0 = new ScatterSeries();

            myScatterSeries0.Title = "Scatter Series 0";
            myScatterSeries0.Values = myValuesScatter0;
            myScatterSeries0.Stroke = System.Windows.Media.Brushes.Black;
            myScatterSeries0.Fill = System.Windows.Media.Brushes.Black;
            myScatterSeries0.PointGeometry = DefaultGeometries.Circle;
            myScatterSeries0.StrokeThickness = 1;
            myScatterSeries0.DataLabels = false;
            myScatterSeries0.MinPointShapeDiameter = 5;
            myScatterSeries0.MaxPointShapeDiameter = 5;
            //myScatterSeries0.PointGeometrySize = 5;
            //myScatterSeries0.Fill = new SolidColorBrush(Colors.Transparent);
            //myScatterSeries0.PointGeometry = null;

            mySeriesCollectionScatter0 = new SeriesCollection();
            mySeriesCollectionScatter0.Add(myScatterSeries0);
            */

            /*
            Random rr = new Random(123);
            for (var i = 0; i < 20; i++)
            {
                myValuesScatter0.Add(new ObservablePoint(rr.NextDouble() * 150, rr.NextDouble() * 150));                
            }
            */

            // Setup Arduino tab chart -----------------------------------------------
            Axis myYAxisArduino = new Axis();
            myYAxisArduino.MinValue = 0.0;
            myYAxisArduino.MaxValue = 1.0;
            myYAxisArduino.Title = "Sensor Value";
            myYAxisArduino.FontSize = 15;
            myYAxisArduino.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisArduino.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxisArduino = new Axis();
            myXAxisArduino.MinValue = 0;
            myXAxisArduino.MaxValue = (N + 1);
            myXAxisArduino.Title = "Sample";
            myXAxisArduino.FontSize = 15;
            myXAxisArduino.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisArduino.Foreground = System.Windows.Media.Brushes.Black;

            MyChartArduino.AxisY.Add(myYAxisArduino);
            MyChartArduino.AxisX.Add(myXAxisArduino);
            MyChartArduino.DisableAnimations = true;

            myValuesArduino = new ChartValues<double>();

            myLineSeriesArduino = new LineSeries();

            myLineSeriesArduino.Title = "Series Arduino";
            myLineSeriesArduino.Values = myValuesArduino;
            myLineSeriesArduino.StrokeThickness = strokeThickness;
            myLineSeriesArduino.Stroke = new SolidColorBrush(strokeColors[0]);
            myLineSeriesArduino.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesArduino.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesArduino.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesArduino.PointGeometry = null;

            mySeriesCollectionArduino = new SeriesCollection();
            mySeriesCollectionArduino.Add(myLineSeriesArduino);

            myGaugeArduino.Value = 0;
            #endregion

            #region Setup Kinematics Charts
            //----------------------------------------------------------------------
            // Setup MyKinematicsChart0 with 3 drawn signals (To be Head yaw, pitch and roll)            
            Axis myYAxisKinematics0 = new Axis();
            myYAxisKinematics0.MinValue = -90.0;
            myYAxisKinematics0.MaxValue = 90.0;
            myYAxisKinematics0.Title = "Subject 0 Angle (deg)";
            myYAxisKinematics0.FontSize = 10;
            myYAxisKinematics0.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisKinematics0.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxisKinematics0 = new Axis();
            myXAxisKinematics0.MinValue = 0; 
            myXAxisKinematics0.MaxValue = (N + 1);
            myXAxisKinematics0.Title = "Sample";
            myXAxisKinematics0.FontSize = 10;
            myXAxisKinematics0.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisKinematics0.Foreground = System.Windows.Media.Brushes.Black;

            MyKinematicsChart0.AxisY.Add(myYAxisKinematics0);
            MyKinematicsChart0.AxisX.Add(myXAxisKinematics0);
            MyKinematicsChart0.DisableAnimations = true;

            myValuesKinematics00 = new ChartValues<double>();
            myValuesKinematics01 = new ChartValues<double>();
            myValuesKinematics02 = new ChartValues<double>();

            myLineSeriesKinematics00 = new LineSeries();
            myLineSeriesKinematics00.Title = "Yaw";
            myLineSeriesKinematics00.Values = myValuesKinematics00;
            myLineSeriesKinematics00.StrokeThickness = strokeThickness;
            myLineSeriesKinematics00.Stroke = new SolidColorBrush(strokeColors[0]);
            myLineSeriesKinematics00.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics00.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics00.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics00.PointGeometry = null;

            myLineSeriesKinematics01 = new LineSeries();
            myLineSeriesKinematics01.Title = "Pitch";
            myLineSeriesKinematics01.Values = myValuesKinematics01;
            myLineSeriesKinematics01.StrokeThickness = strokeThickness;
            myLineSeriesKinematics01.Stroke = new SolidColorBrush(strokeColors[1]);
            myLineSeriesKinematics01.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics01.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics01.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics01.PointGeometry = null;

            myLineSeriesKinematics02 = new LineSeries();
            myLineSeriesKinematics02.Title = "Roll";
            myLineSeriesKinematics02.Values = myValuesKinematics02;
            myLineSeriesKinematics02.StrokeThickness = strokeThickness;
            myLineSeriesKinematics02.Stroke = new SolidColorBrush(strokeColors[2]);
            myLineSeriesKinematics02.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics02.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics02.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics02.PointGeometry = null;

            mySeriesCollectionKinematics0 = new SeriesCollection();

            mySeriesCollectionKinematics0.Add(myLineSeriesKinematics00);
            mySeriesCollectionKinematics0.Add(myLineSeriesKinematics01);
            mySeriesCollectionKinematics0.Add(myLineSeriesKinematics02);

            //--------------------------------------------------------------------------------
            // Setup MyKinematicsChart1 with 3 drawn signals (To be Head yaw, pitch and roll)            
            Axis myYAxisKinematics1 = new Axis();
            myYAxisKinematics1.MinValue = -90.0;
            myYAxisKinematics1.MaxValue = 90.0;
            myYAxisKinematics1.Title = "Subject 1 Angle (deg)";
            myYAxisKinematics1.FontSize = 10;
            myYAxisKinematics1.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisKinematics1.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxisKinematics1 = new Axis();
            myXAxisKinematics1.MinValue = 0;
            myXAxisKinematics1.MaxValue = (N + 1);
            myXAxisKinematics1.Title = "Sample";
            myXAxisKinematics1.FontSize = 10;
            myXAxisKinematics1.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisKinematics1.Foreground = System.Windows.Media.Brushes.Black;

            MyKinematicsChart1.AxisY.Add(myYAxisKinematics1);
            MyKinematicsChart1.AxisX.Add(myXAxisKinematics1);
            MyKinematicsChart1.DisableAnimations = true;

            myValuesKinematics10 = new ChartValues<double>();
            myValuesKinematics11 = new ChartValues<double>();
            myValuesKinematics12 = new ChartValues<double>();

            // Line series 10
            myLineSeriesKinematics10 = new LineSeries();
            myLineSeriesKinematics10.Title = "Yaw";
            myLineSeriesKinematics10.Values = myValuesKinematics10;
            myLineSeriesKinematics10.StrokeThickness = strokeThickness;
            myLineSeriesKinematics10.Stroke = new SolidColorBrush(strokeColors[0]);
            myLineSeriesKinematics10.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics10.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics10.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics10.PointGeometry = null;

            // Line series 11
            myLineSeriesKinematics11 = new LineSeries();
            myLineSeriesKinematics11.Title = "Pitch";
            myLineSeriesKinematics11.Values = myValuesKinematics11;
            myLineSeriesKinematics11.StrokeThickness = strokeThickness;
            myLineSeriesKinematics11.Stroke = new SolidColorBrush(strokeColors[1]);
            myLineSeriesKinematics11.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics11.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics11.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics11.PointGeometry = null;

            // Line series 12
            myLineSeriesKinematics12 = new LineSeries();
            myLineSeriesKinematics12.Title = "Roll";
            myLineSeriesKinematics12.Values = myValuesKinematics12;
            myLineSeriesKinematics12.StrokeThickness = strokeThickness;
            myLineSeriesKinematics12.Stroke = new SolidColorBrush(strokeColors[2]);
            myLineSeriesKinematics12.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics12.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics12.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics12.PointGeometry = null;

            mySeriesCollectionKinematics1 = new SeriesCollection();

            mySeriesCollectionKinematics1.Add(myLineSeriesKinematics10);
            mySeriesCollectionKinematics1.Add(myLineSeriesKinematics11);
            mySeriesCollectionKinematics1.Add(myLineSeriesKinematics12);

            //--------------------------------------------------------------------------------
            // Setup MyKinematicsChart2 with 3 drawn signals (To be Head yaw, pitch and roll)            
            Axis myYAxisKinematics2 = new Axis();
            myYAxisKinematics2.MinValue = -90.0;
            myYAxisKinematics2.MaxValue = 90.0;
            myYAxisKinematics2.Title = "Subject 2 Angle (deg)";
            myYAxisKinematics2.FontSize = 10;
            myYAxisKinematics2.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisKinematics2.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxisKinematics2 = new Axis();
            myXAxisKinematics2.MinValue = 0;
            myXAxisKinematics2.MaxValue = (N + 1);
            myXAxisKinematics2.Title = "Sample";
            myXAxisKinematics2.FontSize = 10;
            myXAxisKinematics2.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisKinematics2.Foreground = System.Windows.Media.Brushes.Black;

            MyKinematicsChart2.AxisY.Add(myYAxisKinematics2);
            MyKinematicsChart2.AxisX.Add(myXAxisKinematics2);
            MyKinematicsChart2.DisableAnimations = true;

            myValuesKinematics20 = new ChartValues<double>();
            myValuesKinematics21 = new ChartValues<double>();
            myValuesKinematics22 = new ChartValues<double>();

            // Line series 20
            myLineSeriesKinematics20 = new LineSeries();
            myLineSeriesKinematics20.Title = "Yaw";
            myLineSeriesKinematics20.Values = myValuesKinematics20;
            myLineSeriesKinematics20.StrokeThickness = strokeThickness;
            myLineSeriesKinematics20.Stroke = new SolidColorBrush(strokeColors[0]);
            myLineSeriesKinematics20.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics20.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics20.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics20.PointGeometry = null;

            // Line series 21
            myLineSeriesKinematics21 = new LineSeries();
            myLineSeriesKinematics21.Title = "Pitch";
            myLineSeriesKinematics21.Values = myValuesKinematics21;
            myLineSeriesKinematics21.StrokeThickness = strokeThickness;
            myLineSeriesKinematics21.Stroke = new SolidColorBrush(strokeColors[1]);
            myLineSeriesKinematics21.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics21.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics21.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics21.PointGeometry = null;

            // Line series 22
            myLineSeriesKinematics22 = new LineSeries();
            myLineSeriesKinematics22.Title = "Roll";
            myLineSeriesKinematics22.Values = myValuesKinematics22;
            myLineSeriesKinematics22.StrokeThickness = strokeThickness;
            myLineSeriesKinematics22.Stroke = new SolidColorBrush(strokeColors[2]);
            myLineSeriesKinematics22.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics22.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics22.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics22.PointGeometry = null;

            mySeriesCollectionKinematics2 = new SeriesCollection();

            mySeriesCollectionKinematics2.Add(myLineSeriesKinematics20);
            mySeriesCollectionKinematics2.Add(myLineSeriesKinematics21);
            mySeriesCollectionKinematics2.Add(myLineSeriesKinematics22);

            //--------------------------------------------------------------------------------
            // Setup MyKinematicsChart3 with 3 drawn signals (To be Head yaw, pitch and roll)            
            Axis myYAxisKinematics3 = new Axis();
            myYAxisKinematics3.MinValue = -90.0;
            myYAxisKinematics3.MaxValue = 90.0;
            myYAxisKinematics3.Title = "Subject 3 Angle (deg)";
            myYAxisKinematics3.FontSize = 10;
            myYAxisKinematics3.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisKinematics3.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxisKinematics3 = new Axis();
            myXAxisKinematics3.MinValue = 0;
            myXAxisKinematics3.MaxValue = (N + 1);
            myXAxisKinematics3.Title = "Sample";
            myXAxisKinematics3.FontSize = 10;
            myXAxisKinematics3.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisKinematics3.Foreground = System.Windows.Media.Brushes.Black;

            MyKinematicsChart3.AxisY.Add(myYAxisKinematics3);
            MyKinematicsChart3.AxisX.Add(myXAxisKinematics3);
            MyKinematicsChart3.DisableAnimations = true;

            myValuesKinematics30 = new ChartValues<double>();
            myValuesKinematics31 = new ChartValues<double>();
            myValuesKinematics32 = new ChartValues<double>();

            // Line series 30
            myLineSeriesKinematics30 = new LineSeries();
            myLineSeriesKinematics30.Title = "Yaw";
            myLineSeriesKinematics30.Values = myValuesKinematics30;
            myLineSeriesKinematics30.StrokeThickness = strokeThickness;
            myLineSeriesKinematics30.Stroke = new SolidColorBrush(strokeColors[0]);
            myLineSeriesKinematics30.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics30.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics30.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics30.PointGeometry = null;

            // Line series 31
            myLineSeriesKinematics31 = new LineSeries();
            myLineSeriesKinematics31.Title = "Pitch";
            myLineSeriesKinematics31.Values = myValuesKinematics31;
            myLineSeriesKinematics31.StrokeThickness = strokeThickness;
            myLineSeriesKinematics31.Stroke = new SolidColorBrush(strokeColors[1]);
            myLineSeriesKinematics31.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics31.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics31.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics31.PointGeometry = null;

            // Line series 32
            myLineSeriesKinematics32 = new LineSeries();
            myLineSeriesKinematics32.Title = "Roll";
            myLineSeriesKinematics32.Values = myValuesKinematics32;
            myLineSeriesKinematics32.StrokeThickness = strokeThickness;
            myLineSeriesKinematics32.Stroke = new SolidColorBrush(strokeColors[2]);
            myLineSeriesKinematics32.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics32.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics32.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics32.PointGeometry = null;

            mySeriesCollectionKinematics3 = new SeriesCollection();

            mySeriesCollectionKinematics3.Add(myLineSeriesKinematics30);
            mySeriesCollectionKinematics3.Add(myLineSeriesKinematics31);
            mySeriesCollectionKinematics3.Add(myLineSeriesKinematics32);

            //--------------------------------------------------------------------------------
            // Setup MyKinematicsChart4 with 3 drawn signals (To be Head yaw, pitch and roll)            
            Axis myYAxisKinematics4 = new Axis();
            myYAxisKinematics4.MinValue = -90.0;
            myYAxisKinematics4.MaxValue = 90.0;
            myYAxisKinematics4.Title = "Subject 4 Angle (deg)";
            myYAxisKinematics4.FontSize = 10;
            myYAxisKinematics4.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisKinematics4.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxisKinematics4 = new Axis();
            myXAxisKinematics4.MinValue = 0;
            myXAxisKinematics4.MaxValue = (N + 1);
            myXAxisKinematics4.Title = "Sample";
            myXAxisKinematics4.FontSize = 10;
            myXAxisKinematics4.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisKinematics4.Foreground = System.Windows.Media.Brushes.Black;

            MyKinematicsChart4.AxisY.Add(myYAxisKinematics4);
            MyKinematicsChart4.AxisX.Add(myXAxisKinematics4);
            MyKinematicsChart4.DisableAnimations = true;

            myValuesKinematics40 = new ChartValues<double>();
            myValuesKinematics41 = new ChartValues<double>();
            myValuesKinematics42 = new ChartValues<double>();

            // Line series 40
            myLineSeriesKinematics40 = new LineSeries();
            myLineSeriesKinematics40.Title = "Yaw";
            myLineSeriesKinematics40.Values = myValuesKinematics40;
            myLineSeriesKinematics40.StrokeThickness = strokeThickness;
            myLineSeriesKinematics40.Stroke = new SolidColorBrush(strokeColors[0]);
            myLineSeriesKinematics40.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics40.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics40.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics40.PointGeometry = null;

            // Line series 41
            myLineSeriesKinematics41 = new LineSeries();
            myLineSeriesKinematics41.Title = "Pitch";
            myLineSeriesKinematics41.Values = myValuesKinematics41;
            myLineSeriesKinematics41.StrokeThickness = strokeThickness;
            myLineSeriesKinematics41.Stroke = new SolidColorBrush(strokeColors[1]);
            myLineSeriesKinematics41.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics41.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics41.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics41.PointGeometry = null;

            // Line series 42
            myLineSeriesKinematics42 = new LineSeries();
            myLineSeriesKinematics42.Title = "Roll";
            myLineSeriesKinematics42.Values = myValuesKinematics42;
            myLineSeriesKinematics42.StrokeThickness = strokeThickness;
            myLineSeriesKinematics42.Stroke = new SolidColorBrush(strokeColors[2]);
            myLineSeriesKinematics42.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics42.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics42.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics42.PointGeometry = null;

            mySeriesCollectionKinematics4 = new SeriesCollection();

            mySeriesCollectionKinematics4.Add(myLineSeriesKinematics40);
            mySeriesCollectionKinematics4.Add(myLineSeriesKinematics41);
            mySeriesCollectionKinematics4.Add(myLineSeriesKinematics42);

            //--------------------------------------------------------------------------------
            // Setup MyKinematicsChart5 with 3 drawn signals (To be Head yaw, pitch and roll)            
            Axis myYAxisKinematics5 = new Axis();
            myYAxisKinematics5.MinValue = -90.0;
            myYAxisKinematics5.MaxValue = 90.0;
            myYAxisKinematics5.Title = "Subject 5 Angle (deg)";
            myYAxisKinematics5.FontSize = 10;
            myYAxisKinematics5.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisKinematics5.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxisKinematics5 = new Axis();
            myXAxisKinematics5.MinValue = 0;
            myXAxisKinematics5.MaxValue = (N + 1);
            myXAxisKinematics5.Title = "Sample";
            myXAxisKinematics5.FontSize = 10;
            myXAxisKinematics5.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisKinematics5.Foreground = System.Windows.Media.Brushes.Black;

            MyKinematicsChart5.AxisY.Add(myYAxisKinematics5);
            MyKinematicsChart5.AxisX.Add(myXAxisKinematics5);
            MyKinematicsChart5.DisableAnimations = true;

            myValuesKinematics50 = new ChartValues<double>();
            myValuesKinematics51 = new ChartValues<double>();
            myValuesKinematics52 = new ChartValues<double>();

            // Line series 50
            myLineSeriesKinematics50 = new LineSeries();
            myLineSeriesKinematics50.Title = "Yaw";
            myLineSeriesKinematics50.Values = myValuesKinematics50;
            myLineSeriesKinematics50.StrokeThickness = strokeThickness;
            myLineSeriesKinematics50.Stroke = new SolidColorBrush(strokeColors[0]);
            myLineSeriesKinematics50.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics50.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics50.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics50.PointGeometry = null;

            // Line series 51
            myLineSeriesKinematics51 = new LineSeries();
            myLineSeriesKinematics51.Title = "Pitch";
            myLineSeriesKinematics51.Values = myValuesKinematics51;
            myLineSeriesKinematics51.StrokeThickness = strokeThickness;
            myLineSeriesKinematics51.Stroke = new SolidColorBrush(strokeColors[1]);
            myLineSeriesKinematics51.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics51.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics51.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics51.PointGeometry = null;

            // Line series 52
            myLineSeriesKinematics52 = new LineSeries();
            myLineSeriesKinematics52.Title = "Roll";
            myLineSeriesKinematics52.Values = myValuesKinematics52;
            myLineSeriesKinematics52.StrokeThickness = strokeThickness;
            myLineSeriesKinematics52.Stroke = new SolidColorBrush(strokeColors[2]);
            myLineSeriesKinematics52.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinematics52.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinematics52.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinematics52.PointGeometry = null;

            mySeriesCollectionKinematics5 = new SeriesCollection();

            mySeriesCollectionKinematics5.Add(myLineSeriesKinematics50);
            mySeriesCollectionKinematics5.Add(myLineSeriesKinematics51);
            mySeriesCollectionKinematics5.Add(myLineSeriesKinematics52);

            #endregion

            #region Initialize Chart Values

            double[] values = new double[N];

            // Set the initial values to zero
            for (int j = 0; j < N; j++)
            {
                //values[j] = 10.0 + 10.0 * Math.Sin(2.0 * Math.PI * freq[k] * (j * 1.0 / N));
                values[j] = 0.0;
                //if (k == 5) values[j] = 0.0;
            }

            //double[] freq = { 1, 2, 3, 4, 5, 6 };

            // Initial values for Biometrics charts
            myValues0.Clear();
            myValues0.AddRange(values);            

            myValues1.Clear();
            myValues1.AddRange(values);            

            myValues2.Clear();
            myValues2.AddRange(values);

            myValues3.Clear();
            myValues3.AddRange(values);

            myValues4.Clear();
            myValues4.AddRange(values);
            
            myValues5.Clear();
            myValues5.AddRange(values);

            //myValuesScatter0.Clear();

            // Initial values for Kinematics charts
            myValuesKinematics00.Clear();
            myValuesKinematics01.Clear();
            myValuesKinematics02.Clear();

            myValuesKinematics10.Clear();
            myValuesKinematics11.Clear();
            myValuesKinematics12.Clear();
            
            myValuesKinematics20.Clear();
            myValuesKinematics21.Clear();
            myValuesKinematics22.Clear();

            myValuesKinematics30.Clear();
            myValuesKinematics31.Clear();
            myValuesKinematics32.Clear();

            myValuesKinematics40.Clear();
            myValuesKinematics41.Clear();
            myValuesKinematics42.Clear();

            myValuesKinematics50.Clear();
            myValuesKinematics51.Clear();
            myValuesKinematics52.Clear();

            myValuesKinematics00.AddRange(values);
            myValuesKinematics01.AddRange(values);
            myValuesKinematics02.AddRange(values);

            myValuesKinematics10.AddRange(values);
            myValuesKinematics11.AddRange(values);
            myValuesKinematics12.AddRange(values);

            myValuesKinematics20.AddRange(values);
            myValuesKinematics21.AddRange(values);
            myValuesKinematics22.AddRange(values);

            myValuesKinematics30.AddRange(values);
            myValuesKinematics31.AddRange(values);
            myValuesKinematics32.AddRange(values);

            myValuesKinematics40.AddRange(values);
            myValuesKinematics41.AddRange(values);
            myValuesKinematics42.AddRange(values);

            myValuesKinematics50.AddRange(values);
            myValuesKinematics51.AddRange(values);
            myValuesKinematics52.AddRange(values);

            #endregion

            #region Setup UI Components

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;

            stopwatchBiometrics = new Stopwatch();
            stopwatchBiometrics.Start();

            stopwatchKinematics = new Stopwatch();
            stopwatchKinematics.Start();

            // Webcam Related
            frameHolder.Visibility = Visibility.Hidden;

            LocalWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            LocalWebCam = new VideoCaptureDevice(LocalWebCamsCollection[0].MonikerString);
            LocalWebCam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);

            // Status page items
            biometricsStatusLed.SetPowerOff();
            heartRateStatusLed.SetPowerOff();
            webCamStatusLed.SetPowerOff();
            kinematicsStatusLed.SetPowerOff();
            kinectV2StatusLed.SetPowerOff();
            arduinoStatusLed.SetPowerOff();

            btnStartRecording.IsEnabled = true;
            btnStopRecording.IsEnabled = false;

            #endregion

            DataContext = this;

            bodyImage.Source = bodyImageSource;
            bodyImageBiometricTab.Source = bodyImageSource;
            bodyImageKinematicsTab.Source = bodyImageSource;
        }

        #region Biometric Tab Event Handlers

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;

            stopwatchBiometrics.Reset();
            stopwatchBiometrics.Start();

            sampleCounter = 0;

            /*
            // Create a new thread and attach it's work handler
            chartThread = new Thread(Update);

            chartThread.Start();
            */

            // Using a Timer at 33 ms or about 30Hz, and the function UpdateProperty()
            // To update the "simulated" signals.
            //
            // Non-simulated signals are updated by their own event handlers asychronously
            timer = new Timer(UpdateProperty, null, 0, 100);

            tbStatus.Text = "Timer timer started.";

            biometricsStatusLed.SetOn();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            writer.Close();

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;

            //chartThread.Abort();

            timer.Dispose();

            stopwatchBiometrics.Stop();

            try
            {
                long elapsedTime = stopwatchBiometrics.ElapsedMilliseconds / 1000;

                //double sps = sampleCounter / elapsedTime / 6;

                tbStatus.Text = "Per Chart Samples Per Second: " + String.Format("{0:00.00}", sampleCounter / 5 / (double)elapsedTime);
            }
            catch (Exception ex)
            {
                tbStatus.Text = ex.Message;
            }

            stopwatchBiometrics.Reset();

            biometricsStatusLed.SetOff();
        }


        private double Noise()
        {
            // +/- 0.05 units
            return -0.05 + 0.1 * rnd.NextDouble();
        }

        // Update the [SIMULATED] Biometric variables
        private void UpdateProperty(object state)
        {
            // Update the user interface
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    long startTime = stopwatchBiometrics.ElapsedMilliseconds;

                    // These values are written out at each timer time step.
                    //writer.WriteLine("Time: {0} Sample Counter: {1} Arduino value: {2}", 
                    //    stopwatch.Elapsed.ToString(), sampleCounter, arduinoValue);

                    // For now, just rotate the values stored for each signal 0-4

                    /*
                    for (int j = 0; j < N; j++)
                    {
                        //values[j] = 10.0 + 10.0 * Math.Sin(2.0 * Math.PI * freq[k] * (j * 1.0 / N));
                        values[j] = 0.0;
                        //if (k == 5) values[j] = 0.0;
                    }
                     */


                    //---------------------------------------

                    if (myValues0.Count < N)
                    {
                        //myValues0.Add(0.1 + Noise());
                        myValues0.Add(heartRate);
                    }
                    else
                    {
                        myValues0.RemoveAt(0);
                        //myValues0.Add(0.1 + Noise());
                        myValues0.Add(heartRate);
                    }

                    if (myValues1.Count < N)
                    {
                        //myValues0.Add(0.1 + Noise());
                        myValues1.Add(heartRate);
                    }
                    else
                    {
                        myValues1.RemoveAt(0);
                        //myValues0.Add(0.1 + Noise());
                        myValues1.Add(heartRate);
                    }

                    if (myValues2.Count < N)
                    {
                        //myValues0.Add(0.1 + Noise());
                        myValues2.Add(heartRate);
                    }
                    else
                    {
                        myValues2.RemoveAt(0);
                        //myValues0.Add(0.1 + Noise());
                        myValues2.Add(heartRate);
                    }

                    if (myValues3.Count < N)
                    {
                        //myValues0.Add(0.1 + Noise());
                        myValues3.Add(heartRate);
                    }
                    else
                    {
                        myValues3.RemoveAt(0);
                        //myValues0.Add(0.1 + Noise());
                        myValues3.Add(heartRate);
                    }

                    if (myValues4.Count < N)
                    {
                        //myValues0.Add(0.1 + Noise());
                        myValues4.Add(heartRate);
                    }
                    else
                    {
                        myValues4.RemoveAt(0);
                        //myValues0.Add(0.1 + Noise());
                        myValues4.Add(heartRate);
                    }

                    if (myValues5.Count < N)
                    {
                        //myValues0.Add(0.1 + Noise());
                        myValues5.Add(heartRate);
                    }
                    else
                    {
                        myValues5.RemoveAt(0);
                        //myValues0.Add(0.1 + Noise());
                        myValues5.Add(heartRate);
                    }
                    
                    /*
                    myValuesScatter0.Clear();
                    for(int k = 0; k < myValues1.Count; k++)
                    {
                        myValuesScatter0.Add(new ObservablePoint(myValues1[k], myValues2[k]));
                    }
                    */

                    //---------------------------------------
                    /*
                    if (myValues2.Count < N)
                    {
                        myValues2.Add(0.3 + Noise());
                    }
                    else
                    {
                        myValues2.RemoveAt(0);
                        myValues2.Add(0.3 + Noise());
                    }
                    //---------------------------------------
                    if (myValues3.Count < N)
                    {
                        myValues3.Add(0.4 + Noise());
                    }
                    else
                    {
                        myValues3.RemoveAt(0);
                        myValues3.Add(0.4 + Noise());
                    }
                    //---------------------------------------
                    if (myValues4.Count < N)
                    {
                        myValues4.Add(0.5 + Noise());
                    }
                    else
                    {
                        myValues4.RemoveAt(0);
                        myValues4.Add(0.5 + Noise());
                    }
                    //---------------------------------------
                    if (myValues5.Count < N)
                    {
                        myValues5.Add(0.6 + Noise());
                    }
                    else
                    {
                        myValues5.RemoveAt(0);
                        myValues5.Add(0.6 + Noise());
                    }                    
                    */

                    // Calculate the time required to generate the simulated data
                    sampleCounter += 5;

                    double elapsedTimeInSec = stopwatchBiometrics.ElapsedMilliseconds / 1000.0;

                    double sps = sampleCounter / elapsedTimeInSec;

                    tbStatus.Text = String.Format("Samples: {0}, Seconds: {1:0.000}, SPS: {2:00.000}",
                        sampleCounter, elapsedTimeInSec, sps);

                    //tbStatus.Text = "Arduino value: " + String.Format("{0:00.00}", arduinoValue);
                }
            );
        }

        // Update the simulated kinematics data
        private void UpdateKinematicProperty(object state)
        {
            // Update the user interface
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    long startTime = stopwatchKinematics.ElapsedMilliseconds;

                    // These values are written out at each timer time step.
                    //writer.WriteLine("Time: {0} Sample Counter: {1} Arduino value: {2}", 
                    //    stopwatch.Elapsed.ToString(), sampleCounter, arduinoValue);

                    //---------------------------------------
                    if (myValuesKinematics00.Count < N)
                    {
                        myValuesKinematics00.Add(0.2 + Noise());
                    }
                    else
                    {
                        myValuesKinematics00.RemoveAt(0);
                        myValuesKinematics00.Add(0.2 + Noise());
                    }
                    //---------------------------------------
                    if (myValuesKinematics01.Count < N)
                    {
                        myValuesKinematics01.Add(0.4 + Noise());
                    }
                    else
                    {
                        myValuesKinematics01.RemoveAt(0);
                        myValuesKinematics01.Add(0.4 + Noise());
                    }
                    //---------------------------------------
                    if (myValuesKinematics02.Count < N)
                    {
                        myValuesKinematics02.Add(0.5 + Noise());
                    }
                    else
                    {
                        myValuesKinematics02.RemoveAt(0);
                        myValuesKinematics02.Add(0.5 + Noise());
                    }

                    // Kinematics chart 1
                    //---------------------------------------
                    if (myValuesKinematics10.Count < N)
                    {
                        myValuesKinematics10.Add(0.6 + Noise());
                    }
                    else
                    {
                        myValuesKinematics10.RemoveAt(0);
                        myValuesKinematics10.Add(0.6 + Noise());
                    }
                    //---------------------------------------
                    if (myValuesKinematics11.Count < N)
                    {
                        myValuesKinematics11.Add(0.7 + Noise());
                    }
                    else
                    {
                        myValuesKinematics11.RemoveAt(0);
                        myValuesKinematics11.Add(0.7 + Noise());
                    }
                    //---------------------------------------
                    if (myValuesKinematics12.Count < N)
                    {
                        myValuesKinematics12.Add(0.9 + Noise());
                    }
                    else
                    {
                        myValuesKinematics12.RemoveAt(0);
                        myValuesKinematics12.Add(0.9 + Noise());
                    }
                    

                    // Calculate the time required to generate the simulated data
                    sampleCounterKinematics += 3;

                    double elapsedTimeInSec = stopwatchKinematics.ElapsedMilliseconds / 1000.0;

                    double sps = sampleCounterKinematics / elapsedTimeInSec;

                    tbStatusKinematics.Text = String.Format("Samples: {0}, Seconds: {1:0.000}, SPS: {2:00.000}",
                        sampleCounterKinematics, elapsedTimeInSec, sps);

                    //tbStatus.Text = "Arduino value: " + String.Format("{0:00.00}", arduinoValue);
                }
            );
        }

        // Function Update() is not currently used
        /*
        private void Update()
        {
            try
            {
                // Simulate some work taking place 
                //counter = 0;

                //for (int k = 0; k < 50; k++)
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(30));

                    // Update the user interface
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                (ThreadStart)delegate ()
                                {
                                    sampleCounter += 6;

                                    double tmp = myValues0[0];
                                    myValues0.RemoveAt(0);
                                    myValues0.Add(tmp);

                                    tmp = myValues1[0];
                                    myValues1.RemoveAt(0);
                                    myValues1.Add(tmp);

                                    tmp = myValues2[0];
                                    myValues2.RemoveAt(0);
                                    myValues2.Add(tmp);

                                    tmp = myValues3[0];
                                    myValues3.RemoveAt(0);
                                    myValues3.Add(tmp);

                                    tmp = myValues4[0];
                                    myValues4.RemoveAt(0);
                                    myValues4.Add(tmp);

                                    tmp = myValues5[0];
                                    myValues5.RemoveAt(0);
                                    myValues5.Add(tmp);

                                    //tbStatus.Text = "Samples Generated: " + sampleCounter;

                                    long elapsedTime = stopwatchBiometrics.ElapsedMilliseconds / 1000;

                                    //double sps = sampleCounter / elapsedTime / 6;

                                    tbStatus.Text = "Chart Samples Per Second: " + String.Format("{0:00.00}", sampleCounter / 6 / (double)elapsedTime);

                                    //tbStatus.Text = "Arduino: " + arduinoValue;
                                }
                                  );
                }

                
                //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                //                (ThreadStart)delegate ()
                //                {
                //                    tbStatus.Text = "Task complete.";
                //                }
                //                  );                  
            }
            catch (ThreadAbortException ex)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                (ThreadStart)delegate ()
                                {
                                    tbStatus.Text = "Thread Stopped." + ex.Message;
                                }
                                  );
            }
        }
        */

        /*
        private void UpdateLineSeries0()
        {
            sampleCounter += 1;

            double tmp = myValues0[0];
            myValues0.RemoveAt(0);
            myValues0.Add(tmp);
        }
        */

        #endregion

        #region Window Event Handlers  

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Initialize Heart Rate sensor

            // Heart rate initializations
            DataContext = this;

            // Initialize the gauge
            gauge0.Value = 0.0;
            gauge1.Value = 0.0;
            gauge2.Value = 0.0;
            gauge3.Value = 0.0;
            gauge4.Value = 0.0;
            gauge5.Value = 0.0;

            // Initialize list of ports
            btnRefresh_Click(sender, e);

            // initialize COM port combobox with list of ports
            //comboPorts.DataSource = new BindingSource(portDict, null);
            //comboPorts.DisplayMember = "Value";
            //comboPorts.ValueMember = "Key";

            // Initialize serial port with all of the normal values (should work with BLED112 on USB)
            serialAPI = new SerialPort();

            serialAPI.Handshake = System.IO.Ports.Handshake.RequestToSend;
            serialAPI.BaudRate = 115200;
            serialAPI.DataBits = 8;
            serialAPI.StopBits = System.IO.Ports.StopBits.One;
            serialAPI.Parity = System.IO.Ports.Parity.None;
            serialAPI.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceivedHandler);

            // Initialize BGLib events we'll need for this script
            bglib.BLEEventGAPScanResponse += new Bluegiga.BLE.Events.GAP.ScanResponseEventHandler(this.GAPScanResponseEvent);
            bglib.BLEEventConnectionStatus += new Bluegiga.BLE.Events.Connection.StatusEventHandler(this.ConnectionStatusEvent);
            bglib.BLEEventATTClientGroupFound += new Bluegiga.BLE.Events.ATTClient.GroupFoundEventHandler(this.ATTClientGroupFoundEvent);
            bglib.BLEEventATTClientFindInformationFound += new Bluegiga.BLE.Events.ATTClient.FindInformationFoundEventHandler(this.ATTClientFindInformationFoundEvent);
            bglib.BLEEventATTClientProcedureCompleted += new Bluegiga.BLE.Events.ATTClient.ProcedureCompletedEventHandler(this.ATTClientProcedureCompletedEvent);
            bglib.BLEEventATTClientAttributeValue += new Bluegiga.BLE.Events.ATTClient.AttributeValueEventHandler(this.ATTClientAttributeValueEvent);

            #endregion

            #region Initialize Kinect Sensor

            InitializeKinectV2Sensor();

            kinectV2StatusLed.SetOn();
            
            #endregion

            // Open a text file for output
            outFile = new StreamWriter("FrameData.csv", false); // <-- Not allowing append to old file

            // Write the output file column labels
            writeOutputLineLabels();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
             chartThread.Abort();

            timer.Dispose();

            LocalWebCam.Stop();

            // Close the output file
            //outFile.Close();

            // Kinect V2 sensor
            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }

            serialAPI.Close();

            LocalWebCam.Stop();

            writer.Close();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Webcam Event Handlers

        private void btnStartWebcam_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                // Display webcam video
                WebcamViewer.StartPreview();
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
                MessageBox.Show("Device is in use by another application");
            }
            */

            LocalWebCam.Start();
            frameHolder.Visibility = Visibility.Visible;

            btnStartWebcam.IsEnabled = false;
            btnStopWebcam.IsEnabled = true;

            webCamStatusLed.SetOn();
        }

        private void btnStopWebcam_Click(object sender, RoutedEventArgs e)
        {
            // Stop the display of webcam video.
            //WebcamViewer.StopPreview();

            LocalWebCam.Stop();
            frameHolder.Visibility = Visibility.Hidden;

            btnStartWebcam.IsEnabled = true;
            btnStopWebcam.IsEnabled = false;

            webCamStatusLed.SetOff();
        }

        void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                bi.Freeze();
                //this.latestFrame = bi;
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    frameHolder.Source = bi;
                }));
            }
            catch (Exception ex)
            {
                tbStatus.Text = ex.Message;
            }
        }

        public BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        #endregion

        #region Heart Rate Controls Event Handlers

        List<Win32DeviceMgmt.Win32DeviceMgmt.DeviceInfo> portList = null;

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Get a list of all available ports on the system

            /*
            comboPorts.Items.Clear();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboPorts.Items.Add(s);
            }
            */

            comboPorts.Items.Clear();

            portList = Win32DeviceMgmt.Win32DeviceMgmt.GetAllCOMPorts();

            foreach (var p in portList)
            {
                string str = p.name + " - " + p.decsription;

                comboPorts.Items.Add(str);
            }

            comboPorts.SelectedIndex = 0;

            /*
            portDict.Clear();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SerialPort");
                //string[] ports = System.IO.Ports.SerialPort.GetPortNames();
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    portDict.Add(String.Format("{0}", queryObj["DeviceID"]), String.Format("{0} - {1}", queryObj["DeviceID"], queryObj["Caption"]));
                }
            }
            catch (ManagementException ex)
            {
                portDict.Add("0", "Error " + ex.Message);
            }
            */
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            if (!isAttached)
            {
                int index = comboPorts.SelectedIndex;
                string selectedPortName = portList[index].name;

                //txtLog.AppendText("Opening serial port '" + comboPorts.SelectedValue.ToString() + "'..." + Environment.NewLine);
                txtLog.AppendText("Opening serial port '" + selectedPortName + "'..." + Environment.NewLine);

                //serialAPI.PortName = comboPorts.SelectedValue.ToString();
                serialAPI.PortName = selectedPortName;
                serialAPI.Open();
                txtLog.AppendText("Port opened" + Environment.NewLine);
                isAttached = true;
                btnAttach.Content = "Detach";
                btnGo.IsEnabled = true;
                btnReset.IsEnabled = true;

                heartRateStatusLed.SetOff();
            }
            else
            {
                txtLog.AppendText("Closing serial port..." + Environment.NewLine);
                serialAPI.Close();
                txtLog.AppendText("Port closed" + Environment.NewLine);
                isAttached = false;
                btnAttach.Content = "Attach";
                btnGo.IsEnabled = false;
                btnReset.IsEnabled = false;

                heartRateStatusLed.SetOff();
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            // Start the scan/connect process now
            Byte[] cmd;

            // Set scan parameters
            cmd = bglib.BLECommandGAPSetScanParameters(0xC8, 0xC8, 1); // 125ms interval, 125ms window, active scanning

            // DEBUG: display bytes read
            //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            String logText = String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine;
            if (debug) SetLogText(logText);

            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // Begin scanning for BLE peripherals
            cmd = bglib.BLECommandGAPDiscover(1); // generic discovery mode

            // DEBUG: display bytes read
            //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            logText = String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine;
            if (debug) SetLogText(logText);

            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // Update state
            app_state = STATE_SCANNING;

            heartRateStatusLed.SetOn();

            // Disable "GO" button since we already started, and sending the same commands again sill not work right
            btnGo.IsEnabled = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Stop everything we're doing, if possible
            Byte[] cmd;

            heartRateStatusLed.SetOff();

            // Disconnect if connected
            cmd = bglib.BLECommandConnectionDisconnect(0);

            // DEBUG: display bytes read
            //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            String logText = String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine;
            if (debug) SetLogText(logText);

            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // Stop scanning if scanning
            cmd = bglib.BLECommandGAPEndProcedure();

            // DEBUG: display bytes read
            //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            logText = String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine;
            if (debug) SetLogText(logText);

            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // Stop advertising if advertising
            cmd = bglib.BLECommandGAPSetMode(0, 0);

            // DEBUG: display bytes read
            //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
            logText = String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine;
            if (debug) SetLogText(logText);

            bglib.SendCommand(serialAPI, cmd);
            //while (bglib.IsBusy()) ;

            // Enable "GO" button to allow them to start again
            btnGo.IsEnabled = true;

            // Update state
            app_state = STATE_STANDBY;

            txtLog.Text = "";
        }

        #endregion

        int bodyFrameCounter = 0;
        int bodyFrameMax = 30;

        private void InitializeKinectV2Sensor()
        {
            //---------------------------------------------------------------------------------------------
            // All that follows can probably be moved to the Window_Loaded() function
            //---------------------------------------------------------------------------------------------

            // Get the kinectSensor object - Only one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();

            // gGt the coordinate mapper
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            // Color Image --------------------------------------------------------------------------------
            // Open the reader for the color frames
            this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

            // Set event handler for color frame arrival
            this.colorFrameReader.FrameArrived += this.ColorReader_ColorFrameArrived;

            // Create the colorFrameDescription from the ColorFrameSource using Bgra format
            FrameDescription colorFrameDescription =
                this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);

            // Create the bitmap to display the color image
            this.colorBitmap = new WriteableBitmap(
                colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

            // Depth Image -------------------------------------------------------------------------------
            // Open the reader for the depth frames
            this.depthFrameReader = this.kinectSensor.DepthFrameSource.OpenReader();

            // Set event handler for depth frame arrival
            this.depthFrameReader.FrameArrived += this.DepthReader_FrameArrived;

            // Get the FrameDescription from DepthFrameSource
            this.depthFrameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

            // Allocate space to store the depth pixels being received and converted
            this.depthPixels = new byte[this.depthFrameDescription.Width * this.depthFrameDescription.Height];

            // Create the bitmap to display the depth image - 512x424
            this.depthBitmap = new WriteableBitmap(
                this.depthFrameDescription.Width, this.depthFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray8, null);

            // Infrared Image ------------------------------------------------------------------------------
            // Open the reader for the infrared frames
            this.infraredFrameReader = this.kinectSensor.InfraredFrameSource.OpenReader();

            // Set event handler for infrared frame arrival
            this.infraredFrameReader.FrameArrived += this.InfraredReader_InfraredFrameArrived;

            // Get the FrameDescription from InfraredFrameSource
            this.infraredFrameDescription = this.kinectSensor.InfraredFrameSource.FrameDescription;

            // Create the bitmap to display the infrared image - 512x424
            this.infraredBitmap = new WriteableBitmap(this.infraredFrameDescription.Width, this.infraredFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray32Float, null);

            // Body Image ----------------------------------------------------------------------------------

            // Get the depth or color (display) extents
            FrameDescription bodyFrameDescription = this.kinectSensor.ColorFrameSource.FrameDescription; // For Body over Color image
            //FrameDescription bodyFrameDescription = this.kinectSensor.DepthFrameSource.FrameDescription; // For Body over Depth image

            // Get size of Body/Joint space
            this.bodyDisplayWidth = bodyFrameDescription.Width;
            this.bodyDisplayHeight = bodyFrameDescription.Height;

            // Open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // Get the number of bodies (that can be tracked???)
            this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;

            // The following code should be moved to a function ---------------------------------------------
            // Create a list of bones.
            //A bone defined as a line between two joints
            this.bones = new List<Tuple<JointType, JointType>>();

            // Torso
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            // Right Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            // Left Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            // Right Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            // Left Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));
            //----------------------------------------------------------------------------------------------------------

            // Create a List of Body colors, one for each BodyIndex
            this.bodyColors = new List<System.Windows.Media.Pen>();

            this.bodyColors.Add(new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, 8));
            this.bodyColors.Add(new System.Windows.Media.Pen(System.Windows.Media.Brushes.Green, 8));
            this.bodyColors.Add(new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 8));
            this.bodyColors.Add(new System.Windows.Media.Pen(System.Windows.Media.Brushes.Orange, 8));
            this.bodyColors.Add(new System.Windows.Media.Pen(System.Windows.Media.Brushes.Indigo, 8));
            this.bodyColors.Add(new System.Windows.Media.Pen(System.Windows.Media.Brushes.Violet, 8));

            // Create the drawing group we'll use for drawing
            this.bodyDrawingGroup = new DrawingGroup();

            // Create an image source that we can use with our Image control
            this.bodyImageSource = new DrawingImage(this.bodyDrawingGroup);

            // Set event handler for body frame arrival event
            this.bodyFrameReader.FrameArrived += this.BodyFrameReader_FrameArrived;

            //----------------------------------------------------------------------------------------------
            // Basic Face Tracking

            // Specify the required face frame results
            FaceFrameFeatures faceFrameFeatures =
                FaceFrameFeatures.BoundingBoxInColorSpace
                | FaceFrameFeatures.PointsInColorSpace
                | FaceFrameFeatures.RotationOrientation
                | FaceFrameFeatures.FaceEngagement
                | FaceFrameFeatures.Glasses
                | FaceFrameFeatures.Happy
                | FaceFrameFeatures.LeftEyeClosed
                | FaceFrameFeatures.RightEyeClosed
                | FaceFrameFeatures.LookingAway
                | FaceFrameFeatures.MouthMoved
                | FaceFrameFeatures.MouthOpen;

            // Create a face frame source and reader to track each face in the FOV
            this.faceFrameSources = new FaceFrameSource[this.bodyCount];
            this.faceFrameReaders = new FaceFrameReader[this.bodyCount];

            // Repeat for each possible body (max. of this.bodyCount can be tracked)
            for (int i = 0; i < this.bodyCount; i++)
            {
                try
                {
                    // Create a face frame source with the required face frame features and an initial tracking Id of 0 // <-- Note this!!
                    this.faceFrameSources[i] = new FaceFrameSource(this.kinectSensor, 0, faceFrameFeatures);

                    // Open the corresponding reader
                    this.faceFrameReaders[i] = this.faceFrameSources[i].OpenReader();
                }
                catch (Exception ea)
                {
                    // Why the above was not possible (maybe no sensor???)
                    MessageBox.Show(ea.Message);
                }
            }

            // Allocate storage to store (Basic) face frame results for each face in the FOV
            this.faceFrameResults = new FaceFrameResult[this.bodyCount];

            // Create a List of face result colors - one for each face index
            this.faceBrush = new List<System.Windows.Media.Brush>()
            {
                System.Windows.Media.Brushes.Red,
                System.Windows.Media.Brushes.Green,
                System.Windows.Media.Brushes.Blue,
                System.Windows.Media.Brushes.Orange,
                System.Windows.Media.Brushes.Indigo,
                System.Windows.Media.Brushes.Violet
            };

            // Create the drawing group we'll use for drawing the Basic faces
            this.faceDrawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            // This ImageSource is used in the Button event handler
            this.faceImageSource = new DrawingImage(this.faceDrawingGroup);

            // Repeat for each possible body (max. of this.bodyCount can be tracked)
            for (int i = 0; i < this.bodyCount; i++)
            {
                if (this.faceFrameReaders[i] != null)
                {
                    // Assign an event handler for the face frame arrival event
                    this.faceFrameReaders[i].FrameArrived += this.FaceReader_FaceFrameArrived;
                }
            }

            // The following values will be the same as the Color Image dimensions 
            // when the Body image is displayed over the Color image.
            faceDisplayWidth = bodyFrameDescription.Width;
            faceDisplayHeight = bodyFrameDescription.Height;

            // Create a rectangle to be used when drawing the Basic face
            faceDisplayRect = new Rect(0.0, 0.0, faceDisplayWidth, faceDisplayHeight);

            //---------------------------------------------------------------------------------------------
            // HD Face Tracking - following CodeProject sample.

            // Get a hdFaceFrameSource from the sensor
            hdFaceFrameSource = new HighDefinitionFaceFrameSource(kinectSensor);

            // Get a hgFaceFrameReader from the hdFaceFrameSource
            hdFaceFrameReader = hdFaceFrameSource.OpenReader();

            // Assign an event handler for the hdFaceFrame arrival event
            hdFaceFrameReader.FrameArrived += hdFaceFrameReader_FrameArrived;

            // Construct one hdFaceModel object
            hdFaceModel = new FaceModel();

            // Construct one FaceAlignment object
            hdFaceAlignment = new FaceAlignment();

            // Do this one time
            colorImage.Width = colorBitmap.Width;
            colorImage.Height = colorBitmap.Height;
            colorImage.Source = this.colorBitmap;
            colorImageKinematicsTab.Source = this.colorBitmap;
            colorImageHeartRateTab.Source = this.colorBitmap;

            depthImage.Source = this.depthBitmap;

            infraredImage.Source = this.infraredBitmap;

            buttonStartRecording.Background = System.Windows.Media.Brushes.Green;

            //----------------------------------------------------------------------------------------------
            // Kinect Sensor

            // Assign an event handler for the IsAvailableChanged event
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // Open the sensor
            this.kinectSensor.Open();

            // Set the status text
            this.statusLabel.Content = this.kinectSensor.IsAvailable ? "Running" : "No Sensor Available";

            //---------------------------------------------------------------------------------------------
            // Speech synthesizer
            synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoiceByHints(VoiceGender.Female);

            // Assign the text of a Label
            statusLabel.Content = "No Image Selected";
        }

        void writeOneBodyOutput(TimeSpan ts, Body outputBody)
        {
            // Reference: https://msdn.microsoft.com/en-us/library/windowspreview.kinect.body.aspx
            // The Body Class has a number of members, some of which are:
            //
            // Various hand data items:
            // HandLeftConfidence, of type enum TrackingConfidence (High=1, fully tracked/Low=0, not tracked)
            // HandLeftState, of type enum HandState( Unknown=0, NotTracked=1, Open=2, Closed=3, Lasso=4)
            // HandRightConfidence, of type enum TrackingConfidence (High=1, fully tracked/Low=0, not tracked)
            // HandRightState, of type enum HandState( Unknown=0, NotTracked=1, Open=2, Closed=3, Lasso=4)
            // IsRestricted, a boolean value
            // IsTracked, a boolean value
            // JointOrientations, the joint orientations of the body, stored in a read-only dictionary indexed by JointType.
            //   Each dictionary entry contains JointType and Orientation. Orientation is a Vector4 value.
            // Joints, the Joint positions of the body, a read-only dictionary indexed by JointType.
            //   Each dictionary entry contains JointType and Joint. Joint contains JointType, Position (in Camera space),
            //   and TrackingState.
            // Lean, the lean vector of the body
            // LeanTrackingState, the tracking state for body lean
            // TrackingId, the assigned Tracking ID for the body

            // In this function we will write one line of comma-separated data containing some/all of the body data.

            // The TimeSpan is passed in as an input parameter, it was obtained from the BodyFrame data
            outFile.Write(ts.TotalMilliseconds + ",");

            // IsTracked
            outFile.Write(outputBody.IsTracked + ",");

            // HandLeftConfidence & State
            outFile.Write(outputBody.HandLeftConfidence + ",");
            outFile.Write(outputBody.HandLeftState + ",");

            // HandRightConfidence & State
            outFile.Write(outputBody.HandRightConfidence + ",");
            outFile.Write(outputBody.HandRightState + ",");

            // IsRestricted
            outFile.Write(outputBody.IsRestricted + ",");

            // JointOrientations
            for (int k = 1; k < jointNames.Length; k++) // Use indices 1..24
            {
                JointType key = getJointTypeValue(k);
                JointOrientation value;
                if (outputBody.JointOrientations.TryGetValue(key, out value))
                {
                    //outFile.Write(value.JointType + ","); // <-- One of the fields in Orientation
                    outFile.Write(value.Orientation.X + ",");
                    outFile.Write(value.Orientation.Y + ",");
                    outFile.Write(value.Orientation.Z + ",");
                    outFile.Write(value.Orientation.W + ",");
                }
                else
                {
                    outFile.Write(",,,,");
                }
            }

            // Joints
            for (int k = 1; k < jointNames.Length; k++) // Use indices 1..24
            {
                JointType key = getJointTypeValue(k);
                Joint joint = outputBody.Joints[key];

                outFile.Write(joint.TrackingState.ToString() + ",");

                outFile.Write(joint.Position.X + ",");
                outFile.Write(joint.Position.Y + ",");
                outFile.Write(joint.Position.Z + ",");
            }

            // Lean Tracked
            outFile.Write(outputBody.LeanTrackingState + ",");

            // Lean
            Microsoft.Kinect.PointF leanVector = outputBody.Lean;
            outFile.Write(leanVector.X + ",");
            outFile.Write(leanVector.Y + ",");
        }

        private JointType getJointTypeValue(int k)
        {
            // Call this with a number 1..24.
            // The e num values represent 1..24
            JointType[] jtvalues = {
                JointType.SpineMid, // 1
                JointType.SpineMid,
                JointType.Neck,
                JointType.Head,
                JointType.ShoulderLeft,
                JointType.ElbowLeft,
                JointType.WristLeft,
                JointType.HandLeft,
                JointType.ShoulderRight,
                JointType.ElbowRight,
                JointType.WristRight,
                JointType.HandRight,
                JointType.HipLeft,
                JointType.KneeLeft,
                JointType.AnkleLeft,
                JointType.FootLeft,
                JointType.HipRight,
                JointType.KneeRight,
                JointType.AnkleRight,
                JointType.FootRight,
                JointType.SpineShoulder,
                JointType.HandTipLeft,
                JointType.ThumbLeft,
                JointType.HandTipRight,
                JointType.ThumbRight }; // 24

            return jtvalues[k];
        }

        private void writeOneFaceOutput(FaceFrameResult ffr)
        {
            // extract each face property information and store it in faceText
            if (ffr.FaceProperties != null)
            {
                foreach (var item in ffr.FaceProperties)
                {
                    // Label
                    //outfile.write( item.Key.ToString() + "," );

                    // consider a "maybe" as a "no" to restrict 
                    // the detection result refresh rate
                    if (item.Value == DetectionResult.Maybe)
                    {
                        outFile.Write(DetectionResult.No + ",");
                    }
                    else // Yes or No or ???
                    {
                        outFile.Write(item.Value.ToString() + ",");
                    }
                }
            }

            // Extract face rotation in degrees as Euler angles
            if (ffr.FaceRotationQuaternion != null)
            {
                int pitch, yaw, roll;
                ExtractFaceRotationInDegrees(ffr.FaceRotationQuaternion, out pitch, out yaw, out roll);
                //faceText += "FaceYaw : " + yaw + "\n" +
                //            "FacePitch : " + pitch + "\n" +
                //            "FacenRoll : " + roll + "\n";
                outFile.Write(yaw + "," + pitch + "," + roll);
            }

            // End the line
            outFile.WriteLine();
        }

        void writeOutputLineLabels()
        {
            // Reference: https://msdn.microsoft.com/en-us/library/windowspreview.kinect.body.aspx
            // The Body Class has a number of members, some of which are:
            //
            // Various hand data items,
            // IsTracked, a boolean value
            // JointOrientations, the joint orientations of the body
            // Joints, the Joint positions of the body
            // Lean, the lean vector of the body
            // LeanTrackingState, the tracking state for body lean
            // TrackingId, the assigned Tracking ID for the body

            // In this function we will write one line of comma-separated data containing some/all if the body data LABELS.

            // The TimeSpan is passed in as an input parameter, it was obtained from the BodyFrame data
            outFile.Write("TimeSpan,");

            // IsTracked
            outFile.Write("IsTracked,");

            // HandConfidence & State
            outFile.Write("HandLeftConfidence,HandLeftState,HandRightConfidence,HandRightState,");

            // IsRestricted
            outFile.Write("IsRestricted,");

            // Joint Orientations
            for (int k = 1; k < jointNames.Length; k++)
            {
                outFile.Write(jointNames[k] + "OrientationX,");
                outFile.Write(jointNames[k] + "OrientationY,");
                outFile.Write(jointNames[k] + "OrientationZ,");
                outFile.Write(jointNames[k] + "OrientationW,");
            }

            // Joint Positions
            for (int k = 1; k < jointNames.Length; k++)
            {
                outFile.Write(jointNames[k] + "TrackingState,");
                outFile.Write(jointNames[k] + "PositionX,");
                outFile.Write(jointNames[k] + "PositionY,");
                outFile.Write(jointNames[k] + "PositionZ,");
            }

            // Lean Tracking State
            outFile.Write("LeanTrackingState,");

            // Lean Vector
            outFile.Write("LeanVectorX,");
            outFile.Write("LeanVectorY,");

            // Face data
            for (int k = 0; k < facePropertyNames.Length; k++)
                if (k < facePropertyNames.Length - 1)
                    outFile.Write(facePropertyNames[k] + ",");
                else
                    outFile.Write(facePropertyNames[k]); // <-- No terminating comma

            outFile.Write("FaceYaw,");
            outFile.Write("FacePitch,");
            outFile.Write("FaceRoll");

            // End the line
            outFile.WriteLine();
        }

        // Save just the Body Joint values to an abbreviated CSV file for testing use in Matlab
        private void saveSkel(long timeStamp, Body body)
        {

            string filePath = @"C:\Temp\" + timeStamp + ".csv";

            StreamWriter cooStream = new StreamWriter(filePath, false);

            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

            Dictionary<JointType, System.Windows.Point> jointPoints = new Dictionary<JointType, System.Windows.Point>();

            foreach (JointType jointType in joints.Keys)
            {
                //Camera space points
                ColorSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(joints[jointType].Position);

                cooStream.WriteLine(
                    //joints[jointType].JointType + "," +
                    //joints[jointType].TrackingState + "," +
                    joints[jointType].Position.X + "," +
                    joints[jointType].Position.Y + "," +
                    joints[jointType].Position.Z);
                //joints[jointType].Position.Z + "," + 
                //depthSpacePoint.X + " " + 
                //depthSpacePoint.Y);
            }
            //If we want to record both hand states
            bool handLassO = false;
            if (handLassO == true)
            {
                string wrtLineData = "LeftHand " + body.HandLeftState + " RightHand " + body.HandRightState;
                cooStream.WriteLine(wrtLineData);
            }

            cooStream.Close();
        }

        #region Kinect V2 Frame Event Handlers

        // Handles the color frame data arriving from the sensor
        private void ColorReader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (bodyFrameCounter != 0)
                return;

            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    // Display the frame rate
                    double thisTime = colorFrame.RelativeTime.TotalMilliseconds;
                    double elapsedTime = thisTime - lastTime;  // milliseconds
                    lastTime = thisTime;
                    double rate = 1000.0 / elapsedTime;
                    frameRateBar.Value = rate;

                    string rateString = String.Format("{0}", (int)(rate));
                    frameRateLabel.Content = "Frame Rate: " + rateString + " (f/sec)";

                    statusLabel.Content = "cis/fis/tis/sis: " + colorImageSelected + "," +
                        frontImageSelected + "," + topImageSelected + "," + sideImageSelected;

                    // Display the color image on the Biometrics tab
                    // Copy the color data to colorBitmap for display
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        this.colorBitmap.Lock();

                        // Verify data and write the new color frame data to the display bitmap
                        if ((colorFrameDescription.Width == this.colorBitmap.PixelWidth) &&
                            (colorFrameDescription.Height == this.colorBitmap.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.colorBitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            this.colorBitmap.AddDirtyRect(new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight));
                        }

                        this.colorBitmap.Unlock();
                    }

                    colorImageBiometricTab.Source = colorBitmap;
                    colorImageKinematicsTab.Source = colorBitmap;
                    colorImageHeartRateTab.Source = colorBitmap;

                    //----------------------------------------------------------------------------
                    if (!colorImageSelected) return; // No additional work to be done here
                    //----------------------------------------------------------------------------


                    if(frontImageSelected || topImageSelected || sideImageSelected)
                    {
                        //MessageBox.Show("Here");

                        // Draw a black rectangle
                        //this.colorBitmap.FillRectangle(0, 0, (int)this.colorBitmap.Width-1, (int)this.colorBitmap.Height-1, Colors.Black);
                        return;
                    }

                    // Copy the color data to colorBitmap for display
                    //FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        this.colorBitmap.Lock();

                        // Verify data and write the new color frame data to the display bitmap
                        if ((colorFrameDescription.Width == this.colorBitmap.PixelWidth) &&
                            (colorFrameDescription.Height == this.colorBitmap.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.colorBitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            this.colorBitmap.AddDirtyRect(new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight));
                        }

                        this.colorBitmap.Unlock();
                    }
                }

                // in MainWiundow(), the statement:

                // colorImage.Source = this.ColorBitmap;

                // "attached" the color bitmap to the color Image control.
                // That Image control can be made visible or invisible as needed.                
            }
        }

        // Handles the depth frame data arriving from the sensor
        private void DepthReader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            bool depthFrameProcessed = false;

            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
            {
                if (depthFrame != null)
                {
                    //---------------------------------------------------------------------------------------------
                    // In this application, the depth image not needed when either of these two images are selected
                    if (colorImageSelected || infraredImageSelected) return; // <-- No work to be done here
                    //---------------------------------------------------------------------------------------------

                    // The fastest way to process the body index data is to directly access 
                    // the underlying buffer
                    using (Microsoft.Kinect.KinectBuffer depthBuffer = depthFrame.LockImageBuffer())
                    {
                        // Verify data and write the depth data to the depth display bitmap
                        if (((this.depthFrameDescription.Width * this.depthFrameDescription.Height) ==
                            (depthBuffer.Size / this.depthFrameDescription.BytesPerPixel)) &&
                            (this.depthFrameDescription.Width == this.depthBitmap.PixelWidth) &&
                            (this.depthFrameDescription.Height == this.depthBitmap.PixelHeight))
                        {
                            // Note: In order to see the full range of depth (including the less reliable far field depth)
                            // we are setting maxDepth to the extreme potential depth threshold
                            ushort maxDepth = ushort.MaxValue;

                            // If you wish to filter by reliable depth distance, uncomment the following line:
                            //// maxDepth = depthFrame.DepthMaxReliableDistance

                            this.ProcessDepthFrameData(
                                depthBuffer.UnderlyingBuffer, depthBuffer.Size, depthFrame.DepthMinReliableDistance, maxDepth);

                            depthFrameProcessed = true;
                        }
                    }
                }
            }

            // Display the depth image
            if (depthFrameProcessed)
            {
                if (depthImageSelected)
                {
                    this.RenderDepthPixels();

                    // In MainWindow(), the following statement:

                    //depthImage.Source = this.depthBitmap;

                    // "attached the depth bitmap to the depth Image control.
                }
            }
        }

        // Directly accesses the underlying image buffer of the DepthFrame to 
        // create a displayable bitmap.
        // This function requires the /unsafe compiler option as we make use of direct
        // access to the native memory pointed to by the depthFrameData pointer.
        private unsafe void ProcessDepthFrameData(IntPtr depthFrameData, uint depthFrameDataSize, ushort minDepth, ushort maxDepth)
        {
            // Depth frame data is a 16 bit value
            ushort* frameData = (ushort*)depthFrameData;

            // Convert depth to a visual representation
            for (int i = 0; i < (int)(depthFrameDataSize / this.depthFrameDescription.BytesPerPixel); ++i)
            {
                // Get the depth for this pixel
                ushort depth = frameData[i];

                // To convert to a byte, we're mapping the depth value to the byte range.
                // Values outside the reliable depth range are mapped to 0 (black).
                this.depthPixels[i] = (byte)(depth >= minDepth && depth <= maxDepth ? (depth / MapDepthToByte) : 0);
            }
        }

        // Renders depth pixels into the writeableBitmap.
        private void RenderDepthPixels()
        {
            this.depthBitmap.WritePixels(
                new Int32Rect(0, 0, this.depthBitmap.PixelWidth, this.depthBitmap.PixelHeight),
                this.depthPixels,
                this.depthBitmap.PixelWidth,
                0);
        }

        // Handles the infrared frame data arriving from the sensor
        private void InfraredReader_InfraredFrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            // InfraredFrame is IDisposable
            using (InfraredFrame infraredFrame = e.FrameReference.AcquireFrame())
            {
                if (infraredFrame != null)
                {
                    //----------------------------------------
                    if (!infraredImageSelected) return;  // <-- No further work needed here
                    //----------------------------------------

                    // The fastest way to process the infrared frame data is to directly access 
                    // the underlying buffer
                    using (Microsoft.Kinect.KinectBuffer infraredBuffer = infraredFrame.LockImageBuffer())
                    {
                        // verify data and write the new infrared frame data to the display bitmap
                        if (((this.infraredFrameDescription.Width * this.infraredFrameDescription.Height) ==
                            (infraredBuffer.Size / this.infraredFrameDescription.BytesPerPixel)) &&
                            (this.infraredFrameDescription.Width == this.infraredBitmap.PixelWidth) &&
                            (this.infraredFrameDescription.Height == this.infraredBitmap.PixelHeight))
                        {
                            this.ProcessInfraredFrameData(infraredBuffer.UnderlyingBuffer, infraredBuffer.Size);
                        }
                    }
                }
            }

            // Display the infrared image
            if (infraredImageSelected)
            {
                // In MainWindow(), the following statement:

                //infraredImage.Source = this.infraredBitmap;

                // "attached" the infrared bitmap to the infrared Image control.
            }
        }

        // Directly accesses the underlying image buffer of the InfraredFrame to 
        // create a displayable bitmap.
        // This function requires the /unsafe compiler option as we make use of direct
        // access to the native memory pointed to by the infraredFrameData pointer.
        private unsafe void ProcessInfraredFrameData(IntPtr infraredFrameData, uint infraredFrameDataSize)
        {
            // infrared frame data is a 16 bit value
            ushort* frameData = (ushort*)infraredFrameData;

            // lock the target bitmap
            this.infraredBitmap.Lock();

            // get the pointer to the bitmap's back buffer
            float* backBuffer = (float*)this.infraredBitmap.BackBuffer;

            // process the infrared data
            for (int i = 0; i < (int)(infraredFrameDataSize / this.infraredFrameDescription.BytesPerPixel); ++i)
            {
                // since we are displaying the image as a normalized grey scale image, we need to convert from
                // the ushort data (as provided by the InfraredFrame) to a value from [InfraredOutputValueMinimum, InfraredOutputValueMaximum]
                backBuffer[i] = Math.Min(InfraredOutputValueMaximum, (((float)frameData[i] / InfraredSourceValueMaximum * InfraredSourceScale) * (1.0f - InfraredOutputValueMinimum)) + InfraredOutputValueMinimum);
            }

            // mark the entire bitmap as needing to be drawn
            this.infraredBitmap.AddDirtyRect(new Int32Rect(0, 0, this.infraredBitmap.PixelWidth, this.infraredBitmap.PixelHeight));

            // unlock the bitmap
            this.infraredBitmap.Unlock();
        }

        // A counter used when generating one frame of Body data for Matlab
        int firstSaveCount = 0;

        // Handles the body frame data arriving from the sensor
        private void BodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            if(bodyFrameCounter < bodyFrameMax)
            {
                bodyFrameCounter++;

                return;
            }
            else
            {
                bodyFrameCounter = 0;

                // Now display the body frame data
            }

            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    // If the array bodies has not yet been created
                    if (this.bodies == null)
                    {
                        // Create an array to store the maximum number of possible Bodies
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);

                    // Yes, we now have BodyData. The Body Image will be created below
                    dataReceived = true;

                    //-------------------------------------------------------------------------------------
                    // For now, I'm adding the following code here because ONE Body needs to be selected.
                    // I'm not sure how many bodies can be HD face tracked.

                    // HD Face Tracking - Select one body
                    Body body = bodies.Where(b => b.IsTracked).FirstOrDefault();

                    if (!hdFaceFrameSource.IsTrackingIdValid)
                    {
                        if (body != null)
                        {
                            hdFaceFrameSource.TrackingId = body.TrackingId;
                        }
                    }
                    //------------------------------------------------------------------------------------

                    //------------------------------------------------------------------------------------
                    // When recording, write the frame body data TO A FILE here.
                    // Note that this will be repeated for each body tracked.
                    //
                    // I'm not sure how to best combine the body and Basic face tracking data for multiple tracked bodies.
                    //
                    // I think the following code is best moved  below, where the Bodys are being drawn
                    //------------------------------------------------------------------------------------
                    foreach (Body b in this.bodies)
                    {
                        // Write Body data to output file here
                        if (b != null)
                        {
                            if (nowRecording)
                            {
                                // If the body is not tracked, there is no data to write...
                                if (b.IsTracked)
                                {
                                    // Write ONLY the body data, without ending the line written.
                                    writeOneBodyOutput(bodyFrame.RelativeTime, b);

                                    // Save some data for Matlab here
                                    if (firstSaveCount == 100)
                                    {
                                        saveSkel(bodyFrame.RelativeTime.Ticks, b);
                                        synthesizer.SpeakAsync("Matlab Data Saved");
                                    }
                                    firstSaveCount++;


                                } // end if b.is tracked
                            } // end if nowrecording
                        } // end if b != null
                    } // end foreach Body b in bodies
                } // end if bodyFrame != null
            } // end using BodyFrame

            // If we have Body data to process, so draw each body using a DrawingGroup
            if (dataReceived)
            {
                // Draw the Bodies here with bodyDrawingGroup
                using (DrawingContext dc = this.bodyDrawingGroup.Open())
                {
                    // Draw a transparent background to set the render size
                    if (frontImageSelected || topImageSelected || sideImageSelected)
                    {
                        dc.DrawRectangle(System.Windows.Media.Brushes.Black, null, new Rect(0.0, 0.0, this.bodyDisplayWidth, this.bodyDisplayHeight));
                    }
                    else
                    {
                        dc.DrawRectangle(System.Windows.Media.Brushes.Transparent, null, new Rect(0.0, 0.0, this.bodyDisplayWidth, this.bodyDisplayHeight));
                    }

                    int penIndex = 0;

                    // Process/draw/write each body in sequence here
                    foreach (Body body in this.bodies) // The bodies are in an array, so penIndex works ok...
                    {
                        System.Windows.Media.Pen drawPen = this.bodyColors[penIndex++];

                        switch(penIndex)
                        {
                            case 0:
                                faceLabel0.Background = new SolidColorBrush(Colors.Transparent);
                                faceTextBlock0.Text = "";
                                break;
                            case 1:
                                faceLabel1.Background = new SolidColorBrush(Colors.Transparent);
                                faceTextBlock1.Text = "";
                                break;
                            case 2:
                                faceLabel2.Background = new SolidColorBrush(Colors.Transparent);
                                faceTextBlock2.Text = "";
                                break;
                            case 3:
                                faceLabel3.Background = new SolidColorBrush(Colors.Transparent);
                                faceTextBlock3.Text = "";
                                break;
                            case 4:
                                faceLabel4.Background = new SolidColorBrush(Colors.Transparent);
                                faceTextBlock4.Text = "";
                                break;
                            case 5:
                                faceLabel5.Background = new SolidColorBrush(Colors.Transparent);
                                faceTextBlock5.Text = "";
                                break;
                        }

                        // When the colorImage or the frontImage is selected
                        // draw the usual body and/or joints
                        if (body.IsTracked &&(colorImageSelected || frontImageSelected))
                        {
                            this.DrawClippedEdges(body, dc);

                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                            // Convert the joint points to depth (display) space
                            Dictionary<JointType, System.Windows.Point> jointPoints = new Dictionary<JointType, System.Windows.Point>();

                            // Map each Joint to color (or depth) space
                            foreach (JointType jointType in joints.Keys)
                            {
                                // sometimes the depth(Z) of an inferred joint may show as negative
                                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                                CameraSpacePoint position = joints[jointType].Position;
                                if (position.Z < 0)
                                {
                                    position.Z = InferredZPositionClamp;
                                }

                                // What follows here depends on what Image you are trying to show the skeletion over

                                // Code for drawing a Body over the depth Image
                                //DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraImagePointToDepthSpace(position);
                                //jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);

                                // Code for drawing a Body over the color Image
                                ColorSpacePoint colorSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(position);
                                jointPoints[jointType] = new System.Windows.Point(colorSpacePoint.X, colorSpacePoint.Y);
                            } // End foreach

                            // Finally, draw the body using the selected Pen
                            this.DrawBody(joints, jointPoints, dc, drawPen);

                            // Now draw the hands as circles
                            this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                            this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);

                        }// End if 
                        else if(body.IsTracked && topImageSelected)
                        {
                            //this.DrawClippedEdges(body, dc);

                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                            // Create a dictionary of Points indexed by JointType
                            Dictionary<JointType, System.Windows.Point> jointPoints = 
                                new Dictionary<JointType, System.Windows.Point>();

                            // Map each 3D Joint to top view X-Z
                            foreach (JointType jointType in joints.Keys)
                            {
                                // sometimes the depth(Z) of an inferred joint may show as negative
                                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                                CameraSpacePoint position = joints[jointType].Position;
                                if (position.Z < 0)
                                {
                                    position.Z = InferredZPositionClamp;
                                }
                                // From here down work with position.X, .Y, .Z values

                                // What follows here depends on what Image you are trying to show the skeletion over

                                // Code for drawing a Body over the depth Image
                                //DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraImagePointToDepthSpace(position);
                                //jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);

                                // Code for drawing a Body's top-view XZ coordinates over the color Image
                                //ColorSpacePoint colorSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(position);
                                // Scale to about 4 meters in the Z direction using a 1920x1080 image
                                float zDistance = 3.0f; // meters = 1080 pixels;
                                float xDistance = zDistance * 1920f / 1080f;

                                float scaledX = (1920 / 2.0f) + position.X * 1080 / xDistance;

                                float scaledZ = 1080f - position.Z * 1080 / zDistance;

                                jointPoints[jointType] = new System.Windows.Point(scaledX, scaledZ);
                            } // End foreach

                            // Finally, draw the body using the selected Pen
                            this.DrawBody(joints, jointPoints, dc, drawPen);

                            // Now draw the hands as circles
                            //this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                            //this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
                        }
                        else if (body.IsTracked && sideImageSelected)
                        {
                            //this.DrawClippedEdges(body, dc);

                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                            // Create a dictionary of Points indexed by JointType
                            Dictionary<JointType, System.Windows.Point> jointPoints =
                                new Dictionary<JointType, System.Windows.Point>();

                            // Map each 3D Joint to side view Z-Y
                            foreach (JointType jointType in joints.Keys)
                            {
                                // sometimes the depth(Z) of an inferred joint may show as negative
                                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                                CameraSpacePoint position = joints[jointType].Position;
                                if (position.Z < 0)
                                {
                                    position.Z = InferredZPositionClamp;
                                }
                                // From here down work with position.X, .Y, .Z values

                                // What follows here depends on what Image you are trying to show the skeletion over

                                // Code for drawing a Body over the depth Image
                                //DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraImagePointToDepthSpace(position);
                                //jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);

                                // Code for drawing a Body's top-view XZ coordinates over the color Image
                                //ColorSpacePoint colorSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(position);
                                // Scale to about 4 meters in the Z direction using a 1920x1080 image
                                float xDistance = 4.0f; // meters = 1920 pixels;
                                float zDistance = xDistance * 1080f / 1920f;

                                float scaledZ = (1080 / 2.0f) - position.Y * 1080 / zDistance;

                                float scaledX = 1920 - position.Z * 1920 / xDistance;

                                jointPoints[jointType] = new System.Windows.Point(scaledX, scaledZ);
                            } // End foreach

                            // Finally, draw the body using the selected Pen
                            this.DrawBody(joints, jointPoints, dc, drawPen);

                            // Now draw the hands as circles
                            //this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                            //this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
                        }

                        // Prevent drawing outside of our render area
                        this.bodyDrawingGroup.ClipGeometry =
                            new RectangleGeometry(new Rect(0.0, 0.0, this.bodyDisplayWidth, this.bodyDisplayHeight));

                    } // End foreach body  

                }  // End using DrawingContext dc       

                // Here draw the Basic Face using the faceDrawingGroup
                using (DrawingContext dcf = this.faceDrawingGroup.Open())
                {
                    if (faceImageSelected)
                    {
                        // draw the transparent (or change color to dark) background
                        dcf.DrawRectangle(System.Windows.Media.Brushes.Transparent, null, this.faceDisplayRect);

                        bool drawFaceResult = false;

                        // Repeat for each possible face source
                        for (int i = 0; i < this.bodyCount; i++)
                        {
                            // Check if a valid face is tracked in this face source
                            if (this.faceFrameSources[i].IsTrackingIdValid)
                            {
                                // Check if we have valid face frame results
                                if (this.faceFrameResults[i] != null)
                                {
                                    // Draw face frame results using a function
                                    this.DrawFaceFrameResults(i, this.faceFrameResults[i], dcf);

                                    // Write the Basic face tracking data to the output 
                                    if (nowRecording)
                                    {
                                        writeOneFaceOutput(this.faceFrameResults[i]);
                                    }

                                    if (!drawFaceResult)
                                    {
                                        drawFaceResult = true;
                                    }
                                }
                            }
                            else
                            {
                                // check if the corresponding body is tracked 
                                if (this.bodies[i].IsTracked)
                                {
                                    // update the face frame source to track this body
                                    this.faceFrameSources[i].TrackingId = this.bodies[i].TrackingId;
                                }
                            }

                        }

                        if (!drawFaceResult)
                        {
                            // if no faces were drawn then this indicates one of the following:
                            // a body was not tracked 
                            // a body was tracked but the corresponding face was not tracked
                            // a body and the corresponding face was tracked though the face box or the face points were not valid
                            /*
                            dcf.DrawText(
                                this.textFaceNotTracked,
                                this.textLayoutFaceNotTracked);
                            */
                            //faceLabel.Content = "No bodies or faces" + "\n" + "are tracked ...";
                        }

                        this.faceDrawingGroup.ClipGeometry = new RectangleGeometry(this.faceDisplayRect);
                    }// End if
                    else
                    {
                        // faceImageSelected is false. Do not draw or write any Basic face tracking data.
                    }
                } // End using...
            } // End if
        }
        

        

        private void hdFaceFrameReader_FrameArrived(object sender, HighDefinitionFaceFrameArrivedEventArgs e)
        {
            //----------------------------------------------------------
            if (!hdFaceImageSelected) return; // <-- No more work to be done here
            //----------------------------------------------------------

            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null && frame.IsFaceTracked)
                {
                    frame.GetAndRefreshFaceAlignmentResult(hdFaceAlignment);

                    UpdateFacePoints();
                }
            }
        }

        private void UpdateFacePoints()
        {
            if (hdFaceModel == null) return;

            var vertices = hdFaceModel.CalculateVerticesForAlignment(hdFaceAlignment);

            if (vertices.Count > 0)
            {
                if (hdFacePoints.Count == 0)
                {
                    for (int index = 0; index < vertices.Count; index++)
                    {
                        Ellipse ellipse = new Ellipse
                        {
                            Width = 2.0,
                            Height = 2.0,
                            Fill = new SolidColorBrush(Colors.Blue)
                        };

                        hdFacePoints.Add(ellipse);
                    }

                    foreach (Ellipse ellipse in hdFacePoints)
                    {
                        hdFaceCanvas.Children.Add(ellipse);
                    }
                }

                for (int index = 0; index < vertices.Count; index++)
                {
                    CameraSpacePoint vertice = vertices[index];
                    //DepthSpacePoint point = kinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(vertice);

                    ColorSpacePoint point = kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(vertice);

                    if (float.IsInfinity(point.X) || float.IsInfinity(point.Y)) return;

                    Ellipse ellipse = hdFacePoints[index];

                    Canvas.SetLeft(ellipse, point.X);
                    Canvas.SetTop(ellipse, point.Y);
                }
            }
        }

        // Handles the face frame data arriving from the sensor
        private void FaceReader_FaceFrameArrived(object sender, FaceFrameArrivedEventArgs e)
        {
            //------------------------------------------------------------------
            if (!faceImageSelected) return;
            //------------------------------------------------------------------

            using (FaceFrame faceFrame = e.FrameReference.AcquireFrame())
            {
                if (faceFrame != null)
                {
                    // get the index of the face source from the face source array
                    int index = this.GetFaceSourceIndex(faceFrame.FaceFrameSource);

                    // check if this face frame has valid face frame results
                    if (this.ValidateFaceBoxAndPoints(faceFrame.FaceFrameResult))
                    {
                        // store this face frame result to draw later
                        this.faceFrameResults[index] = faceFrame.FaceFrameResult;
                    }
                    else
                    {
                        // indicates that the latest face frame result from this reader is invalid
                        this.faceFrameResults[index] = null;
                    }
                }
            }
        }

        // Returns the index of the face frame source
        private int GetFaceSourceIndex(FaceFrameSource faceFrameSource)
        {
            int index = -1;

            for (int i = 0; i < this.bodyCount; i++)
            {
                if (this.faceFrameSources[i] == faceFrameSource)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        // Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            this.statusLabel.Content = this.kinectSensor.IsAvailable ? "Running" : "No Sensor Available";
        }

        #endregion

        #region Kinect V2 Drawing Functions

        // Draws face frame results - To a Label for UI display, but previously to a rectangle region for display
        private void DrawFaceFrameResults(int faceIndex, FaceFrameResult faceResult, DrawingContext drawingContext)
        {
            // choose the brush based on the face index
            System.Windows.Media.Brush drawingBrush = this.faceBrush[0];
            if (faceIndex < this.bodyCount)
            {
                drawingBrush = this.faceBrush[faceIndex];
            }

            System.Windows.Media.Pen drawingPen = new System.Windows.Media.Pen(drawingBrush, DrawFaceShapeThickness);
            //Pen drawingPen = new Pen(Brushes.Red, 5);

            // draw the face bounding box
            var faceBoxSource = faceResult.FaceBoundingBoxInColorSpace;
            Rect faceBox = new Rect(faceBoxSource.Left, faceBoxSource.Top, faceBoxSource.Right - faceBoxSource.Left, faceBoxSource.Bottom - faceBoxSource.Top);
            drawingContext.DrawRectangle(null, drawingPen, faceBox);

            if (faceResult.FacePointsInColorSpace != null)
            {
                // draw each face point
                foreach (Microsoft.Kinect.PointF pointF in faceResult.FacePointsInColorSpace.Values)
                {
                    drawingContext.DrawEllipse(null, drawingPen, new System.Windows.Point(pointF.X, pointF.Y), FacePointRadius, FacePointRadius);
                }
            }

            string faceText = string.Empty;
            faceText = "Face: " + faceIndex + "\n";

            // extract each face property information and store it in faceText
            if (faceResult.FaceProperties != null)
            {
                foreach (var item in faceResult.FaceProperties)
                {
                    faceText += item.Key.ToString() + " : ";

                    // consider a "maybe" as a "no" to restrict 
                    // the detection result refresh rate
                    if (item.Value == DetectionResult.Maybe)
                    {
                        faceText += DetectionResult.No + "\n";
                    }
                    else
                    {
                        faceText += item.Value.ToString() + "\n";
                    }
                }
            }

            // extract face rotation in degrees as Euler angles
            int pitch=0, yaw=0, roll=0;

            if (faceResult.FaceRotationQuaternion != null)
            {   
                ExtractFaceRotationInDegrees(faceResult.FaceRotationQuaternion, out pitch, out yaw, out roll);
                faceText += "FaceYaw : " + yaw + "\n" +
                            "FacePitch : " + pitch + "\n" +
                            "FaceRoll : " + roll + "\n";
            }

            // render the face property and face rotation information
            System.Windows.Point faceTextLayout;
            if (this.GetFaceTextPositionInColorSpace(faceIndex, out faceTextLayout))
            {
                /*
                drawingContext.DrawText(
                        new FormattedText(
                            faceText,
                            CultureInfo.GetCultureInfo("en-us"),
                            FlowDirection.LeftToRight,
                            new Typeface("Georgia"),
                            DrawTextFontSize,
                            drawingBrush),
                        faceTextLayout);
                */

                if (faceImageSelected)
                {
                    switch (faceIndex)
                    {
                        case 0:
                            faceTextBlock0.Text = faceText;
                            faceLabel0.Background = drawingBrush;
                            break;
                        case 1:
                            faceTextBlock1.Text = faceText;
                            faceLabel1.Background = drawingBrush;
                            break;
                        case 2:
                            faceTextBlock2.Text = faceText;
                            faceLabel2.Background = drawingBrush;
                            break;
                        case 3:
                            faceTextBlock3.Text = faceText;
                            faceLabel3.Background = drawingBrush;
                            break;
                        case 4:
                            faceTextBlock4.Text = faceText;
                            faceLabel4.Background = drawingBrush;
                            break;
                        case 5:
                            faceTextBlock5.Text = faceText;
                            faceLabel5.Background = drawingBrush;
                            break;
                    }

                    // Capture the pitch, yaw and roll values for the plot
                    switch(faceIndex)
                    {
                        case 0:
                            // Use Kinetics chart 0
                            if(myValuesKinematics00.Count < NKinematics)
                            {
                                myValuesKinematics00.Add((double)yaw);
                                myValuesKinematics01.Add((double)pitch);
                                myValuesKinematics02.Add((double)roll);
                            }
                            else
                            {
                                myValuesKinematics00.RemoveAt(0);
                                myValuesKinematics01.RemoveAt(0);
                                myValuesKinematics02.RemoveAt(0);

                                myValuesKinematics00.Add((double)yaw);
                                myValuesKinematics01.Add((double)pitch);
                                myValuesKinematics02.Add((double)roll);
                            }
                            break;

                        case 1:
                            // Use Kinetics chart 1
                            if (myValuesKinematics10.Count < NKinematics)
                            {
                                myValuesKinematics10.Add((double)yaw);
                                myValuesKinematics11.Add((double)pitch);
                                myValuesKinematics12.Add((double)roll);
                            }
                            else
                            {
                                myValuesKinematics10.RemoveAt(0);
                                myValuesKinematics11.RemoveAt(0);
                                myValuesKinematics12.RemoveAt(0);

                                myValuesKinematics10.Add((double)yaw);
                                myValuesKinematics11.Add((double)pitch);
                                myValuesKinematics12.Add((double)roll);
                            }
                            break;
                        case 2:
                            // Use Kinetics chart 2
                            if (myValuesKinematics20.Count < NKinematics)
                            {
                                myValuesKinematics20.Add((double)yaw);
                                myValuesKinematics21.Add((double)pitch);
                                myValuesKinematics22.Add((double)roll);
                            }
                            else
                            {
                                myValuesKinematics20.RemoveAt(0);
                                myValuesKinematics21.RemoveAt(0);
                                myValuesKinematics22.RemoveAt(0);

                                myValuesKinematics20.Add((double)yaw);
                                myValuesKinematics21.Add((double)pitch);
                                myValuesKinematics22.Add((double)roll);
                            }
                            break;

                        case 3:
                            // Use Kinetics chart 3
                            if (myValuesKinematics30.Count < NKinematics)
                            {
                                myValuesKinematics30.Add((double)yaw);
                                myValuesKinematics31.Add((double)pitch);
                                myValuesKinematics32.Add((double)roll);
                            }
                            else
                            {
                                myValuesKinematics30.RemoveAt(0);
                                myValuesKinematics31.RemoveAt(0);
                                myValuesKinematics32.RemoveAt(0);

                                myValuesKinematics30.Add((double)yaw);
                                myValuesKinematics31.Add((double)pitch);
                                myValuesKinematics32.Add((double)roll);
                            }
                            break;
                        case 4:
                            // Use Kinetics chart 4
                            if (myValuesKinematics40.Count < NKinematics)
                            {
                                myValuesKinematics40.Add((double)yaw);
                                myValuesKinematics41.Add((double)pitch);
                                myValuesKinematics42.Add((double)roll);
                            }
                            else
                            {
                                myValuesKinematics40.RemoveAt(0);
                                myValuesKinematics41.RemoveAt(0);
                                myValuesKinematics42.RemoveAt(0);

                                myValuesKinematics40.Add((double)yaw);
                                myValuesKinematics41.Add((double)pitch);
                                myValuesKinematics42.Add((double)roll);
                            }
                            break;

                        case 5:
                            // Use Kinetics chart 5
                            if (myValuesKinematics50.Count < NKinematics)
                            {
                                myValuesKinematics50.Add((double)yaw);
                                myValuesKinematics51.Add((double)pitch);
                                myValuesKinematics52.Add((double)roll);
                            }
                            else
                            {
                                myValuesKinematics50.RemoveAt(0);
                                myValuesKinematics51.RemoveAt(0);
                                myValuesKinematics52.RemoveAt(0);

                                myValuesKinematics50.Add((double)yaw);
                                myValuesKinematics51.Add((double)pitch);
                                myValuesKinematics52.Add((double)roll);
                            }
                            break;
                    }
                }
                /*
                if (faceImageSelected)
                    faceLabel0.Content = faceText;
                else
                    faceLabel0.Content = "";
                */
            }
        }

        // Computes the face result text position by adding an offset to the corresponding 
        // body's head joint in cameraImage space and then by projecting it to screen space
        private bool GetFaceTextPositionInColorSpace(int faceIndex, out System.Windows.Point faceTextLayout)
        {
            faceTextLayout = new System.Windows.Point();
            bool isLayoutValid = false;

            Body body = this.bodies[faceIndex];
            if (body.IsTracked)
            {
                var headJoint = body.Joints[JointType.Head].Position;

                CameraSpacePoint textPoint = new CameraSpacePoint()
                {
                    X = headJoint.X + TextLayoutOffsetX,
                    Y = headJoint.Y + TextLayoutOffsetY,
                    Z = headJoint.Z
                };

                ColorSpacePoint textPointInColor = this.coordinateMapper.MapCameraPointToColorSpace(textPoint);

                faceTextLayout.X = textPointInColor.X;
                faceTextLayout.Y = textPointInColor.Y;
                isLayoutValid = true;
            }

            return isLayoutValid;
        }


        // Validates face bounding box and face points to be within screen space
        private bool ValidateFaceBoxAndPoints(FaceFrameResult faceResult)
        {
            bool isFaceValid = faceResult != null;

            if (isFaceValid)
            {
                var faceBox = faceResult.FaceBoundingBoxInColorSpace;
                if (faceBox != null)
                {
                    // check if we have a valid rectangle within the bounds of the screen space
                    isFaceValid = (faceBox.Right - faceBox.Left) > 0 &&
                                  (faceBox.Bottom - faceBox.Top) > 0 &&
                                  faceBox.Right <= this.faceDisplayWidth &&
                                  faceBox.Bottom <= this.faceDisplayHeight;

                    if (isFaceValid)
                    {
                        var facePoints = faceResult.FacePointsInColorSpace;
                        if (facePoints != null)
                        {
                            foreach (Microsoft.Kinect.PointF pointF in facePoints.Values)
                            {
                                // check if we have a valid face point within the bounds of the screen space
                                bool isFacePointValid = pointF.X > 0.0f &&
                                                        pointF.Y > 0.0f &&
                                                        pointF.X < this.faceDisplayWidth &&
                                                        pointF.Y < this.faceDisplayHeight;

                                if (!isFacePointValid)
                                {
                                    isFaceValid = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return isFaceValid;
        }

        // Draws a body in the front view using CameraSpace coordinates
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, 
            System.Windows.Point> jointPoints, DrawingContext drawingContext, System.Windows.Media.Pen drawingPen)
        {
            
            // Draw the bones
            foreach (var bone in this.bones)
            {
                this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, drawingPen);
            }

            // Are joints to be drawn
            if (!(bool)cbJoints.IsChecked) return;

            // Draw the joints
            foreach (JointType jointType in joints.Keys)
            {
                System.Windows.Media.Brush drawBrush = null;

                TrackingState trackingState = joints[jointType].TrackingState;

                if (trackingState == TrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (trackingState == TrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, jointPoints[jointType], JointThickness, JointThickness);
                }
            }            
        }

        private void DrawBoneTopView(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, System.Windows.Point> jointPoints,
            JointType jointType0, JointType jointType1, DrawingContext drawingContext, System.Windows.Media.Pen drawingPen)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TrackingState.NotTracked ||
                joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == TrackingState.Inferred &&
                joint1.TrackingState == TrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            System.Windows.Media.Pen drawPen = this.inferredBonePen;
            if ((joint0.TrackingState == TrackingState.Tracked) && (joint1.TrackingState == TrackingState.Tracked))
            {
                drawPen = drawingPen;
            }

            drawingContext.DrawLine(drawPen, jointPoints[jointType0], jointPoints[jointType1]);
        }

        // Draws one bone of a body (joint to joint)
        private void DrawBone(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, System.Windows.Point> jointPoints, 
            JointType jointType0, JointType jointType1, DrawingContext drawingContext, System.Windows.Media.Pen drawingPen)
        {
            // Are bones to be drawn?
            if (!(bool)cbBones.IsChecked) return;

            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TrackingState.NotTracked ||
                joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            System.Windows.Media.Pen drawPen = this.inferredBonePen;
            if ((joint0.TrackingState == TrackingState.Tracked) && (joint1.TrackingState == TrackingState.Tracked))
            {
                drawPen = drawingPen;
            }

            drawingContext.DrawLine(drawPen, jointPoints[jointType0], jointPoints[jointType1]);
        }

        // Draws a hand symbol if the hand is tracked: red circle = closed, green circle = opened; blue circle = lasso
        private void DrawHand(HandState handState, System.Windows.Point handPosition, DrawingContext drawingContext)
        {
            switch (handState)
            {
                case HandState.Closed:
                    drawingContext.DrawEllipse(this.handClosedBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Open:
                    drawingContext.DrawEllipse(this.handOpenBrush, null, handPosition, HandSize, HandSize);
                    break;

                case HandState.Lasso:
                    drawingContext.DrawEllipse(this.handLassoBrush, null, handPosition, HandSize, HandSize);
                    break;
            }
        }

        // Draws indicators to show which edges are clipping body data
        private void DrawClippedEdges(Body body, DrawingContext drawingContext)
        {
            FrameEdges clippedEdges = body.ClippedEdges;

            if (clippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    System.Windows.Media.Brushes.Red,
                    null,
                    new Rect(0, this.bodyDisplayHeight - ClipBoundsThickness, this.bodyDisplayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    System.Windows.Media.Brushes.Red,
                    null,
                    new Rect(0, 0, this.bodyDisplayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    System.Windows.Media.Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, this.bodyDisplayHeight));
            }

            if (clippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    System.Windows.Media.Brushes.Red,
                    null,
                    new Rect(this.bodyDisplayWidth - ClipBoundsThickness, 0, ClipBoundsThickness, this.bodyDisplayHeight));
            }
        }

        #endregion

        // Converts rotation quaternion to Euler angles 
        // And then maps them to a specified range of values to control the refresh rate
        private static void ExtractFaceRotationInDegrees(Vector4 rotQuaternion, out int pitch, out int yaw, out int roll)
        {
            double x = rotQuaternion.X;
            double y = rotQuaternion.Y;
            double z = rotQuaternion.Z;
            double w = rotQuaternion.W;

            // convert face rotation quaternion to Euler angles in degrees
            double yawD, pitchD, rollD;
            pitchD = Math.Atan2(2 * ((y * z) + (w * x)), (w * w) - (x * x) - (y * y) + (z * z)) / Math.PI * 180.0;
            yawD = Math.Asin(2 * ((w * y) - (x * z))) / Math.PI * 180.0;
            rollD = Math.Atan2(2 * ((x * y) + (w * z)), (w * w) + (x * x) - (y * y) - (z * z)) / Math.PI * 180.0;

            // clamp the values to a multiple of the specified increment to control the refresh rate
            double increment = FaceRotationIncrementInDegrees;
            pitch = (int)(Math.Floor((pitchD + ((increment / 2.0) * (pitchD > 0 ? 1.0 : -1.0))) / increment) * increment);
            yaw = (int)(Math.Floor((yawD + ((increment / 2.0) * (yawD > 0 ? 1.0 : -1.0))) / increment) * increment);
            roll = (int)(Math.Floor((rollD + ((increment / 2.0) * (rollD > 0 ? 1.0 : -1.0))) / increment) * increment);
        }

        #region Kinect V2 Tab Event Handlers

        private void clearAllFlags()
        {
            colorImageSelected = false;
            depthImageSelected = false;
            infraredImageSelected = false;
            bodyImageSelected = false;
            faceImageSelected = false;
            hdFaceImageSelected = false;

            frontImageSelected = false;
            sideImageSelected = false;
            topImageSelected = false;
        }

        private void hideAllImages()
        {
            colorImage.Visibility = Visibility.Hidden;
            depthImage.Visibility = Visibility.Hidden;
            infraredImage.Visibility = Visibility.Hidden;
            bodyImage.Visibility = Visibility.Hidden;
            faceTrackImage.Visibility = Visibility.Hidden;
            hdFaceCanvas.Visibility = Visibility.Hidden;
        }

        private void buttonColor_Click(object sender, RoutedEventArgs e)
        {
            clearAllFlags();
            colorImageSelected = true;
            bodyImageSelected = true;

            hideAllImages();
            colorImage.Visibility = Visibility.Visible;
            bodyImage.Visibility = Visibility.Visible;

            bodyImage.Source = bodyImageSource;
            bodyImageBiometricTab.Source = bodyImageSource;
            bodyImageKinematicsTab.Source = bodyImageSource;

            //synthesizer.SpeakAsync("Color Image Selected");
            //statusLabel.Content = "Color Image Selected";
        }

        private void buttonDepth_Click(object sender, RoutedEventArgs e)
        {
            clearAllFlags();
            depthImageSelected = true;

            hideAllImages();
            depthImage.Visibility = Visibility.Visible;

            //synthesizer.SpeakAsync("Depth Image Selected");
            //statusLabel.Content = "Depth Image Selected";
        }

        private void buttonInfrared_Click(object sender, RoutedEventArgs e)
        {
            clearAllFlags();
            infraredImageSelected = true;

            hideAllImages();
            infraredImage.Visibility = Visibility.Visible;

            //synthesizer.SpeakAsync("Infrared Image Selected");
            //statusLabel.Content = "Infrared Image Selected";
        }

        private void buttonBody_Click(object sender, RoutedEventArgs e)
        {
            clearAllFlags();
            bodyImageSelected = true;

            hideAllImages();
            bodyImage.Visibility = Visibility.Visible;

            bodyImage.Source = bodyImageSource;
            bodyImageBiometricTab.Source = bodyImageSource;
            bodyImageKinematicsTab.Source = bodyImageSource;
            bodyImageHeartRateTab.Source = bodyImageSource;

            //---------------------------------------------------
            clearAllFlags();
            //colorImageSelected = true;

            colorImageSelected = true; // Will be Black

            // Want these displayed
            bodyImageSelected = true;
            faceImageSelected = true;

            frontImageSelected = false;
            topImageSelected = false;
            sideImageSelected = false;

            hideAllImages();
            colorImage.Visibility = Visibility.Visible;
            bodyImage.Visibility = Visibility.Visible;
            faceTrackImage.Visibility = Visibility.Visible;

            colorImage.Source = colorBitmap;
            bodyImage.Source = bodyImageSource;
            bodyImageBiometricTab.Source = bodyImageSource;
            bodyImageKinematicsTab.Source = bodyImageSource;
            bodyImageHeartRateTab.Source = bodyImageSource;
            faceTrackImage.Source = faceImageSource;
            faceTrackImageBiometricTab.Source = faceImageSource;
            faceTrackImageKinematicsTab.Source = faceImageSource;
            faceTrackImageHeartRateTab.Source = faceImageSource;

            //synthesizer.SpeakAsync("Body Image Selected");
            //statusLabel.Content = "Body Image Selected";
        }

        private void buttonFaceTracking_Click(object sender, RoutedEventArgs e)
        {
            clearAllFlags();
            faceImageSelected = true;
            bodyImageSelected = true;
            colorImageSelected = true;

            //----------------------------------------------

            hideAllImages();
            bodyImage.Visibility = Visibility.Visible;
            faceTrackImage.Visibility = Visibility.Visible;
            colorImage.Visibility = Visibility.Visible;

            //--------------------------------------------

            bodyImage.Source = bodyImageSource;
            bodyImageBiometricTab.Source = bodyImageSource;
            bodyImageKinematicsTab.Source = bodyImageSource;
            faceTrackImage.Source = faceImageSource;
            faceTrackImageBiometricTab.Source = faceImageSource;
            faceTrackImageKinematicsTab.Source = faceImageSource;

            //synthesizer.SpeakAsync("Basic Face Tracking Image Selected");
            //statusLabel.Content = "Basic Face Tracking Image Selected";
        }

        private void buttonHDFaceTracking_Click(object sender, RoutedEventArgs e)
        {
            clearAllFlags();
            hdFaceImageSelected = true;

            hideAllImages();
            bodyImage.Visibility = Visibility.Visible;
            hdFaceCanvas.Visibility = Visibility.Visible;

            bodyImage.Source = bodyImageSource;
            bodyImageBiometricTab.Source = bodyImageSource;
            bodyImageKinematicsTab.Source = bodyImageSource;
            faceTrackImage.Source = faceImageSource;
            faceTrackImageBiometricTab.Source = faceImageSource;
            faceTrackImageKinematicsTab.Source = faceImageSource;

            //synthesizer.SpeakAsync("High Definition Face Tracking Image Selected");
            //statusLabel.Content = "HD Face Tracking Image Selected";
        }


        private void buttonFront_Click(object sender, RoutedEventArgs e)
        {
            clearAllFlags();
            //colorImageSelected = true;

            colorImageSelected = true; // Will be Black

            // Want these displayed
            bodyImageSelected = true;
            faceImageSelected = true;

            frontImageSelected = true;
            topImageSelected = false;
            sideImageSelected = false;

            hideAllImages();
            colorImage.Visibility = Visibility.Visible;
            bodyImage.Visibility = Visibility.Visible;
            faceTrackImage.Visibility = Visibility.Visible;

            colorImage.Source = colorBitmap;
            bodyImage.Source = bodyImageSource;
            bodyImageBiometricTab.Source = bodyImageSource;
            bodyImageKinematicsTab.Source = bodyImageSource;
            bodyImageHeartRateTab.Source = bodyImageSource;
            faceTrackImage.Source = faceImageSource;
            faceTrackImageBiometricTab.Source = faceImageSource;
            faceTrackImageKinematicsTab.Source = faceImageSource;
            faceTrackImageHeartRateTab.Source = faceImageSource;

            //synthesizer.SpeakAsync("Front Image Selected");
            //statusLabel.Content = "Front Image Selected";
        }

        private void buttonTop_Click(object sender, RoutedEventArgs e)
        {
            clearAllFlags();
            //colorImageSelected = true;

            colorImageSelected = false; // Will be Black

            // Want these displayed
            bodyImageSelected = true;
            //faceImageSelected = true;

            frontImageSelected = false;
            topImageSelected = true;
            sideImageSelected = false;

            hideAllImages();
            colorImage.Visibility = Visibility.Hidden;
            bodyImage.Visibility = Visibility.Visible;
            faceTrackImage.Visibility = Visibility.Hidden;

            colorImage.Source = colorBitmap;
            bodyImage.Source = bodyImageSource;
            bodyImageBiometricTab.Source = bodyImageSource;
            bodyImageKinematicsTab.Source = bodyImageSource;
            bodyImageHeartRateTab.Source = bodyImageSource;
            faceTrackImage.Source = faceImageSource;
            faceTrackImageBiometricTab.Source = faceImageSource;
            faceTrackImageKinematicsTab.Source = faceImageSource;
            faceTrackImageHeartRateTab.Source = faceImageSource;

            //synthesizer.SpeakAsync("Top Image Selected");
            //statusLabel.Content = "Top Image Selected";
        }

        private void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            clearAllFlags();
            //colorImageSelected = true;

            colorImageSelected = false; // Will be Black

            // Want these displayed
            bodyImageSelected = true;
            //faceImageSelected = true;

            frontImageSelected = false;
            topImageSelected = false;
            sideImageSelected = true;

            hideAllImages();
            colorImage.Visibility = Visibility.Hidden;
            bodyImage.Visibility = Visibility.Visible;
            faceTrackImage.Visibility = Visibility.Hidden;

            colorImage.Source = colorBitmap;
            bodyImage.Source = bodyImageSource;

            faceTrackImage.Source = faceImageSource;
            faceTrackImageBiometricTab.Source = faceImageSource;
            faceTrackImageKinematicsTab.Source = faceImageSource;
            faceTrackImageHeartRateTab.Source = faceImageSource;

            //synthesizer.SpeakAsync("Left Image Selected");
            //statusLabel.Content = "Left Image Selected";
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        // A function to set the Text value of the txtLog TextBox control
        private void SetLogText(String text)
        {
            // This if statement ensures that this code executes in the main thread
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    //txtLog.Text = text += txtLog.Text;

                    //gauge.Value = new Random().Next(50, 250);
                });
                return;
            }

            // Code beyond here is running in the main thread
            //txtLog.Text = text += txtLog.Text;

            //gauge.Value = new Random().Next(50, 250);
        }

        // A function to set the value of a LiveCharts Gauge control
        private void SetGaugeValue(double value)
        {
            // This if statement ensures that this code executes in the main thread
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    gauge0.Value = value;
                    gauge1.Value = value;
                    gauge2.Value = value;
                    gauge3.Value = value;
                    gauge4.Value = value;
                    gauge5.Value = value;
                });
                return;
            }

            // Code beyond here is running in the main thread
            gauge0.Value = value;
            gauge1.Value = value;
            gauge2.Value = value;
            gauge3.Value = value;
            gauge4.Value = value;
            gauge5.Value = value;
        }

        #region Arduino Bluetooth Sensor Controls Event Handlers

        private void btnConnectArduino_Click(object sender, RoutedEventArgs e)
        {
            ConnectBluetoothDevice();

            /*
            if (!serialPort.IsOpen)
            {
                ConnectBluetoothDevice();

                serialPort.Open();
            }
             */

            running = true;

            btnConnect.IsEnabled = false;
            btnLedOn.IsEnabled = true;
            btnLedOff.IsEnabled = false;
            btnDisconnect.IsEnabled = true;

            arduinoStatusLed.SetPowerOn(); // White
        }

        private void btnDisconnectASrduino_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                // Turn off the LED
                serialPort.WriteLine("d");

                serialPort.Close();

                //lineSeries8.Values.Clear();


            }

            btnConnect.IsEnabled = true;
            btnLedOn.IsEnabled = false;
            btnLedOff.IsEnabled = false;
            btnDisconnect.IsEnabled = false;

            arduinoValue = 0.0;
            myLineSeriesArduino.Values.Clear();

            myGaugeArduino.Value = 0;

            arduinoStatusLed.SetOff();
        }

        private void btnLedOn_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
                serialPort.WriteLine("a");

            //btnConnect.IsEnabled = true;
            btnLedOn.IsEnabled = false;
            btnLedOff.IsEnabled = true;
            //btnDisconnect.IsEnabled = false;
        }

        private void btnLedOff_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
                serialPort.WriteLine("d");

            //btnConnect.IsEnabled = true;
            btnLedOn.IsEnabled = true;
            btnLedOff.IsEnabled = false;
            //btnDisconnect.IsEnabled = false;
        }

        // Connect to the Arduino
        private void ConnectBluetoothDevice()
        {
            serialPort = new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One);
            serialPort.Handshake = Handshake.None;

            serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);

            serialPort.ReadTimeout = 800; // ms
            serialPort.WriteTimeout = 800;

            serialPort.Open();

            running = true;

            btnConnect.IsEnabled = false;
            btnLedOn.IsEnabled = true;
            btnLedOff.IsEnabled = false;
            btnDisconnect.IsEnabled = true;
        }

        //----------------------------------------------------------------------------------------------
        // Now create the "sp_DataReceived" method that will be executed when
        // data is received through the SerialPort.
        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(30);

            try
            {
                int len = serialPort.BytesToRead;

                if (running && (len > 0))
                {
                    string data = serialPort.ReadLine();

                    //MessageBox.Show("len: " + len + "["+ data + "]");

                    data = data.Trim();

                    // Check the reply from the Arduino
                    if ((data == "ON") || (data == "OFF"))
                    {
                        // Use Invoke() to modify the UI
                        Dispatcher.Invoke((Action)(() =>
                        {
                            // You can modify the UI here...
                            try
                            {
                                txtBlock1.Text = "The LED is " + data;
                            }
                            catch (Exception ex1)
                            {
                                // Ignore
                                //MessageBox.Show("Exception ex1: " + ex1.Message);
                            }
                        }
                        ));
                    }
                    else
                    {
                        // Use Invoke() to modify the UI
                        Dispatcher.Invoke((Action)(() =>
                        {
                            // You can modify the UI here...
                            try
                            {
                                //label2.Content = "Analog signal value: " + data;
                                //progressBar1.Value = int.Parse(data);
                                //myGauge0.Value = int.Parse(data);

                                // Scale the sensor value to 0.0 ... 1.0 for charting
                                arduinoValue = double.Parse(data) / 1024;

                                // Update the chart LineSeries values with the latest samples
                                if (myLineSeriesArduino.Values.Count < N)
                                {
                                    myLineSeriesArduino.Values.Add(arduinoValue);
                                }
                                else
                                {
                                    myLineSeriesArduino.Values.RemoveAt(0);
                                    myLineSeriesArduino.Values.Add(arduinoValue);
                                }

                                YFormatterArduino = value => value.ToString("F3");

                                myGaugeArduino.Value = Math.Round(arduinoValue * 1000);

                                txtBlock1.Text = String.Format("Sensor Value: {0:0.000}", arduinoValue);

                                arduinoStatusLed.SetOn();
                            }
                            catch (Exception ex2)
                            {
                                // Ignore
                                //MessageBox.Show("sp_DataReceived Exception ex2: " + ex2.Message);
                            }
                        }
                        ));
                    }
                }
            }
            catch (Exception ex3)
            {
                // Ignore the exception
                //MessageBox.Show("sp_DataReceived Exception ex3: " + ex3.Message);
            }
        }

        public Func<double, string> YFormatterArduino { get; set; }

        #endregion

        #region StatusPage Event Handlers

        private void btnStartRecording_Click(object sender, RoutedEventArgs e)
        {
            btnStartRecording.IsEnabled = false;
            btnStopRecording.IsEnabled = true;
        }

        private void btnStopRecording_Click(object sender, RoutedEventArgs e)
        {
            btnStartRecording.IsEnabled = true;
            btnStopRecording.IsEnabled = false;
        }

        #endregion

        #region Settings Tab Control Event Handlers

        private void buttonStartRecording_Click(object sender, RoutedEventArgs e)
        {
            nowRecording = !nowRecording;

            if (nowRecording)
            {
                buttonStartRecording.Background = System.Windows.Media.Brushes.Red;
                buttonStartRecording.Content = "Stop Recording";
                statusLabel.Content = "Recording Body Data";
                synthesizer.SpeakAsync("Recording Started");
            }
            else
            {
                buttonStartRecording.Background = System.Windows.Media.Brushes.Green;
                buttonStartRecording.Content = "Start Recording";
                statusLabel.Content = "Running";
                synthesizer.SpeakAsync("Recording Stopped");
            }
        }

        #endregion

        #region Kinematics Tab Control Event Handlers

        private void btnStartKinematics_Click(object sender, RoutedEventArgs e)
        {
            btnStartKinematics.IsEnabled = false;
            btnStopKinematics.IsEnabled = true;

            stopwatchKinematics.Reset();
            stopwatchKinematics.Start();

            sampleCounterKinematics = 0;

            /*
            // Create a new thread and attach it's work handler
            chartThread = new Thread(Update);

            chartThread.Start();
            */

            // Using a Timer at 33 ms or about 30Hz, and the function UpdateProperty()
            // To update the "simulated" signals.
            //
            // Non-simulated signals are updated by their own event handlers asychronously
            timerKinematics = new Timer(UpdateKinematicProperty, null, 0, 100);

            tbStatusKinematics.Text = "Timer timerKinematics started.";

            kinematicsStatusLed.SetOn();
        }

        private void btnStopKinematics_Click(object sender, RoutedEventArgs e)
        {
            btnStartKinematics.IsEnabled = true;
            btnStopKinematics.IsEnabled = false;

            timerKinematics.Dispose();

            stopwatchKinematics.Stop();

            try
            {
                long elapsedTime = stopwatchKinematics.ElapsedMilliseconds / 1000;

                //double sps = sampleCounter / elapsedTime / 6;

                tbStatusKinematics.Text = "Per Chart Samples Per Second: " + String.Format("{0:00.00}", sampleCounterKinematics / 6 / (double)elapsedTime);
            }
            catch (Exception ex)
            {
                tbStatusKinematics.Text = ex.Message;
            }

            stopwatchKinematics.Reset();

            kinematicsStatusLed.SetOff();
        }

        #endregion
    }
}
