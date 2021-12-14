using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NAudio;
using System.Diagnostics; //rmb to remove this.
using NAudio.Wave;

namespace Sounnerd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> soundBinds = new Dictionary<string, string>();
        private HotkeyManager hotkeys;
        private string testFilePath = "F:\\Coding\\Sounnerd\\Sounnerd\\roza.mp3";
        private string testFilePath2 = "F:\\Coding\\Sounnerd\\Sounnerd\\sova.mp3";
        private string testFilePath3 = "F:\\Coding\\Sounnerd\\Sounnerd\\cypher.mp3";
        private int virtualMic;
        public MainWindow()
        {
            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
            hotkeys = new HotkeyManager();
            hotkeys.HotkeyPressed += HotkeyManagerOnHotkeyPressed;
            hotkeys.Start();
            for(int n = -1;n < WaveOut.DeviceCount; n++)
            {
                var output = WaveOut.GetCapabilities(n);
                if (output.ProductName.Contains("Virtual"))
                {
                    virtualMic = n;
                    break;
                }
            }
            InitializeComponent();
        }
        
        private void HotkeyManagerOnHotkeyPressed(object sender, HotkeyPressedEventArgs e)
        {
            if(e.Key == Key.D0 && e.Modifiers == ModifierKeys.None)
            {
                Trace.WriteLine("testing hotkey press");
                var reader = new Mp3FileReader(testFilePath);
                var waveOut = new WaveOut() { DeviceNumber = virtualMic };
                waveOut.Init(reader);
                waveOut.Play();
            }
            if (e.Key == Key.D9 && e.Modifiers == ModifierKeys.None)
            {
                Trace.WriteLine("testing hotkey press");
                var reader = new Mp3FileReader(testFilePath2);
                var waveOut = new WaveOut() { DeviceNumber = virtualMic };
                waveOut.Init(reader);
                waveOut.Play();
            }
            if (e.Key == Key.D8 && e.Modifiers == ModifierKeys.None)
            {
                Trace.WriteLine("testing hotkey press");
                var reader = new Mp3FileReader(testFilePath3);
                var waveOut = new WaveOut() { DeviceNumber = virtualMic };
                waveOut.Init(reader);
                waveOut.Play();
            }
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("Window loaded");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Trace.WriteLine("Window closing");
        }

        private void btnAdd_Click(object sender,RoutedEventArgs e)
        {
            Trace.WriteLine("btn pressed");
        }
    }
}
