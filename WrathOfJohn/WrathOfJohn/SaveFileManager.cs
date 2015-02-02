using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VoidEngine;

namespace WrathOfJohn
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
	public class SaveFileManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game myGame;

		string version = "v0.1-alpha";

		public SaveFileManager(Game game) : base(game)
		{
			myGame = game;
		}

		public void SerializeData(SaveFileData saveFileData)
		{
			SaveFileData obj = saveFileData;

			obj.version = version;

			IFormatter formatter = new BinaryFormatter();

			Stream stream = new FileStream(@".\Data.bin", FileMode.Create, FileAccess.Write, FileShare.None);

			formatter.Serialize(stream, obj);

			stream.close();
		}

		public SaveFileData DeserializeData(string path)
		{
			Iformatter formatter = new BinaryFormatter();

			Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

			SaveFileData saveFileData = (SaveFileData)formatter.Deserialize(stream);

			if (saveFileData.version != version)
			{
				throw new System.ArgumentException("Debug Temp Save File Version does not match.", "original");
			}
			else
			{
				return saveFileData;
			}
		}
	}

	[Serializable]
	public class SaveFileData
	{
		string version = "-placeholder-";
	}
}