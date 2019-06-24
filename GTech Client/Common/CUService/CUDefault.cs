
using System;

namespace GTechnology.Oncor.CustomAPI
{	
    public static class Information
    {
        public static bool IsNumeric(this string text)
        {
            double test;
            return double.TryParse(text, out test);
        }
        public static bool IsDBNull(this object value)
        {           
            return Convert.IsDBNull(value);
        }
    }
}
