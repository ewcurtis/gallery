using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Gallery
{
    public partial class MainPage : ContentPage
    {
        int rowCount, colCount = 0;
        bool disabled = false;


        public MainPage()
        {
            InitializeComponent();
            var t = new TapGestureRecognizer();
            t.Tapped += async (s, e) =>
            {
                if (disabled)
                    return;

                disabled = true;
                Image img = (Image)s;
                await Navigation.PushAsync(new Display(img.ClassId));
                disabled = false;
            };


            //grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200, GridUnitType.Absolute) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(175, GridUnitType.Absolute) });

            for (int i = 0; i < Photos.images.Length; i++)
            {
                Image img = new Image { Source = Photos.images[i].ToString(), Aspect = Aspect.AspectFill, ClassId = i.ToString() };
                img.GestureRecognizers.Add(t);
                //Photos.images[i] = img;
                grid.Children.Add(img, colCount, rowCount);
                colCount += 1;

                if ((i + 1) % 3 == 0)
                {
                    //Dynamically adds new row when needed
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(175, GridUnitType.Absolute) });
                    rowCount += 1;
                    colCount = 0;
                }

            }
        }


    }
}

