using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace MediaPanther.Framework.Content
{
    public static class Xml
    {
        #region xml file methods
        public static void SerializeToFile<T>(T obj, string path)
        {
            var folder = path.Substring(0, path.IndexOf(Path.GetFileName(path)));
            Directory.CreateDirectory(folder);
            var xml = SerializeObject(obj);
            File.AppendAllText(path, xml);
        }
        #endregion

        #region serialization methods
        /// <summary>
		/// Serialize an object into an XML string.
		/// </summary>
		public static string SerializeObject<T>(T obj)
		{
            try
            {
                var memoryStream = new MemoryStream();
                var xs = new XmlSerializer(typeof(T));
                var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                //xmlTextWriter.Settings.Indent = true; // <--- breaks it!
                xs.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                var xml = Utf8ByteArrayToString(memoryStream.ToArray());
                memoryStream.Close();
                xmlTextWriter.Close();
                return xml;
            }
            catch
            {
                return string.Empty;
            }
		}

		/// <summary>
		/// Reconstruct an object from an XML string
		/// </summary>
		/// <param name="xml">The XML document content representing object T.</param>
		/// <returns>A populated object of type T, made up from the serialised XML document.</returns>
		public static T DeserializeObject<T>(string xml)
		{
            var xs = new XmlSerializer(typeof(T));
            var memoryStream = new MemoryStream(StringToUtf8ByteArray(xml));
            var obj = (T)xs.Deserialize(memoryStream);
            memoryStream.Close();
            return obj;
		}
        #endregion

        #region private methods
        /// <summary>
		/// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
		/// </summary>
		/// <param name="characters">Unicode Byte Array to be converted to String</param>
		/// <returns>String converted from Unicode Byte Array</returns>
		private static string Utf8ByteArrayToString(byte[] characters)
		{
		   var encoding = new UTF8Encoding();
		   var constructedString = encoding.GetString(characters);
		   return (constructedString);
		}

		/// <summary>
		/// Converts the String to UTF8 Byte array and is used in De serialization
		/// </summary>
		private static Byte[] StringToUtf8ByteArray(string pXmlString)
		{
		   var encoding = new UTF8Encoding();
		   var byteArray = encoding.GetBytes(pXmlString);
		   return byteArray;
		}
		#endregion
	}
}