using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Virulent.Graphics;

namespace Virulent.Input
{
    class InputManager
    {
        GamePadState currentState;
        GamePadState previousState;
        KeyboardState currentKeyState;
        KeyboardState previousKeyState;
		MouseState currentMouseState;
		MouseState previousMouseState;

        public void Update(GameTime gameTime)
        {
            previousState = currentState;
            currentState = GamePad.GetState(PlayerIndex.One);

            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState(PlayerIndex.One);

			previousMouseState = currentMouseState;
			currentMouseState = Mouse.GetState ();

        }

        public bool IsBackPressed()
        {
			bool result = currentState.Buttons.Back == ButtonState.Pressed;

			return result;
        }

        public bool APressed()
        {
			bool result =  previousState.Buttons.A == ButtonState.Released
                && currentState.Buttons.A == ButtonState.Pressed;

			return result;
        }

        public bool StartPressed()
        {
            bool gamepad = currentState.Buttons.Start == ButtonState.Pressed
            && previousState.Buttons.Start == ButtonState.Released;
            bool keyboard = currentKeyState.IsKeyDown(Keys.Escape)
            && previousKeyState.IsKeyUp(Keys.Escape);
            return gamepad || keyboard;
        }

        public bool SKeyPressed()
        {
            return currentKeyState.IsKeyDown(Keys.S);
        }

        public bool AnyPressed()
        {
            return StartPressed();
        }

        public bool DownPressed()
        {
            return currentKeyState.IsKeyDown(Keys.Down) && previousKeyState.IsKeyUp(Keys.Down);
        }
        public bool UpPressed()
        {
            return currentKeyState.IsKeyDown(Keys.Up) && previousKeyState.IsKeyUp(Keys.Up);
        }
        public bool IsDownPressed()
        {
            return currentKeyState.IsKeyDown(Keys.Down);
        }
        public bool IsUpPressed()
        {
            return currentKeyState.IsKeyDown(Keys.Up);
        }

        public bool IsLeftPressed()
        {
            return currentKeyState.IsKeyDown(Keys.Left);
        }
        public bool IsRightPressed()
        {
            return currentKeyState.IsKeyDown(Keys.Right);
		}
		public bool JumpPressed()
		{
			return currentKeyState.IsKeyDown(Keys.Space) && previousKeyState.IsKeyUp(Keys.Space);
		}
		public bool IsJumpPressed()
		{
			return currentKeyState.IsKeyDown(Keys.Space);
		}
		public bool JumpReleased()
		{
			return currentKeyState.IsKeyUp(Keys.Space) && previousKeyState.IsKeyDown(Keys.Space);
		}

        public bool EnterPressed()
        {
            return currentKeyState.IsKeyDown(Keys.Enter) && previousKeyState.IsKeyUp(Keys.Enter);
        }
        public bool EscPressed()
        {
            return currentKeyState.IsKeyDown(Keys.Escape) && previousKeyState.IsKeyUp(Keys.Escape);
        }

        public bool BackspacePressed()
        {
            return currentKeyState.IsKeyDown(Keys.Back) && previousKeyState.IsKeyUp(Keys.Back);
        }

        public bool MoveLeftPressed()
        {
            return currentKeyState.IsKeyDown(Keys.A);
        }
        public bool MoveRightPressed()
        {
            return currentKeyState.IsKeyDown(Keys.D);
        }
        public bool MoveUpPressed()
        {
            return currentKeyState.IsKeyDown(Keys.W);
        }
        public bool MoveDownPressed()
        {
            return currentKeyState.IsKeyDown(Keys.S);
		}
		public int MouseLB()
		{
			int result = 0;
			if(currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed) result = 1;
			if(currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed) result = 2;
			return result;
		}
		public int MouseRB()
		{
			int result = 0;
			if(currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed) result = 1;
			if(currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed) result = 2;
			return result;
		}
		private Camera cam;
		private Vector2 viewSize;
		public void RecieveWorldMouseInfo(Camera c, Vector2 viewportSize)
		{
			cam = c;
			viewSize = viewportSize;
		}
		public Vector2 GetWorldMousePos()
		{
			Vector2 initial = new Vector2 (currentMouseState.Position.X, currentMouseState.Position.Y);
			Vector2 result = cam.pos + initial - (viewSize / 2);
			return result;
		}
    }
}
