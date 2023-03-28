using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Integration.Abstract.Helpers;

namespace Integration.Abstract
{
    public abstract class ConversionFunctions
    {
        public static Connection ContextConnection 
        { 
            get 
            {
                return (Connection)CallContext.GetData("Connection");
            }
        }

        /// <summary>
        /// Returns the first non-null element
        /// </summary>
        /// <param name="list">A list of objects (of any length)</param>
        /// <example>Coalesce(ADDL_DESCR_2, ADDL_DESCR_1)</example>
        /// <returns>object</returns>
        public static object Coalesce(params object[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null)
                    return list[i];
            }
            return null;
        }

        /// <summary>
        /// Returns the first non-null element, converted to a DateTime
        /// </summary>
        /// <param name="list">A list of objects (of any length)</param>
        /// <example>CoalesceToDateTime(PROF_DAT_1, PROF_DAT_2)</example>
        /// <returns>DateTime</returns>
        public static DateTime CoalesceToDateTime(params object[] list)
        {
            return Convert.ToDateTime(Coalesce(list));
        }

        /// <summary>
        /// Returns the first non-null element, converted to a DateTime
        /// </summary>
        /// <param name="list">A list of objects (of any length)</param>
        /// <example>CoalesceToDecimal(QuantityOnHand, 0)</example>
        /// <returns>decimal</returns>
        public static decimal CoalesceToDecimal(params object[] list)
        {
            return Convert.ToDecimal(Coalesce(list));
        }

        /// <summary>
        /// Returns the first non-null element, converted to an Int
        /// </summary>
        /// <param name="list">A list of objects (of any length)</param>
        /// <example>CoalesceToInt(QuantitySold, 0)</example>
        /// <returns>Int</returns>
        public static int CoalesceToInt(params object[] list)
        {
            //Note we have to do a little trickery here, due to some subtleties in how C# handles type conversions.
            //if the value were a non-int, e.g. 82.5 and we tried to do Convert.ToInt32() on it, that would give us the following
            //error message: Input string was not in a correct format
            //BUT, in general, we want to just convert 82.5 to 82 and save that int value. There's no int-based way to do exactly that, 
            //so we convert it to a double, take the FLOOR, then cast the double to an int, which gives us a truncated int parse function.
            //Note that due to the FLOOR, we always round down, e.g. 82.1 becomes 82 and 82.8 also becomes 82.
            var retVal = Convert.ToInt32(Math.Floor(Convert.ToDouble(Coalesce(list))));

            return retVal;
        }

        /// <summary>
        /// Convert an object to a long (int64)
        /// </summary>
        /// <param name="obj">An object that can be converted to a long</param>
        /// <example>ConvertToLong(DOC_ID)</example>
        /// <returns>long</returns>
        public static long ConvertToLong(object obj)
        {
            return Convert.ToInt64(obj);
        }


        /// <summary>
        /// Convert an object to an int
        /// </summary>
        /// <param name="obj">An object that can be converted to an int</param>
        /// <example>ConvertToInt("7")</example>
        /// <returns>int</returns>
        public static int ConvertToInt(object obj)
        {
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// Convert an object to a decimal
        /// </summary>
        /// <param name="obj">An object that can be converted to a decimal</param>
        /// <example>ConvertToDecimal("7.5")</example>
        /// <returns>decimal</returns>
        public static decimal ConvertToDecimal(object obj)
        {
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        /// Covnert an object to a double
        /// </summary>
        /// <param name="obj">An object that can be converted to a double</param>
        /// <example>ConvertToDouble("7.5")</example>
        /// <returns>double</returns>
        public static double ConvertToDouble(object obj)
        {
            return Convert.ToDouble(obj);
        }

        /// <summary>
        /// Allows for a nullable- and type-safe multiplication.
        /// </summary>
        /// <param name="a">An object that can be converted to a double</param>
        /// <param name="b">An object that can be converted to a double</param>
        /// <example>TypesafeMultiplication(PRC_1, QTY_SOLD)</example>
        /// <returns>double</returns>
        public static double TypesafeMultiplication(object a, object b)
        {
            if (a == null)
                return 0;

            if (b == null)
                return 0;

            return Convert.ToDouble(a) * Convert.ToDouble(b);
        }

        /// <summary>
        /// Allows for a nullable- and type-safe division.
        /// </summary>
        /// <param name="numerator">An object that can be converted to a double</param>
        /// <param name="denomonator">An object that can be converted to a double. Cannot be null.</param>
        /// <example>TypesafeDivision(EXT_PRC, QTY_SOLD)</example>
        /// <returns>double</returns>
        public static double TypesafeDivision(object numerator, object denomonator)
        {
            if (numerator == null)
                return 0;

            if (denomonator == null)
                throw new Exception("Error in TypesafeDivision call: deonomonator was null.");

            if (Convert.ToDouble(denomonator) == 0)
                throw new Exception("Error in TypesafeDivision call: attempt to divide by zero.");

            return Convert.ToDouble(numerator) / Convert.ToDouble(denomonator);
        }

        /// <summary>
        /// Returns the first object in a collection
        /// </summary>
        /// <param name="inputList">An ojbect that implements IEnumerable</param>
        /// <example>First(Barcodes)</example>
        /// <returns>object</returns>
        public static object First(IEnumerable inputList)
        {
            if (inputList == null)
                return null;

            foreach (object candidate in inputList)
            {
                //Just return the first one
                return candidate;
            }

            return null;
        }

        /// <summary>
        /// Converts the input to a DateTime or DateTimeOffset and then returns only the date portion, in a DateTime.
        /// </summary>
        /// <param name="input"></param>
        /// <example>DateOnly(ExpirationDate)</example>
        /// <returns>DateTime (nullable)</returns>
        public static DateTime? DateOnly(object input)
        {
            //First check if we actually have a value
            if (input == null)
                return null;

            //If the input is already a DateTime or DateTimeOffset, we just extract the date and return that.
            if (input is DateTime)
                return ((DateTime)input).Date;
            if (input is DateTimeOffset)
                return ((DateTimeOffset)input).Date;

            //If the data is any other type, we convert it to a string and try to parse that, then extract the date
            var input_str = Convert.ToString(input);
            if (input_str == "0") //Some APIs treat null DateTimes as 0s, so we check for that
                return null;

            if (DateTime.TryParse(input_str, out DateTime dateTime))
                return ((DateTime)input).Date;

            if (DateTimeOffset.TryParse(input_str, out DateTimeOffset dateTimeOffset))
                return ((DateTimeOffset)input).Date;

            //Many dates coming from BigCommerce are in the format of a unix timestamp (e.g. the number of seconds since 1/1/1970). To convert 
            //  we make sure that the number we get is 9 or 10 chars long and parses to a long int. 9-10 chars ensures that we are in the range of
            //  1975ish to 2286 or so, which should cover what we want.
            if ((input_str.Length == 9 || input_str.Length == 10) && Int64.TryParse(input_str, out Int64 intLong))
            {
                DateTimeOffset dateTimeOffsetFromUnix = DateTimeOffset.FromUnixTimeSeconds(intLong);
                return DateOnly(dateTimeOffsetFromUnix);
            }

            return null;
        }


        /// <summary>
        /// Convert a DateTimeOffset to a DateTime in the local time zone
        /// </summary>
        /// <param name="input">A DateTimeOffset value</param>
        /// <example>LocalDateTimeFromDateTimeOffset(ImportedDateTimeOffset)</example>
        /// <returns>DateTime (nullable)</returns>
        public static DateTime? LocalDateTimeFromDateTimeOffset(object input)
        {
            ValidateDataType(input, typeof(DateTimeOffset), "LocalDateTimeFromDateTimeOffset", "input");
            var dateTimeOffset = (DateTimeOffset)input;

            if (dateTimeOffset == DateTimeOffset.MinValue)
                return null;

            return dateTimeOffset.LocalDateTime;
        }

        /// <summary>
        /// Convert a DateTime in the local time zone to a DateTimeOffset
        /// </summary>
        /// <param name="input">A DateTime value</param>
        /// <example>DateTimeOffsetFromLocalDateTime(LST_MAINT_DT)</example>
        /// <returns>DateTimeOffset</returns>
        public static DateTimeOffset DateTimeOffsetFromLocalDateTime(object input)
        {
            ValidateDataType(input, typeof(DateTime), "DateTimeOffsetFromLocalDateTime", "input");
            var localDateTime = (DateTime)input;

            return new DateTimeOffset(localDateTime, TimeZoneInfo.Local.GetUtcOffset(localDateTime));
        }

        /// <summary>
        /// Returns the current DateTime in the local time zone
        /// </summary>
        /// <example>CurrentDateTime()</example>
        /// <returns>DateTime</returns>
        public static DateTime CurrentDateTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Returns the minimum DateTime (aka DateTime.MinValue)
        /// </summary>
        /// <example>MinimumDateTime()</example>
        /// <returns>DateTime</returns>
        public static DateTime MinimumDateTime()
        {
            return DateTime.MinValue;
        }

        /// <summary>
        /// Returns the maximum DateTime (aka DateTime.MaxValue)
        /// </summary>
        /// <example>MaximumDateTime()</example>
        /// <returns>DateTime</returns>
        public static DateTime MaximumDateTime()
        {
            return DateTime.MaxValue;
        }

        /// <summary>
        /// Returns the current DateTimeOffset in the local time zone
        /// </summary>
        /// <example>CurrentDateTimeOffset()</example>
        /// <returns>DateTimeOffset</returns>
        public static DateTimeOffset CurrentDateTimeOffset()
        {
            return DateTimeOffset.Now;
        }


        /// <summary>
        /// This will allow you to turn a string into a list of integers. This function is useful for fields that take in an array of ints but are coming from a string field 
        ///     (e.g. static mapping or a flat string field such as "[1,2]") 
        /// </summary>
        /// <param name="input">a string that can be parsed into a list of strings</param>
        /// <example>IntListFromString("1,2,3")</example>
        /// <returns>List&lt;int&gt;</returns>
        public static List<int> IntListFromString(object input)
        {
            ValidateDataType(input, typeof(string), "IntListFromString", "input");
            var intListStr = Convert.ToString(input);

            if (String.IsNullOrEmpty(intListStr))
                return null;

            intListStr = (intListStr.StartsWith("[") ? intListStr.Substring(1) : intListStr);
            intListStr = (intListStr.EndsWith("]") ? intListStr.Substring(0, intListStr.Length - 1) : intListStr);
            var tmp = intListStr.Split(',');

            int[] array = intListStr.Split(',').Select(str => int.Parse(str)).ToArray();
            return array.ToList();
        }

        /// <summary>
        /// This will allow you to turn a string into a list of strings. This function is useful for fields that take an array of strings but are coming from a flat string field 
        ///     (e.g. static mapping or a flat string field, "[keyword1, keyword2]")
        /// </summary>
        /// <remarks>INTERNAL ONLY - One weakness of this function is that it does not notice embedded quotes or commas. E.g. ["string, string2", string3]</remarks> would have three strings: 1) "string  2) string2"  and 3) string3
        /// <param name="input">An object that can be converted to a string</param>
        /// <returns>List&lt;string&gt;</returns>
        public static List<string> StringListFromString(object input)
        {
            ValidateDataType(input, typeof(string), "StringListFromString", "input");
            var strListStr = Convert.ToString(input);

            if (String.IsNullOrEmpty(strListStr))
                return null;

            strListStr = (strListStr.StartsWith("[") ? strListStr.Substring(1) : strListStr);
            strListStr = (strListStr.EndsWith("]") ? strListStr.Substring(0, strListStr.Length - 1) : strListStr);
            var tmp = strListStr.Split(',');

            string[] array = strListStr.Split(',').ToArray();
            return array.ToList();
        }

        /// <summary>
        /// Return the larger of two numeric objects. Flee does not handle nullable types well e.g decimal? > decimal,
        ///  so this lets us do a simple comparison of two values and keep the largest.
        /// </summary>
        /// <param name="a">An object that can be converted to a decimal</param>
        /// <param name="b">An object that can be converted to a decimal</param>
        /// <example>Larger(QtyOnHnd,0) would allow us to send the qty on hand, as long as it's non-negative</example>
        /// <returns>decimal</returns>
        public static decimal Larger(object a, object b)
        {
            if (a == null && b == null)
                return 0;

            if (a == null)
                return Convert.ToDecimal(b);

            if (b == null)
                return Convert.ToDecimal(a);

            ValidateDataType(a, typeof(decimal), "Larger", "a");
            ValidateDataType(b, typeof(decimal), "Larger", "b");

            var a_decimal = Convert.ToDecimal(a);
            var b_decimal = Convert.ToDecimal(b);

            if (a_decimal > b_decimal)
                return a_decimal;
            return b_decimal;
        }

        /// <summary>
        /// Parse a JSON string and convert it to a string/string dictionary
        /// </summary>
        /// <param name="input">An object that can be converted to a string</param>
        /// <example>JSONToDictionary("{"GiftCardId":"" + GiftCardNumber(GiftCertificate) + ""}")</example>
        /// <returns>Dictionary&lt;string, string&gt;</returns>
        public static Dictionary<string, string> JSONToDictionary(object input)
        {
            if (input == null)
                return null;

            //Validate and convert the parameter
            ValidateDataType(input, typeof(string), "JSONToDictionary", "input");
            var json = Convert.ToString(input);

            var retVal = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return retVal;
        }

        /// <summary>
        /// Returns the value for the specified key from a dictionary. If the key does not exist, it returns null
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns>string</returns>
        /// <example>ReadFromStringDictionary(LineInfo, "GiftCardId")</example>
        public static string ReadFromStringDictionary(object dictionary, object key)
        {
            //Validate and convert the parameters
            ValidateDataType(dictionary, typeof(Dictionary<string, string>), "ReadFromStringDictionary", "dictionary");
            var dictionaryInternal = (Dictionary<string, string>)dictionary;

            //Validate and convert the parameters
            ValidateDataType(key, typeof(string), "ReadFromStringDictionary", "key");
            var keyInternal = (string)key;

            if (dictionaryInternal == null)
                return null;

            if (dictionaryInternal.ContainsKey(keyInternal))
                return dictionaryInternal[keyInternal];
            return null;
        }

        //This function is provided to parse ID fields into constituent parts. E.g. if we have DIR-L78-JWT-6GQ|1, we might want to grab the 1 as a sequence number.
        /// <summary>
        /// This function returns the portion of a string after the last occurance of a given character. This is useful for parsing sections of compound IDs. 
        ///     E.g. if we have DIR-L78-JWT-6GQ|1, we might want to grab the 1 as a sequence number.
        /// </summary>
        /// <param name="haystack">An object that can be converted to a string</param>
        /// <param name="needle">An object that can be converted to a string</param>
        /// <example>SubstringAfterLastMatch("DIR-L78-JWT-6GQ|1", "|")</example>
        /// <returns>string</returns>
        public static string SubstringAfterLastMatch(object haystack, object needle)
        {
            //Validate and convert the parameters
            ValidateDataType(haystack, typeof(string), "SubstringAfterLastMatch", "haystack");
            var haystackInternal = Convert.ToString(haystack);

            ValidateDataType(needle, typeof(string), "SubstringAfterLastMatch", "needle");
            var needleInternal = Convert.ToString(needle);

            if (string.IsNullOrEmpty(haystackInternal) || string.IsNullOrEmpty(needleInternal))
                return null;

            //Note we use LastIndexOf 
            if (haystackInternal.LastIndexOf(needleInternal) == -1)
                return null;

            //Truncate everything before the search string, then add chars for the length of the search string.
            haystackInternal = haystackInternal.Substring(haystackInternal.LastIndexOf(needleInternal) + needleInternal.Length);

            return haystackInternal;
        }

        /// <summary>
        /// This function provides a safe way to remove a given prefix. IF the prefix is not present, no changes are made. E.g., if we want to turn BC123 into 123, we can call RemovePrefix("BC", "BC123").
        /// </summary>
        /// <param name="prefix">An object that can be converted into a string</param>
        /// <param name="value">An object that can be converted into a string</param>
        /// <example>RemovePrefix("BC", "BC123")</example>
        /// <returns>string</returns>
        public static string RemovePrefix(object prefix, object value)
        {
            //Validate and convert the parameters
            ValidateDataType(prefix, typeof(string), "RemovePrefix", "prefix");
            var prefixInternal = Convert.ToString(prefix);

            ValidateDataType(value, typeof(string), "RemovePrefix", "value");
            var valueInternal = Convert.ToString(value);

            //If we don't start with the prefix, just return the value as-is
            if (!valueInternal.StartsWith(prefixInternal))
                return valueInternal;

            return valueInternal.Substring(prefixInternal.Length);
        }


        /// <summary>
        /// Determine if the input string matches the given pattern in RegEx.
        /// </summary>
        /// <param name="input">An object that can be converted to a string</param>
        /// <param name="pattern">An object that can be converted to a string</param>
        /// <example>Regex("The dog is running", "^The")</example>
        /// <returns>bool</returns>
        public static bool RegExMatch(object input, object pattern)
        {
            //Validate and convert the parameters
            ValidateDataType(input, typeof(string), "RegExMatch", "input");
            var inputInternal = Convert.ToString(input);

            ValidateDataType(pattern, typeof(string), "RegExMatch", "pattern");
            var patternInternal = Convert.ToString(pattern);

            var regexMatchResults = System.Text.RegularExpressions.Regex.Match(inputInternal, patternInternal);
            return regexMatchResults.Success;
        }

        //

        /// <summary>
        /// This function is intended to replicate SQL-style likeness comparison. E.g. WildcardMatch("test1234", "%est1%") would return true
        ///  We accomplish this by replacign the SQL wildcard with a regex wildcard .* (meaning one or more of any character) and passing it to our regex
        ///  function. Any non-regex friendly input would not be supported. E.g. WildcardMatch("abcdefg", "%[1]%") would return true because of it's regex conversion
        /// </summary>
        /// <param name="input">An object that can be converted to a string</param>
        /// <param name="pattern">An object that can be converted to a string</param>
        /// <example>WildcardMatch("abcdefg", "%[1]%")</example>
        /// <returns>bool</returns>
        public static bool WildcardMatch(object input, object pattern)
        {
            //Validate and convert the parameters
            ValidateDataType(input, typeof(string), "RegExMatch", "input");
            var inputInternal = Convert.ToString(input);

            ValidateDataType(pattern, typeof(string), "RegExMatch", "pattern");
            var patternInternal = Convert.ToString(pattern);

            string RegExPattern = patternInternal.Replace("%", ".*");
            return RegExMatch(inputInternal, RegExPattern);
        }

        /// <summary>
        /// Truncates the input at the 
        /// </summary>
        /// <param name="input">An object that can be converted to a string</param>
        /// <param name="maxLength">An object that can be converted to an int</param>
        /// <example>Truncate("This is a really long string that I need to shorten", 10)</example>
        /// <returns>string</returns>
        public static string Truncate(object input, object maxLength)
        {
            if (input == null)
                return null;

            //Validate and convert the parameters
            ValidateDataType(maxLength, typeof(int), "Truncate", "maxLength");
            var maxLengthInternal = Convert.ToInt32(maxLength);

            ValidateDataType(input, typeof(string), "Truncate", "input");
            var inputInternal = Convert.ToString(input);

            if (inputInternal.Length <= maxLengthInternal)
                return inputInternal;

            return inputInternal.Substring(0, maxLengthInternal);
        }

        /// <summary>
        /// Determines if the input object is one of several values indicating empty.
        /// Null and DBNull will return true. For non-nullable types with edge default values, we also return true. E.g. these type values all indicate an empty value:
        ///     Guid.Empty, DateTime.MinValue, DateTimeOffset.MinValue, "" (empty string)
        /// </summary>
        /// <param name="input">An object of any type</param>
        /// <example>IsEmpty(DESCR)</example>
        /// <returns>bool</returns>
        public static bool IsEmpty(object input)
        {
            if (input == null)
                return true;

            if (DBNull.Value == input)
                return true;

            if (input is Guid)
                return (Guid)input == Guid.Empty;

            if (input is DateTime)
                return (DateTime)input == DateTime.MinValue;

            if (input is DateTimeOffset)
                return (DateTimeOffset)input == DateTimeOffset.MinValue;

            if (input is string)
                return string.IsNullOrEmpty(input.ToString());

            if (input is decimal)
                return (decimal)input == 0;

            return false;
        }

        /// <summary>
        /// Determines if the passed object is null or not
        /// </summary>
        /// <param name="value">An object of any type</param>
        /// <example>IsNull(CustomFields)</example>
        /// <returns>bool</returns>
        public static bool IsNull(object value)
        {
            if (value == null)
                return true;

            return false;
        }

        /// <summary>
        /// Convert an object to XML and return the XML as a string
        /// </summary>
        /// <param name="o"></param>
        /// <returns>string</returns>
        /// <remarks>DONOTEXPORT</remarks>
        //Note: I believe this is in the wrong place. It is used by the Klevu integration and not by any formulas. It should likely be moved to a different class, as this is only for 
        //  FLEE formula functions.
        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            tw = new XmlTextWriter(sw);
            serializer.Serialize(tw, o);
            sw.Close();
            if (tw != null)
            {
                tw.Close();
            }
            return sw.ToString();
        }

        /// <summary>
        /// Determine if the supplied object belongs to the specified type. If it is not, this proc may throw an error to indicate the problem.
        /// Recommended usage of this procedure would be to include the method and parameter names and leave throwError as true. However, there are
        /// cases where you may not need to throw an error (e.g. you are checking to see if the data is one of multiple types), in which case the method and
        /// parameter name fields can be excluded and the throwError flag should be set to true.
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="dataType"></param>
        /// <param name="methodName"></param>
        /// <param name="parameterName"></param>
        /// <param name="throwError"></param>
        /// <returns>bool</returns>
        /// <remarks>DONOTEXPORT</remarks>
        public static bool ValidateDataType(object candidate, Type dataType, string methodName = null, string parameterName = null, bool throwError = true)
        {
            //There are a few types that we can attempt to convert manually. E.g. if the dataType is a decimal and an int was passed, that will not pass the instance of check, 
            //  but we can attempt to manually convert it.
            if (dataType == typeof(int))
            {
                try
                {
                    Convert.ToInt32(candidate); //If this line does not throw an error, we have a candidate that can be converted to an int and we do not throw an error.
                    return true;
                }
                catch { return ValidateDatType_ThrowFormattedException(candidate, dataType, methodName, parameterName, throwError); }
            }
            else if (dataType == typeof(decimal))
            {
                try
                {
                    Convert.ToDecimal(candidate); //If this line does not throw an error, we have a candidate that can be converted to an int and we do not throw an error.
                    return true;
                }
                catch { return ValidateDatType_ThrowFormattedException(candidate, dataType, methodName, parameterName, throwError); }
            }
            else if (dataType == typeof(double))
            {
                try
                {
                    Convert.ToDouble(candidate); //If this line does not throw an error, we have a candidate that can be converted to an int and we do not throw an error.
                    return true;
                }
                catch { return ValidateDatType_ThrowFormattedException(candidate, dataType, methodName, parameterName, throwError); }
            }
            else if (dataType == typeof(long))
            {
                try
                {
                    Convert.ToInt64(candidate); //If this line does not throw an error, we have a candidate that can be converted to an int and we do not throw an error.
                    return true;
                }
                catch { return ValidateDatType_ThrowFormattedException(candidate, dataType, methodName, parameterName, throwError); }
            }
            else if (dataType == typeof(string))
            {
                try
                {
                    Convert.ToString(candidate); //If this line does not throw an error, we have a candidate that can be converted to an int and we do not throw an error.
                    return true;
                }
                catch { return ValidateDatType_ThrowFormattedException(candidate, dataType, methodName, parameterName, throwError); }
            }
            else
            {
                //If the request is for a non-simple type and the candidate is null, then we can't actually determine if the type matches, so we assume it does.
                //  This allows null handling to be handled as needed in each conversion function.
                if (candidate == null)
                    return true;

                //If we are aren't testing a basic type, check the IsInstanceOfType call. This should return true for any match or inheritor of the requested class.
                if (!dataType.IsInstanceOfType(candidate))
                {
                    return ValidateDatType_ThrowFormattedException(candidate, dataType, methodName, parameterName, throwError);
                }
            }

            return true;
        }

        //A private helper function for ValidateDatType. If we are expected the throw an error, we will format one appropriately.
        private static bool ValidateDatType_ThrowFormattedException(object candidate, Type dataType, string methodName, string parameterName, bool throwError)
        {
            if (throwError)
            {
                if (!string.IsNullOrEmpty(methodName) && !string.IsNullOrEmpty(parameterName))
                    throw new Exception($"Invalid parameter passed to the method {methodName}. The parameter {parameterName} is expected to be of the type {dataType.Name} but is the type {candidate.GetType().Name}");
                else
                    throw new Exception($"Invalid parameter. The parameter is expected to be of the type {dataType.Name} but is the type {candidate.GetType().Name}");
            }
            else
                return false;
        }
    }
}
