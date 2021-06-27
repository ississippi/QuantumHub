using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumHub.Common
{
    public static class DataUtil
    {

        #region Data Conversion Methods
        public static bool IsNull(object param)
        {
            try
            {
                return (param == null) || (param == System.DBNull.Value);
            }
            catch
            {
                return true;
            }
        }

        public static string NullToEmpty(object param)
        {
            if (IsNull(param))
            {
                return "";
            }
            else
            {
                return Convert.ToString(param);
            }
        }

        public static string NullToEmptyTrim(object param)
        {
            if (IsNull(param))
            {
                return "";
            }
            else
            {
                return Convert.ToString(param).Trim();
            }
        }

        public static int NullToZero(object param)
        {
            if (IsNull(param))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(param);
            }

        }

        public static bool NullToBool(object param)
        {
            if (IsNull(param))
            {
                return false;
            }
            else
            {
                string work = NullToEmpty(param);
                return (work == "1") ? true : false;
            }

        }

        public static bool NullYNToBool(object param)
        {
            if (IsNull(param))
            {
                return false;
            }
            else
            {
                string work = NullToEmpty(param);
                return (work == "Y" || work == "y") ? true : false;
            }

        }

        public static int NullToByteZero(object param)
        {
            if (IsNull(param))
            {
                return 0;
            }
            else
            {
                return (Byte)param;
            }

        }

        public static int NullToInt16Zero(object param)
        {
            if (IsNull(param))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt16(param);
            }

        }

        public static double NullToZeroDouble(object param)
        {
            if (IsNull(param))
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(param);
            }
        }

        public static decimal NullToDecimalZero(object param)
        {
            if (IsNull(param))
            {
                return decimal.Zero;
            }
            else
            {
                return Convert.ToDecimal(param);
            }
        }

        /// <summary>
        /// Somebody tell Brian the built-in function that makes this unnecessary.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool IsNumeric(string param)
        {
            try
            {
                double ignore = double.Parse(param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static int NullToIntMinValue(object param)
        {
            if (IsNull(param))
            {
                return int.MinValue;
            }
            else
            {
                return (int)param;
            }

        }

        public static decimal NullToDecimalMinValue(object param)
        {
            if (IsNull(param))
            {
                return decimal.MinValue;
            }
            else
            {
                return Convert.ToDecimal(param);
            }
        }

        public static DateTime NullToDateTimeMinValue(object param)
        {
            if (IsNull(param))
            {
                return DateTime.MinValue;
            }
            else
            {
                return (DateTime)param;
            }

        }

        public static object EmptyToNull(string param)
        {
            if (param.Length == 0)
            {
                return null;
            }
            else
            {
                return param;
            }
        }

        #endregion

    }
}
