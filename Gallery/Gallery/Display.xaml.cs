using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Gallery
{
    
    public partial class Display : ContentPage
	{

        private double width = 0;
        private double height = 0;
        private int id;
        private bool favorites;
        //Display initial image and adds left and right tap controls to navigate between images
		void displayImage()
		{
            view.Children.Clear();
            Image img;
            if (this.favorites)
            {
                Label l = new Label { Text = "Favorites", HorizontalTextAlignment = TextAlignment.Center, FontSize = 30 };
                view.Children.Add(l);
                img = new Image { Source = Photos.favorites[this.id].ToString(), Aspect = Aspect.AspectFit, 
                                  HeightRequest = this.height - 10, ClassId = this.id.ToString() };
            } else
            {
                Label l = new Label { Text = "Gallery", HorizontalTextAlignment = TextAlignment.Center, FontSize = 30 };
                view.Children.Add(l);
                img = new Image { Source = Photos.images[this.id].ToString(), Aspect = Aspect.AspectFit, 
                                  HeightRequest = this.height - 10, ClassId = this.id.ToString() };
            }
			
            var swipeRight = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            var swipeLeft = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            swipeRight.Swiped += async (s, e) =>
            {
                try
                {
                    if (this.id == 0)
                    {
                        if (this.favorites)
                        {
                            this.id = Photos.favorites.Count - 1;
                        } else
                        {
                            this.id = Photos.images.Length - 1;
                        }
                        
                    }
                    else
                    {
                        this.id -= 1;
                    }

                    if (view.Children.Count > 0)
                    {
                        //Added slight delay to synchronize function and prevent crashing
                        await Task.Delay(100);
                        //Remove current image and create new one for display
                        view.Children.Clear();
                        displayImage();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };

            swipeLeft.Swiped += async (s, e) =>
            {
                try
                {
                    if ((this.id == Photos.images.Length - 1 && !this.favorites) || (this.id == Photos.favorites.Count-1 && this.favorites))
                    {
                        this.id = 0;
                    }
                    else
                    {
                        this.id += 1;
                    }

                    if (view.Children.Count > 0)
                    {
                        //Added slight delay to synchronize function and prevent crashing
                        await Task.Delay(100);
                        //Remove current image and create new one for display
                        view.Children.Clear();
                        displayImage();
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };

            img.GestureRecognizers.Add(swipeRight);
            img.GestureRecognizers.Add(swipeLeft);
            try
            {
                view.Children.Add(img);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

		public Display (string id, bool favorites=false)
		{
            this.favorites = favorites;
            this.id = int.Parse(id);
			InitializeComponent ();
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
                    displayImage();
                }
                else
                {
                    displayImage();
                }
            }
        }
    }
}

