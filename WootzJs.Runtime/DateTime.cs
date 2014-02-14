﻿#region License

//-----------------------------------------------------------------------
// <copyright>
// The MIT License (MIT)
// 
// Copyright (c) 2014 Kirk S Woll
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

#endregion

using System.Runtime.WootzJs;

namespace System
{
    public struct DateTime : IComparable, IFormattable, IConvertible, IComparable<DateTime>, IEquatable<DateTime>
    {
        private JsDate value;
        private DateTimeKind kind;

        internal DateTime(JsDate value, DateTimeKind kind = DateTimeKind.Unspecified)
        {
            this.value = value;
            this.kind = kind;
        }

        /// <summary>
        /// Gets the date component of this instance.
        /// </summary>
        /// 
        /// <returns>
        /// A new object with the same date as this instance, and the time value set to 12:00:00 midnight 
        /// (00:00:00).
        /// </returns>
        public DateTime Date
        {
            get
            {
                var clone = new JsDate(value.getTime());
                clone.setHours(0);
                clone.setMinutes(0);
                clone.setSeconds(0);
                clone.setMilliseconds(0);
                return new DateTime(clone, kind);
            }
        }

        /// <summary>
        /// Gets the day of the month represented by this instance.
        /// </summary>
        /// 
        /// <returns>
        /// The day component, expressed as a value between 1 and 31.
        /// </returns>
        public int Day
        {
            get { return value.getDate(); }
        }

        /// <summary>
        /// Gets the day of the week represented by this instance.
        /// </summary>
        /// 
        /// <returns>
        /// An enumerated constant that indicates the day of the week of this <see cref="T:System.DateTime"/> value.
        /// </returns>
        public DayOfWeek DayOfWeek
        {
            get { return (DayOfWeek)value.getDay(); }
        }

        /// <summary>
        /// Gets the day of the year represented by this instance.
        /// </summary>
        /// 
        /// <returns>
        /// The day of the year, expressed as a value between 1 and 366.
        /// </returns>
        public int DayOfYear
        {
            get
            {
                var now = new JsDate();
                var start = new JsDate(now.getFullYear(), 0);
                var diff = now - start;
                var oneDay = 1000*60*60*24;
                var day = JsMath.ceil(diff/oneDay);
                return day;
            }
        }

        /// <summary>
        /// Gets the hour component of the date represented by this instance.
        /// </summary>
        /// 
        /// <returns>
        /// The hour component, expressed as a value between 0 and 23.
        /// </returns>
        public int Hour
        {
            get { return value.getHours(); }
        }

        /// <summary>
        /// Gets a value that indicates whether the time represented by this instance is based on local time, Coordinated Universal Time (UTC), or neither.
        /// </summary>
        /// 
        /// <returns>
        /// One of the enumeration values that indicates what the current time represents. The default is <see cref="F:System.DateTimeKind.Unspecified"/>.
        /// </returns>
        public DateTimeKind Kind
        {
            get { return kind; }
        }

        /// <summary>
        /// Gets the milliseconds component of the date represented by this instance.
        /// </summary>
        /// 
        /// <returns>
        /// The milliseconds component, expressed as a value between 0 and 999.
        /// </returns>
        public int Millisecond
        {
            get { return value.getMilliseconds(); }
        }

        /// <summary>
        /// Gets the minute component of the date represented by this instance.
        /// </summary>
        /// 
        /// <returns>
        /// The minute component, expressed as a value between 0 and 59.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public int Minute
        {
            get { return value.getMinutes(); }
        }

        /// <summary>
        /// Gets the month component of the date represented by this instance.
        /// </summary>
        /// 
        /// <returns>
        /// The month component, expressed as a value between 1 and 12.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public int Month
        {
            get { return value.getMonth(); }
        }

        /// <summary>
        /// Gets a <see cref="T:System.DateTime"/> object that is set to the current date and time on this computer, expressed as the local time.
        /// </summary>
        /// 
        /// <returns>
        /// An object whose value is the current local date and time.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public static DateTime Now
        {
            get { return new DateTime(new JsDate(), DateTimeKind.Local); }
        }

        /// <summary>
        /// Gets a <see cref="T:System.DateTime"/> object that is set to the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// 
        /// <returns>
        /// An object whose value is the current UTC date and time.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public static DateTime UtcNow
        {
            get
            {
                var now = new JsDate();
                var utcNow = new JsDate(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds());
                return new DateTime(utcNow, DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Gets the seconds component of the date represented by this instance.
        /// </summary>
        /// 
        /// <returns>
        /// The seconds component, expressed as a value between 0 and 59.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public int Second
        {
            get { return value.getSeconds(); }
        }

        /// <summary>
        /// Gets the current date.
        /// </summary>
        /// 
        /// <returns>
        /// An object that is set to today's date, with the time component set to 00:00:00.
        /// </returns>
        public static DateTime Today
        {
            get { return Now.Date; }
        }

        /// <summary>
        /// Gets the year component of the date represented by this instance.
        /// </summary>
        /// 
        /// <returns>
        /// The year, between 1 and 9999.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public int Year
        {
            get { return value.getFullYear(); }
        }

        /// <summary>
        /// Creates a new <see cref="T:System.DateTime"/> object that has the same number of ticks as the specified <see cref="T:System.DateTime"/>, but is designated as either local time, Coordinated Universal Time (UTC), or neither, as indicated by the specified <see cref="T:System.DateTimeKind"/> value.
        /// </summary>
        /// 
        /// <returns>
        /// A new object that has the same number of ticks as the object represented by the <paramref name="value"/> parameter and the <see cref="T:System.DateTimeKind"/> value specified by the <paramref name="kind"/> parameter.
        /// </returns>
        /// <param name="value">A date and time. </param><param name="kind">One of the enumeration values that indicates whether the new object represents local time, UTC, or neither.</param><filterpriority>2</filterpriority>
        public static DateTime SpecifyKind(DateTime value, DateTimeKind kind)
        {
            return new DateTime(value.value, kind);
        }

        public int CompareTo(DateTime other)
        {
            return (int)(value.getTime() - other.value.getTime());
        }

        public int CompareTo(object obj)
        {
            if (!(obj is DateTime))
                return 1;
            return CompareTo((DateTime)obj);
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.DateTime;
        }

        public bool Equals(DateTime other)
        {
            return value.getTime() == other.value.getTime();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return this;
        }

        public override string ToString()
        {
            return value.toString();
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'Boolean'");
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'Char'");
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'SByte'");
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'Byte'");
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'Int16'");
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'UInt16'");
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'Int32'");
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'UInt32'");
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'Int64'");
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'UInt64'");
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'Single'");
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'Double'");
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException("Invalid cast from 'DateTime' to 'Decimal'");
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(DateTime))
                return this;
            else if (conversionType == typeof(string))
                return ToString();
            else
                throw new InvalidCastException("Invalid cast from 'DateTime' to '" + conversionType.Name + "'");
        }
    }
}