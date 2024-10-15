using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestProg
{
    public static class FragmentsWorker//расширение ObservableCollection для генерации случайного  
    {
        static public ObservableCollection<FragmentInfo> GenerateRandomList(this ObservableCollection<FragmentInfo> ints, int CountOfNumbs,List<System.Windows.Point> PictureSize, Point fragmetSize)
        {
            ObservableCollection<FragmentInfo> result = new ObservableCollection<FragmentInfo>();
            Random rnd = new Random();
            int RandPicture;
            int RandWidth;
            int RandHeight;

            for (int i=0;i<CountOfNumbs;i++)
            {
                RandPicture = rnd.Next(0, PictureSize.Count);//генерация номера случайной картинки
                RandWidth = rnd.Next(0, (int)PictureSize[RandPicture].X- fragmetSize.X);//генерация X координаты (учитывая также необходимый размер фрагмента
                RandHeight = rnd.Next(0, (int)PictureSize[RandPicture].Y - fragmetSize.Y);//генерация Y координаты (учитывая также необходимый размер фрагмента
                FragmentInfo info = new FragmentInfo(RandPicture,new Point(RandWidth,RandHeight));//создание объекта с данными о фрагменте
                result.Add(info);
            }
            return result;
        }
        static public ObservableCollection <Button> EmptyFragmentsGenerate(this ObservableCollection<Button> ints,ObservableCollection<FragmentInfo> infoCollection)
        {
            ObservableCollection<Button> result = new ObservableCollection<Button>();
            for (int i = 0; i < infoCollection.Count; i++)//генерация кнопок на основании информации о них
            {
                Button button = new Button();
                button.Height = 80;
                button.Width = 80;
                button.Content = infoCollection[i].ToString();
                result.Add(button);
            }
            return result;
        }
        static public Dictionary<int,List<int>> GroupByPicturesNumb(this ObservableCollection<FragmentInfo> StartCollection)
        {
            Dictionary<int,List<int>> result = new Dictionary<int,List<int>>();

            FragmentInfo current = new FragmentInfo();

            for (int i=0;i<StartCollection.Count;i++)
            {
                current = StartCollection[i];
                if (result.ContainsKey(current.PictureNumb))
                {
                    result[current.PictureNumb].Add(i);//сохранеем индексы 
                }
                else
                {
                    result.Add(current.PictureNumb, new List<int> { i });
                }
            }

            return result;
        }

    }
}
