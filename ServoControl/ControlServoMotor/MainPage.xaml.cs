using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ControlServoMotor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static GpioPin UpDownPin;
        public static GpioPin LeftRightPin;

        private static IAsyncAction workItemThread;
        public static GpioController gpio;

        private const int LEFT_RIGHT_PIN_NUMBER = 26;
        private const int UP_DOWN_PIN_NUMBER = 6;
        private static long ticksPeruS;
        private const long cyclesPeruS = 1000000l;

        private static long rightPulseIntervaluS = 1000;
        private static long rightPulseIntervalTicks;
        private const long leftPulseIntervaluS = 2500;
        private static long leftPulseIntervalTicks;

        public MainPage()
        {
            this.InitializeComponent();

            gpio = GpioController.GetDefault();
            ticksPeruS = Stopwatch.Frequency / cyclesPeruS;
            leftPulseIntervalTicks = leftPulseIntervaluS * ticksPeruS;
            rightPulseIntervalTicks = rightPulseIntervaluS * ticksPeruS;

            InitGPIO();

            IAsyncAction wwwThread = Windows.System.Threading.ThreadPool.RunAsync(
                    (source) =>
                    {
                        // Create a listener.
                        HttpListener listener = new HttpListener();
                        // Add the prefixes.
                        listener.Prefixes.Add("http://192.168.128.100:2020/");

                        listener.Start();
                        while (true)
                        {
                            HttpListenerContext context = listener.GetContext();
                            HttpListenerRequest request = context.Request;
                            if (request.QueryString.AllKeys.Contains("gestures"))
                            {
                                switch(request.QueryString["gestures"])
                                {
                                    case "1":
                                        MoveMotorRight(UpDownPin);
                                        break;
                                    case "2":
                                        MoveMotorRight(LeftRightPin);
                                        break;
                                    case "3":
                                        MoveMotorLeft(UpDownPin);
                                        break;
                                    case "4":
                                        MoveMotorLeft(LeftRightPin);
                                        break;
                                }
                            }
                            HttpListenerResponse response = context.Response;
                            string responseString = "<HTML><BODY>HR=100,OX=100,Temp=100</BODY></HTML>";
                            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                            response.ContentLength64 = buffer.Length;
                            System.IO.Stream output = response.OutputStream;
                            output.Write(buffer, 0, buffer.Length);
                            output.Close();
                        }
                    }, WorkItemPriority.Normal);
        }

        public static void SendPulse(GpioPin pin, long pulseWidthTicks)
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            mre.WaitOne(20);
            var stopwatch = Stopwatch.StartNew();

            pin.Write(GpioPinValue.High);

            while (stopwatch.ElapsedTicks < pulseWidthTicks) ;

            pin.Write(GpioPinValue.Low);
        }

        public static void MoveMotorLeft(GpioPin requestedPin)
        {
            workItemThread = Windows.System.Threading.ThreadPool.RunAsync(
                (source) =>
                {
                    SendPulse(requestedPin, leftPulseIntervalTicks);
                }, WorkItemPriority.High);
       }

        public static void MoveMotorRight(GpioPin requestedPin)
        {
            workItemThread = Windows.System.Threading.ThreadPool.RunAsync(
                (source) =>
                {
                    SendPulse(requestedPin, rightPulseIntervalTicks);
                }, WorkItemPriority.High);
        }

        private void InitGPIO()
        {
            if (gpio == null)
            {
                UpDownPin = null;
                LeftRightPin = null;
                return;
            }

            UpDownPin = gpio.OpenPin(UP_DOWN_PIN_NUMBER);
            UpDownPin.Write(GpioPinValue.Low);
            UpDownPin.SetDriveMode(GpioPinDriveMode.Output);

            LeftRightPin = gpio.OpenPin(LEFT_RIGHT_PIN_NUMBER);
            LeftRightPin.Write(GpioPinValue.Low);
            LeftRightPin.SetDriveMode(GpioPinDriveMode.Output);
        }

        private void moveLeft(object sender, RoutedEventArgs e)
        {
            MoveMotorLeft(LeftRightPin);
        }

        private void moveRight(object sender, RoutedEventArgs e)
        {
            MoveMotorRight(LeftRightPin);
        }

        private void moveUp(object sender, RoutedEventArgs e)
        {
            MoveMotorRight(UpDownPin);
        }

        private void moveDown(object sender, RoutedEventArgs e)
        {
            MoveMotorLeft(UpDownPin);
        }
    }
}
