using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
using System.Net;
using System.Security.Policy;
using System.IO;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Media3D;
using System.Windows.Controls.Primitives;
using System;
using System.Net.Http;
namespace TestProg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<FragmentInfo> fragmentsInfo;

        ObservableCollection<Button> fragments;//фрагменты, представляющие собой кнопки

        List<string> Data = new List<string> { "https://upload.wikimedia.org/wikipedia/commons/9/9d/Stones-in-water.jpg",
            "http://www.kryzuy.com/wp-content/uploads/2015/12/KryzUy.Com-Sumilon-Island-Blue-Water-Resort-2.jpg",
            "https://img.freepik.com/free-photo/reflection-of-rocky-mountains-and-sky-on-beautiful-lake_23-2148153610.jpg?w=2000&t=st=1707286026~exp=1707286626~hmac=33cfd04e4e898e7a58210bba5129a7e07a5c4bde22abf12467472f3073d6dd0e",
            "https://img.freepik.com/free-photo/vestrahorn-mountains-in-stokksnes-iceland_335224-667.jpg?w=2000&t=st=1707286136~exp=1707286736~hmac=e87482823b94ab19abc794f7eafcb0356b97e0bbf563aec15dd4596f026aff72",
            "https://img.freepik.com/free-photo/pier-at-a-lake-in-hallstatt-austria_181624-44201.jpg?w=2000&t=st=1707286209~exp=1707286809~hmac=9e29fcb6cf717450de3ebe4b34222b356166d91e6ad651fedbfa63a2822b4f77"
            };
        List<Point> Size = new List<Point> { new Point(1600,1200),new Point(1600,1200), new Point(2000, 1333), new Point(2000, 1125), new Point(2000, 1333) };//ограничения

        public MainWindow()
        {
            InitializeComponent();

        }
        public bool Checker()
        {
            if (string.IsNullOrEmpty(WidthBlock.Text) || string.IsNullOrEmpty(HeightBlock.Text) || string.IsNullOrEmpty(CountBlock.Text))
            {
                MessageBox.Show("Заполните все поля");
                return false;
            }
            if (int.Parse(WidthBlock.Text)>Size.Min(p=>p.X))
            {
                MessageBox.Show($"Ширина фрагмента больше положеного. Максимальная доступная Ширина - {Size.Min(p => p.X)}. Введите новое значение");
                return false;
            }
            if (int.Parse(HeightBlock.Text) > Size.Min(p => p.Y))
            {
                MessageBox.Show($"Высота фрагмента больше положеного. Максимальная доступная Высота - {Size.Min(p => p.Y)}. Введите новое значение");
                return false;
            }
            return true;
        }
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (Checker())
            {
                int UserCount = int.Parse(CountBlock.Text);//количество изображений
                int UserWidth = int.Parse(WidthBlock.Text);//ширина пользователя
                int UserHeight = int.Parse(HeightBlock.Text);//высота пользователя

                fragmentsInfo =  fragmentsInfo.GenerateRandomList(UserCount, Size,new System.Drawing.Point(UserWidth, UserHeight));//список используемых изображений
                ListPanel.ItemsSource = fragmentsInfo;//вывод всех сгенерированных сведений

                Dictionary<int, List<int>> GroupsOfFragments = fragmentsInfo.GroupByPicturesNumb();//номера картинок и индексы фрагментов в которых они используются

                fragments = fragments.EmptyFragmentsGenerate(fragmentsInfo);//генерация пустых фрагментов
                PicturesPanel.ItemsSource = fragments;//вывод всех созданных фрагментов

                foreach (var group in GroupsOfFragments)
                {
                    int PictureNumb = group.Key;//номер картинки

                    List<int> indexes = group.Value;

                    LoadAndShow(PictureNumb,indexes);
                }

            }

        }
        private async void LoadAndShow(int PictureNumb,List<int> indexes)
        {
            try
            {
                var url = new Uri(Data[PictureNumb], UriKind.Absolute);
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");//возникали ошибки при отсутсствии заголовка пользователя

                var responseStream = await httpClient.GetStreamAsync(url);

                var Picture = new BitmapImage();
                using (var memoryStream = new MemoryStream())
                {
                    await responseStream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    Picture.BeginInit();
                    Picture.CacheOption = BitmapCacheOption.OnLoad;
                    Picture.StreamSource = memoryStream;
                    Picture.EndInit();
                    Picture.Freeze();
                }
                CroppedBitmap cb;
                ImageBrush brush;
                for (int i = 0; i < indexes.Count; i++)
                {
                    cb = new CroppedBitmap(Picture, new Int32Rect(fragmentsInfo[indexes[i]].Coords.X, fragmentsInfo[indexes[i]].Coords.Y, int.Parse(WidthBlock.Text), int.Parse(HeightBlock.Text)));
                    brush = new ImageBrush(cb);
                    fragments[indexes[i]].Background = brush;
                }
            }
            catch(Exception ex) {
                MessageBox.Show($"Ошибка: {ex.Message}");

            }
        }
    }
}