using System;

namespace BusinessLayer.Helpers
{
    public static class ActualType
    {
        public static Type GetActualTypeForUsage(this Types actualTypes, Type insertedType)
        {
            Type foundType;
            if (insertedType.BaseType.Name.Equals("object", StringComparison.InvariantCultureIgnoreCase) &&
                !insertedType.BaseType.Name.StartsWith("Identity", StringComparison.InvariantCultureIgnoreCase))
            {
                foundType = actualTypes
                    .GetType()
                    .GetProperty(insertedType.Name)?
                    .GetValue(actualTypes) as Type;


                if (foundType == null)
                    foundType = insertedType;
            }
            else
            {
                foundType = insertedType;
            }

            return foundType;
        }
    }
}