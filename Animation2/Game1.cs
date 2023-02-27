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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        AnimatedSprite sprite;
        AnimatedSprite backgroundSprite;

        Texture2D sheet;
        Texture2D backgroundSheet;
        Texture2D firstHouseSheet;
        Texture2D houseInterior;
        Texture2D lanternTexture;
        Texture2D secondHouseSheet;

        Rectangle lanternRectangle1 = new Rectangle(300, 300, 25, 50);
        Rectangle lanternRectangle2 = new Rectangle(450, 400, 25, 50);
        Rectangle lanternRectangle3 = new Rectangle(500, 300, 25, 50);
        Rectangle houseRect = new Rectangle(175, 275, 250, 200);
        Rectangle secondHouseRect = new Rectangle(475, 275, 250, 200);
        Rectangle houseCollisionRectangle1 = new Rectangle(175, 275, 250, 200);
        Rectangle houseCollisionRectangle2 = new Rectangle(475, 275, 250, 200);



        float houseInteriorTransparency1 = 1;
        float houseInteriorTransparency2 = 1;
        float sceneTransparency2 = 0;

        

        int WIDTH_RESOLUTION = 1920; 
        int HEIGHT_RESOLUTION = 1080;

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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sheet = Content.Load<Texture2D>("AnimationSpriteSheet");
            sprite = new AnimatedSprite(sheet, 1, 32, 48);

            backgroundSheet = Content.Load<Texture2D>("Background");
            backgroundSprite = new AnimatedSprite(backgroundSheet, 1, WIDTH_RESOLUTION, HEIGHT_RESOLUTION);

            firstHouseSheet = Content.Load<Texture2D>("HouseImage");

            houseInterior = Content.Load<Texture2D>("HouseInteriorNew");
            secondHouseSheet = Content.Load<Texture2D>("HouseImage");

            lanternTexture = Content.Load<Texture2D>("LanternSprite");


            backgroundSprite.Position = new Vector2(0, 0);
            sprite.Position = new Vector2(400, 300);

            graphics.PreferredBackBufferWidth = WIDTH_RESOLUTION;
            graphics.PreferredBackBufferHeight = HEIGHT_RESOLUTION;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            sprite.HandleSpriteMovement(gameTime);

            detectInteriorCollision();

            detectLanternCollision();

            sceneAlteration();

            base.Update(gameTime);

            base.Draw(gameTime);
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
                    lanternRectangle1 = Rectangle.Empty;
            }

            if (lanternRectangle2.Contains(sprite.Position.X, sprite.Position.Y))
            {
                if (sceneTransparency2 == 0f)
                    lanternRectangle2 = Rectangle.Empty;
            }

            if (lanternRectangle3.Contains(sprite.Position.X, sprite.Position.Y) && houseInteriorTransparency2 == 1)
            {
                if (sceneTransparency2 == 0f)
                    lanternRectangle3 = Rectangle.Empty;
            }
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
           

            GraphicsDevice.Clear(Color.Green);
            


            spriteBatch.Begin();


            drawFirstSceneAssets();
            drawSecondSceneAssets();
            

            spriteBatch.Draw(sprite.Texture, sprite.Position, sprite.SourceRect, Color.White, 0f,
                sprite.Origin, 1.0f, SpriteEffects.None, 0);



            spriteBatch.End();

            base.Draw(gameTime);

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
