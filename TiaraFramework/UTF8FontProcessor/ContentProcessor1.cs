using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.IO;
using System.ComponentModel;

namespace UTF8FontProcessor
{
    [ContentProcessor(DisplayName = "UTF-8 Font Processor")]
    public class ContentProcessor1 : FontDescriptionProcessor
    {
        static string ProjectName = "TiaraFramework";


        public override SpriteFontContent Process(FontDescription input, ContentProcessorContext context)
        {
            string fullPath = Path.GetFullPath(MessageFile);

            context.AddDependency(fullPath);

            string letters = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);

            foreach (char c in letters)
            {
                input.Characters.Add(c);
            }

            return base.Process(input, context);
        }

        [DefaultValue("messages.txt")]
        [DisplayName("Message File")]
        [Description("The characters in this file will be automatically added to the font.")]
        public string MessageFile
        {
            get { return messageFile; }
            set { messageFile = value; }
        }
        private string messageFile = @"..\" + ProjectName + @"\UTF8Text\messages.txt";
    }
}