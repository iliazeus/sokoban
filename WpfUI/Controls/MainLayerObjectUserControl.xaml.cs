using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Sokoban.WpfUI.Controls
{
	public partial class MainLayerObjectUserControl : UserControl
	{
		public static readonly DependencyProperty SceneObjectProperty =
			DependencyProperty.Register(
				"SceneObject", typeof(SceneTree.SceneObject),
				typeof(MainLayerObjectUserControl),
				new FrameworkPropertyMetadata(
					propertyChangedCallback: SceneObjectProperty_PropertyChanged
				)
			);
		public SceneTree.SceneObject SceneObject
		{
			get { return (SceneTree.SceneObject) GetValue(SceneObjectProperty); }
			set { SetValue(SceneObjectProperty, value); }
		}
		
		static void SceneObjectProperty_PropertyChanged(
			object sender, DependencyPropertyChangedEventArgs e)
		{
			var self = sender as MainLayerObjectUserControl;
			var oldSceneObject = e.OldValue as SceneTree.SceneObject;
			var newSceneObject = e.NewValue as SceneTree.SceneObject;
			
			self.DataContext = newSceneObject;
			
			if (oldSceneObject != null) {
				oldSceneObject.PropertyChanged -= self.sceneObject_PropertyChanged;
			}
			if (newSceneObject != null) {
				self.lastRow = newSceneObject.Row;
				self.lastColumn = newSceneObject.Column;
				
				if (newSceneObject.ObjectType == SceneTree.ObjectType.Box) {
					self.leftAnimation.AccelerationRatio = 0.5;
					self.topAnimation.AccelerationRatio = 0.5;
					self.leftAnimation.DecelerationRatio = 0.1;
					self.topAnimation.DecelerationRatio = 0.1;
				}
				if (newSceneObject.ObjectType == SceneTree.ObjectType.Player) {
					self.leftAnimation.AccelerationRatio = 0.1;
					self.topAnimation.AccelerationRatio = 0.1;
					self.leftAnimation.DecelerationRatio = 0.5;
					self.topAnimation.DecelerationRatio = 0.5;
				}
				
				newSceneObject.PropertyChanged += self.sceneObject_PropertyChanged;
			}
		}
		
		private int lastRow, lastColumn;
		private Storyboard leftStoryboard, topStoryboard;
		private DoubleAnimation leftAnimation, topAnimation;

		void sceneObject_PropertyChanged(
			object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Row") {
				topAnimation.From = -(SceneObject.Row - lastRow) * Image.Source.Height;
				lastRow = SceneObject.Row;
				topStoryboard.Begin(this);
			}
			if (e.PropertyName == "Column") {
				leftAnimation.From = -(SceneObject.Column - lastColumn) * Image.Source.Width;
				lastColumn = SceneObject.Column;
				leftStoryboard.Begin(this);
			}
		}
		
		public MainLayerObjectUserControl()
		{
			InitializeComponent();
			
			leftStoryboard = new Storyboard();
			leftAnimation = new DoubleAnimation(0, 0, new Duration(TimeSpan.FromSeconds(0.2)));
			leftStoryboard.Children.Add(leftAnimation);
			Storyboard.SetTargetName(leftAnimation, Image.Name);
			Storyboard.SetTargetProperty(leftAnimation, new PropertyPath("(Canvas.Left)"));
			
			topStoryboard = new Storyboard();
			topAnimation = new DoubleAnimation(0, 0, new Duration(TimeSpan.FromSeconds(0.2)));
			topStoryboard.Children.Add(topAnimation);
			Storyboard.SetTargetName(topAnimation, Image.Name);
			Storyboard.SetTargetProperty(topAnimation, new PropertyPath("(Canvas.Top)"));
		}
	}
}