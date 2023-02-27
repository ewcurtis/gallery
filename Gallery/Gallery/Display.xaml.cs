using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Gallery
{
    
    public partial class Display : ContentPage
	{
        //Display initial image and adds left and right tap controls to navigate between images
		void displayImage(int id)
		{
			Image img = new Image { Source = Photos.images[id].ToString(), Aspect = Aspect.AspectFill, ClassId = id.ToString() };
            var swipeRight = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            var swipeLeft = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            swipeRight.Swiped += async (s, e) =>
            {
                try
                {
                    if (id == 0)
                    {
                        id = Photos.images.Length - 1;
                    }
                    else
                    {
                        id -= 1;
                    }

                    if (view.Children.Count > 0)
                    {
                        //Added slight delay to synchronize function and prevent crashing
                        await Task.Delay(100);
                        //Remove current image and create new one for display
                        view.Children.Clear();
                        displayImage(id);
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
                    if (id == Photos.images.Length - 1)
                    {
                        id = 0;
                    }
                    else
                    {
                        id += 1;
                    }

                    if (view.Children.Count > 0)
                    {
                        //Added slight delay to synchronize function and prevent crashing
                        await Task.Delay(100);
                        //Remove current image and create new one for display
                        view.Children.Clear();
                        displayImage(id);
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

		public Display (string id)
		{
			
			InitializeComponent ();
            displayImage(int.Parse(id));
        }
	}
}

