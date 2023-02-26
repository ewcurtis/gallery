using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Gallery
{	
	public partial class AppShell : Shell
	{	
		public AppShell ()
		{
			InitializeComponent ();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
	}
}

