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
        private readonly VisualData _visualData;

        public GameWindow()
        {
            InitializeComponent();

            _visualData = CreateTeapot();

            _cbState.ItemsSource = _visualData.States.Select(st => st.Name).OrderBy(n => n);

            Loaded += GameWindow_Loaded;
        }

        private static VisualData CreateCube()
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
            return new VisualData
            {
                States = new[]
                {
                    new State
                    {
                        Name = "Idle",
                        Views = new[] {idle}
                    },
                    new State
                    {
                        Name = "Active",
                        Views = new[] {rotating}
                    }
                }
            };
        }

        private static VisualData CreateTeapot()
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

            return new VisualData
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
            _cubeVisualizer.VisualData = _visualData;
        }

        private static byte[] GetRawData(string fileName)
        {
            var path = fileName.StartsWith("Teapot") ? @"C:\_\11\08\Teapot" : @"C:\_\11\08\CubeAnimation";
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
            var state = _visualData.States.First(st => st.Name == name);
            _visualData.State = state;
        }

        private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _visualData.Angle = (int)_slider.Value;
        }
    }
}
