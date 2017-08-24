using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Phoenix
{
    class Animation
    {
        //local variables
        Texture2D spriteStrip;
        int elapsedTime;
        int frameCount;
        int frameTime;
        int currentFrame;
        float scale;
        Color color;
        Rectangle sourceRect = new Rectangle();
        Rectangle destinationRect = new Rectangle();
        //public variables
        public int frameHeight;
        public int frameWidth;
        public bool active;
        public bool moving;
        public bool looping;
        public Vector2 position;

        //Initilaize the Animation
        public void Initialize(Texture2D texture, Vector2 _position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool _looping, bool _moving)
        {
            //Store arguments into local variables
            this.color = color;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;
            looping = _looping;
            position = _position;
            moving = _moving;
            spriteStrip = texture;

            //Set times to zero
            elapsedTime = 0;
            currentFrame = 0;

            //Set active default state
            active = true;
        }

        public void Update(GameTime gameTime)
        {
            //Does not update if the animation is flagged as inactive
            if (!active) return;

            //Update the time that has passed
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Update to the next frame if the frame time has been exceeded
            if (elapsedTime > frameTime)
            {
                if (!moving) currentFrame = 0; //Keep frame zero if animation is not moving
                else currentFrame++;//Increment the next frame

                //If currentframe is greater than the frame count, reset to frame zero
                if (currentFrame > frameCount)
                {
                    currentFrame = 0;
                    if (!looping) active = false; //Deactivate if this is not a looping animation
                    
                }
                elapsedTime = 0;
            }

            //Grab the frame
            sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            destinationRect = new Rectangle((int)position.X - (int)(frameWidth * scale) / 2, (int)position.Y - (int)(frameHeight * scale) / 2, (int)(frameWidth * scale), (int)(frameHeight * scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw only if active
            if (active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
            }
        }
    }
}
