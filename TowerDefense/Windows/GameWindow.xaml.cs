using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Kalavarda.Primitives.Visualization;
using Frame = Kalavarda.Primitives.Visualization.Frame;

namespace TowerDefense.Windows
{
    public partial class GameWindow
    {
        private readonly VisualObject _visualObject;

        public GameWindow()
        {
            InitializeComponent();

            //_visualData = CreateTeapot();
            _visualObject = CreateCube();

            _cbState.ItemsSource = _visualObject.States.Select(st => st.Name).OrderBy(n => n);

            Loaded += GameWindow_Loaded;
        }

        private static VisualObject CreateCube()
        {
            var idle = new View
            {
                Angle = 321,
                Frames = new[]
                {
                    new Frame {RawData = GetRawData("Cube-0000.png")}
                },
                DurationSec = 2.5f
            };
            var rotating = new View
            {
                Angle = 12,
                Frames = new[]
                {
                    new Frame {RawData = GetRawData("Cube-0000.png")},
                    new Frame {RawData = GetRawData("Cube-0010.png")},
                    new Frame {RawData = GetRawData("Cube-0020.png")},
                    new Frame {RawData = GetRawData("Cube-0030.png")},
                    new Frame {RawData = GetRawData("Cube-0040.png")},
                    new Frame {RawData = GetRawData("Cube-0050.png")},
                    new Frame {RawData = GetRawData("Cube-0060.png")},
                    new Frame {RawData = GetRawData("Cube-0070.png")},
                    new Frame {RawData = GetRawData("Cube-0080.png")},
                    new Frame {RawData = GetRawData("Cube-0090.png")}
                },
                DurationSec = 1f
            };
            return new VisualObject
            {
                States = new[]
                {
                    new State
                    {
                        Name = "Idle",
                        Views = new[] { idle },
                        Sound = new StateSound { RawData = GetRawData(@"C:\Work\Code\Kalavarda\Room\Room\Resources", "Blow_01.mp3") }
                    },
                    new State
                    {
                        Name = "Active",
                        Views = new[] { rotating },
                        Sound = new StateSound { RawData = GetRawData(@"C:\Work\Code\Kalavarda\Room\Room\Resources", "Fireball_01.mp3") },
                        Looping = false
                    }
                }
            };
        }

        private static VisualObject CreateTeapot()
        {
            var views = new View[16];
            for (var i = 0; i < views.Length; i++)
            {
                var fNo = (int)(100 * (float)i / views.Length);
                var s = fNo.ToString("###");
                while (s.Length < 3)
                    s = "0" + s;
                views[i] = new View
                {
                    Angle = (int) (360 * (float) i / views.Length),
                    Frames = new[]
                    {
                        new Frame { RawData = GetRawData($"Teapot-0{s}.png") }
                    }
                };
            }

            return new VisualObject
            {
                States = new[]
                {
                    new State
                    {
                        Name = "Idle",
                        Views = views
                    }
                }
            };
        }

        private void GameWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _cubeVisualizer1.VisualObject = _visualObject;
            _cubeVisualizer2.VisualObject = _visualObject;
            _cubeVisualizer3.VisualObject = _visualObject;
        }

        private static byte[] GetRawData(string fileName)
        {
            var path = fileName.StartsWith("Teapot") ? @"C:\_\11\08\Teapot" : @"C:\_\11\08\CubeAnimation";
            return GetRawData(path, fileName);
        }

        private static byte[] GetRawData(string path, string fileName)
        {
            using var file = new FileStream(Path.Combine(path, fileName), FileMode.Open, FileAccess.Read, FileShare.Read);
            var data = new byte[file.Length];
            file.Read(data);
            return data;
        }

        private void OnStateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            var name = (string)_cbState.SelectedItem;
            var state = _visualObject.States.First(st => st.Name == name);
            _visualObject.CurrentState = state;
        }

        private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _visualObject.CurrentAngle = (int)_slider.Value;
        }
    }
}
