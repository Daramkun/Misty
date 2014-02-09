Imports Daramkun.Misty.Nodes
Imports Daramkun.Misty.Common
Imports Daramkun.Misty.Contents.Tables
Imports Daramkun.Misty.Graphics.Spirit
Imports Daramkun.Misty.Mathematics
Imports Daramkun.Misty.Mathematics.Transforms
Imports Daramkun.Misty
Imports Daramkun.Misty.Graphics
Imports Daramkun.Misty.Contents.FileSystems
Imports Daramkun.Misty.Contents.Loaders
Imports Daramkun.Misty.Graphics.Spirit.Fonts
Imports Daramkun.Misty.Inputs

<MainNode>
Public Class Container
	Inherits Node

	Dim contentManager As ResourceTable

	Dim sprite As Sprite
	Dim font As Font
	Dim world As World2

	Dim animate As Animate

	Public Overrides Sub Intro(ParamArray args() As Object)
		Core.GraphicsDevice.BlendState = True
		Core.GraphicsDevice.BlendOperation = BlendOperation.AlphaBlend

		contentManager = New ResourceTable(FileSystemManager.GetFileSystem("ManifestFileSystem"))
		contentManager.AddDefaultContentLoader()
		Texture2DContentLoader.AddDefaultDecoders()
		sprite = New Sprite(contentManager.Load(Of ITexture2D)("Resources/Dodge/daram.png", Color.Magenta))
		font = contentManager.Load(Of TrueTypeFont)("Resources/test.ttf", 24)

		animate = New Animate(TimeSpan.FromSeconds(4), 400)
		Add(InputHelper.CreateInstance())

		world = New World2()

		MyBase.Intro(args)
	End Sub

	Public Overrides Sub Outro()
		sprite.Dispose()
		contentManager.Dispose()
		MyBase.Outro()
	End Sub

	Public Overrides Sub Update(gameTime As GameTime)
		If (InputHelper.IsKeyboardKeyUpRightNow(Key.A)) Then
			animate.Start()
		End If
		If (InputHelper.IsKeyboardKeyUpRightNow(Key.S)) Then
			animate.Pause()
		End If
		If (InputHelper.IsKeyboardKeyUpRightNow(Key.D)) Then
			animate.Stop()
		End If

		animate.Update(gameTime)

		MyBase.Update(gameTime)
	End Sub

	Public Overrides Sub Draw(gameTime As GameTime)
		Core.GraphicsDevice.BeginScene()
		Core.GraphicsDevice.Clear(ClearBuffer.AllBuffer, Color.Black)

		font.DrawFont(String.Format("Animate state: {0}, Position: {1}/{2}, Animated: {3:0.000}/{4:0.000}", animate.IsAnimating,
			animate.Position, animate.Duration, animate.Animated, animate.TotalAnimated), Color.White,
			New Vector2(animate.Animated, 0))

		world.Translate = New Vector2(animate.TotalAnimated, 100)
		sprite.Draw(world)

		MyBase.Draw(gameTime)

		Core.GraphicsDevice.EndScene()
		Core.GraphicsDevice.SwapBuffer()
	End Sub

End Class
