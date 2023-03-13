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

        private double width = 0;
        private double height = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void displayGrid(bool portrait)
        {
            rowCount = 0;
            colCount = 0;
            var t = new TapGestureRecognizer();
            //Tapping image will blow it up in the display page
            t.Tapped += async (s, e) =>
            {
                //Prevents tapped action from executing twice
                if (disabled)
                    return;

                disabled = true;
                Image img = (Image)s;
                await Navigation.PushAsync(new Display(img.ClassId));
                disabled = false;
            };

            grid.Children.Clear();
            //Resets layouts and number of rows and columns to default value;
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            if (portrait)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.33, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.33, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.33, GridUnitType.Star) });
                //Adds initial row
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(175, GridUnitType.Absolute) });

                //Creates Images using paths stored in Photos.images and adds them to the display in a grid view
                for (int i = 0; i < Photos.images.Length; i++)
                {
                    Image img = new Image { Source = Photos.images[i].ToString(), Aspect = Aspect.AspectFill, ClassId = i.ToString() };
                    //Adds functionality to blow up image when tapped
                    img.GestureRecognizers.Add(t);
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
            } else
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
                //Adds initial row
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });

                //Creates Images using paths stored in Photos.images and adds them to the display in a grid view
                for (int i = 0; i < Photos.images.Length; i++)
                {
                    Image img = new Image { Source = Photos.images[i].ToString(), Aspect = Aspect.AspectFill, ClassId = i.ToString() };
                    //Adds functionality to blow up image when tapped
                    img.GestureRecognizers.Add(t);
                    grid.Children.Add(img, colCount, rowCount);
                    colCount += 1;

                    if ((i + 1) % 5 == 0)
                    {
                        //Dynamically adds new row when needed
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
                        rowCount += 1;
                        colCount = 0;
                    }
                }
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); //must be called
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;
                
                //reconfigure layout
                if (width > height)
                {
                    displayGrid(false);
                }
                else
                {                    
                    displayGrid(true);
                }
            }
        }

        


    }
}

