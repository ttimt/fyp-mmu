﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace fyp1_prototype
{
	/// <summary>
	/// Interaction logic for HighScore.xaml
	/// </summary>
	public partial class HighScore : Window
	{
		private HandPointer capturedHandPointer;
		private KinectSensorChooser kinectSensorChooser;
		private int gameMode;

		public HighScore(KinectSensorChooser kinectSensorChooser, int gameMode)
		{
			InitializeComponent();

			//	Set screen to center
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

			//	Set values
			this.kinectSensorChooser = kinectSensorChooser;
			this.gameMode = gameMode;

			//	Data binding
			var kinectRegionandSensorBinding = new Binding("Kinect") { Source = kinectSensorChooser };
			BindingOperations.SetBinding(kinectKinectRegion, KinectRegion.KinectSensorProperty, kinectRegionandSensorBinding);

			//	Set title
			if (gameMode == 0)
			{
				Title = "Survival Mode Highscore";
			}
			else if (gameMode == 1)
			{
				Title = "Time Attack Mode Highscore";
			}

			//	Set highscore text
			var textHeader = new Label
			{
				Content = "Name\t\t\t\t\t\t\t\t\t" + "Score",
				FontWeight = FontWeights.Bold,
				FontSize = 26
			};
			scrollContent.Children.Add(textHeader);

			ScoreRepository sro = new ScoreRepository();
			PlayersRepository pro = new PlayersRepository();

			var highscore = sro.GetAllScore(gameMode);
			for (int i = highscore.Count - 1; i >= 0; i--)
			{
				List<PlayersRepository.PlayerDto> user = pro.GetPlayerWithId(highscore[i].PlayerScore);
				string name = "";
				if (user.Count == 1)
				{
					name = user[0].Username;
				}

				var textBody = new Label
				{
					FontSize = 26,
					Content = " " + name + "\t\t\t\t\t\t\t\t\t" + highscore[i].Value
				};
				scrollContent.Children.Add(textBody);
			}

			#region KinectRegion
			//	Setup Kinect region press target and event handlers
			KinectRegion.SetIsPressTarget(back, true);

			KinectRegion.AddHandPointerEnterHandler(back, HandPointerEnterEvent);
			KinectRegion.AddHandPointerLeaveHandler(back, HandPointerLeaveEvent);

			KinectRegion.AddHandPointerPressHandler(back, HandPointerPressEvent);
			KinectRegion.AddHandPointerPressReleaseHandler(back, HandPointerPressReleaseEvent);

			KinectRegion.AddHandPointerGotCaptureHandler(back, HandPointerCaptureEvent);
			KinectRegion.AddHandPointerLostCaptureHandler(back, HandPointerLostCaptureEvent);
			#endregion
		}

		private void HandPointerEnterEvent(object sender, HandPointerEventArgs e)
		{
			if (e.HandPointer.GetIsOver(back) && e.HandPointer.IsPrimaryHandOfUser)
			{
				VisualStateManager.GoToState(back, "MouseOver", true);
			}

			e.Handled = true;
		}

		private void HandPointerLeaveEvent(object sender, HandPointerEventArgs e)
		{
			if (!e.HandPointer.GetIsOver(back) && e.HandPointer.IsPrimaryHandOfUser)
			{
				VisualStateManager.GoToState(back, "Normal", true);
			}

			if (capturedHandPointer == e.HandPointer)
			{
				capturedHandPointer = null;
				e.Handled = true;
			}
		}

		private void HandPointerPressEvent(object sender, HandPointerEventArgs e)
		{
			if (capturedHandPointer == null && e.HandPointer.IsPrimaryHandOfUser && e.HandPointer.IsPrimaryHandOfUser)
			{
				if (e.HandPointer.GetIsOver(back))
				{
					e.HandPointer.Capture(back);
					capturedHandPointer = e.HandPointer;
					e.Handled = true;
				}
			}
		}

		//	Execute press functions
		private void HandPointerPressReleaseEvent(object sender, HandPointerEventArgs e)
		{
			if (capturedHandPointer == e.HandPointer)
			{
				if (e.HandPointer.GetIsOver(back))
				{
					Close();

					VisualStateManager.GoToState(back, "MouseOver", true);
				}
				else
				{
					VisualStateManager.GoToState(back, "Normal", true);
				}
				e.HandPointer.Capture(null);
				e.Handled = true;
			}
		}

		private void HandPointerCaptureEvent(object sender, HandPointerEventArgs e)
		{
			if (capturedHandPointer == null)
			{
				capturedHandPointer = e.HandPointer;
				e.Handled = true;
			}
		}

		private void HandPointerLostCaptureEvent(object sender, HandPointerEventArgs e)
		{
			if(capturedHandPointer == e.HandPointer)
			{
				capturedHandPointer = null;
				e.Handled = true;
			}
		}

		//	Disable maximizing window
		private void Window_StateChanged(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				WindowState = WindowState.Normal;
			}
		}
    }
}
