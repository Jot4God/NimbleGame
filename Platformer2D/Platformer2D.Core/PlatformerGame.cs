#region File Description
//-----------------------------------------------------------------------------
// PlatformerGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;


namespace NimbleGame
{

    public class PlatformerGame : Microsoft.Xna.Framework.Game
    {

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Vector2 baseScreenSize = new Vector2(800, 480);
        private Matrix globalTransformation;
        int backbufferWidth, backbufferHeight;


        private SpriteFont hudFont;

        private Texture2D winOverlay;
        private Texture2D loseOverlay;
        private Texture2D diedOverlay;


        private int levelIndex = -1;
        private Level level;
        private bool wasContinuePressed;

        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);

        private KeyboardState keyboardState;

        private const int numberOfLevels = 3;

        public PlatformerGame()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.IsFullScreen = false;


            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

        }


        protected override void LoadContent()
        {
            this.Content.RootDirectory = "Content";


            spriteBatch = new SpriteBatch(GraphicsDevice);


            hudFont = Content.Load<SpriteFont>("Fonts/Hud");

            winOverlay = Content.Load<Texture2D>("Overlays/you_win");
            loseOverlay = Content.Load<Texture2D>("Overlays/you_lose");
            diedOverlay = Content.Load<Texture2D>("Overlays/you_died");

            ScalePresentationArea();

            LoadNextLevel();
        }

        public void ScalePresentationArea()
        {
            backbufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            backbufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            float horScaling = backbufferWidth / baseScreenSize.X;
            float verScaling = backbufferHeight / baseScreenSize.Y;
            Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);
            globalTransformation = Matrix.CreateScale(screenScalingFactor);
            System.Diagnostics.Debug.WriteLine("Screen Size - Width[" + GraphicsDevice.PresentationParameters.BackBufferWidth + "] Height [" + GraphicsDevice.PresentationParameters.BackBufferHeight + "]");
        }

        


        protected override void Update(GameTime gameTime)
        {
            if (backbufferHeight != GraphicsDevice.PresentationParameters.BackBufferHeight ||
                backbufferWidth != GraphicsDevice.PresentationParameters.BackBufferWidth)
            {
                ScalePresentationArea();
            }
            HandleInput(gameTime);

            level.Update(gameTime, keyboardState,  Window.CurrentOrientation);

            if (level.Player.Velocity != Vector2.Zero)

            base.Update(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            bool continuePressed =
                keyboardState.IsKeyDown(Keys.Space);

            if (!wasContinuePressed && continuePressed)
            {
                if (!level.Player.IsAlive)
                {
                    level.StartNewLife();
                }
                else if (level.TimeRemaining == TimeSpan.Zero)
                {
                    if (level.ReachedExit)
                        LoadNextLevel();
                    else
                        ReloadCurrentLevel();
                }
            }

            wasContinuePressed = continuePressed;

        }

        private void LoadNextLevel()
        {
            levelIndex = (levelIndex + 1) % numberOfLevels;

            if (level != null)
                level.Dispose();

            string levelPath = string.Format("Content/Levels/{0}.txt", levelIndex);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(Services, fileStream, levelIndex);
        }

        private void ReloadCurrentLevel()
        {
            --levelIndex;
            LoadNextLevel();
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            level.Draw(gameTime, spriteBatch);

            DrawHud();

            base.Draw(gameTime);
        }

        private void DrawHud()
        {
            spriteBatch.Begin();
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);

            Vector2 center = new Vector2(baseScreenSize.X / 2, baseScreenSize.Y / 2);

            string timeString = "TIME: " + level.TimeRemaining.Minutes.ToString("00") + ":" + level.TimeRemaining.Seconds.ToString("00");
            Color timeColor;
            if (level.TimeRemaining > WarningTime ||
                level.ReachedExit ||
                (int)level.TimeRemaining.TotalSeconds % 2 == 0)
            {
                timeColor = Color.LightPink;
            }
            else
            {
                timeColor = Color.MediumPurple;
            }
            DrawShadowedString(hudFont, timeString, hudLocation, timeColor);

            float timeHeight = hudFont.MeasureString(timeString).Y;
            DrawShadowedString(hudFont, "SCORE: " + level.Score.ToString(), hudLocation + new Vector2(0.0f, timeHeight * 1.2f), Color.Pink);
           
            Texture2D status = null;
            if (level.TimeRemaining == TimeSpan.Zero)
            {
                if (level.ReachedExit)
                {
                    status = winOverlay;
                }
                else
                {
                    status = loseOverlay;
                }
            }
            else if (!level.Player.IsAlive)
            {
                status = diedOverlay;
            }

            if (status != null)
            {
                Vector2 statusSize = new Vector2(status.Width, status.Height);
                spriteBatch.Draw(status, center - statusSize / 2, Color.White);
            }
            spriteBatch.End();
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
