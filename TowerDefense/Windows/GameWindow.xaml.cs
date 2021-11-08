using System.IO;
using System.Linq;
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

            var idle = new View
            {
                Angle = 321,
                Frames = new []
                {
                    new Frame { RawData = GetRawData("Cube-0000.png") }
                }, 
                DurationSec = 2.5f
            };
            var rotating = new View
            {
                Angle = 12,
                Frames = new[]
                {
                    new Frame { RawData = GetRawData("Cube-0000.png") },
                    new Frame { RawData = GetRawData("Cube-0010.png") },
                    new Frame { RawData = GetRawData("Cube-0020.png") },
                    new Frame { RawData = GetRawData("Cube-0030.png") },
                    new Frame { RawData = GetRawData("Cube-0040.png") },
                    new Frame { RawData = GetRawData("Cube-0050.png") },
                    new Frame { RawData = GetRawData("Cube-0060.png") },
                    new Frame { RawData = GetRawData("Cube-0070.png") },
                    new Frame { RawData = GetRawData("Cube-0080.png") },
                    new Frame { RawData = GetRawData("Cube-0090.png") }
                },
                DurationSec = 1f
            };
            _visualData = new VisualData
            {
                States = new []
                {
                    new State
                    {
                        Name = "Idle",
                        Views = new [] { idle } 
                    },
                    new State
                    {
                        Name = "Active",
                        Views = new [] { rotating }
                    }
                }
            };

            _cbState.ItemsSource = _visualData.States.Select(st => st.Name).OrderBy(n => n);

            Loaded += GameWindow_Loaded;
        }

        private void GameWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _cubeVisualizer.VisualData = _visualData;
        }

        private static byte[] GetRawData()
        {
            var data = new byte[123];
            for (var i = 0; i < data.Length; i++)
                data[i] = (byte)(i % 250);
            return data;
        }

        private static byte[] GetRawData(string fileName)
        {
            using var file = new FileStream(Path.Combine(@"C:\_\11\08\CubeAnimation", fileName), FileMode.Open, FileAccess.Read, FileShare.Read);
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
    }
}
