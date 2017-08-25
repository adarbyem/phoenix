using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Phoenix;

namespace PlayerNS
{
    class Player
    {
        //Player attributes
        public Animation animation;
        public Vector2 position;
        public bool active;
        public bool moving;
        public string playerGender;
        public string gender
        {
            get
            {
                return playerGender;
            }
        }
        public int width
        {
            get
            {
                return animation.frameWidth;
            }
        }
        public int height
        {
            get
            {
                return animation.frameHeight;
            }
        }


        public void Initialize(Animation _animation, Vector2 _position, String gender)
        {
            animation = _animation;
            position = _position;
            moving = false;
            playerGender = gender;
        }

        public void Update(GameTime gameTime)
        {
            animation.position = position;
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }

        public string setGender(String gender)
        {
            return gender;
        }
    }
}
