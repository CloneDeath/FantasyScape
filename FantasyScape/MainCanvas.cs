using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLImp;
using System.Diagnostics;
using OpenTK.Input;
using OpenTK;
using Gwen.Control;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace FantasyScape
{
	class MainCanvas {
		private static Gwen.Input.OpenTK input;
		private static Gwen.Renderer.OpenTK renderer;
		private static Gwen.Skin.Base skin;
		private static Gwen.Control.Canvas canvas;

		private static bool altDown = false;

		public static Canvas GetCanvas() {
			return canvas;
		}

		static Camera2D Camera = new Camera2D();
		static MainCanvas() {
			GraphicsManager.keyboard.KeyDown += Keyboard_KeyDown;
			GraphicsManager.keyboard.KeyUp += Keyboard_KeyUp;

			GraphicsManager.mouse.ButtonDown += Mouse_ButtonDown;
			GraphicsManager.mouse.ButtonUp += Mouse_ButtonUp;
			GraphicsManager.mouse.Move += Mouse_Move;
			GraphicsManager.mouse.WheelChanged += Mouse_Wheel;

			Camera.OnRender += new GraphicsManager.Renderer(OnRenderFrame);
			Camera.Layer = 10;
			GraphicsManager.Update += new GraphicsManager.Updater(OnUpdateFrame);
            GraphicsManager.OnWindowResize += new GraphicsManager.Resizer(OnWindowResize);

			GraphicsManager.OnDispose += new GraphicsManager.Disposer(Dispose);

			renderer = new Gwen.Renderer.OpenTK();
			skin = new Gwen.Skin.TexturedBase(renderer, @"Data\DefaultSkin.png");
			//skin = new Gwen.Skin.Simple(renderer);
			//skin.DefaultFont = new Gwen.Font(renderer, "Courier", 10);
			canvas = new Canvas(skin);

			input = new Gwen.Input.OpenTK(GraphicsManager.Instance);
			input.Initialize(canvas);

			canvas.SetSize(GraphicsManager.WindowWidth, GraphicsManager.WindowHeight);
			canvas.ShouldDrawBackground = false;
			canvas.BackgroundColor = System.Drawing.Color.FromArgb(255, 150, 170, 170);
			//canvas.KeyboardInputEnabled = true;
		}

        static void OnWindowResize()
        {
            GetCanvas().SetSize(GraphicsManager.WindowWidth, GraphicsManager.WindowHeight);
        }

		private static bool disposed = false;

		public static void Dispose() {
			if(!disposed) {
				canvas.Dispose();
				skin.Dispose();
				renderer.Dispose();
				disposed = true;
			}
		}

		/// <summary>
		/// Occurs when a key is pressed.
		/// </summary>
		/// <param name="sender">The KeyboardDevice which generated this event.</param>
		/// <param name="e">The key that was pressed.</param>
		static void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e) {
			if(e.Key == global::OpenTK.Input.Key.AltLeft)
				altDown = true;
			else if(altDown && e.Key == global::OpenTK.Input.Key.Enter) {
				if(GraphicsManager.windowstate == WindowState.Fullscreen)
					GraphicsManager.windowstate = WindowState.Normal;
				else
					GraphicsManager.windowstate = WindowState.Fullscreen;
			}

			input.ProcessKeyDown(e);
		}

		static void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e) {
			altDown = false;
			input.ProcessKeyUp(e);
		}

		static void Mouse_ButtonDown(object sender, MouseButtonEventArgs args) {
			input.ProcessMouseMessage(args);
		}

		static void Mouse_ButtonUp(object sender, MouseButtonEventArgs args) {
			input.ProcessMouseMessage(args);
		}

		static void Mouse_Move(object sender, MouseMoveEventArgs args) {
			input.ProcessMouseMessage(args);
		}

		static void Mouse_Wheel(object sender, MouseWheelEventArgs args) {
			input.ProcessMouseMessage(args);
		}

		/// <summary>
		/// Respond to resize events here.
		/// </summary>
		/// <param name="e">Contains information on the new GameWindow size.</param>
		/// <remarks>There is no need to call the base implementation.</remarks>
		public static void OnResize(EventArgs e) {
			canvas.SetSize(GraphicsManager.WindowWidth, GraphicsManager.WindowHeight);
		}

		/// <summary>
		/// Add your game logic here.
		/// </summary>
		/// <param name="e">Contains timing information.</param>
		/// <remarks>There is no need to call the base implementation.</remarks>
		public static void OnUpdateFrame() {
			if(renderer.TextCacheSize > 1000) // each cached string is an allocated texture, flush the cache once in a while in your real project
				renderer.FlushTextCache();
		}

		/// <summary>
		/// Add your game rendering code here.
		/// </summary>
		/// <param name="e">Contains timing information.</param>
		/// <remarks>There is no need to call the base implementation.</remarks>
		public static void OnRenderFrame() {
			GL.Disable(EnableCap.AlphaTest);
			canvas.RenderCanvas();
			GL.Enable(EnableCap.AlphaTest);
		}
	}
}
