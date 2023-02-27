using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Animation2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //MANAGERS
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //ANIMATED SPRITES
        AnimatedSprite sprite;
        AnimatedSprite backgroundSprite;
        //TEXTURES
        Texture2D sheet;
        Texture2D backgroundSheet;
        Texture2D firstHouseSheet;
        Texture2D houseInterior;
        Texture2D lanternTexture;
        Texture2D secondHouseSheet;
        Texture2D progressBarTexture;
        //RECTANGLES
        Rectangle lanternRectangle1 = new Rectangle(300, 300, 25, 50);
        Rectangle lanternRectangle2 = new Rectangle(450, 400, 25, 50);
        Rectangle lanternRectangle3 = new Rectangle(500, 300, 25, 50);
        Rectangle houseRect = new Rectangle(175, 275, 250, 200);
        Rectangle secondHouseRect = new Rectangle(475, 275, 250, 200);
        Rectangle houseCollisionRectangle1 = new Rectangle(175, 275, 250, 200);
        Rectangle houseCollisionRectangle2 = new Rectangle(475, 275, 250, 200);
        //FLOATS
        float houseInteriorTransparency1 = 1;
        float houseInteriorTransparency2 = 1;
        float sceneTransparency2 = 0;
        float numberOfLanternsCollected = 0;
        float numberOfLanterns = 3;
        //INTS
        int WIDTH_RESOLUTION = 1920; 
        int HEIGHT_RESOLUTION = 1080;
        int progressBarWidth = 200;
        int progressBarHeight = 20;
        int progressWidth = 0;
        //COLORS
        Color progressBarColor = Color.Green;
        //VECTORS
        Vector2 progressBarPosition = new Vector2(10, 10);



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

        }

        protected override void LoadContent()
        {
            //CHARACTER
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sheet = Content.Load<Texture2D>("AnimationSpriteSheet");
            sprite = new AnimatedSprite(sheet, 1, 32, 48);
            sprite.Position = new Vector2(400, 300);

            //BACKGROUND
            backgroundSheet = Content.Load<Texture2D>("Background");
            backgroundSprite = new AnimatedSprite(backgroundSheet, 1, WIDTH_RESOLUTION, HEIGHT_RESOLUTION);
            backgroundSprite.Position = new Vector2(0, 0);

            //HOUSE SHEETS
            firstHouseSheet = Content.Load<Texture2D>("HouseImage");
            secondHouseSheet = Content.Load<Texture2D>("HouseImage");

            //HOUSE INTERIOR
            houseInterior = Content.Load<Texture2D>("HouseInteriorNew");
            
            //LANTERN
            lanternTexture = Content.Load<Texture2D>("LanternSprite");

            //PROGRESS BAR
            progressBarTexture = new Texture2D(GraphicsDevice, 1, 1);
            progressBarTexture.SetData(new Color[] { Color.White });


            graphics.PreferredBackBufferWidth = WIDTH_RESOLUTION;
            graphics.PreferredBackBufferHeight = HEIGHT_RESOLUTION;

           

        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            sprite.HandleSpriteMovement(gameTime);

            detectInteriorCollision();

            detectLanternCollision();

            sceneAlteration();

            calculateProgressBarWidth();



            base.Update(gameTime);

            base.Draw(gameTime);
        }

        private void calculateProgressBarWidth()
        {
            progressWidth = (int)((numberOfLanternsCollected / numberOfLanterns) * progressBarWidth);
        }

        private void sceneAlteration()
        {
            //SCENE-HANDLER
            if (sprite.Position.X <= 20)
            {


                if (sceneTransparency2 == 0f)
                {

                    sceneTransparency2 = 1f;
                    sprite.Position = new Vector2(770f, sprite.Position.Y);
                }
                else
                {
                    sceneTransparency2 = 0f;
                    sprite.Position = new Vector2(770f, sprite.Position.Y);
                }
            }
            else if (sprite.Position.X >= 780)
            {
                if (sceneTransparency2 == 0f)
                {
                    sceneTransparency2 = 1f;
                    sprite.Position = new Vector2(30f, sprite.Position.Y);
                }
                else
                {
                    sceneTransparency2 = 0f;
                    sprite.Position = new Vector2(30f, sprite.Position.Y);
                }

            }
        }

        private void detectInteriorCollision()
        {
            //HOUSE-INTERIOR DISPLAY
            if (houseCollisionRectangle1.Contains(sprite.Position.X, sprite.Position.Y))
                houseInteriorTransparency1 = 1f;
            else
                houseInteriorTransparency1 = 0f;


            if (houseCollisionRectangle2.Contains(sprite.Position.X, sprite.Position.Y))
                houseInteriorTransparency2 = 1f;
            else
                houseInteriorTransparency2 = 0f;
        }

        private void detectLanternCollision()
        {
            //LANTERN-COLLECTION
            if (lanternRectangle1.Contains(sprite.Position.X, sprite.Position.Y) && houseInteriorTransparency1 == 1)
            {
                if (sceneTransparency2 == 0f)
                {
                    if (!lanternRectangle1.IsEmpty)
                    {
                        Debug.WriteLine("Collected");
                        lanternRectangle1 = Rectangle.Empty;
                        numberOfLanternsCollected++;
                    }
                }

                    
                    
            }

            if (lanternRectangle2.Contains(sprite.Position.X, sprite.Position.Y))
            {
                if (sceneTransparency2 == 1f)
                    if (!lanternRectangle2.IsEmpty)
                    {
                        lanternRectangle2 = Rectangle.Empty;
                        numberOfLanternsCollected++;
                    }
            }

            if (lanternRectangle3.Contains(sprite.Position.X, sprite.Position.Y) && houseInteriorTransparency2 == 1)
            {
                if (sceneTransparency2 == 0f)
                    if (!lanternRectangle3.IsEmpty)
                    {
                        lanternRectangle3 = Rectangle.Empty;
                        numberOfLanternsCollected++;
                    }
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);
            
            spriteBatch.Begin();

            drawFirstSceneAssets();
            drawSecondSceneAssets();

            drawCharacter();

            drawProgressBar();

            spriteBatch.End();

            base.Draw(gameTime);

        }
        private void drawCharacter()
        {
            spriteBatch.Draw(sprite.Texture, sprite.Position, sprite.SourceRect, Color.White, 0f,
                sprite.Origin, 1.0f, SpriteEffects.None, 0);
        }

        private void drawProgressBar()
        {
            spriteBatch.Draw(progressBarTexture, progressBarPosition, null, Color.Gray, 0, Vector2.Zero, new Vector2(progressBarWidth, progressBarHeight), SpriteEffects.None, 0);
            spriteBatch.Draw(progressBarTexture, progressBarPosition, null, progressBarColor, 0, Vector2.Zero, new Vector2(progressWidth, progressBarHeight), SpriteEffects.None, 0);
        }

        private void drawFirstSceneAssets()
        {
            spriteBatch.Draw(backgroundSprite.Texture, backgroundSprite.Position, new Rectangle(0, 0, WIDTH_RESOLUTION * (WIDTH_RESOLUTION - backgroundSprite.SourceRect.Width), HEIGHT_RESOLUTION * (WIDTH_RESOLUTION - backgroundSprite.SourceRect.Width)), Color.White, 0f, sprite.Origin, 1.0f, SpriteEffects.None, 0);

            spriteBatch.Draw(firstHouseSheet, houseRect, Color.White);
            spriteBatch.Draw(secondHouseSheet, secondHouseRect, Color.White);
            spriteBatch.Draw(houseInterior, houseCollisionRectangle1, Color.White * houseInteriorTransparency1);
            spriteBatch.Draw(houseInterior, houseCollisionRectangle2, Color.White * houseInteriorTransparency2);
            spriteBatch.Draw(lanternTexture, lanternRectangle1, Color.White * houseInteriorTransparency1);



            spriteBatch.Draw(lanternTexture, lanternRectangle3, Color.White * houseInteriorTransparency2);
        }

        private void drawSecondSceneAssets()
        {
            spriteBatch.Draw(backgroundSprite.Texture, backgroundSprite.Position, new Rectangle(0, 0, WIDTH_RESOLUTION * (WIDTH_RESOLUTION - backgroundSprite.SourceRect.Width), HEIGHT_RESOLUTION * (WIDTH_RESOLUTION - backgroundSprite.SourceRect.Width)), Color.White * sceneTransparency2, 0f, sprite.Origin, 1.0f, SpriteEffects.None, 0);

            spriteBatch.Draw(lanternTexture, lanternRectangle2, Color.White * sceneTransparency2);
        }
    }
}
