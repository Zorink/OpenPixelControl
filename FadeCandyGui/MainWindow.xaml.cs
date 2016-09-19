﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using OpenPixelControl;

namespace FadeCandyGui
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double LightToggleAnimLength = 1.8;

        private const double LedOnOpacity = 0.88;
        private const double LedOffOpacity = 0.35;
        private const double LogoOnOpacity = 1.00;
        private const double LogoOffOpacity = 0.40;


        private const double LedBlurRadius = 5;
        private const double LogoBlurRadius = 20;


        private readonly BlurEffect _ledBlurEffect;
        private readonly BlurEffect _logoBlurEffect;
        private readonly OpcClient _opcClient;
        private double _offDuration;
        private double _onDuration;
        private int _port;

        private bool _prevConnectionState;
        private string _server;

        public MainWindow()
        {
            InitializeComponent();

            //set opacity on lights to off value
            ConnectionStatusLedImage.Opacity = LedOffOpacity;
            LogoImage.Opacity = LogoOffOpacity;
            LogoImageBlurLayer.Opacity = LogoOffOpacity;

            //init effects to 0 - will animate later
            _ledBlurEffect = new BlurEffect {Radius = 0};
            _logoBlurEffect = new BlurEffect {Radius = 0};
            //assign effects
            ConnectionStatusLedImage.Effect = _ledBlurEffect;
            LogoImageBlurLayer.Effect = _logoBlurEffect;


            _opcClient = new OpcClient();
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            //convert string chars to light IDs
            //make commands
            //send commands 
        }

        private void OnDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _onDuration = e.NewValue;
        }

        private void OffDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _offDuration = e.NewValue;
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_prevConnectionState)
                {
                    //disconnect
                    //port actuall disconnects automatically after each message sent
                    //just update UI
                }
                else
                {
                    //connect
                    //port doenst connect until messages are sent
                    //try to blink the led to see if valid port

                    //is there a tcp response message I can check?


                    _opcClient.Server = ServerTextBox.Text;
                    _opcClient.Port = Convert.ToInt32(PortTextBox.Text);
                }


                //update UI
                UpdateUiForConnectButtonClick();
                //toggle connection state
                _prevConnectionState = !_prevConnectionState;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void UpdateUiForConnectButtonClick()
        {
            //if prev was on
            if (_prevConnectionState)
            {
                //turn off

                //update button text
                ConnectButton.Content = "Connect";

                //disable server and port text boxes
                ServerTextBox.IsEnabled = false;
                PortTextBox.IsEnabled = false;

                //create animations
                var ledBlurRadiusAnimation = new DoubleAnimation(LedBlurRadius, 0,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoImageBlurRadiusAnimation = new DoubleAnimation(LogoBlurRadius, 0,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var ledOpacityAnimation = new DoubleAnimation(LedOnOpacity, LedOffOpacity,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoOpacityAnimation = new DoubleAnimation(LogoOnOpacity, LogoOffOpacity,
                    TimeSpan.FromSeconds(LightToggleAnimLength));

                //start animations
                _ledBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, ledBlurRadiusAnimation);
                _logoBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, logoImageBlurRadiusAnimation);
                ConnectionStatusLedImage.BeginAnimation(OpacityProperty, ledOpacityAnimation);
                LogoImageBlurLayer.BeginAnimation(OpacityProperty, logoOpacityAnimation);
                LogoImage.BeginAnimation(OpacityProperty, logoOpacityAnimation);
            }
            else
            {
                //else turn on

                // update button text
                ConnectButton.Content = "Disconnect";

                //enable server and port text boxes
                ServerTextBox.IsEnabled = true;
                PortTextBox.IsEnabled = true;

                //create animations
                var ledBlurRadiusAnimation = new DoubleAnimation(0, LedBlurRadius,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoImageBlurRadiusAnimation = new DoubleAnimation(0, LogoBlurRadius,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var ledOpacityAnimation = new DoubleAnimation(LedOffOpacity, LedOnOpacity,
                    TimeSpan.FromSeconds(LightToggleAnimLength));
                var logoOpacityAnimation = new DoubleAnimation(LogoOffOpacity, LogoOnOpacity,
                    TimeSpan.FromSeconds(LightToggleAnimLength));

                //start animations
                _ledBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, ledBlurRadiusAnimation);
                _logoBlurEffect.BeginAnimation(BlurEffect.RadiusProperty, logoImageBlurRadiusAnimation);
                ConnectionStatusLedImage.BeginAnimation(OpacityProperty, ledOpacityAnimation);
                LogoImageBlurLayer.BeginAnimation(OpacityProperty, logoOpacityAnimation);
                LogoImage.BeginAnimation(OpacityProperty, logoOpacityAnimation);
            }
        }
    }
}