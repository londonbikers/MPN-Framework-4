using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MediaPanther.Framework.Content
{
    public static class Text
    {
        #region members
        private static readonly string[][] HtmlNamedEntities = new[]
        { 
            new[] { "&quot;", "\"" },
            new[] { "&lt;", "<" },
            new[] { "&gt;", ">" },
            new[] { "&nbsp;", " " },
            new[] { "&iexcl;", "¡" },
            new[] { "&cent;", "¢" },
            new[] { "&pound;", "£" },
            new[] { "&curren;", "¤" },
            new[] { "&yen;", "¥" },
            new[] { "&brvbar;", "¦" },
            new[] { "&sect;", "§" },
            new[] { "&uml;", "¨" },
            new[] { "&copy;", "©" },
            new[] { "&ordf;", "ª" },
            new[] { "&laquo;", "«" },
            new[] { "&not;", "¬" },
            new[] { "&shy;", "­" },
            new[] { "&reg;", "®" },
            new[] { "&macr;", "¯" },
            new[] { "&deg;", "°" },
            new[] { "&plusmn;", "±" },
            new[] { "&sup2;", "²" },
            new[] { "&sup3;", "³" },
            new[] { "&acute;", "´" },
            new[] { "&micro;", "µ" },
            new[] { "&para;", "¶" },
            new[] { "&middot;", "·" },
            new[] { "&cedil;", "¸" },
            new[] { "&sup1;", "¹" },
            new[] { "&ordm;", "º" },
            new[] { "&raquo;", " »" },
            new[] { "&frac14;", "¼" },
            new[] { "&frac12;", "½" },
            new[] { "&frac34;", "¾" },
            new[] { "&iquest;", "¿" },
            new[] { "&Agrave;", "À" },
            new[] { "&Aacute;", "Á" },
            new[] { "&Acirc;", "Â" },
            new[] { "&Atilde;", "Ã" },
            new[] { "&Auml;", "Ä" },
            new[] { "&Aring;", "Å" },
            new[] { "&AElig;", "Æ" },
            new[] { "&Ccedil;", "Ç" },
            new[] { "&Egrave;", "È" },
            new[] { "&Eacute;", "É" },
            new[] { "&Ecirc;", "Ê" },
            new[] { "&Euml;", "Ë" },
            new[] { "&Igrave;", "Ì" },
            new[] { "&Iacute;", "Í" },
            new[] { "&Icirc;", "Î" },
            new[] { "&Iuml;", "Ï" },
            new[] { "&ETH;", "Ð" },
            new[] { "&Ntilde;", "Ñ" },
            new[] { "&Ograve;", "Ò" },
            new[] { "&Oacute;", "Ó" },
            new[] { "&Ocirc;", "Ô" },
            new[] { "&Otilde;", "Õ" },
            new[] { "&Ouml;", "Ö" },
            new[] { "&times;", "×" },
            new[] { "&Oslash;", "Ø" },
            new[] { "&Ugrave;", "Ù" },
            new[] { "&Uacute;", "Ú" },
            new[] { "&Ucirc;", "Û" },
            new[] { "&Uuml;", "Ü" },
            new[] { "&Yacute;", "Ý" },
            new[] { "&THORN;", "Þ" },
            new[] { "&szlig;", "ß" },
            new[] { "&agrave;", "à" },
            new[] { "&aacute;", "á" },
            new[] { "&acirc;", "â" },
            new[] { "&atilde;", "ã" },
            new[] { "&auml;", "ä" },
            new[] { "&aring;", "å" },
            new[] { "&aelig;", "æ" },
            new[] { "&ccedil;", "ç" },
            new[] { "&egrave;", "è" },
            new[] { "&eacute;", "é" },
            new[] { "&ecirc;", "ê" },
            new[] { "&euml;", "ë" },
            new[] { "&igrave;", "ì" },
            new[] { "&iacute;", "í" },
            new[] { "&icirc;", "î" },
            new[] { "&iuml;", "ï" },
            new[] { "&eth;", "ð" },
            new[] { "&ntilde;", "ñ" },
            new[] { "&ograve;", "ò" },
            new[] { "&oacute;", "ó" },
            new[] { "&ocirc;", "ô" },
            new[] { "&otilde;", "õ" },
            new[] { "&ouml;", "ö" },
            new[] { "&divide;", "÷" },
            new[] { "&oslash;", "ø" },
            new[] { "&ugrave;", "ù" },
            new[] { "&uacute;", "ú" },
            new[] { "&ucirc;", "û" },
            new[] { "&uuml;", "ü" },
            new[] { "&yacute;", "ý" },
            new[] { "&thorn;", "þ" },
            new[] { "&yuml;", "ÿ" },
            new[] { "&OElig;", "Œ" },
            new[] { "&oelig;", "œ" },
            new[] { "&Scaron;", "Š" },
            new[] { "&scaron;", "š" },
            new[] { "&Yuml;", "Ÿ" },
            new[] { "&fnof;", "ƒ" },
            new[] { "&circ;", "ˆ" },
            new[] { "&tilde;", "˜" },
            new[] { "&Alpha;", "Α" },
            new[] { "&Beta;", "Β" },
            new[] { "&Gamma;", "Γ" },
            new[] { "&Delta;", "Δ" },
            new[] { "&Epsilon;", "Ε" },
            new[] { "&Zeta;", "Ζ" },
            new[] { "&Eta;", "Η" },
            new[] { "&Theta;", "Θ" },
            new[] { "&Iota;", "Ι" },
            new[] { "&Kappa;", "Κ" },
            new[] { "&Lambda;", "Λ" },
            new[] { "&Mu;", "Μ" },
            new[] { "&Nu;", "Ν" },
            new[] { "&Xi;", "Ξ" },
            new[] { "&Omicron;", "Ο" },
            new[] { "&Pi;", "Π" },
            new[] { "&Rho;", "Ρ" },
            new[] { "&Sigma;", "Σ" },
            new[] { "&Tau;", "Τ" },
            new[] { "&Upsilon;", "Υ" },
            new[] { "&Phi;", "Φ" },
            new[] { "&Chi;", "Χ" },
            new[] { "&Psi;", "Ψ" },
            new[] { "&Omega;", "Ω" },
            new[] { "&alpha;", "α" },
            new[] { "&beta;", "β" },
            new[] { "&gamma;", "γ" },
            new[] { "&delta;", "δ" },
            new[] { "&epsilon;", "ε" },
            new[] { "&zeta;", "ζ" },
            new[] { "&eta;", "η" },
            new[] { "&theta;", "θ" },
            new[] { "&iota;", "ι" },
            new[] { "&kappa;", "κ" },
            new[] { "&lambda;", "λ" },
            new[] { "&mu;", "μ" },
            new[] { "&nu;", "ν" },
            new[] { "&xi;", "ξ" },
            new[] { "&omicron;", "ο" },
            new[] { "&pi;", "π" },
            new[] { "&rho;", "ρ" },
            new[] { "&sigmaf;", "ς" },
            new[] { "&sigma;", "σ" },
            new[] { "&tau;", "τ" },
            new[] { "&upsilon;", "υ" },
            new[] { "&phi;", "φ" },
            new[] { "&chi;", "χ" },
            new[] { "&psi;", "ψ" },
            new[] { "&omega;", "ω" },
            new[] { "&thetasym;", "ϑ" },
            new[] { "&upsih;", "ϒ" },
            new[] { "&piv;", "ϖ" },
            new[] { "&ensp;", " " },
            new[] { "&emsp;", " " },
            new[] { "&thinsp;", " " },
            new[] { "&zwnj;", "‌" },
            new[] { "&zwj;", "‍" },
            new[] { "&lrm;", "‎" },
            new[] { "&rlm;", "‏" },
            new[] { "&ndash;", "–" },
            new[] { "&mdash;", "—" },
            new[] { "&lsquo;", "‘" },
            new[] { "&rsquo;", "’" },
            new[] { "&sbquo;", "‚" },
            new[] { "&ldquo;", "“" },
            new[] { "&rdquo;", "”" },
            new[] { "&bdquo;", "„" },
            new[] { "&dagger;", "†" },
            new[] { "&Dagger;", "‡" },
            new[] { "&bull;", "•" },
            new[] { "&hellip;", "…" },
            new[] { "&permil;", "‰" },
            new[] { "&prime;", "′" },
            new[] { "&Prime;", "″" },
            new[] { "&lsaquo;", "‹" },
            new[] { "&rsaquo;", "›" },
            new[] { "&oline;", "‾" },
            new[] { "&frasl;", "⁄" },
            new[] { "&euro;", "€" },
            new[] { "&image;", "ℑ" },
            new[] { "&weierp;", "℘" },
            new[] { "&real;", "ℜ" },
            new[] { "&trade;", "™" },
            new[] { "&alefsym;", "ℵ" },
            new[] { "&larr;", "←" },
            new[] { "&uarr;", "↑" },
            new[] { "&rarr;", "→" },
            new[] { "&darr;", "↓" },
            new[] { "&harr;", "↔" },
            new[] { "&crarr;", "↵" },
            new[] { "&lArr;", "⇐" },
            new[] { "&uArr;", "⇑" },
            new[] { "&rArr;", "⇒" },
            new[] { "&dArr;", "⇓" },
            new[] { "&hArr;", "⇔" },
            new[] { "&forall;", "∀" },
            new[] { "&part;", "∂" },
            new[] { "&exist;", "∃" },
            new[] { "&empty;", "∅" },
            new[] { "&nabla;", "∇" },
            new[] { "&isin;", "∈" },
            new[] { "&notin;", "∉" },
            new[] { "&ni;", "∋" },
            new[] { "&prod;", "∏" },
            new[] { "&sum;", "∑" },
            new[] { "&minus;", "−" },
            new[] { "&lowast;", "∗" },
            new[] { "&radic;", "√" },
            new[] { "&prop;", "∝" },
            new[] { "&infin;", "∞" },
            new[] { "&ang;", "∠" },
            new[] { "&and;", "∧" },
            new[] { "&or;", "∨" },
            new[] { "&cap;", "∩" },
            new[] { "&cup;", "∪" },
            new[] { "&int;", "∫" },
            new[] { "&there4;", "∴" },
            new[] { "&sim;", "∼" },
            new[] { "&cong;", "≅" },
            new[] { "&asymp;", "≈" },
            new[] { "&ne;", "≠" },
            new[] { "&equiv;", "≡" },
            new[] { "&le;", "≤" },
            new[] { "&ge;", "≥" },
            new[] { "&sub;", "⊂" },
            new[] { "&sup;", "⊃" },
            new[] { "&nsub;", "⊄" },
            new[] { "&sube;", "⊆" },
            new[] { "&supe;", "⊇" },
            new[] { "&oplus;", "⊕" },
            new[] { "&otimes;", "⊗" },
            new[] { "&perp;", "⊥" },
            new[] { "&sdot;", "⋅" },
            new[] { "&lceil;", "⌈" },
            new[] { "&rceil;", "⌉" },
            new[] { "&lfloor;", "⌊" },
            new[] { "&rfloor;", "⌋" },
            new[] { "&lang;", "〈" },
            new[] { "&rang;", "〉" },
            new[] { "&loz;", "◊" },
            new[] { "&spades;", "♠" },
            new[] { "&clubs;", "♣" },
            new[] { "&hearts;", "♥" },
            new[] { "&diams;", "♦" },
            new[] { "&amp;", "&" }
        };
        #endregion

        /// <summary>
		/// Splits a camel-case string into separate words, i.e. "HelloThereFriend" becomes "Hello There Friend".
		/// </summary>
		public static string SplitCamelCaseWords(string stringToSplit)
		{
			for (var i = 1; i < stringToSplit.Length; i++)
			{
			    if (stringToSplit[i].ToString().ToUpper() != stringToSplit[i].ToString()) continue;
			    stringToSplit = stringToSplit.Insert(i, " ");
			    i++;
			}

			return stringToSplit;
		}

        /// <summary>
        /// Returns the first paragraph from a block of text.
        /// </summary>
        public static string GetFirstParagraph(string text)
        {
			if (string.IsNullOrEmpty(text))
				return string.Empty;

            // perform some norminalisation
            text = text.Replace("<br />  <br />", "<br /><br />");

            if (text.IndexOf("\r\n\r\n") == -1)
            {
                if (text.IndexOf("<br /><br />") > -1)
                {
                    // strip any preceeding P tag.
                    if (text.StartsWith("<p>"))
                        text = text.Substring(3);

                    text = text.Substring(0, text.IndexOf("<br /><br />"));
                }
            }
            else
            {
                var fourElementPos = text.IndexOf("\r\n\r\n");
                var threeELementPos = text.IndexOf("\r\n\r");
                if (fourElementPos == -1) fourElementPos = 99999;
                if (threeELementPos == -1) threeELementPos = 99999;

                text = text.Substring(0, fourElementPos < threeELementPos ? text.IndexOf("\r\n\r\n") : text.IndexOf("\r\n\r"));
            }

            return text.Trim();
        }

        public static string HtmlStripTags(string htmlContent, bool replaceNamedEntities, bool replaceNumberedEntities)
        {
            if (htmlContent == null)
                return null;
            htmlContent = htmlContent.Trim();
            if (htmlContent == string.Empty)
                return string.Empty;

            var bodyStartTagIdx = htmlContent.IndexOf("<body", StringComparison.CurrentCultureIgnoreCase);
            var bodyEndTagIdx = htmlContent.IndexOf("</body>", StringComparison.CurrentCultureIgnoreCase);

            int startIdx = 0, endIdx = htmlContent.Length - 1;
            if (bodyStartTagIdx >= 0)
                startIdx = bodyStartTagIdx;
            if (bodyEndTagIdx >= 0)
                endIdx = bodyEndTagIdx;

            bool insideTag = false,
                    insideAttributeValue = false,
                    insideHtmlComment = false,
                    insideScriptBlock = false,
                    insideNoScriptBlock = false,
                    insideStyleBlock = false;
            var attributeValueDelimiter = '"';

            var sb = new StringBuilder(htmlContent.Length);
            for (var i = startIdx; i <= endIdx; i++)
            {
                // html comment block
                if (!insideHtmlComment)
                {
                    if (i + 3 < htmlContent.Length &&
                        htmlContent[i] == '<' &&
                        htmlContent[i + 1] == '!' &&
                        htmlContent[i + 2] == '-' &&
                        htmlContent[i + 3] == '-')
                    {
                        i += 3;
                        insideHtmlComment = true;
                        continue;
                    }
                }
                else // inside html comment
                {
                    if (i + 2 < htmlContent.Length &&
                        htmlContent[i] == '-' &&
                        htmlContent[i + 1] == '-' &&
                        htmlContent[i + 2] == '>')
                    {
                        i += 2;
                        insideHtmlComment = false;
                        continue;
                    }
                    continue;
                }

                // noscript block
                if (!insideNoScriptBlock)
                {
                    if (i + 9 < htmlContent.Length &&
                        htmlContent[i] == '<' &&
                        (htmlContent[i + 1] == 'n' || htmlContent[i + 1] == 'N') &&
                        (htmlContent[i + 2] == 'o' || htmlContent[i + 2] == 'O') &&
                        (htmlContent[i + 3] == 's' || htmlContent[i + 3] == 'S') &&
                        (htmlContent[i + 4] == 'c' || htmlContent[i + 4] == 'C') &&
                        (htmlContent[i + 5] == 'r' || htmlContent[i + 5] == 'R') &&
                        (htmlContent[i + 6] == 'i' || htmlContent[i + 6] == 'I') &&
                        (htmlContent[i + 7] == 'p' || htmlContent[i + 7] == 'P') &&
                        (htmlContent[i + 8] == 't' || htmlContent[i + 8] == 'T') &&
                        (char.IsWhiteSpace(htmlContent[i + 9]) || htmlContent[i + 9] == '>'))
                    {
                        i += 9;
                        insideNoScriptBlock = true;
                        continue;
                    }
                }
                else // inside noscript block
                {
                    if (i + 10 < htmlContent.Length &&
                        htmlContent[i] == '<' &&
                        htmlContent[i + 1] == '/' &&
                        (htmlContent[i + 2] == 'n' || htmlContent[i + 2] == 'N') &&
                        (htmlContent[i + 3] == 'o' || htmlContent[i + 3] == 'O') &&
                        (htmlContent[i + 4] == 's' || htmlContent[i + 4] == 'S') &&
                        (htmlContent[i + 5] == 'c' || htmlContent[i + 5] == 'C') &&
                        (htmlContent[i + 6] == 'r' || htmlContent[i + 6] == 'R') &&
                        (htmlContent[i + 7] == 'i' || htmlContent[i + 7] == 'I') &&
                        (htmlContent[i + 8] == 'p' || htmlContent[i + 8] == 'P') &&
                        (htmlContent[i + 9] == 't' || htmlContent[i + 9] == 'T') &&
                        (char.IsWhiteSpace(htmlContent[i + 10]) || htmlContent[i + 10] == '>'))
                    {
                        if (htmlContent[i + 10] != '>')
                        {
                            i += 9;
                            while (i < htmlContent.Length && htmlContent[i] != '>')
                                i++;
                        }
                        else
                            i += 10;
                        insideNoScriptBlock = false;
                    }
                    continue;
                }

                // script block
                if (!insideScriptBlock)
                {
                    if (i + 7 < htmlContent.Length &&
                        htmlContent[i] == '<' &&
                        (htmlContent[i + 1] == 's' || htmlContent[i + 1] == 'S') &&
                        (htmlContent[i + 2] == 'c' || htmlContent[i + 2] == 'C') &&
                        (htmlContent[i + 3] == 'r' || htmlContent[i + 3] == 'R') &&
                        (htmlContent[i + 4] == 'i' || htmlContent[i + 4] == 'I') &&
                        (htmlContent[i + 5] == 'p' || htmlContent[i + 5] == 'P') &&
                        (htmlContent[i + 6] == 't' || htmlContent[i + 6] == 'T') &&
                        (char.IsWhiteSpace(htmlContent[i + 7]) || htmlContent[i + 7] == '>'))
                    {
                        i += 6;
                        insideScriptBlock = true;
                        continue;
                    }
                }
                else // inside script block
                {
                    if (i + 8 < htmlContent.Length &&
                        htmlContent[i] == '<' &&
                        htmlContent[i + 1] == '/' &&
                        (htmlContent[i + 2] == 's' || htmlContent[i + 2] == 'S') &&
                        (htmlContent[i + 3] == 'c' || htmlContent[i + 3] == 'C') &&
                        (htmlContent[i + 4] == 'r' || htmlContent[i + 4] == 'R') &&
                        (htmlContent[i + 5] == 'i' || htmlContent[i + 5] == 'I') &&
                        (htmlContent[i + 6] == 'p' || htmlContent[i + 6] == 'P') &&
                        (htmlContent[i + 7] == 't' || htmlContent[i + 7] == 'T') &&
                        (char.IsWhiteSpace(htmlContent[i + 8]) || htmlContent[i + 8] == '>'))
                    {
                        if (htmlContent[i + 8] != '>')
                        {
                            i += 7;
                            while (i < htmlContent.Length && htmlContent[i] != '>')
                                i++;
                        }
                        else
                            i += 8;
                        insideScriptBlock = false;
                    }
                    continue;
                }

                // style block
                if (!insideStyleBlock)
                {
                    if (i + 7 < htmlContent.Length &&
                        htmlContent[i] == '<' &&
                        (htmlContent[i + 1] == 's' || htmlContent[i + 1] == 'S') &&
                        (htmlContent[i + 2] == 't' || htmlContent[i + 2] == 'T') &&
                        (htmlContent[i + 3] == 'y' || htmlContent[i + 3] == 'Y') &&
                        (htmlContent[i + 4] == 'l' || htmlContent[i + 4] == 'L') &&
                        (htmlContent[i + 5] == 'e' || htmlContent[i + 5] == 'E') &&
                        (char.IsWhiteSpace(htmlContent[i + 6]) || htmlContent[i + 6] == '>'))
                    {
                        i += 5;
                        insideStyleBlock = true;
                        continue;
                    }
                }
                else // inside script block
                {
                    if (i + 8 < htmlContent.Length &&
                        htmlContent[i] == '<' &&
                        htmlContent[i + 1] == '/' &&
                        (htmlContent[i + 2] == 's' || htmlContent[i + 2] == 'S') &&
                        (htmlContent[i + 3] == 't' || htmlContent[i + 3] == 'C') &&
                        (htmlContent[i + 4] == 'y' || htmlContent[i + 4] == 'R') &&
                        (htmlContent[i + 5] == 'l' || htmlContent[i + 5] == 'I') &&
                        (htmlContent[i + 6] == 'e' || htmlContent[i + 6] == 'P') &&
                        (char.IsWhiteSpace(htmlContent[i + 7]) || htmlContent[i + 7] == '>'))
                    {
                        if (htmlContent[i + 7] != '>')
                        {
                            i += 7;
                            while (i < htmlContent.Length && htmlContent[i] != '>')
                                i++;
                        }
                        else
                            i += 7;
                        insideStyleBlock = false;
                    }
                    continue;
                }

                if (!insideTag)
                {
                    if (i < htmlContent.Length &&
                        htmlContent[i] == '<')
                    {
                        insideTag = true;
                        continue;
                    }
                }
                else // inside tag
                {
                    if (!insideAttributeValue)
                    {
                        if (htmlContent[i] == '"' || htmlContent[i] == '\'')
                        {
                            attributeValueDelimiter = htmlContent[i];
                            insideAttributeValue = true;
                            continue;
                        }
                        if (htmlContent[i] == '>')
                        {
                            insideTag = false;
                            sb.Append(' '); // prevent words from different tags (<td>s for example) from joining together
                            continue;
                        }
                    }
                    else // inside tag and inside attribute value
                    {
                        if (htmlContent[i] == attributeValueDelimiter)
                        {
                            insideAttributeValue = false;
                            continue;
                        }
                    }
                    continue;
                }

                sb.Append(htmlContent[i]);
            }

            if (replaceNamedEntities)
                foreach (var htmlNamedEntity in HtmlNamedEntities)
                    sb.Replace(htmlNamedEntity[0], htmlNamedEntity[1]);

            if (replaceNumberedEntities)
                for (var i = 0; i < 512; i++)
                    sb.Replace("&#" + i + ";", ((char)i).ToString());

            var text = sb.ToString().Trim();
            text = RemoveUnwantedNewLines(text);
            return text;
        }

        /// <summary>
        /// Turns each word into lower case and then capitalises the first letter of each word.
        /// </summary>
		public static string CapitaliseEachWord(string phrase)
		{
            if (string.IsNullOrEmpty(phrase))
                return string.Empty;

			var builder = new StringBuilder();
			var words = phrase.Split(char.Parse(" "));
            foreach (var t in words)
            {
                if (t.Length > 1)
                    builder.Append(char.ToUpper(t[0]) + t.Substring(1).ToLower() + " ");
                else
                    builder.Append(t.ToUpper());
            }

		    return builder.ToString().Trim();			
		}

        /// <summary>
        /// Atttempts to convert html content to text. Strips tags and preserves paragraph structures where possible.
        /// </summary>
        /// <param name="html">The HTML document/fragment to convert.</param>
        /// <param name="preserveHtmlCharCodes">If true (false by default), html codes such as &amp; will be preserved and not converted.</param>
        /// <returns>A plain-text version of the marked-up source content.</returns>
        public static string HtmlToString(string html, bool preserveHtmlCharCodes = false)
        {
            // replace <BR>'s with something we can convert back after stripping-out html.
            var reg = new Regex("<br>|<br/>|<br />", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            html = reg.Replace(html, @"{{N}}").Trim();

            // work-around for specific tags. convert opening tag to include constant which will get converted back after.
            reg = new Regex(@"<p", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            html = reg.Replace(html, "{{N}}{{N}}<p");
            reg = new Regex(@"<div", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            html = reg.Replace(html, "{{N}}{{N}}<div");

            // strip out the html.
            var text = HtmlStripTags(html, !preserveHtmlCharCodes, !preserveHtmlCharCodes);

            // put back new-lines.
            reg = new Regex(@"{{N}}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            text = reg.Replace(text, "\r\n").Trim();

            // convert html character-codes.
            text = HtmlNamedEntities.Aggregate(text, (current, htmlNamedEntity) => current.Replace(htmlNamedEntity[0], htmlNamedEntity[1]));

            // remove unwated spaces.
            var doubleSpaces = Regex.Matches(text, "  ").Count;
            while (doubleSpaces > 0)
            {
                text = text.Replace("  ", " ");
                doubleSpaces = Regex.Matches(text, "  ").Count;
            }

            if (preserveHtmlCharCodes)
                text = HtmlEncodeSpecialChars(text);

            text = RemoveUnwantedNewLines(text);
            return text;
        }

        /// <summary>
        /// Removes any excessive new-lines, i.e ones where there's more than two together.
        /// </summary>
        /// <param name="content">The content to normalise the new-lines for.</param>
        public static string RemoveUnwantedNewLines(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            var reg = new Regex(@"[\t]+\r\n", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            content = reg.Replace(content, string.Empty);

            reg = new Regex(@"[ ]+\r\n", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            content = reg.Replace(content, string.Empty);

            reg = new Regex(@"[\r\n\r\n\r\n]+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            content = reg.Replace(content, "\r\n\r\n");

            reg = new Regex(@"\t", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            content = reg.Replace(content, string.Empty);

            reg = new Regex(@"\n ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            content = reg.Replace(content, string.Empty);

            return content;
        }

        /// <summary>
        /// Converts special characters to their html-encoded equivilents.
        /// </summary>
        public static string HtmlEncodeSpecialChars(string text)
        {
            var sb = new StringBuilder();
            foreach (var c in text)
            {
                if (c > 127) // special chars
                    sb.Append(String.Format("&#{0};", (int)c));
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
	}
}