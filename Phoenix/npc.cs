using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Phoenix
{
    class npc
    {
        //Global variables
        XmlDocument spriteData;
        XmlNodeList spriteList;
        public string spritePathUp;
        public string spritePathDown;
        public string spritePathLeft;
        public string spritePathRight;
        public string direction;
        public Texture2D texture;
        public Animation upper;
        public Animation lower;
        public bool moving;
        public decimal[] defaultLocation;
        public string dialogueID;
        public int speed;
        public int mfreq;
        public int[] changeFromDefaultLocation;
        public long lastMoveTime;
        public decimal distanceToTravel;
        
        public void Initialize(string id)
        {
            lastMoveTime = DateTime.Now.Ticks;
            defaultLocation = new decimal[2];
            changeFromDefaultLocation = new int[2];
            direction = "down";
            spriteData = new XmlDocument();
            spriteData.Load("Content/sprites/npc/npc.xml");
            spriteList = spriteData.DocumentElement.SelectNodes("/npcs/npc");
            moving = false;
            distanceToTravel = 0;
            upper = new Phoenix.Animation();
            lower = new Phoenix.Animation();
            for (int x = 0; x < spriteList.Count; x++)
            {
                if(spriteList[x].Attributes["id"].Value == id)
                {
                    spritePathUp = "sprites/npc/" + spriteList[x].Attributes["sprite"].Value + "_up";
                    spritePathDown = "sprites/npc/" + spriteList[x].Attributes["sprite"].Value + "_down";
                    spritePathLeft = "sprites/npc/" + spriteList[x].Attributes["sprite"].Value + "_left";
                    spritePathRight = "sprites/npc/" + spriteList[x].Attributes["sprite"].Value + "_right";
                    defaultLocation[0] = int.Parse(spriteList[x].Attributes["x"].Value);
                    defaultLocation[1] = int.Parse(spriteList[x].Attributes["y"].Value) - 1;
                    dialogueID = spriteList[x].Attributes["dialogue"].Value;
                    mfreq = int.Parse(spriteList[x].Attributes["mfreq"].Value);
                    speed = int.Parse(spriteList[x].Attributes["speed"].Value);
                }
            }
        }
        public void Update(GameTime gameTime, int mapX, int mapY)
        {
            upper.position = new Vector2(mapX + (float)(defaultLocation[0] * 16) + 8, mapY + (float)defaultLocation[1] * 16 + 24);
            lower.position = new Vector2(mapX + (float)(defaultLocation[0] * 16) + 8, mapY + (float)defaultLocation[1] * 16 + 8);
            upper.Update(gameTime);
            lower.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            upper.Draw(spriteBatch);
            lower.Draw(spriteBatch);
        }
    }
}
