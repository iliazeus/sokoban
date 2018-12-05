using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Text;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Sokoban.WpfUI
{
	public partial class AboutWindow : Window
	{
		public string GameVersion
		{
			get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}
		
		public AboutWindow()
		{
			InitializeComponent();
			DataContext = this;
		}
		
		public void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}
	}
}