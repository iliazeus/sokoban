using System;
using System.Windows;
using System.Windows.Controls;

namespace SokobanGui.Controls
{
	public class EvenGrid : Grid
	{
		public static readonly DependencyProperty RowCountProperty =
			DependencyProperty.Register(
				"RowCount", typeof(int),
				typeof(EvenGrid),
				new FrameworkPropertyMetadata(
					propertyChangedCallback: OnRowCountChanged
				)
			);
		public int RowCount
		{
			get { return (int) GetValue(RowCountProperty); }
			set { SetValue(RowCountProperty, value); }
		}
		
		public static readonly DependencyProperty ColumnCountProperty =
			DependencyProperty.Register(
				"ColumnCount", typeof(int),
				typeof(EvenGrid),
				new FrameworkPropertyMetadata(
					propertyChangedCallback: OnColumnCountChanged
				)
			);
		public int ColumnCount
		{
			get { return (int) GetValue(ColumnCountProperty); }
			set { SetValue(ColumnCountProperty, value); }
		}
		
		private void updateRowDefinitions()
		{
			while (RowDefinitions.Count < RowCount) {
				var definition = new RowDefinition();
				definition.Height = new GridLength(1, GridUnitType.Star);
				RowDefinitions.Add(definition);
			}
			while (RowDefinitions.Count > RowCount) {
				RowDefinitions.RemoveAt(RowDefinitions.Count - 1);
			}
		}
		
		private void updateColumnDefinitions()
		{
			while (ColumnDefinitions.Count < ColumnCount) {
				var definition = new ColumnDefinition();
				definition.Width = new GridLength(1, GridUnitType.Star);
				ColumnDefinitions.Add(definition);
			}
			while (ColumnDefinitions.Count > ColumnCount) {
				ColumnDefinitions.RemoveAt(ColumnDefinitions.Count - 1);
			}
		}
		
		public static void OnRowCountChanged(
			DependencyObject d,
		    DependencyPropertyChangedEventArgs e)
		{
			var self = d as EvenGrid;
			self.updateRowDefinitions();
		}
		
		public static void OnColumnCountChanged(
			DependencyObject d,
		    DependencyPropertyChangedEventArgs e)
		{
			var self = d as EvenGrid;
			self.updateColumnDefinitions();
		}
	}
}
