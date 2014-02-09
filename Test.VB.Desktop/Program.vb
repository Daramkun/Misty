Imports Daramkun.Misty
Imports Daramkun.Misty.Log
Imports Daramkun.Misty.Platforms
Imports System.Reflection

Public Module Program
	Public Sub Main()
		Logger.AddDefaultLogWriter()
		Core.FixedUpdateTimeStep = New TimeSpan
		Core.FixedDrawTimeStep = New TimeSpan
		Dim chooseWindow As ChooseWindow = New ChooseWindow("Tester via VisualBasic.NET",
			New Assembly() {Assembly.Load("Daramkun.Misty.Platform.OpenTK"), Assembly.Load("Daramkun.Misty.Platform.DirectX9")},
			New Assembly() {Assembly.Load("Test.VB.Game")},
			Nothing,
			Nothing,
			New Assembly() {Assembly.Load("Daramkun.Misty.Core"), Assembly.Load("Daramkun.Misty.Platform.Desktop")}
		)
		chooseWindow.Run()
	End Sub
End Module
