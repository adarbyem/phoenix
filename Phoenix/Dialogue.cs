using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix
{
    class Dialogue
    {
        //Global Variables
        public List<string> content;

        XmlDocument document;
        XmlNodeList nodeList;
        XmlNodeList dialogueLines;
		XmlNode contentNode;
        string path;

		public void initialize(string id)
        {
			//Setup the dialogue file and read the specific id tag
			path = "Content/dialogue/dialogue.xml";
            content = new List<string>();
            document = new XmlDocument();
            document.Load(path);
            nodeList = document.DocumentElement.SelectNodes("/dialogues/dialogue");
			for(int x = 0; x < nodeList.Count; x++)
            {
				if(nodeList[x].Attributes["id"].Value == id)
                {
                    contentNode = document.DocumentElement.SelectNodes("/dialogues/dialogue").Item(x);
                    break;
                }
            }
            
            dialogueLines = contentNode.SelectNodes("line");
            for (int x = 0; x < dialogueLines.Count; x++)
            {
                content.Add(dialogueLines[x].InnerText);
            }
        }
    }
}
