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
using FaceTrackingBasics;

using Microsoft.Expression.Encoder.Devices;
using System.Collections.ObjectModel;
using Microsoft.Kinect.Toolkit.FaceTracking;
using Microsoft.Kinect.Toolkit;

// Ref: https://www.codeproject.com/Articles/285964/WPF-Webcam-Control

// Ref: https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread?f1url=https%3A%2F%2Fmsdn.microsoft.com%2Fquery%2Fdev15.query%3FappId%3DDev15IDEF1%26l%3DEN-US%26k%3Dk(System.Threading.Thread);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.6.1);k(DevLang-csharp)%26rd%3Dtrue%26f%3D255%26MSPPError%3D-2147217396&view=netframework-4.7.2

// Ref: https://lvcharts.net/App/examples/v1/wpf/Basic%20Line%20Chart

// Ref: https://stackoverflow.com/questions/2006055/implementing-a-webcam-on-a-wpf-app-using-aforge-net

// Ref: https://github.com/jrowberg/bglib

namespace WpfDashboardProject01
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
        public SeriesCollection mySeriesCollectionArduino { get; set; }
        public SeriesCollection mySeriesCollectionKinectV1 { get; set; }

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

        public ChartValues<double> myValuesArduino = null;
        public LineSeries myLineSeriesArduino = null;

        public ChartValues<double> myValuesKinectV1 = null;
        public LineSeries myLineSeriesKinectV1 = null;

        #endregion       

        #region Thread/Timer declarations

        int period = 1000;

        private System.Threading.Timer timer;

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

        #region Kinect V1 declarations

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        private readonly KinectSensorChooser sensorChooser = new KinectSensorChooser();

        // Depth Image items ----------------------------------------------

        /// <summary>
        /// Bitmap that will hold color information
        /// </summary>
        private WriteableBitmap depthColorBitmap;

        /// <summary>
        /// Intermediate storage for the depth data received from the camera
        /// </summary>
        private DepthImagePixel[] depthPixels;

        /// <summary>
        /// Intermediate storage for the depth data converted to color
        /// </summary>
        private byte[] depthColorPixels;

        // Sleleton Image Items --------------------------------------------

        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 10;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 20;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point
        /// </summary>
        private readonly System.Windows.Media.Brush centerPointBrush = System.Windows.Media.Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly System.Windows.Media.Brush trackedJointBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly System.Windows.Media.Brush inferredJointBrush = System.Windows.Media.Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        private readonly System.Windows.Media.Pen trackedBonePen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Green, 10);

        private int colorIndex = 0;

        private System.Windows.Media.Color[] colors =
            {
            System.Windows.Media.Colors.Red,
            System.Windows.Media.Colors.Green,
            System.Windows.Media.Colors.Blue,
            System.Windows.Media.Colors.Yellow,
            System.Windows.Media.Colors.Magenta,
            System.Windows.Media.Colors.Orange
        };

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly System.Windows.Media.Pen inferredBonePen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Gray, 10);

        /// <summary>
        /// Drawing group for skeleton rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display the skeletons
        /// </summary>
        private DrawingImage skeletonImageSource;

        //----------------------------------------------------------------------
        bool drawingNormalView = true;
        bool drawingFrontJoints = false; // Draw x-y values, mapped to camera image coordinates.
        bool drawingLeftJoints = false;  // Draw y-z values, scaled to 0 - 4 meters, no camera used
        bool drawingTopJoints = false;

        double[] bodyDistance = new double[6];

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

        Stopwatch stopwatch = null;

        int sampleCounter = 0;

        public MainWindow()
        {
            InitializeComponent();

            #region Setup LiveChart components

            // Setup Chart 0-------------------------------------------------------------
            Axis myYAxis0 = new Axis();
            myYAxis0.MinValue = 0.0;
            myYAxis0.MaxValue = 150;
            myYAxis0.Title = "Heart Rate";
            myYAxis0.FontSize = 10;
            myYAxis0.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis0.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis0 = new Axis();
            myXAxis0.MinValue = 0;
            myXAxis0.MaxValue = (N + 1);
            myXAxis0.Title = "Sample";
            myXAxis0.FontSize = 10;
            myXAxis0.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis0.Foreground = System.Windows.Media.Brushes.Black;

            MyChart0.AxisY.Add(myYAxis0);
            MyChart0.AxisX.Add(myXAxis0);
            MyChart0.DisableAnimations = true;

            myValues0 = new ChartValues<double>();

            myLineSeries0 = new LineSeries();

            myLineSeries0.Title = "Heart Rate";
            myLineSeries0.Values = myValues0;
            myLineSeries0.StrokeThickness = 1;
            myLineSeries0.Stroke = new SolidColorBrush(Colors.Red);
            myLineSeries0.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries0.DataLabels = false;
           // myLineSeries0.PointGeometrySize = 0;
            myLineSeries0.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries0.PointGeometry = null;

            mySeriesCollection0 = new SeriesCollection();
            mySeriesCollection0.Add(myLineSeries0);

            // Setup Chart 1-------------------------------------------------------------
            Axis myYAxis1 = new Axis();
            myYAxis1.MinValue = 0.0;
            myYAxis1.MaxValue = 1.0;
            myYAxis1.Title = "Signal 1";
            myYAxis1.FontSize = 10;
            myYAxis1.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis1.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis1 = new Axis();
            myXAxis1.MinValue = 0;
            myXAxis1.MaxValue = (N + 1);
            myXAxis1.Title = "Sample";
            myXAxis1.FontSize = 10;
            myXAxis1.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis1.Foreground = System.Windows.Media.Brushes.Black;

            MyChart1.AxisY.Add(myYAxis1);
            MyChart1.AxisX.Add(myXAxis1);
            MyChart1.DisableAnimations = true;

            myValues1 = new ChartValues<double>();

            myLineSeries1 = new LineSeries();

            myLineSeries1.Title = "Series 1";
            myLineSeries1.Values = myValues1;
            myLineSeries1.StrokeThickness = 1;
            myLineSeries1.Stroke = new SolidColorBrush(Colors.Green);
            myLineSeries1.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries1.DataLabels = false;
            //myLineSeries1.PointGeometrySize = 0;
            myLineSeries1.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries1.PointGeometry = null;

            mySeriesCollection1 = new SeriesCollection();
            mySeriesCollection1.Add(myLineSeries1);

            // Setup Chart 2-------------------------------------------------------------
            Axis myYAxis2 = new Axis();
            myYAxis2.MinValue = 0.0;
            myYAxis2.MaxValue = 1.0;
            myYAxis2.Title = "Signal 2";
            myYAxis2.FontSize = 10;
            myYAxis2.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis2.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis2 = new Axis();
            myXAxis2.MinValue = 0;
            myXAxis2.MaxValue = (N + 1);
            myXAxis2.Title = "Sample";
            myXAxis2.FontSize = 10;
            myXAxis2.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis2.Foreground = System.Windows.Media.Brushes.Black;

            MyChart2.AxisY.Add(myYAxis2);
            MyChart2.AxisX.Add(myXAxis2);
            MyChart2.DisableAnimations = true;

            myValues2 = new ChartValues<double>();

            myLineSeries2 = new LineSeries();

            myLineSeries2.Title = "Series 2";
            myLineSeries2.Values = myValues2;
            myLineSeries2.StrokeThickness = 1;
            myLineSeries2.Stroke = new SolidColorBrush(Colors.Blue);
            myLineSeries2.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries2.DataLabels = false;
            //myLineSeries2.PointGeometrySize = 0;
            myLineSeries2.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries2.PointGeometry = null;

            mySeriesCollection2 = new SeriesCollection();
            mySeriesCollection2.Add(myLineSeries2);

            // Setup Chart 3-------------------------------------------------------------
            Axis myYAxis3 = new Axis();
            myYAxis3.MinValue = 0.0;
            myYAxis3.MaxValue = 1.0;
            myYAxis3.Title = "Signal 3";
            myYAxis3.FontSize = 10;
            myYAxis3.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis3.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis3 = new Axis();
            myXAxis3.MinValue = 0;
            myXAxis3.MaxValue = (N + 1);
            myXAxis3.Title = "Sample";
            myXAxis3.FontSize = 10;
            myXAxis3.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis3.Foreground = System.Windows.Media.Brushes.Black;

            MyChart3.AxisY.Add(myYAxis3);
            MyChart3.AxisX.Add(myXAxis3);
            MyChart3.DisableAnimations = true;

            myValues3 = new ChartValues<double>();

            myLineSeries3 = new LineSeries();

            myLineSeries3.Title = "Series 2";
            myLineSeries3.Values = myValues2;
            myLineSeries3.StrokeThickness = 1;
            myLineSeries3.Stroke = new SolidColorBrush(Colors.Magenta);
            myLineSeries3.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries3.DataLabels = false;
            //myLineSeries2.PointGeometrySize = 0;
            myLineSeries3.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries3.PointGeometry = null;

            mySeriesCollection3 = new SeriesCollection();
            mySeriesCollection3.Add(myLineSeries3);

            // Setup Chart 4-------------------------------------------------------------
            Axis myYAxis4 = new Axis();
            myYAxis4.MinValue = 0.0;
            myYAxis4.MaxValue = 1.0;
            myYAxis4.Title = "Signal 4";
            myYAxis4.FontSize = 10;
            myYAxis4.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis4.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis4 = new Axis();
            myXAxis4.MinValue = 0;
            myXAxis4.MaxValue = (N + 1);
            myXAxis4.Title = "Sample";
            myXAxis4.FontSize = 10;
            myXAxis4.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis4.Foreground = System.Windows.Media.Brushes.Black;

            MyChart4.AxisY.Add(myYAxis4);
            MyChart4.AxisX.Add(myXAxis4);
            MyChart4.DisableAnimations = true;

            myValues4 = new ChartValues<double>();

            myLineSeries4 = new LineSeries();

            myLineSeries4.Title = "Series 4";
            myLineSeries4.Values = myValues4;
            myLineSeries4.StrokeThickness = 1;
            myLineSeries4.Stroke = new SolidColorBrush(Colors.Orange);
            myLineSeries4.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries4.DataLabels = false;
            //myLineSeries4.PointGeometrySize = 0;
            myLineSeries4.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries4.PointGeometry = null;

            mySeriesCollection4 = new SeriesCollection();
            mySeriesCollection4.Add(myLineSeries4);

            // Setup Chart 5-------------------------------------------------------------
            Axis myYAxis5 = new Axis();
            myYAxis5.MinValue = 0.0;
            myYAxis5.MaxValue = 1.0;
            myYAxis5.Title = "Signal 5";
            myYAxis5.FontSize = 10;
            myYAxis5.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxis5.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxis5 = new Axis();
            myXAxis5.MinValue = 0;
            myXAxis5.MaxValue = (N + 1);
            myXAxis5.Title = "Sample";
            myXAxis5.FontSize = 10;
            myXAxis5.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxis5.Foreground = System.Windows.Media.Brushes.Black;

            MyChart5.AxisY.Add(myYAxis5);
            MyChart5.AxisX.Add(myXAxis5);
            MyChart5.DisableAnimations = true;

            myValues5 = new ChartValues<double>();

            myLineSeries5 = new LineSeries();

            myLineSeries5.Title = "Arduino";
            myLineSeries5.Values = myValues5;
            myLineSeries5.StrokeThickness = 2;
            myLineSeries5.Stroke = new SolidColorBrush(Colors.Black);
            myLineSeries5.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeries5.DataLabels = false;
            myLineSeries5.PointGeometrySize = 5;
            myLineSeries5.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeries5.PointGeometry = null;

            mySeriesCollection5 = new SeriesCollection();
            mySeriesCollection5.Add(myLineSeries5);

            // Setup Arduino tab chart -----------------------------------------------
            Axis myYAxisArduino = new Axis();
            myYAxisArduino.MinValue = 0.0;
            myYAxisArduino.MaxValue = 1.0;
            myYAxisArduino.Title = "Sensor Value";
            myYAxisArduino.FontSize = 10;
            myYAxisArduino.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisArduino.Foreground = System.Windows.Media.Brushes.Black;


            Axis myXAxisArduino = new Axis();
            myXAxisArduino.MinValue = 0;
            myXAxisArduino.MaxValue = (N + 1);
            myXAxisArduino.Title = "Sample";
            myXAxisArduino.FontSize = 10;
            myXAxisArduino.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisArduino.Foreground = System.Windows.Media.Brushes.Black;

            MyChartArduino.AxisY.Add(myYAxisArduino);
            MyChartArduino.AxisX.Add(myXAxisArduino);
            MyChartArduino.DisableAnimations = true;

            myValuesArduino = new ChartValues<double>();

            myLineSeriesArduino = new LineSeries();

            myLineSeriesArduino.Title = "Series 5";
            myLineSeriesArduino.Values = myValues5;
            myLineSeriesArduino.StrokeThickness = 1;
            myLineSeriesArduino.Stroke = new SolidColorBrush(Colors.Cyan);
            myLineSeriesArduino.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesArduino.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesArduino.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesArduino.PointGeometry = null;

            mySeriesCollectionArduino = new SeriesCollection();
            mySeriesCollectionArduino.Add(myLineSeriesArduino);

            myGaugeArduino.Value = 0;

            // Setup Kinect V1 tab chart -----------------------------------------------
            Axis myYAxisKinectV1 = new Axis();
            myYAxisKinectV1.MinValue = 0.0;
            myYAxisKinectV1.MaxValue = 4.0;
            myYAxisKinectV1.Title = "Distance (m)";
            myYAxisKinectV1.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myYAxisKinectV1.Foreground = System.Windows.Media.Brushes.Black;

            Axis myXAxisKinectV1 = new Axis();
            myXAxisKinectV1.MinValue = 0;
            myXAxisKinectV1.MaxValue = (N + 1);
            myXAxisKinectV1.Title = "Sample";
            myXAxisKinectV1.Separator.Stroke = System.Windows.Media.Brushes.Black;
            myXAxisKinectV1.Foreground = System.Windows.Media.Brushes.Black;

            MyChartKinectV1.AxisY.Add(myYAxisKinectV1);
            MyChartKinectV1.AxisX.Add(myXAxisKinectV1);
            MyChartKinectV1.DisableAnimations = true;
            //MyChartKinectV1.Foreground = System.Windows.Media.Brushes.Black;

            myValuesKinectV1 = new ChartValues<double>();

            myLineSeriesKinectV1 = new LineSeries();

            myLineSeriesKinectV1.Title = "Position";
            myLineSeriesKinectV1.Values = myValuesKinectV1;
            myLineSeriesKinectV1.StrokeThickness = 1;
            myLineSeriesKinectV1.Stroke = new SolidColorBrush(Colors.Red);
            myLineSeriesKinectV1.LineSmoothness = 0; // 0: straight lines, 1: really smooth lines
            myLineSeriesKinectV1.DataLabels = false;
            //myLineSeries5.PointGeometrySize = 0;
            myLineSeriesKinectV1.Fill = new SolidColorBrush(Colors.Transparent);
            myLineSeriesKinectV1.PointGeometry = null;

            mySeriesCollectionKinectV1 = new SeriesCollection();

            mySeriesCollectionKinectV1.Add(myLineSeriesKinectV1);

            #endregion

            #region Initialize Chart Values

            double[] values = new double[N];

            for (int k = 0; k < 6; k++)
            {
                double[] freq = { 1, 2, 3, 4, 5, 6 };

                for (int j = 0; j < N; j++)
                {
                    //values[j] = 10.0 + 10.0 * Math.Sin(2.0 * Math.PI * freq[k] * (j * 1.0 / N));

                    values[j] = 0.0;

                    //if (k == 5) values[j] = 0.0;
                }

                switch (k)
                {
                    case 0:
                        myValues0.Clear();
                        myValues0.AddRange(values);
                        break;
                    case 1:
                        myValues1.Clear();
                        myValues1.AddRange(values);
                        break;
                    case 2:
                        myValues2.Clear();
                        myValues2.AddRange(values);
                        break;
                    case 3:
                        myValues3.Clear();
                        myValues3.AddRange(values);
                        break;
                    case 4:
                        myValues4.Clear();
                        myValues4.AddRange(values);
                        break;
                    case 5:
                        myValues5.Clear();
                        myValues5.AddRange(values);                        
                        break;
                }
            }

            #endregion

            #region Setup UI Components

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;

            stopwatch = new Stopwatch();

            stopwatch.Start();

            // Webcam Related
            frameHolder.Visibility = Visibility.Hidden;

            LocalWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            LocalWebCam = new VideoCaptureDevice(LocalWebCamsCollection[0].MonikerString);
            LocalWebCam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);

            // Status page items
            biometricsStatusLed.SetPowerOff();
            heartRateStatusLed.SetPowerOff();
            webCamStatusLed.SetPowerOff();
            kinectV1StatusLed.SetPowerOff();
            kinectV2StatusLed.SetPowerOff();
            arduinoStatusLed.SetPowerOff();

            btnStartRecording.IsEnabled = true;
            btnStopRecording.IsEnabled = false;

            #endregion

            DataContext = this;
        }        

        #region Biometric Tab Event Handlers

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;

            stopwatch.Reset();
            stopwatch.Start();

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
            timer = new Timer(UpdateProperty, null, 0, 33);

            tbStatus.Text = "Timer timer started.";
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            writer.Close();

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;

            //chartThread.Abort();

            timer.Dispose();

            stopwatch.Stop();

            try
            {
                long elapsedTime = stopwatch.ElapsedMilliseconds / 1000;

                //double sps = sampleCounter / elapsedTime / 6;

                tbStatus.Text = "Per Chart Samples Per Second: " + String.Format("{0:00.00}", sampleCounter / 5 / (double)elapsedTime);
            }
            catch (Exception ex)
            {
                tbStatus.Text = ex.Message;
            }

            stopwatch.Reset();
        }

        

        private double Noise()
        {
            return -0.05 + 0.1 * rnd.NextDouble();
        }

        private void UpdateProperty(object state)
        {            
            // Update the user interface
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    long startTime = stopwatch.ElapsedMilliseconds;

                    // These values are written out at each timer time step.
                    //writer.WriteLine("Time: {0} Sample Counter: {1} Arduino value: {2}", 
                    //    stopwatch.Elapsed.ToString(), sampleCounter, arduinoValue);

                    // For now, just rotate the values stored for each signal 0-4
                    //---------------------------------------
                    /*
                    if(myValues0.Count < N)
                    {
                        myValues0.Add(0.1);
                    }
                    else
                    {
                        myValues0.RemoveAt(0);
                        myValues0.Add(0.1);
                    }
                    */
                    //---------------------------------------
                    if (myValues1.Count < N)
                    {
                        myValues1.Add(0.2 + Noise());
                    }
                    else
                    {
                        myValues1.RemoveAt(0);
                        myValues1.Add(0.2 + Noise());
                    }
                    //---------------------------------------
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

                    // Calculate the time required to generate the simulated data
                    sampleCounter += 5;

                    double elapsedTimeInSec = stopwatch.ElapsedMilliseconds / 1000.0;

                    double sps = sampleCounter / elapsedTimeInSec;

                    tbStatus.Text = String.Format("Samples: {0}, Seconds: {1:0.000}, SPS: {2:00.000}",
                        sampleCounter, elapsedTimeInSec, sps);

                    //tbStatus.Text = "Arduino value: " + String.Format("{0:00.00}", arduinoValue);
                }
            );
        }



        // Function Update() is not currently used

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

                                    /*
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
                                    */

                                    //tbStatus.Text = "Samples Generated: " + sampleCounter;

                                    long elapsedTime = stopwatch.ElapsedMilliseconds / 1000;

                                    //double sps = sampleCounter / elapsedTime / 6;

                                    tbStatus.Text = "Chart Samples Per Second: " + String.Format("{0:00.00}", sampleCounter / 6 / (double)elapsedTime);

                                    //tbStatus.Text = "Arduino: " + arduinoValue;
                                }
                                  );
                }

                /*
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                (ThreadStart)delegate ()
                                {
                                    tbStatus.Text = "Task complete.";
                                }
                                  );
                                  */
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

            InitializeKinect();

            /*
            // Initialize Kinect V1
            sensor = KinectSensor.KinectSensors.FirstOrDefault();

            if (sensor == null)
            {
                MessageBox.Show("No Kinect Sensor Available");
            }
            else
            {
                // Initialization for color stream

                // Turn on the color stream to receive color frames
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Allocate space to put the pixels we'll receive
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];

                // This is the bitmap we'll display on-screen
                this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Set the image we display to point to the bitmap where we'll put the image data
                colorImage.Source = this.colorBitmap;

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.ColorFrameReady += this.SensorColorFrameReady;

                // Initialization for skeleton stream

                // Create the drawing group we'll use for drawing
                this.drawingGroup = new DrawingGroup();

                // Create an image source that we can use in our image control
                this.imageSource = new DrawingImage(this.drawingGroup);

                // Display the drawing using our image control
                skeletonImage.Source = this.imageSource;

                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // Start the sensor!
                kinectV1StatusLed.SetOff();
                try
                {
                    this.sensor.Start();

                    kinectV1StatusLed.SetOn();
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            */
            #endregion
        }


        private bool InitializeKinect()
        {
            try
            {
                sensor = KinectSensor.KinectSensors.FirstOrDefault();

                if (sensor != null)
                {
                    // Enable sensor streams
                    sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    sensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                    sensor.SkeletonStream.Enable();

                    sensor.DepthStream.Range = DepthRange.Default;

                    // Attach sensor event handlers
                    //sensor.ColorFrameReady += Sensor_ColorFrameReady;
                    //sensor.DepthFrameReady += Sensor_DepthFrameReady;
                    //sensor.SkeletonFrameReady += Sensor_SkeletonFrameReady;
                    sensor.AllFramesReady += Sensor_AllFramesReady;
                    
                    // Color Image Items --------------------------------------------------------------


                    // Depth Image Items----------------------------------------------------------------
                    // Allocate space to put the depth pixels we'll receive
                    this.depthPixels = new DepthImagePixel[this.sensor.DepthStream.FramePixelDataLength];

                    // Allocate space to put the color pixels we'll create
                    this.depthColorPixels = new byte[this.sensor.DepthStream.FramePixelDataLength * sizeof(int)];

                    // This is the bitmap we'll display on-screen
                    this.depthColorBitmap = new WriteableBitmap(this.sensor.DepthStream.FrameWidth,
                        this.sensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                    // Set the image we display to point to the bitmap where we'll put the depth image data
                    //this.depthImage.Source = this.depthColorBitmap;

                    // Skeleton Image Items -------------------------------------------------------------
                    // Create the drawing group we'll use for drawing
                    this.drawingGroup = new DrawingGroup();

                    // Create an image source that we can use in our image control
                    this.skeletonImageSource = new DrawingImage(this.drawingGroup);

                    // Display the drawing using our image control
                    skeletonImage.Source = this.skeletonImageSource;

                    var faceTrackingViewerBinding = new Binding("Kinect") { Source = sensorChooser };

                    faceTrackingViewer.SetBinding(FaceTrackingViewer.KinectProperty, faceTrackingViewerBinding);

                    // Attach an event handler to the sensorChooser
                    sensorChooser.KinectChanged += SensorChooser_KinectChanged;

                    // Start the sensorChooser, which in turn starts the Kinect sensor
                    sensorChooser.Start();
                }
                else if (sensor == null)
                {
                    MessageBox.Show("sensor was null");

                    return false;
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return true;
        }

        private void SensorChooser_KinectChanged(object sender, KinectChangedEventArgs kinectChangedEventArgs)
        {
            KinectSensor oldSensor = kinectChangedEventArgs.OldSensor;
            KinectSensor newSensor = kinectChangedEventArgs.NewSensor;

            if (oldSensor != null)
            {
                oldSensor.AllFramesReady -= Sensor_AllFramesReady;
                oldSensor.ColorStream.Disable();
                oldSensor.DepthStream.Disable();
                oldSensor.DepthStream.Range = DepthRange.Default;
                oldSensor.SkeletonStream.Disable();
                oldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                oldSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
            }

            if (newSensor != null)
            {
                try
                {
                    newSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    newSensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                    try
                    {
                        // This will throw on non Kinect For Windows devices.
                        newSensor.DepthStream.Range = DepthRange.Default; // Was .Near;
                        newSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    }
                    catch (InvalidOperationException)
                    {
                        newSensor.DepthStream.Range = DepthRange.Default;
                        newSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }

                    newSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default; // Was .Seated;
                    newSensor.SkeletonStream.Enable();
                    newSensor.AllFramesReady += Sensor_AllFramesReady;

                    kinectV1StatusLed.SetOn();
                }
                catch (InvalidOperationException)
                {
                    // This exception can be thrown when we are trying to
                    // enable streams on a device that has gone away.  This
                    // can occur, say, in app shutdown scenarios when the sensor
                    // goes away between the time it changed status and the
                    // time we get the sensor changed notification.
                    //
                    // Behavior here is to just eat the exception and assume
                    // another notification will come along if a sensor
                    // comes back.
                }
            }
        }

        private void StartKinectSensor()
        {
            if (sensor != null)
            {
                sensor.Start();
            }
        }

        private void StopKinectSensor()
        {
            if ((sensor != null) && (sensor.IsRunning == true))
            {
                sensor.Stop();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            chartThread.Abort();

            timer.Dispose();

            LocalWebCam.Stop();

            // Kinect V1 sensor
            StopKinectSensor();
            sensor.Dispose();

            sensorChooser.Stop();
            
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

        // A function to set the Text value of the txtLog TextBox control
        private void SetLogText(String text)
        {
            // This if statement ensures that this code executes in the main thread
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    txtLog.Text = text += txtLog.Text;
                    //gauge.Value = new Random().Next(50, 250);
                });
                return;
            }

            // Code beyond here is running in the main thread
            txtLog.Text = text += txtLog.Text;
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

        #endregion

        #region Kinect V1 Event Handlers

        private void Sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            //throw new NotImplementedException();
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    depthFrame.CopyDepthImagePixelDataTo(this.depthPixels);

                    // Get the min and max reliable depth for the current frame
                    int minDepth = depthFrame.MinDepth;
                    int maxDepth = depthFrame.MaxDepth;

                    // Convert the depth to RGB
                    int colorPixelIndex = 0;
                    for (int i = 0; i < this.depthPixels.Length; ++i)
                    {
                        // Get the depth for this pixel
                        short depth = depthPixels[i].Depth;

                        // To convert to a byte, we're discarding the most-significant
                        // rather than least-significant bits.
                        // We're preserving detail, although the intensity will "wrap."
                        // Values outside the reliable depth range are mapped to 0 (black).

                        // Note: Using conditionals in this loop could degrade performance.
                        // Consider using a lookup table instead when writing production code.
                        // See the KinectDepthViewer class used by the KinectExplorer sample
                        // for a lookup table example.
                        byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                        // Write out blue byte
                        this.depthColorPixels[colorPixelIndex++] = intensity;

                        // Write out green byte
                        this.depthColorPixels[colorPixelIndex++] = intensity;

                        // Write out red byte                        
                        this.depthColorPixels[colorPixelIndex++] = intensity;

                        // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                        // If we were outputting BGRA, we would write alpha here.
                        ++colorPixelIndex;
                    }

                    // Write the pixel data into our bitmap
                    this.depthColorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.depthColorBitmap.PixelWidth, this.depthColorBitmap.PixelHeight),
                        this.depthColorPixels,
                        this.depthColorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }

            ColorImageFrame colorImageFrame = e.OpenColorImageFrame();

            //colorImage.Source = ImageToBitmap(colorImageFrame);
            //colorImage2.Source = ImageToBitmap(colorImageFrame);

            if (colorImageFrame != null)
            {

                BitmapSource bitmapSource = ImageToBitmap(colorImageFrame);

                colorImageFrame.Dispose();

                //colorImage.Source = bitmapSource;
                colorImage2.Source = bitmapSource;
            }


            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    // Create an array to store the Skeleton data
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];

                    // Copy the data from the frame to the array
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            // Here the data in the array named skeletons is available
            // Let's plot the distance from the sensor for the first skeleton
            int counter = 0;
            foreach (Skeleton skel in skeletons)
            {
                if ((skel.TrackingState == SkeletonTrackingState.Tracked) && (counter == 0))
                {
                    // Use Invoke() to modify the UI
                    Dispatcher.Invoke((Action)(() =>
                    {
                        // You can modify the UI here...
                        try
                        {
                            double distance = Math.Sqrt(Math.Pow(skel.Position.X, 2) +
                                Math.Pow(skel.Position.Y, 2) + Math.Pow(skel.Position.Z, 2));

                            // Update the chart LineSeries values with the latest samples
                            if (myLineSeriesKinectV1.Values.Count < N)
                            {
                                myLineSeriesKinectV1.Values.Add(distance);
                            }
                            else
                            {
                                myLineSeriesKinectV1.Values.RemoveAt(0);
                                myLineSeriesKinectV1.Values.Add(distance);
                            }
                        }
                        catch (Exception ex2)
                        {
                            // Ignore
                            //MessageBox.Show("sp_DataReceived Exception ex2: " + ex2.Message);
                        }
                    }
                    ));

                    counter++;
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a black or transparent background to set the render size
                if (drawingFrontJoints)
                    dc.DrawRectangle(System.Windows.Media.Brushes.Black, null, new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));
                else if (drawingLeftJoints)
                    dc.DrawRectangle(System.Windows.Media.Brushes.Black, null, new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));
                else if (drawingTopJoints)
                    dc.DrawRectangle(System.Windows.Media.Brushes.Black, null, new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));
                else if (drawingNormalView)
                    dc.DrawRectangle(System.Windows.Media.Brushes.Transparent, null, new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));

                if (skeletons.Length != 0)
                {
                    colorIndex = 0;

                    foreach (Skeleton skel in skeletons)
                    {
                        if (drawingNormalView || drawingFrontJoints)
                        {
                            RenderClippedEdges(skel, dc);

                            if (skel.TrackingState == SkeletonTrackingState.Tracked)
                            {
                                this.DrawBonesAndJoints(skel, dc);

                                int pitch = (int)Math.Round(faceTrackingViewer.rotation.X);
                                int yaw = (int)Math.Round(faceTrackingViewer.rotation.Y);
                                int roll = (int)Math.Round(faceTrackingViewer.rotation.Z);

                                float ux, uy, uz;
                                float degToRad = (float)(Math.PI / 180);
                                ux = (float)(Math.Sin(yaw * degToRad));
                                uy = (float)(Math.Sin(pitch * degToRad));// * Math.Cos(yaw * degToRad));
                                //uz = (float)(Math.Cos(pitch * degToRad) * Math.Cos(yaw * degToRad));

                                SkeletonPoint ph = skel.Joints[JointType.Head].Position;
                                SkeletonPoint plook = new SkeletonPoint();
                                plook.X = ph.X - ux / 3;
                                plook.Y = ph.Y + uy / 3;
                                plook.Z = ph.Z; // + uz/3;

                                dc.DrawLine(trackedBonePen, this.SkeletonPointToScreen(ph), this.SkeletonPointToScreen(plook));


                            }
                            else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                            {
                                dc.DrawEllipse(
                                this.centerPointBrush,
                                null,
                                this.SkeletonPointToScreen(skel.Position),
                                BodyCenterThickness,
                                BodyCenterThickness);
                            }
                        }
                        else if (drawingLeftJoints)
                        {
                            if (skel.TrackingState == SkeletonTrackingState.Tracked)
                            {
                                this.DrawBonesAndJointsLeftView(skel, dc);
                            }
                            else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                            {
                                /*
                                dc.DrawEllipse(
                                this.centerPointBrush,
                                null,
                                this.SkeletonPointToScreen(skel.Position),
                                BodyCenterThickness,
                                BodyCenterThickness);
                                */
                            }
                        }
                        else if (drawingTopJoints)
                        {
                            if (skel.TrackingState == SkeletonTrackingState.Tracked)
                            {
                                this.DrawBonesAndJointsTopView(skel, dc);
                            }
                            else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                            {
                                /*
                                dc.DrawEllipse(
                                this.centerPointBrush,
                                null,
                                this.SkeletonPointToScreen(skel.Position),
                                BodyCenterThickness,
                                BodyCenterThickness);
                                */
                            }
                        }

                        colorIndex++;
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }


        }

        private void Sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    depthFrame.CopyDepthImagePixelDataTo(this.depthPixels);

                    // Get the min and max reliable depth for the current frame
                    int minDepth = depthFrame.MinDepth;
                    int maxDepth = depthFrame.MaxDepth;

                    // Convert the depth to RGB
                    int colorPixelIndex = 0;
                    for (int i = 0; i < this.depthPixels.Length; ++i)
                    {
                        // Get the depth for this pixel
                        short depth = depthPixels[i].Depth;

                        // To convert to a byte, we're discarding the most-significant
                        // rather than least-significant bits.
                        // We're preserving detail, although the intensity will "wrap."
                        // Values outside the reliable depth range are mapped to 0 (black).

                        // Note: Using conditionals in this loop could degrade performance.
                        // Consider using a lookup table instead when writing production code.
                        // See the KinectDepthViewer class used by the KinectExplorer sample
                        // for a lookup table example.
                        byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                        // Write out blue byte
                        this.depthColorPixels[colorPixelIndex++] = intensity;

                        // Write out green byte
                        this.depthColorPixels[colorPixelIndex++] = intensity;

                        // Write out red byte                        
                        this.depthColorPixels[colorPixelIndex++] = intensity;

                        // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                        // If we were outputting BGRA, we would write alpha here.
                        ++colorPixelIndex;
                    }

                    // Write the pixel data into our bitmap
                    this.depthColorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.depthColorBitmap.PixelWidth, this.depthColorBitmap.PixelHeight),
                        this.depthColorPixels,
                        this.depthColorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }

        private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            ColorImageFrame colorImageFrame = e.OpenColorImageFrame();

            //colorImage.Source = ImageToBitmap(colorImageFrame);
            //colorImage2.Source = ImageToBitmap(colorImageFrame);

            if (colorImageFrame != null)
            {

                BitmapSource bitmapSource = ImageToBitmap(colorImageFrame);

                colorImageFrame.Dispose();

                //colorImage.Source = bitmapSource;
                colorImage2.Source = bitmapSource;
            }
        }

        private void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    // Create an array to store the Skeleton data
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];

                    // Copy the data from the frame to the array
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            // Here the data in the array named skeletons is available
            // Let's plot the distance from the sensor for the first skeleton
            int counter = 0;
            foreach(Skeleton skel in skeletons)
            {
                if( (skel.TrackingState == SkeletonTrackingState.Tracked) && (counter == 0))
                {
                    // Use Invoke() to modify the UI
                    Dispatcher.Invoke((Action)(() =>
                    {
                        // You can modify the UI here...
                        try
                        {
                            double distance = Math.Sqrt(Math.Pow(skel.Position.X, 2) +
                                Math.Pow(skel.Position.Y, 2) + Math.Pow(skel.Position.Z, 2));

                            // Update the chart LineSeries values with the latest samples
                            if (myLineSeriesKinectV1.Values.Count < N)
                            {
                                myLineSeriesKinectV1.Values.Add(distance);
                            }
                            else
                            {
                                myLineSeriesKinectV1.Values.RemoveAt(0);
                                myLineSeriesKinectV1.Values.Add(distance);
                            }                            
                        }
                        catch (Exception ex2)
                        {
                            // Ignore
                            //MessageBox.Show("sp_DataReceived Exception ex2: " + ex2.Message);
                        }
                    }
                    ));

                    counter++;
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a black or transparent background to set the render size
                if (drawingFrontJoints)
                    dc.DrawRectangle(System.Windows.Media.Brushes.Black, null, new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));
                else if (drawingLeftJoints)
                    dc.DrawRectangle(System.Windows.Media.Brushes.Black, null, new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));
                else if (drawingTopJoints)
                    dc.DrawRectangle(System.Windows.Media.Brushes.Black, null, new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));
                else if (drawingNormalView)
                    dc.DrawRectangle(System.Windows.Media.Brushes.Transparent, null, new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));

                if (skeletons.Length != 0)
                {
                    colorIndex = 0;

                    foreach (Skeleton skel in skeletons)
                    {
                        if (drawingNormalView || drawingFrontJoints)
                        {
                            RenderClippedEdges(skel, dc);

                            if (skel.TrackingState == SkeletonTrackingState.Tracked)
                            {
                                this.DrawBonesAndJoints(skel, dc);

                                int pitch = (int)Math.Round(faceTrackingViewer.rotation.X);
                                int yaw = (int)Math.Round(faceTrackingViewer.rotation.Y);
                                int roll = (int)Math.Round(faceTrackingViewer.rotation.Z);

                                float ux, uy, uz;
                                float degToRad = (float)(Math.PI / 180);
                                ux = (float)(Math.Sin(yaw * degToRad));
                                uy = (float)(Math.Sin(pitch * degToRad));// * Math.Cos(yaw * degToRad));
                                //uz = (float)(Math.Cos(pitch * degToRad) * Math.Cos(yaw * degToRad));

                                SkeletonPoint ph = skel.Joints[JointType.Head].Position;
                                SkeletonPoint plook = new SkeletonPoint();
                                plook.X = ph.X - ux / 3;
                                plook.Y = ph.Y + uy / 3;
                                plook.Z = ph.Z; // + uz/3;

                                dc.DrawLine(trackedBonePen, this.SkeletonPointToScreen(ph), this.SkeletonPointToScreen(plook));


                            }
                            else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                            {
                                dc.DrawEllipse(
                                this.centerPointBrush,
                                null,
                                this.SkeletonPointToScreen(skel.Position),
                                BodyCenterThickness,
                                BodyCenterThickness);
                            }
                        }
                        else if (drawingLeftJoints)
                        {
                            if (skel.TrackingState == SkeletonTrackingState.Tracked)
                            {
                                this.DrawBonesAndJointsLeftView(skel, dc);
                            }
                            else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                            {
                                /*
                                dc.DrawEllipse(
                                this.centerPointBrush,
                                null,
                                this.SkeletonPointToScreen(skel.Position),
                                BodyCenterThickness,
                                BodyCenterThickness);
                                */
                            }
                        }
                        else if (drawingTopJoints)
                        {
                            if (skel.TrackingState == SkeletonTrackingState.Tracked)
                            {
                                this.DrawBonesAndJointsTopView(skel, dc);
                            }
                            else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                            {
                                /*
                                dc.DrawEllipse(
                                this.centerPointBrush,
                                null,
                                this.SkeletonPointToScreen(skel.Position),
                                BodyCenterThickness,
                                BodyCenterThickness);
                                */
                            }
                        }

                        colorIndex++;
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new System.Windows.Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }


        BitmapSource ImageToBitmap(ColorImageFrame Image)
        {
            byte[] pixeldata = new byte[Image.PixelDataLength];
            Image.CopyPixelDataTo(pixeldata);
            BitmapSource bmap = BitmapSource.Create(
             Image.Width,
             Image.Height,
             96, 96,
             PixelFormats.Bgr32,
             null,
             pixeldata,
             Image.Width * Image.BytesPerPixel);
            return bmap;
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping skeleton data
        /// </summary>
        /// <param name="skeleton">skeleton to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    System.Windows.Media.Brushes.Red,
                    null,
                    new System.Windows.Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    System.Windows.Media.Brushes.Red,
                    null,
                    new System.Windows.Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    System.Windows.Media.Brushes.Red,
                    null,
                    new System.Windows.Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    System.Windows.Media.Brushes.Red,
                    null,
                    new System.Windows.Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            if ((bool)cbBones.IsChecked)
            {
                // Render Torso
                this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
                this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
                this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
                this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

                // Left Arm
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
                this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
                this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

                // Right Arm
                this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
                this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
                this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

                // Left Leg
                this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
                this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
                this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

                // Right Leg
                this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
                this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
                this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);
            }

            if ((bool)cbJoints.IsChecked)
            {
                // Render Joints
                foreach (Joint joint in skeleton.Joints)
                {
                    System.Windows.Media.Brush drawBrush = null;

                    if (joint.TrackingState == JointTrackingState.Tracked)
                    {
                        drawBrush = this.trackedJointBrush;
                    }
                    else if (joint.TrackingState == JointTrackingState.Inferred)
                    {
                        drawBrush = this.inferredJointBrush;
                    }

                    if (drawBrush != null)
                    {
                        drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                    }
                }
            }
        }

        private void DrawBonesAndJointsLeftView(Skeleton skeleton, DrawingContext drawingContext)
        {
            if ((bool)cbBones.IsChecked)
            {
                // Render Torso
                this.DrawBoneLeft(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

                // Left Arm
                this.DrawBoneLeft(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

                // Right Arm
                this.DrawBoneLeft(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

                // Left Leg
                this.DrawBoneLeft(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

                // Right Leg
                this.DrawBoneLeft(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
                this.DrawBoneLeft(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);
            }

            if ((bool)cbJoints.IsChecked)
            {
                // Render Joints
                foreach (Joint joint in skeleton.Joints)
                {
                    System.Windows.Media.Brush drawBrush = null;

                    if (joint.TrackingState == JointTrackingState.Tracked)
                    {
                        drawBrush = this.trackedJointBrush;
                    }
                    else if (joint.TrackingState == JointTrackingState.Inferred)
                    {
                        drawBrush = this.inferredJointBrush;
                    }

                    if (drawBrush != null)
                    {
                        // Fix for left view
                        //drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                        System.Windows.Point point1 = new System.Windows.Point();
                        point1.X = 640 - joint.Position.Z * 640 / 4;
                        point1.Y = 240 - joint.Position.Y * 480 / 4;
                        drawingContext.DrawEllipse(drawBrush, null, point1, JointThickness, JointThickness);
                    }
                }
            }
        }

        private void DrawBonesAndJointsTopView(Skeleton skeleton, DrawingContext drawingContext)
        {
            if ((bool)cbBones.IsChecked)
            {
                // Render Torso
                this.DrawBoneTop(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
                this.DrawBoneTop(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
                this.DrawBoneTop(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
                this.DrawBoneTop(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
                this.DrawBoneTop(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
                this.DrawBoneTop(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
                this.DrawBoneTop(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

                // Left Arm
                this.DrawBoneTop(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
                this.DrawBoneTop(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
                this.DrawBoneTop(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

                // Right Arm
                this.DrawBoneTop(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
                this.DrawBoneTop(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
                this.DrawBoneTop(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

                // Left Leg
                this.DrawBoneTop(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
                this.DrawBoneTop(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
                this.DrawBoneTop(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

                // Right Leg
                this.DrawBoneTop(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
                this.DrawBoneTop(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
                this.DrawBoneTop(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);
            }

            if ((bool)cbJoints.IsChecked)
            {
                // Render Joints
                foreach (Joint joint in skeleton.Joints)
                {
                    System.Windows.Media.Brush drawBrush = null;

                    if (joint.TrackingState == JointTrackingState.Tracked)
                    {
                        drawBrush = this.trackedJointBrush;
                    }
                    else if (joint.TrackingState == JointTrackingState.Inferred)
                    {
                        drawBrush = this.inferredJointBrush;
                    }

                    if (drawBrush != null)
                    {
                        // Modify to draw top view
                        System.Windows.Point point1 = new System.Windows.Point();
                        point1.X = 320 + joint.Position.X * 640 / 4;
                        point1.Y = joint.Position.Z * 480 / 4;

                        drawingContext.DrawEllipse(drawBrush, null, point1, JointThickness, JointThickness);
                    }
                }
            }
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private System.Windows.Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new System.Windows.Point(depthPoint.X, depthPoint.Y);
        }

        /// <summary>
        /// Draws a bone line between two joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            System.Windows.Media.Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                //drawPen = this.trackedBonePen;
                //new System.Windows.Media.Pen(System.Windows.Media.Brushes.Green, 10);
                System.Windows.Media.Brush boneBrush = new SolidColorBrush(colors[colorIndex]);
                drawPen = new System.Windows.Media.Pen(boneBrush, 10);
            }

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
        }

        private void DrawBoneLeft(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            System.Windows.Media.Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                //drawPen = this.trackedBonePen;
                System.Windows.Media.Brush boneBrush = new SolidColorBrush(colors[colorIndex]);
                drawPen = new System.Windows.Media.Pen(boneBrush, 10);
            }

            //drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
            System.Windows.Point point1 = new System.Windows.Point();
            point1.X = 640 - joint0.Position.Z * 640 / 4;
            point1.Y = 240 - joint0.Position.Y * 480 / 4;

            System.Windows.Point point2 = new System.Windows.Point();
            point2.X = 640 - joint1.Position.Z * 640 / 4;
            point2.Y = 240 - joint1.Position.Y * 480 / 4;

            //drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
            drawingContext.DrawLine(drawPen, point1, point2);
        }

        private void DrawBoneTop(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            System.Windows.Media.Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                //drawPen = this.trackedBonePen;
                System.Windows.Media.Brush boneBrush = new SolidColorBrush(colors[colorIndex]);
                drawPen = new System.Windows.Media.Pen(boneBrush, 10);
            }

            //drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
            System.Windows.Point point1 = new System.Windows.Point();
            point1.X = 320 + joint0.Position.X * 640 / 4;
            point1.Y = joint0.Position.Z * 480 / 4;

            System.Windows.Point point2 = new System.Windows.Point();
            point2.X = 320 + joint1.Position.X * 640 / 4;
            point2.Y = joint1.Position.Z * 480 / 4;

            //drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
            drawingContext.DrawLine(drawPen, point1, point2);
        }

        private void btnFrontView_Click(object sender, RoutedEventArgs e)
        {
            colorImage2.Visibility = Visibility.Hidden;

            drawingNormalView = false;
            drawingFrontJoints = true;
            drawingLeftJoints = false;
            drawingTopJoints = false;
        }

        private void btnLeftView_Click(object sender, RoutedEventArgs e)
        {
            colorImage2.Visibility = Visibility.Hidden;

            drawingNormalView = false;
            drawingFrontJoints = false;
            drawingLeftJoints = true;
            drawingTopJoints = false;
        }

        private void btnResetView_Click(object sender, RoutedEventArgs e)
        {
            colorImage2.Visibility = Visibility.Visible;

            drawingNormalView = true;
            drawingFrontJoints = false;
            drawingLeftJoints = false;
            drawingTopJoints = false;
        }

        private void btnTopView_Click(object sender, RoutedEventArgs e)
        {
            colorImage2.Visibility = Visibility.Hidden;

            drawingNormalView = false;
            drawingFrontJoints = false;
            drawingLeftJoints = false;
            drawingTopJoints = true;
        }

        #endregion

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

        #region StatusPage Event Handldrs

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
    }
}
