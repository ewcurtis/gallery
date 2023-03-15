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
            //Resets layouts and number of rows and columns to default value;
            
            grid.ColumnDefinitions.Clear();

            if (portrait)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.33, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.33, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.33, GridUnitType.Star) });

                populateGrid(3, 175);

            } else
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });

                populateGrid(5, 100);
            }
        }
        //Populates grid with images stored in Photos.images and Photos.favorites
        private void populateGrid(int col, int h)
        {
            rowCount = 0;
            colCount = 0;
            grid.Children.Clear();
            grid.RowDefinitions.Clear();

            var t = new TapGestureRecognizer();
            var tF = new TapGestureRecognizer();
            var favorite = new TapGestureRecognizer();
            var unfavorite = new TapGestureRecognizer();
            var unfavoriteF = new TapGestureRecognizer();

            //Tapping image will blow it up in the display page display all images in the gallery
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

            //Tapping image will blow it up in the display page and display images from the Favorites section
            tF.Tapped += async (s, e) =>
            {
                //Prevents tapped action from executing twice
                if (disabled)
                    return;

                disabled = true;
                Image img = (Image)s;
                await Navigation.PushAsync(new Display(img.ClassId, true));
                disabled = false;
            };

            //Tapping an unfilled star will add the related image to the favorites tab/arraylist
            favorite.Tapped += async (s, e) =>
            {
                if (disabled)
                    return;
                disabled = true;
                
                await Task.Delay(100);
                Image img = (Image)s;
                Image c = (Image)grid.Children[int.Parse(img.ClassId) - 2];
                Debug.WriteLine("Image: " + c.Source.ToString());
                //Adds associated image to favorites
                Photos.favorites.Add(Photos.images[int.Parse(c.ClassId)]);

                //rerenders grid
                populateGrid(col, h);
                disabled = false;
            };

            //Tapping a filled star will remove the related image from the favorites tab/arraylist
            unfavorite.Tapped += async (s, e) =>
            {
                if (disabled)
                    return;
                disabled = true;
                await Task.Delay(100);
                Image img = (Image)s;
                Image c = (Image)grid.Children[int.Parse(img.ClassId) - 2];
                Photos.favorites.Remove(Photos.images[int.Parse(c.ClassId)]);
                //rerenders grid
                populateGrid(col, h);
                disabled = false;
            };

            //Tapping a filled star in the favorites menu will remove the related image from the favorites tab/arraylist
            unfavoriteF.Tapped += async (s, e) =>
            {
                if (disabled)
                    return;
                disabled = true;
                await Task.Delay(100);
                Image img = (Image)s;
                Image c = (Image)grid.Children[int.Parse(img.ClassId) - 2];
                Photos.favorites.Remove(Photos.favorites[int.Parse(c.ClassId)]);
                //rerenders grid
                populateGrid(col, h);
                disabled = false;
            };

            //Adds initial row
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(h, GridUnitType.Absolute) });

            //Creates Images using paths stored in Photos.images and adds them to the display in a grid view
            for (int i = 0; i < Photos.images.Length; i++)
            {
                Image img = new Image { Source = Photos.images[i].ToString(), Aspect = Aspect.AspectFill, ClassId = i.ToString() };
                //Adds functionality to blow up image when tapped
                img.GestureRecognizers.Add(t);
                grid.Children.Add(img, colCount, rowCount);

                //Adds favorites toggle
                if (Photos.favorites.Contains(Photos.images[i].ToString()))
                {
                    //Favorited image
                    Image svg = new Image { Source = "star.png", ClassId = (grid.Children.Count + 1).ToString(),
                                    VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.End, HeightRequest = 40, WidthRequest = 40 };
                    svg.GestureRecognizers.Add(unfavorite);
                    grid.Children.Add(svg, colCount, rowCount);
                } else
                {
                    //Unfavorited image
                    Image svg = new Image { Source = "star_outline.png", ClassId = (grid.Children.Count + 1).ToString(),
                                    VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.End, HeightRequest = 40, WidthRequest = 40 };
                    svg.GestureRecognizers.Add(favorite);
                    grid.Children.Add(svg, colCount, rowCount);
                }

                colCount += 1;

                if (((i + 1) % col == 0) && i != Photos.images.Length-1)
                {
                    //Dynamically adds new row when needed
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(h, GridUnitType.Absolute) });
                    rowCount += 1;
                    colCount = 0;
                }
            }

            rowCount += 1;
            colCount = 0;
            Debug.WriteLine("RowCount: " + rowCount);
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute) });
            Label l = new Label { Text = "Favorites", TextDecorations = TextDecorations.Underline, VerticalTextAlignment = TextAlignment.Center,
                                  FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.Black };

            grid.Children.Add(l, colCount, rowCount);

            //Renders favorited images, if any
            if (Photos.favorites.Count > 0)
            {
                rowCount += 1;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(h, GridUnitType.Absolute) });
                for (int i = 0; i < Photos.favorites.Count; i++)
                {
                    Image img = new Image { Source = Photos.favorites[i].ToString(), Aspect = Aspect.AspectFill, ClassId = i.ToString() };
                    //Adds functionality to blow up image when tapped
                    img.GestureRecognizers.Add(tF);
                    grid.Children.Add(img, colCount, rowCount);

                    //Favorited image
                    Image svg = new Image { Source = "star.png", ClassId = (grid.Children.Count + 1).ToString(),
                                    VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.End, HeightRequest = 40, WidthRequest = 40 };
                    svg.GestureRecognizers.Add(unfavoriteF);
                    grid.Children.Add(svg, colCount, rowCount);

                    colCount += 1;

                    if (((i + 1) % col == 0) && i != Photos.favorites.Count-1)
                    {
                        //Dynamically adds new row when needed
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(h, GridUnitType.Absolute) });
                        rowCount += 1;
                        colCount = 0;
                    }
                } 
            }
            Debug.WriteLine("Final colcount: " + colCount + " Final RowCount: " + rowCount);
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

