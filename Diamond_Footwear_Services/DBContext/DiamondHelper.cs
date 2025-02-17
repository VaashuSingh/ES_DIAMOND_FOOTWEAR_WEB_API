﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Diamond_Footwear_Services.DBContext
{
    public class DiamondHelper
    {
        public static string CreateXml(object yourClassObject)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(yourClassObject.GetType());

                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, yourClassObject);
                    xmlStream.Position = 0;

                    xmlDoc.Load(xmlStream);

                    // Optionally, include XML declaration
                    //XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    //xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);

                    return xmlDoc.InnerXml;
                }
            }
            catch (Exception ex)
            {
                // Handle serialization exceptions
                Console.WriteLine($"Error in CreateXml: {ex.Message}");
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        //public static string CreateXml(Object YourClassObject)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
        //                                              // Initializes a new instance of the XmlDocument class.          
        //    XmlSerializer xmlSerializer = new XmlSerializer(YourClassObject.GetType());
        //    // Creates a stream whose backing store is memory. 
        //    using (MemoryStream xmlStream = new MemoryStream())
        //    {
        //        xmlSerializer.Serialize(xmlStream, YourClassObject);
        //        xmlStream.Position = 0;
        //        //Loads the XML document from the specified string.
        //        xmlDoc.Load(xmlStream);
        //        return xmlDoc.InnerXml;
        //    }
        //}

        // Function to format date string from "dd-MM-yyyy" to "dd-MMM-yyyy"
        public static string FormatDate(string dateString)
        {
            if (dateString == null) return null;

            // Parse the received date string into a DateTime object using the specified format
            DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // Format the DateTime object as "dd-MMM-yyyy"
            string formattedDate = date.ToString("dd-MMM-yyyy");

            // Return the formatted date
            return formattedDate;
        }

        public static DateTime? ParseDateTime(string dateToParse, string[] formats = null, IFormatProvider provider = null, DateTimeStyles styles = DateTimeStyles.None)
        {
            var CUSTOM_DATE_FORMATS = new string[]
            {
            "yyyyMMddTHHmmssZ",
            "yyyyMMddTHHmmZ",
            "yyyyMMddTHHmmss",
            "yyyyMMddTHHmm",
            "yyyyMMddHHmmss",
            "yyyyMMddHHmm",
            "yyyyMMdd",
            "yyyy-MM-ddTHH-mm-ss",
            "yyyy-MM-dd-HH-mm-ss",
            "yyyy-MM-dd-HH-mm",
            "yyyy-MM-dd",
            "MM-dd-yyyy",
            "dd-MMM-yyyy"    // New format added

            };

            if (formats == null || !formats.Any())
            {
                formats = CUSTOM_DATE_FORMATS;
            }

            DateTime validDate;

            foreach (var format in formats)
            {
                if (format.EndsWith("Z"))
                {
                    if (DateTime.TryParseExact(dateToParse, format,
                             provider,
                             DateTimeStyles.AssumeUniversal,
                             out validDate))
                    {
                        return validDate;
                    }
                }

                if (DateTime.TryParseExact(dateToParse, format,
                         provider, styles, out validDate))
                {
                    return validDate;
                }
            }

            return null;
        }

        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public static string StringManipulatorExtractFirstTag(string input)
        {
            // Regular expression pattern to match HTML/XML-like tags
            string pattern = @"<[^>]+>";

            // Find the first match in the input string
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                // Return the matched tag
                return match.Value;
            }
            else
            {
                // Return an empty string if no match is found
                return input.ToString();
            }
        }
    }
}
