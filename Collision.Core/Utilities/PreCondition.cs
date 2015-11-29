using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision.Core.Utilities
{
    public static class PreCondition
    {
        public static T NotNull<T>(this T value, string paramName = null, string errorMessage = null) where T : class
        {
            bool isNull = false;
            if (typeof(T) != typeof(string))
            {
                if (value == null)
                {
                    isNull = true;
                }
            }
            else{
                if(string.IsNullOrWhiteSpace(value as string))
                {
                    isNull = true;
                }
            }

            if (isNull)
            {
                if(string.IsNullOrWhiteSpace(paramName) && string.IsNullOrWhiteSpace(errorMessage))
                {
                    throw new ArgumentNullException();
                }
                else if (string.IsNullOrEmpty(errorMessage))
                {
                    throw new ArgumentNullException(paramName);
                }
                else
                {
                    throw new ArgumentNullException(paramName, errorMessage);
                }
            }

            return value;
        }
    }
}
