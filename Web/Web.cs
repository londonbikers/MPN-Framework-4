using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using MediaPanther.Framework.Content;

namespace MediaPanther.Framework
{
    public class Web
    {
        #region enums
        /// <summary>
        /// Identifies an encoded string style, i.e. one that looks nicer or one that more accurately encodes characters so they
        /// </summary>
        public enum EncodedStringMode
        {
            /// <summary>
            /// Drops more non alpha-numeric characters to create a nicer looking string.
            /// </summary>
            Aggressive,
            /// <summary>
            /// Encodes more non alpha-numeric characters to preserve the content. Not as nice looking.
            /// </summary>
            Compliant
        }

		/// <summary>
		/// Denotes what type of url-encoding is necessary for a target website.
		/// </summary>
		public enum UrlEncodingType
		{
			Normal = 1,
			MediaPanther = 2,
			Lb = 3
		}
        #endregion
		
		#region structs
		public struct LinkItem
		{
			public string Href;
			public string Text;

			public override string ToString()
			{
				return Href + "\n\t" + Text;
			}
		}
		#endregion

        /// <summary>
        /// Turns any literal URL references in a block of text into ANCHOR html elements.
        /// </summary>
        public static string UrlsToAnchors(string source)
        {
            source = " " + source + " ";
            // easier to convert BR's to something more neutral for now.
            source = Regex.Replace(source, "<br>|<br />|<br/>", "\n");
            source = Regex.Replace(source, @"([\s])(www\..*?|http://.*?)([\s])", "$1<a href=\"$2\" target=\"_blank\">$2</a>$3");
            source = Regex.Replace(source, @"href=""www\.", "href=\"http://www.");
            return source.Trim();
        }

		/// <summary>
		/// Attempts to identify whether or not the current user is a search-engine or other known bot.
		/// Won't raise false-positives but is not guaranteed to identify all bots.
		/// </summary>
		public static bool IsUserABot()
		{
		    if (HttpContext.Current.Request.UserAgent != null)
		    {
		        var userAgent = HttpContext.Current.Request.UserAgent.ToLower();
		        var botKeywords = new[] { "bot", "spider", "google", "yahoo", "search", "crawl", "slurp", "msn", "teoma", "ask.com" };
		        var n = botKeywords.Count(userAgent.Contains);
		        return (n > 0);
		    }

		    return false;
		}

        /// <summary>
        /// Returns the page segment of a URL.
        /// </summary>
        public static string PageNameFromUrl(string path)
        {
            return path.Substring((path.LastIndexOf("/") + 1));
        }

		/// <summary>
		/// Encodes a string in a chosen format so that it is URL-safe.
		/// </summary>
		public static string EncodeString(UrlEncodingType urlEncodingType, string stringToEncode)
		{
			return EncodeString(urlEncodingType, EncodedStringMode.Aggressive, stringToEncode);
		}

        /// <summary>
        /// Encodes a string in a chosen format so that it is URL-safe.
        /// </summary>
        public static string EncodeString(UrlEncodingType urlEncodingType, EncodedStringMode mode, string stringToEncode)
        {
            var output = String.Empty;
            switch (urlEncodingType)
            {
                case UrlEncodingType.Normal:
                    output = HttpUtility.UrlEncode(stringToEncode);
                    break;
                case UrlEncodingType.Lb:
                    output = stringToEncode.Replace(" ", "_");
                    break;
                case UrlEncodingType.MediaPanther:
                    {
                        const string hyphen = "oo00HYP00oo";
                        if (mode == EncodedStringMode.Compliant)
                            stringToEncode = stringToEncode.Replace("-", hyphen);

                        stringToEncode = stringToEncode.Replace(" ", "-");
                        var urlRegex = new Regex(@"[^\w-_]");
                        stringToEncode = urlRegex.Replace(stringToEncode, string.Empty);
                        stringToEncode = Regex.Replace(stringToEncode, "-{2,}", "-");
                        stringToEncode = Regex.Replace(stringToEncode, "^-|-$", String.Empty);

                        if (mode == EncodedStringMode.Compliant)
                            stringToEncode = stringToEncode.Replace(hyphen, "--");

                        output = stringToEncode.ToLower();
                    }
                    break;
            }

            return output;
        }

		/// <summary>
		/// Decodes a string from a chosen URL encoding format.
		/// </summary>
		public static string DecodeString(UrlEncodingType urlEncodingType, string stringToDecode)
		{
			var output = String.Empty;
			switch (urlEncodingType)
			{
			    case UrlEncodingType.Normal:
			        output = HttpUtility.UrlDecode(stringToDecode);
			        break;
			    case UrlEncodingType.Lb:
			        output = stringToDecode.Replace("_", " ");
			        break;
			    case UrlEncodingType.MediaPanther:
			        stringToDecode = stringToDecode.Replace("--", "!!=!!");
			        stringToDecode = stringToDecode.Replace("-", " ");
			        stringToDecode = stringToDecode.Replace("!!=!!", "-");
			        stringToDecode = HttpUtility.UrlDecode(stringToDecode);
			        output = stringToDecode;
			        break;
			}

			return output;
		}

		/// <summary>
		/// Determines the next available numeric ID for a session container.
		/// </summary>
		/// <param name="containerType">The type of the session container.</param>
		public static int GetNextSessionContainerId(Type containerType)
		{
			var c = HttpContext.Current;
			var typeName = containerType.ToString();
			var id = 0;

		    if (c.Session == null)
                return id;

		    var id1 = id;
		    foreach (var tempId in from string key in c.Session.Keys
			                       where key.StartsWith(typeName + ":")
			                       select key.Split(char.Parse(":"))
			                       into parts select Convert.ToInt32(parts[1])
			                       into tempId where tempId > id1 select tempId)
			{
			    id = tempId;
			}

			return id + 1;
		}

        /// <summary>
        /// Ensures that a string will not break a html element if placed into a parameter. Useful for ensuring tooltips are safe.
        /// </summary>
        public static string ToSafeHtmlParameter(string textToMakeSafe)
        {
            return textToMakeSafe.Replace("\"", "'");
        }

		/// <summary>
		/// Fills the contents of a DropDownList with the names and values from an Enum.
		/// </summary>
		public static void PopulateDropDownFromEnum(DropDownList list, object enumToUse, bool useEnumValueAsItemValue)
		{
			if (!enumToUse.GetType().IsEnum)
				return;

			ListItem item;
			var values = Enum.GetValues(enumToUse.GetType());

			for (var i = 0; i < values.Length; i++)
			{
				item = new ListItem { Text = Text.SplitCamelCaseWords(Enum.GetName(enumToUse.GetType(), values.GetValue(i))) };
			    item.Value = (useEnumValueAsItemValue) ? ((int)Enum.Parse(enumToUse.GetType(), values.GetValue(i).ToString())).ToString() : item.Text;
				list.Items.Add(item);
			}
		}

        /// <summary>
        /// Returns a simple list of all of the href attribute values in any links present in any html document.
        /// </summary>
        /// <param name="html">The HTML document to parse for links.</param>
        public static IList<LinkItem> GetLinkUrlsFromHtml(string html)
        {
			// find all matches in the content.
            var list = new List<LinkItem>();
			var m1 = Regex.Matches(html, @"(<a.*?>.*?</a>)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

			foreach (Match m in m1)
			{
				var value = m.Groups[1].Value;
				var i = new LinkItem();

				// get href attribute.
				var m2 = Regex.Match(value, @"href=\""(.*?)\""", RegexOptions.Singleline);
				if (m2.Success)
					i.Href = m2.Groups[1].Value;

				// remove inner tags from text.
				var t = Regex.Replace(value, @"\s*<.*?>\s*", "", RegexOptions.Singleline);
				i.Text = t;

                // don't add duplicates.
                if (list.Count(q=>q.Href == i.Href) == 0)
				    list.Add(i);
			}
			
			return list;
        }

        /// <summary>
        /// Attempts to get the html returned from a Url.
        /// </summary>
        /// <param name="url">The Url to get the content from.</param>
        public static string GetHtmlFromUrl(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var html = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return html;
        }
    }
}