using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MVCBlog.Website.Code
{
    public class SiteMethods
    {
        public static string Substring(string str, int length)
        {
            if (str.Length > length)
                return (str.Substring(0, length));
            else
                return str;
        }

        public static string LeftSubstringChar(string str, int length)
        {
            if (str.Length > length)
                return (str.Substring(str.IndexOf('-') + 1, length));
            else
                return str;
        }

        public static string LeftSubstring(string str, int length)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > length)
                return (str.Substring(0, length)).Trim();
            else
                return str;
        }

        public static string RightSubstring(string str, int length)
        {
            if (str.Length > length)
                return (str.Substring(str.Length - length, length)).Trim();
            else
                return str;
        }

        public static string[] ConvertGuidArrayToString(Guid[] guids)
        {
            List<string> l = new List<string>();
            foreach (Guid item in guids)
                l.Add(item.ToString());
            return l.ToArray();
        }

        public static string CleanInput(string strIn)
        {
            return strIn.Replace(" [ ", " ").Replace(" ^ ", " ").Replace(" & ", " ").Replace(" . ", " ")
                .Replace(" ¿ ", " ").Replace(" * ", " ").Replace(" ] ", " ").Replace(" ' ", " ")
                .Replace(" < ", " ").Replace(" > ", " ").Replace(" - ", " ").Replace(" _ ", " ")
                .Replace(" | ", " ").Replace(" ? ", " ").Replace(" ; ", " ").Replace(" " + (char)34 + " ", " ")
                .Replace(" " + (char)92 + " ", " ").Replace(" , ", " ").Replace(" { ", " ")
                .Replace(" } ", " ").Replace(" $ ", " ").Replace(" # ", " ").Replace(" @ ", " ")
                .Replace(" + ", " ").Replace(" / ", " ").Replace(" ( ", " ").Replace(" ) ", " ")
                .Replace(" = ", " ").Replace(" % ", " ").Replace(" · ", " ").Replace(" ! ", " ")
                .Replace(" ¡ ", " ");
        }

        public static string ClearNumber(string number)
        {
            string tmp = string.Empty;
            if (!string.IsNullOrEmpty(number))
            {
                foreach (char item in number.ToCharArray())
                    if (IsNumeric(item.ToString()))
                        tmp += item.ToString();

                if (IsNumeric(tmp))
                    tmp = Decimal.Parse(tmp).ToString();
            }
            return tmp;
        }

        public static string normalizeNumber(string s)
        {
            //nos deja nada mas que las numeros
            return Regex.Replace(s, "[^0-9]+", "", RegexOptions.Compiled); ;
        }

        public static string normalizeChar(string s)
        {
            //nos deja nada mas que las letras
            return Regex.Replace(s, "[^a-zA-Z]+", "", RegexOptions.Compiled);
        }

        public static string normalizeCharNumber(string s)
        {
            //nos deja nada mas que las letras y nros
            return Regex.Replace(s, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }

        public static object Coalesce(object s, object o)
        {
            if (s == null || s is System.DBNull || s.ToString() == "")
                return o;
            else
                return s;
        }

        public static double CoalesceDouble(string s)
        {
            if (s.Trim() == "")
                return 0;
            else
                return double.Parse(s);
        }

        public static decimal CoalesceDecimal(object s)
        {
            if (s == null || s is System.DBNull || s.ToString().Trim() == string.Empty)
                return 0m;
            else
                return decimal.Parse(s.ToString());
        }

        public static int CoalesceInt(object s)
        {
            if (s == null || s is System.DBNull || s.ToString().Trim() == string.Empty)
                return 0;
            else
                return int.Parse(s.ToString());
        }

        public static int? parseInt(string s)
        {
            if (IsNumeric(s))
                return int.Parse(s);
            else
                return null;
        }

        public static bool IsNumeric(string s)
        {
            try
            {
                Decimal.Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDateTime(string theValue)
        {
            try
            {
                Convert.ToDateTime(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDecimal(string theValue)
        {
            try
            {
                Convert.ToDouble(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsGuid(object s)
        {
            try
            {
                new Guid(s.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retorna la edad para una fecha ingresada
        /// </summary>
        /// <param name="dateOfBirth">fecha de nacimiento</param>
        /// <param name="dateCompare">fecha que deseas saber la edad</param>
        /// <returns></returns>
        public static Int32 GetAge(DateTime? dateOfBirth, DateTime dateCompare)
        {
            if (dateOfBirth == null)
                return 0;

            var date = Convert.ToDateTime(dateOfBirth);

            var a = (dateCompare.Year * 100 + dateCompare.Month) * 100 + dateCompare.Day;
            var b = (date.Year * 100 + date.Month) * 100 + date.Day;

            return (a - b) / 10000;
        }

        //calcula la edad de una persona
        //public static decimal age(DateTime dob)
        //{
        //    decimal age = ((decimal)Microsoft.VisualBasic.DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Year, dob, DateTime.Now,
        //            Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.Jan1));
        //    decimal tmp = 0;
        //    if (DateTime.Today < DateTime.Parse(dob.Month + "/" + dob.Day + "/" + DateTime.Now.Year))
        //        tmp = (System.Math.Truncate(age) - 1);
        //    else
        //        tmp = System.Math.Truncate(age);
        //    return (tmp < 0 ? 0 : tmp);
        //}

        //public static decimal DateDiff(Microsoft.VisualBasic.DateInterval dateInterval, DateTime datetime1, DateTime datetime2)
        //{
        //    decimal tmp = ((decimal)Microsoft.VisualBasic.DateAndTime.DateDiff(dateInterval, datetime1, datetime2,
        //        Microsoft.VisualBasic.FirstDayOfWeek.Sunday, Microsoft.VisualBasic.FirstWeekOfYear.Jan1));
        //    return tmp;
        //}

        public static int DateDiffInDays(DateTime datetime1, DateTime datetime2)
        {
            TimeSpan ts = datetime1 - datetime2;
            int days = ts.Days;
            return days;
        }

        /// <summary>
        /// Obtiene el Digito Verificador para un numero ingresado
        /// </summary>
        /// <param name="number">largo 11</param>
        /// <returns></returns>
        public static int GetDV(string number)
        {
            // tipo cuota (1) + año (2) + mes (2) + nro. socio (6)
            // Etapa 1: Comenzar desde la izquierda, sumar todos los caracteres ubicados en las posiciones impares. 
            // Etapa 2: Multiplicar la suma obtenida en la etapa 1 por el número 3.
            // Etapa 3: Comenzar desde la izquierda, sumar todos los caracteres que están ubicados en las posiciones pares. 
            // Etapa 4: Sumar los resultados obtenidos en las etapas 2 y 3.
            // Etapa 5: Buscar el menor número que sumado al resultado obtenido en la etapa 4 dé un número múltiplo de 10. Este será el valor del dígito verificador del módulo 10.

            int sumaFinal = 0;

            if (number.Length != 11)
                return -1;

            char[] nro = number.ToArray();

            sumaFinal = (nro[0] + nro[2] + nro[4] + nro[6] + nro[8] + nro[10]) * 3 + nro[1] + nro[3] + nro[5] + nro[7] + nro[9];

            int multiplo = 10;
            while (sumaFinal >= multiplo)
                multiplo = multiplo + 10;

            sumaFinal = multiplo - sumaFinal;

            return sumaFinal > 9 ? 0 : sumaFinal;
        }

        public static string GetBarcode(int type, int year, int month, int? number)
        {
            return string.Format("{0}{1:00}{2:00}{3:000000}", type, year.ToString().Substring(2, 2), month, number);
        }

        public static string GetBarcodeDV(int type, int year, int month, int? number)
        {
            string barcode = GetBarcode(type, year, month, number);
            return barcode + GetDV(barcode);
        }

        public static string LeaveLastThree(string input)
        {
            int lenth = input.Length;
            input = RightSubstring(input, 3);
            for (int i = 0; i < lenth - 3; i++)
            {
                input = "*" + input;
            }
            return input;
        }

        public static DateTime GetFirstDayOfMonth(DateTime input)
        {
            int year = input.Year;
            int month = input.Month;
            return new DateTime(year, month, 1);
        }

        public static DateTime GetLastDayOfMonth(DateTime input)
        {
            int year = input.Year;
            int month = input.Month + 1;
            DateTime day = new DateTime(year, month, 1);
            return day.AddDays(-1);
        }
    }
}