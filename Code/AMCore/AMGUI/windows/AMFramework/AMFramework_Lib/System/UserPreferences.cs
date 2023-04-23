using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace AMFramework_Lib.AMSystem
{
	public class UserPreferences
	{
		#region Properties
		public string IAM_API_PATH { get; set; } = "";
		public ObservableCollection<string> RecentFiles { get; set; } = new();

		#endregion

		public void save()
		{
			WriteToXmlFile<UserPreferences>("UserPreferences", this);
		}

		public static UserPreferences load()
		{
			if (!File.Exists("UserPreferences")) return new();

			UserPreferences? Result = ReadFromXmlFile<UserPreferences>("UserPreferences");
			return Result;
		}


		//----------------------------------------------------------------------------------------------------------------------------------------------
		// Code snippet from
		// Reference from https://stackoverflow.com/questions/6115721/how-to-save-restore-serializable-object-to-from-file
		// Author: Daniel Schroeder
		//----------------------------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Writes the given object instance to an XML file.
		/// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
		/// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
		/// <para>Object type must have a parameterless constructor.</para>
		/// </summary>
		/// <typeparam name="T">The type of object being written to the file.</typeparam>
		/// <param name="filePath">The file path to write the object instance to.</param>
		/// <param name="objectToWrite">The object instance to write to the file.</param>
		/// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
		public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
		{
			TextWriter writer = null;
			try
			{
				var serializer = new XmlSerializer(typeof(T));
				writer = new StreamWriter(filePath, append);
				serializer.Serialize(writer, objectToWrite);
			}
			finally
			{
				writer?.Close();
			}
		}

		/// <summary>
		/// Reads an object instance from an XML file.
		/// <para>Object type must have a parameterless constructor.</para>
		/// </summary>
		/// <typeparam name="T">The type of object to read from the file.</typeparam>
		/// <param name="filePath">The file path to read the object instance from.</param>
		/// <returns>Returns a new instance of the object read from the XML file.</returns>
		public static T ReadFromXmlFile<T>(string filePath) where T : new()
		{
			TextReader reader = null;
			try
			{
				var serializer = new XmlSerializer(typeof(T));
				reader = new StreamReader(filePath);
				return (T)serializer.Deserialize(reader);
			}
			finally
			{
				reader?.Close();
			}
		}
		//----------------------------------------------------------------------------------------------------------------------------------------------
		// End code snippet
		//----------------------------------------------------------------------------------------------------------------------------------------------

	}
}
