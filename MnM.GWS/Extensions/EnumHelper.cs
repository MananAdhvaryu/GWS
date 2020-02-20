using System;
using System.Collections.Generic;
using System.Text;

namespace MnM.GWS.EnumExtensions
{
    public static class EnumHelper
    {
        #region CURVE TYPE
        public static bool NegativeMotion(this CurveType type) => type.HasFlag(CurveType.NegativeMotion);
        public static bool CrossStroke(this CurveType type) => type.HasFlag(CurveType.CrossStroke);
        public static bool SweepAngle(this CurveType type) => !type.HasFlag(CurveType.NoSweepAngle);
        public static bool IsArc(this CurveType type) =>  type.HasFlag( CurveType.Arc);
        public static bool IsClosedArc(this CurveType type) => type.HasFlag(CurveType.ClosedArc);
        public static bool IsPie(this CurveType type) => type.HasFlag( CurveType.Pie);
        #endregion

        #region DRAW MODE
        public static bool Front(this DrawMode mode) => mode.HasFlag(DrawMode.Front);
        public static bool Back(this DrawMode mode) => mode.HasFlag(DrawMode.Back);
        public static bool Animation(this DrawMode mode) => mode.HasFlag(DrawMode.Animation);
        public static bool ColorKeyIgnore(this DrawMode mode) => mode.HasFlag(DrawMode.ColorKeyIgnore);
        public static bool FloodFill(this DrawMode mode) => mode.HasFlag(DrawMode.FloodFill);
        public static bool IntersectExclude(this DrawMode mode) => mode.HasFlag(DrawMode.IntersectExclude);
        public static bool Mask(this DrawMode mode) => mode.HasFlag(DrawMode.Mask);
        public static bool MixColor(this DrawMode mode) => mode.HasFlag(DrawMode.MixColor);
        #endregion

        #region TYPE CODE
        /// <summary>
        /// To the type.
        /// </summary>
        /// <param name="typecode">The typecode.</param>
        /// <returns>Type.</returns>
        public static Type ToType(this TypeCode typecode)
        {
            switch (typecode)
            {
                case TypeCode.Boolean:
                    return typeof(bool);
                case TypeCode.Byte:
                    return typeof(byte);
                case TypeCode.Char:
                    return typeof(char);
                //case TypeCode.DBNull:
                //    return typeof(DBNull);
                case TypeCode.DateTime:
                    return typeof(DateTime);
                case TypeCode.Decimal:
                    return typeof(decimal);
                case TypeCode.Double:
                    return typeof(double);
                case TypeCode.Empty:
                    return null;
                case TypeCode.Int16:
                    return typeof(Int16);
                case TypeCode.Int32:
                    return typeof(Int32);
                case TypeCode.Int64:
                    return typeof(Int64);
                case TypeCode.Object:
                    return typeof(object);
                case TypeCode.SByte:
                    return typeof(sbyte);
                case TypeCode.Single:
                    return typeof(Single);
                case TypeCode.String:
                    return typeof(string);
                case TypeCode.UInt16:
                    return typeof(UInt16);
                case TypeCode.UInt32:
                    return typeof(UInt32);
                case TypeCode.UInt64:
                    return typeof(UInt64);
                default:
                    return null;
            }
        }

        /// <summary>
        /// To the type code.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>TypeCode.</returns>
        public static TypeCode ToTypeCode(this Type type)
        {
            return System.Type.GetTypeCode(type);
        }

        /// <summary>
        /// Determines whether the specified type is numeric.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is numeric; otherwise, <c>false</c>.</returns>
        public static bool IsNumeric(this TypeCode type)
        {
            switch (type)
            {
                case TypeCode.DateTime:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether [is date time] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is date time] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsDateTime(this TypeCode type)
        {
            switch (type)
            {
                case TypeCode.DateTime:
                    return true;
                default:
                    return false;
            }
        }
        #endregion

        #region GRAPHICS MODE
        public static bool SingleBuffer(this GraphicsMode mode) => 
            !mode.HasFlag(GraphicsMode.SingleBuffer);
        public static bool DoubleBuffer(this GraphicsMode mode) =>
            mode.HasFlag(GraphicsMode.DoubleBuffer);
        public static bool Dicard(this GraphicsMode mode) =>
            mode.HasFlag(GraphicsMode.DiscardChanges);
        public static bool Commit(this GraphicsMode mode) =>
            mode.HasFlag(GraphicsMode.CommitChanges);
        #endregion

        #region INVERT
        /// <summary>
        /// Inverts the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Criteria.</returns>
        public static Criteria Invert(this Criteria criteria)
        {
            int i = criteria.EnumValue<int>();
            return (Criteria)(-i - 1);
        }

        /// <summary>
        /// Inverts the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>MultCriteria.</returns>
        public static MultCriteria Invert(this MultCriteria criteria)
        {
            int i = criteria.EnumValue<int>();
            return (MultCriteria)(-i - 1);
        }
        #endregion

        #region INCLUDE
        /// <summary>
        /// Includes the specified value2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>T.</returns>
        public static T Include<T>(this T value1, T value2)      
        {
            return (T)(object)(Convert.ToInt32(value1) | Convert.ToInt32(value2));
        }

        public static bool Includes<T>(this T value, T valueToCheck) 
        {
            return value.HasFlag(valueToCheck);
        }

        /// <summary>
        /// Includeses the specified value set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="valueSet">The value set.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Includes<T>(this T value, params T[] valueSet)  
        {
            if (valueSet.Length == 0) return false;
            foreach (var item in valueSet)
            {
                if (value.HasFlag(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Includeses the specified first occurance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="firstOccurance">The first occurance.</param>
        /// <param name="valueSet">The value set.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Includes<T>(this Enum value,
            out T firstOccurance, params Enum[] valueSet)   
        {
            firstOccurance = default(T);
            if (valueSet.Length == 0) return false;
            foreach (var item in valueSet)
            {
                if (value.HasFlag(item))
                {
                    firstOccurance = (T)(object)item;
                    return true;
                }
            }
            return false;
        }

       public static bool HasFlag<T>(this T value, T check) 
        {
            var a = Convert.ToInt32(value);
            var b = Convert.ToInt32(check);
            return((a & b) == b);
        }

        /// <summary>
        /// Determines whether [is one of] [the specified value set].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="valueSet">The value set.</param>
        /// <returns><c>true</c> if [is one of] [the specified value set]; otherwise, <c>false</c>.</returns>
        public static bool IsOneOf<T>(this T value, params T[] valueSet)
        {
            foreach (var item in valueSet)
            {
                if (Equals(value, item)) return true;
            }
            return false;
        }
        public static bool IsOneOf<T>(this T? value, params T[] valueSet) where T : struct
        {
            foreach (var item in valueSet)
            {
                if (Equals(value, item)) return true;
            }
            return false;
        }
        #endregion

        #region EXCLUDE
        /// <summary>
        /// Excludes the specified value2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>T.</returns>
        public static T Exclude<T>(this T value1, T value2)  
        {
            return (T)(object)(Convert.ToInt32(value1) & ~Convert.ToInt32(value2));
        }

        public static T Exclude<T>(this T value1, params T[] values)  
        {
            var v = Convert.ToInt32(value1);
            foreach (var v1 in values)
            {
                v = v & ~Convert.ToInt32(v1);
            }
            return (T)(object)v;
        }

        public static T Replace<T>(this T value, T oldValue, T newValue)  
        {
            return (T)(object)((Convert.ToInt32(value) & ~Convert.ToInt32(oldValue))
                | Convert.ToInt32(newValue));
        }

        /// <summary>
        /// Excludeses the specified value set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="valueSet">The value set.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Excludes<T>(this T value, params T[] valueSet)
        {
            if (valueSet.Length == 0) return false;
            foreach (var item in valueSet)
                if (value.HasFlag(item)) 
                    return false;
            
            return true;
        }

         
         
        /// <summary>
        /// Excludeses the specified first occurance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="firstOccurance">The first occurance.</param>
        /// <param name="valueSet">The value set.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Excludes<T>(this T value, out T firstOccurance, params T[] valueSet)
        {
            firstOccurance = default(T);
            if (valueSet.Length == 0) return false;
            foreach (var item in valueSet)
            {
                if (value.HasFlag(item))
                {
                    firstOccurance =(T)(Object) item;
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region get name
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string EnumName<T>(this T value)
        {
            return value.ToString();
        }
        #endregion

        #region get value
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        public static T EnumValue<T>(string value) 
        {
            if (string.IsNullOrEmpty(value)) return default(T);
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch { }
            return default(T);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        public static T EnumValue<T>(int value)  
        {
            try
            {
                return (T)(object)value;
            }
            catch { }
            return default(T);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <returns>T.</returns>
        public static T EnumValue<T>(string value, out bool success)  
        {
            success = false;
            try
            {
                T val = (T)Enum.Parse(typeof(T), value, true);
                success = true;
                return val;
            }
            catch { }
            return default(T);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        public static T EnumValue<T>(this object value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch { }
            return default(T);
        }

        /// <summary>
        /// Gets the concate value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nameprefix">The nameprefix.</param>
        /// <param name="namesuffix">The namesuffix.</param>
        /// <returns>T.</returns>
        public static T ConcateEnumValue<T>(this T nameprefix, T namesuffix)
        {
            string s = EnumName(nameprefix) + EnumName(namesuffix);
            return (T)Enum.Parse(typeof(T), s, true);
        }

        /// <summary>
        /// Gets the name of the concate.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>System.String.</returns>
        public static string ConcateEnumName<T>(this T prefix, T suffix)
        {
            return prefix.EnumName() + suffix.EnumName();
        }
        #endregion
    }
}
