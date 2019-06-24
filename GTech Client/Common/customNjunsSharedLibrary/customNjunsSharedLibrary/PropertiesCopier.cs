//----------------------------------------------------------------------------+
//        Class: PropertiesCopier
//  Description: This class is used to copy properties from parent(source) object to child(target) object
//
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 20/03/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: PropertiesCopier.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 20/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

namespace GTechnology.Oncor.CustomAPI
{
    public static class PropertiesCopier
    {
        /// <summary>
        /// Extension method that copies from property values from source to target
        /// </summary>
        /// <param name="self">target class</param>
        /// <param name="parent">source class</param>
        public static void CopyPropertiesFrom(this object self, object parent)
        {
            try
            {


                var fromProperties = parent.GetType().GetProperties();
                var toProperties = self.GetType().GetProperties();
                foreach (var fromProperty in fromProperties)
                {
                    foreach (var toProperty in toProperties)
                    {
                        if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                        {
                            toProperty.SetValue(self, fromProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
