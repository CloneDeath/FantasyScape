using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gwen.Control;
using System.Drawing;
using Gwen.ControlInternal;
using Gwen;

namespace FantasyScape.Client.Editor {
	class DrawingArea : Base {
		private Point m_CursorPos;
        private bool m_Depressed;
        private float m_Hue;
        private Texture m_Texture;

		private Color DrawingColor;

		private PixelData Canvas;

		public delegate void OnDrawEvent(int X, int Y, PixelData Data, ref Color CurrentColor);
		public OnDrawEvent OnDraw;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorLerpBox"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
		public DrawingArea(Base parent) : base(parent)
        {
			SetColor(Color.Black);
			Canvas = new PixelData(16, 16);
            SetSize(128, 128);
            MouseInputEnabled = true;
            m_Depressed = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (m_Texture != null)
                m_Texture.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Handler invoked on mouse moved event.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="dx">X change.</param>
        /// <param name="dy">Y change.</param>
        protected override void OnMouseMoved(int x, int y, int dx, int dy)
        {
            if (m_Depressed)
            {
                m_CursorPos = CanvasPosToLocal(new Point(x, y));
                //Do we have clamp?
                if (m_CursorPos.X < 0)
                    m_CursorPos.X = 0;
                if (m_CursorPos.X > Width)
                    m_CursorPos.X = Width;

                if (m_CursorPos.Y < 0)
                    m_CursorPos.Y = 0;
                if (m_CursorPos.Y > Height)
                    m_CursorPos.Y = Height;

				if (OnDraw != null) {
					OnDraw((m_CursorPos.X * Canvas.Width) / Width, (m_CursorPos.Y * Canvas.Height) / Height, Canvas, ref DrawingColor);
				}
				//SetColorAt(m_CursorPos.X, m_CursorPos.Y, DrawingColor);
				Invalidate();
            }
        }

        /// <summary>
        /// Handler invoked on mouse click (left) event.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="down">If set to <c>true</c> mouse button is down.</param>
        protected override void OnMouseClickedLeft(int x, int y, bool down)
        {
            m_Depressed = down;
            if (down)
                Gwen.Input.InputHandler.MouseFocus = this;
            else
                Gwen.Input.InputHandler.MouseFocus = null;

            OnMouseMoved(x, y, 0, 0);
        }

        /// <summary>
        /// Invalidates the control.
        /// </summary>
        public override void Invalidate()
        {
			if (Canvas != null) {
				Canvas.Update();
			}
            if (m_Texture != null)
            {
                m_Texture.Dispose();
                m_Texture = null;
            }
            base.Invalidate();
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Gwen.Skin.Base skin)
        {
            if (m_Texture == null)
            {
                byte[] pixelData = new byte[Width*Height*4];

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        Color c = GetColorAt(x, y);
                        pixelData[4*(x + y*Width)] = c.R;
                        pixelData[4*(x + y*Width) + 1] = c.G;
                        pixelData[4*(x + y*Width) + 2] = c.B;
                        pixelData[4*(x + y*Width) + 3] = c.A;
                    }
                }

                m_Texture = new Texture(skin.Renderer);
                m_Texture.Width = Width;
                m_Texture.Height = Height;
                m_Texture.LoadRaw(Width, Height, pixelData);
            }

            skin.Renderer.DrawColor = Color.White;
            skin.Renderer.DrawTexturedRect(m_Texture, RenderBounds);


            skin.Renderer.DrawColor = Color.Black;
            skin.Renderer.DrawLinedRect(RenderBounds);

            base.Render(skin);
        }

		private Color GetColorAt(int x, int y) {
			return Canvas[(x * Canvas.Width) / Width, (y * Canvas.Height) / Height];
		}

		private void SetColorAt(int x, int y, Color c) {
			Canvas[(x * Canvas.Width) / Width, (y * Canvas.Height) / Height] = c;
		}

		internal void SetColor(Color color) {
			DrawingColor = color;
		}

		internal void LoadTexture(GLImp.Texture Tex) {
			Canvas.Load(Tex);
			this.Invalidate();
		}
	}
}
