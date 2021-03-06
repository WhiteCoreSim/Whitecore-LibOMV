/*
 * Copyright (c) 2006-2016, openmetaverse.co
 * All rights reserved.
 *
 * - Redistribution and use in source and binary forms, with or without
 *   modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * - Neither the name of the openmetaverse.co nor the names
 *   of its contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace OpenMetaverse.StructuredData
{
    /// <summary>
    /// 
    /// </summary>
    public enum OSDType
    {
        /// <summary></summary>
        Unknown,
        /// <summary></summary>
        Boolean,
        /// <summary></summary>
        Integer,
        /// <summary></summary>
        Real,
        /// <summary></summary>
        String,
        /// <summary></summary>
        UUID,
        /// <summary></summary>
        Date,
        /// <summary></summary>
        URI,
        /// <summary></summary>
        Binary,
        /// <summary></summary>
        Map,
        /// <summary></summary>
        Array
    }

    public enum OSDFormat
    {
        Xml = 0,
        Json,
        Binary
    }

    /// <summary>
    /// 
    /// </summary>
    public class OSDException : Exception
    {
        public OSDException(string message) : base(message) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class OSD
    {
        public virtual OSDType Type { get { return OSDType.Unknown; } }

        public virtual bool AsBoolean() { return false; }
        public virtual int AsInteger() { return 0; }
        public virtual uint AsUInteger() { return 0; }
        public virtual long AsLong() { return 0; }
        public virtual ulong AsULong() { return 0; }
        public virtual double AsReal() { return 0d; }
        public virtual string AsString() { return string.Empty; }
        public virtual UUID AsUUID() { return UUID.Zero; }
        public virtual DateTime AsDate() { return Utils.Epoch; }
        public virtual Uri AsUri() { return null; }
        public virtual byte[] AsBinary() { return Utils.EmptyBytes; }
        public virtual Vector2 AsVector2() { return Vector2.Zero; }
        public virtual Vector3 AsVector3() { return Vector3.Zero; }
        public virtual Vector3d AsVector3d() { return Vector3d.Zero; }
        public virtual Vector4 AsVector4() { return Vector4.Zero; }
        public virtual Quaternion AsQuaternion() { return Quaternion.Identity; }
        public virtual Color4 AsColor4() { return Color4.Black; }
        public virtual OSD Copy() { return new OSD(); }

        public override string ToString() { return "undef"; }

        public static OSD FromBoolean(bool value) { return new OSDBoolean(value); }
        public static OSD FromInteger(int value) { return new OSDInteger(value); }
        public static OSD FromInteger(uint value) { return new OSDInteger((int)value); }
        public static OSD FromInteger(short value) { return new OSDInteger(value); }
        public static OSD FromInteger(ushort value) { return new OSDInteger(value); }
        public static OSD FromInteger(sbyte value) { return new OSDInteger(value); }
        public static OSD FromInteger(byte value) { return new OSDInteger(value); }
        public static OSD FromUInteger(uint value) { return new OSDBinary(value); }
        public static OSD FromLong(long value) { return new OSDBinary(value); }
        public static OSD FromULong(ulong value) { return new OSDBinary(value); }
        public static OSD FromReal(double value) { return new OSDReal(value); }
        public static OSD FromReal(float value) { return new OSDReal(value); }
        public static OSD FromString(string value) { return new OSDString(value); }
        public static OSD FromUUID(UUID value) { return new OSDUUID(value); }
        public static OSD FromDate(DateTime value) { return new OSDDate(value); }
        public static OSD FromUri(Uri value) { return new OSDUri(value); }
        public static OSD FromBinary(byte[] value) { return new OSDBinary(value); }

        public static OSD FromVector2(Vector2 value)
        {
            var array = new OSDArray
            {
                FromReal(value.X),
                FromReal(value.Y)
            };
            return array;
        }

        public static OSD FromVector3(Vector3 value)
        {
            var array = new OSDArray
            {
                FromReal(value.X),
                FromReal(value.Y),
                FromReal(value.Z)
            };

            return array;
        }

        public static OSD FromVector3d(Vector3d value)
        {
            var array = new OSDArray
            {
                FromReal(value.X),
                FromReal(value.Y),
                FromReal(value.Z)
            };
            return array;
        }

        public static OSD FromVector4(Vector4 value)
        {
            var array = new OSDArray
            {
                FromReal(value.X),
                FromReal(value.Y),
                FromReal(value.Z),
                FromReal(value.W)
            };
            return array;
        }

        public static OSD FromQuaternion(Quaternion value)
        {
            var array = new OSDArray
            {
                FromReal(value.X),
                FromReal(value.Y),
                FromReal(value.Z),
                FromReal(value.W)
            };
            return array;
        }

        public static OSD FromColor4(Color4 value)
        {
            var array = new OSDArray
            {
               FromReal(value.R),
                FromReal(value.G),
                FromReal(value.B),
                FromReal(value.A)
            };
            return array;
        }

        public static OSD FromObject(object value)
        {
            if (value == null) { return new OSD(); }
            if (value is bool) { return new OSDBoolean((bool)value); }
            if (value is int) { return new OSDInteger((int)value); }
            if (value is uint) { return new OSDBinary((uint)value); }
            if (value is short) { return new OSDInteger((short)value); }
            if (value is ushort) { return new OSDInteger((ushort)value); }
            if (value is sbyte) { return new OSDInteger((sbyte)value); }
            if (value is byte) { return new OSDInteger((byte)value); }
            if (value is double) { return new OSDReal((double)value); }
            if (value is float) { return new OSDReal((float)value); }
            if (value is string) { return new OSDString((string)value); }
            if (value is UUID) { return new OSDUUID((UUID)value); }
            if (value is DateTime) { return new OSDDate((DateTime)value); }
            if (value is Uri) { return new OSDUri((Uri)value); }
            if (value is byte[]) { return new OSDBinary((byte[])value); }
            if (value is long) { return new OSDBinary((long)value); }
            if (value is ulong) { return new OSDBinary((ulong)value); }
            if (value is Vector2) { return FromVector2((Vector2)value); }
            if (value is Vector3) { return FromVector3((Vector3)value); }
            if (value is Vector3d) { return FromVector3d((Vector3d)value); }
            if (value is Vector4) { return FromVector4((Vector4)value); }
            if (value is Quaternion) { return FromQuaternion((Quaternion)value); }
            if (value is Color4) { return FromColor4((Color4)value); }
            return new OSD();
        }

        public static object ToObject(Type type, OSD value)
        {
            if (type == typeof(ulong))
            {
                if (value.Type == OSDType.Binary)
                {
                    byte[] bytes = value.AsBinary();
                    return Utils.BytesToUInt64(bytes);
                }
                return (ulong)value.AsInteger();
            }
            if (type == typeof(uint))
            {
                if (value.Type == OSDType.Binary)
                {
                    byte[] bytes = value.AsBinary();
                    return Utils.BytesToUInt(bytes);
                }
                return (uint)value.AsInteger();
            }
            if (type == typeof(ushort))
            {
                return (ushort)value.AsInteger();
            }
            if (type == typeof(byte))
            {
                return (byte)value.AsInteger();
            }
            if (type == typeof(short))
            {
                return (short)value.AsInteger();
            }
            if (type == typeof(string))
            {
                return value.AsString();
            }
            if (type == typeof(bool))
            {
                return value.AsBoolean();
            }
            if (type == typeof(float))
            {
                return (float)value.AsReal();
            }
            if (type == typeof(double))
            {
                return value.AsReal();
            }
            if (type == typeof(int))
            {
                return value.AsInteger();
            }
            if (type == typeof(UUID))
            {
                return value.AsUUID();
            }
            if (type == typeof(Vector3))
            {
                if (value.Type == OSDType.Array)
                    return ((OSDArray)value).AsVector3();
                return Vector3.Zero;
            }
            if (type == typeof(Vector4))
            {
                if (value.Type == OSDType.Array)
                    return ((OSDArray)value).AsVector4();
                return Vector4.Zero;
            }
            if (type == typeof(Quaternion))
            {
                if (value.Type == OSDType.Array)
                    return ((OSDArray)value).AsQuaternion();
                return Quaternion.Identity;
            }
            if (type == typeof(OSDArray))
            {
                var newArray = new OSDArray();
                foreach (OSD o in (OSDArray)value)
                    newArray.Add(o);
                return newArray;
            }
            if (type == typeof(OSDMap))
            {
                var newMap = new OSDMap();
                foreach (KeyValuePair<string, OSD> o in (OSDMap)value)
                    newMap.Add(o);
                return newMap;
            }
            return null;
        }

        #region Implicit Conversions

        public static implicit operator OSD(bool value) { return new OSDBoolean(value); }
        public static implicit operator OSD(int value) { return new OSDInteger(value); }
        public static implicit operator OSD(uint value) { return new OSDInteger((int)value); }
        public static implicit operator OSD(short value) { return new OSDInteger(value); }
        public static implicit operator OSD(ushort value) { return new OSDInteger(value); }
        public static implicit operator OSD(sbyte value) { return new OSDInteger(value); }
        public static implicit operator OSD(byte value) { return new OSDInteger(value); }
        public static implicit operator OSD(long value) { return new OSDBinary(value); }
        public static implicit operator OSD(ulong value) { return new OSDBinary(value); }
        public static implicit operator OSD(double value) { return new OSDReal(value); }
        public static implicit operator OSD(float value) { return new OSDReal(value); }
        public static implicit operator OSD(string value) { return new OSDString(value); }
        public static implicit operator OSD(UUID value) { return new OSDUUID(value); }
        public static implicit operator OSD(DateTime value) { return new OSDDate(value); }
        public static implicit operator OSD(Uri value) { return new OSDUri(value); }
        public static implicit operator OSD(byte[] value) { return new OSDBinary(value); }
        public static implicit operator OSD(Vector2 value) { return FromVector2(value); }
        public static implicit operator OSD(Vector3 value) { return FromVector3(value); }
        public static implicit operator OSD(Vector3d value) { return FromVector3d(value); }
        public static implicit operator OSD(Vector4 value) { return FromVector4(value); }
        public static implicit operator OSD(Quaternion value) { return FromQuaternion(value); }
        public static implicit operator OSD(Color4 value) { return FromColor4(value); }

        public static implicit operator bool(OSD value) { return value.AsBoolean(); }
        public static implicit operator int(OSD value) { return value.AsInteger(); }
        public static implicit operator uint(OSD value) { return value.AsUInteger(); }
        public static implicit operator long(OSD value) { return value.AsLong(); }
        public static implicit operator ulong(OSD value) { return value.AsULong(); }
        public static implicit operator double(OSD value) { return value.AsReal(); }
        public static implicit operator float(OSD value) { return (float)value.AsReal(); }
        public static implicit operator string(OSD value) { return value.AsString(); }
        public static implicit operator UUID(OSD value) { return value.AsUUID(); }
        public static implicit operator DateTime(OSD value) { return value.AsDate(); }
        public static implicit operator Uri(OSD value) { return value.AsUri(); }
        public static implicit operator byte[](OSD value) { return value.AsBinary(); }
        public static implicit operator Vector2(OSD value) { return value.AsVector2(); }
        public static implicit operator Vector3(OSD value) { return value.AsVector3(); }
        public static implicit operator Vector3d(OSD value) { return value.AsVector3d(); }
        public static implicit operator Vector4(OSD value) { return value.AsVector4(); }
        public static implicit operator Quaternion(OSD value) { return value.AsQuaternion(); }
        public static implicit operator Color4(OSD value) { return value.AsColor4(); }

        #endregion Implicit Conversions

        /// <summary>
        /// Uses reflection to create an SDMap from all of the SD
        /// serializable types in an object
        /// </summary>
        /// <param name="obj">Class or struct containing serializable types</param>
        /// <returns>An SDMap holding the serialized values from the
        /// container object</returns>
        public static OSDMap SerializeMembers(object obj)
        {
            Type t = obj.GetType();
            FieldInfo[] fields = t.GetFields();

            var map = new OSDMap(fields.Length);

            foreach (FieldInfo field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(NonSerializedAttribute)))
                {
                    OSD serializedField = FromObject(field.GetValue(obj));

                    if (serializedField.Type != OSDType.Unknown
                        || field.FieldType == typeof(string)
                        || field.FieldType == typeof(byte[]))
                    {
                        map.Add(field.Name, serializedField);
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// Uses reflection to deserialize member variables in an object from
        /// an SDMap
        /// </summary>
        /// <param name="obj">Reference to an object to fill with deserialized
        /// values</param>
        /// <param name="serialized">Serialized values to put in the target
        /// object</param>
        public static void DeserializeMembers(ref object obj, OSDMap serialized)
        {
            Type t = obj.GetType();
            FieldInfo[] fields = t.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(NonSerializedAttribute)))
                {
                    OSD serializedField;
                    if (serialized.TryGetValue(field.Name, out serializedField))
                        field.SetValue(obj, ToObject(field.FieldType, serializedField));
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class OSDBoolean : OSD
    {
        readonly bool m_bool;

        static readonly byte[] trueBinary = { 0x31 };
        static readonly byte[] falseBinary = { 0x30 };

        public override OSDType Type { get { return OSDType.Boolean; } }

        public OSDBoolean(bool value)
        {
            m_bool = value;
        }

        public override bool AsBoolean() { return m_bool; }
        public override int AsInteger() { return m_bool ? 1 : 0; }
        public override double AsReal() { return m_bool ? 1d : 0d; }
        public override string AsString() { return m_bool ? "1" : "0"; }
        public override byte[] AsBinary() { return m_bool ? trueBinary : falseBinary; }
        public override OSD Copy() { return new OSDBoolean(m_bool); }

        public override string ToString() { return AsString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OSDInteger : OSD
    {
        readonly int m_int;

        public override OSDType Type { get { return OSDType.Integer; } }

        public OSDInteger(int value)
        {
            m_int = value;
        }

        public override bool AsBoolean() { return m_int != 0; }
        public override int AsInteger() { return m_int; }
        public override uint AsUInteger() { return (uint)m_int; }
        public override long AsLong() { return m_int; }
        public override ulong AsULong() { return (ulong)m_int; }
        public override double AsReal() { return m_int; }
        public override string AsString() { return m_int.ToString(); }
        public override byte[] AsBinary() { return Utils.IntToBytesBig(m_int); }
        public override OSD Copy() { return new OSDInteger(m_int); }

        public override string ToString() { return AsString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OSDReal : OSD
    {
        readonly double m_double;

        public override OSDType Type { get { return OSDType.Real; } }

        public OSDReal(double value)
        {
            m_double = value;
        }

        public override bool AsBoolean() { return (!double.IsNaN(m_double) && Math.Abs (m_double) > 0.0000001f); }
        public override OSD Copy() { return new OSDReal(m_double); }
       
        public override int AsInteger()
        {
            if (double.IsNaN(m_double))
                return 0;
            if (m_double > int.MaxValue)
                return int.MaxValue;
            if (m_double < int.MinValue)
                return int.MinValue;
            return (int)Math.Round(m_double);
        }

        public override uint AsUInteger()
        {
            if (double.IsNaN(m_double))
                return 0;
            if (m_double > uint.MaxValue)
                return uint.MaxValue;
            if (m_double < uint.MinValue)
                return uint.MinValue;
            return (uint)Math.Round(m_double);
        }

        public override long AsLong()
        {
            if (double.IsNaN(m_double))
                return 0;
            if (m_double > long.MaxValue)
                return long.MaxValue;
            if (m_double < long.MinValue)
                return long.MinValue;
            return (long)Math.Round(m_double);
        }

        public override ulong AsULong()
        {
            if (double.IsNaN(m_double))
                return 0;
            if (m_double > ulong.MaxValue)
                return ulong.MaxValue;
            if (m_double < ulong.MinValue)
                return ulong.MinValue;
            return (ulong)Math.Round(m_double);
        }

        public override double AsReal() { return m_double; }
        // "r" ensures the value will correctly round-trip back through Double.TryParse
        public override string AsString() { return m_double.ToString("r", Utils.EnUsCulture); }
        public override byte[] AsBinary() { return Utils.DoubleToBytesBig(m_double); }
        public override string ToString() { return AsString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OSDString : OSD
    {
        readonly string m_string;

        public override OSDType Type { get { return OSDType.String; } }

        public override OSD Copy() { return new OSDString(m_string); }

        public OSDString(string value)
        {
            // Refuse to hold null pointers
            m_string = value ?? string.Empty;
        }

        public override bool AsBoolean()
        {
            if (string.IsNullOrEmpty(m_string))
                return false;

            return m_string != "0" && m_string.ToLower() != "false";
        }

        public override int AsInteger()
        {
            double dbl;
            if (double.TryParse (m_string, out dbl))
                return (int)Math.Floor (dbl);
            return 0;
        }

        public override uint AsUInteger()
        {
            double dbl;
            if (double.TryParse (m_string, out dbl))
                return (uint)Math.Floor (dbl);
            return 0;
        }

        public override long AsLong()
        {
            double dbl;
            if (double.TryParse(m_string, out dbl))
                return (long)Math.Floor(dbl);
            return 0;
        }

        public override ulong AsULong()
        {
            double dbl;
            if (double.TryParse (m_string, out dbl))
                return (ulong)Math.Floor (dbl);
            return 0;
        }

        public override double AsReal()
        {
            double dbl;
            return double.TryParse(m_string, out dbl) ? dbl : 0d;
        }

        public override string AsString() { return m_string; }
        public override byte[] AsBinary() { return Encoding.UTF8.GetBytes(m_string); }
        public override UUID AsUUID()
        {
            UUID uuid;
            return UUID.TryParse(m_string, out uuid) ? uuid : UUID.Zero;
        }
        public override DateTime AsDate()
        {
            DateTime dt;
            return DateTime.TryParse(m_string, out dt) ? dt : Utils.Epoch;
        }
        public override Uri AsUri()
        {
            Uri uri;
            return Uri.TryCreate(m_string, UriKind.RelativeOrAbsolute, out uri) ? uri : null;
        }

        public override string ToString() { return AsString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OSDUUID : OSD
    {
        readonly UUID m_uuid;

        public override OSDType Type { get { return OSDType.UUID; } }

        public OSDUUID(UUID value)
        {
            m_uuid = value;
        }

        public override OSD Copy() { return new OSDUUID(m_uuid); }
        public override bool AsBoolean() { return (m_uuid != UUID.Zero); }
        public override string AsString() { return m_uuid.ToString(); }
        public override UUID AsUUID() { return m_uuid; }
        public override byte[] AsBinary() { return m_uuid.GetBytes(); }
        public override string ToString() { return AsString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OSDDate : OSD
    {
        readonly DateTime m_dt;

        public override OSDType Type { get { return OSDType.Date; } }

        public OSDDate(DateTime value)
        {
            m_dt = value;
        }

        public override string AsString()
        {
            var format = m_dt.Millisecond > 0 ? "yyyy-MM-ddTHH:mm:ss.ffZ" : "yyyy-MM-ddTHH:mm:ssZ";
            return m_dt.ToUniversalTime().ToString(format);
        }

        public override int AsInteger()
        {
            return (int)Utils.DateTimeToUnixTime(m_dt);
        }

        public override uint AsUInteger()
        {
            return Utils.DateTimeToUnixTime(m_dt);
        }

        public override long AsLong()
        {
            return Utils.DateTimeToUnixTime(m_dt);
        }

        public override ulong AsULong()
        {
            return Utils.DateTimeToUnixTime(m_dt);
        }

        public override byte[] AsBinary()
        {
            TimeSpan ts = m_dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Utils.DoubleToBytes(ts.TotalSeconds);
        }

        public override OSD Copy() { return new OSDDate(m_dt); }
        public override DateTime AsDate() { return m_dt; }
        public override string ToString() { return AsString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OSDUri : OSD
    {
        readonly Uri m_uri;

        public override OSDType Type { get { return OSDType.URI; } }

        public OSDUri(Uri value)
        {
            m_uri = value;
        }

        public override string AsString()
        {
            if (m_uri == null) return string.Empty;
            return m_uri.IsAbsoluteUri ? m_uri.AbsoluteUri : m_uri.ToString();
        }

        public override OSD Copy() { return new OSDUri(m_uri); }
        public override Uri AsUri() { return m_uri; }
        public override byte[] AsBinary() { return Encoding.UTF8.GetBytes(AsString()); }
        public override string ToString() { return AsString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OSDBinary : OSD
    {
        readonly byte [] m_bin;

        public override OSDType Type { get { return OSDType.Binary; } }

        public OSDBinary(byte[] value)
        {
            m_bin = value ?? Utils.EmptyBytes;
        }

        public OSDBinary(uint value)
        {
            m_bin = new byte[]
            {
                (byte)((value >> 24) % 256),
                (byte)((value >> 16) % 256),
                (byte)((value >> 8) % 256),
                (byte)(value % 256)
            };
        }

        public OSDBinary(long value)
        {
            m_bin = new byte[]
            {
                (byte)((value >> 56) % 256),
                (byte)((value >> 48) % 256),
                (byte)((value >> 40) % 256),
                (byte)((value >> 32) % 256),
                (byte)((value >> 24) % 256),
                (byte)((value >> 16) % 256),
                (byte)((value >> 8) % 256),
                (byte)(value % 256)
            };
        }

        public OSDBinary(ulong value)
        {
            m_bin = new byte[]
            {
                (byte)((value >> 56) % 256),
                (byte)((value >> 48) % 256),
                (byte)((value >> 40) % 256),
                (byte)((value >> 32) % 256),
                (byte)((value >> 24) % 256),
                (byte)((value >> 16) % 256),
                (byte)((value >> 8) % 256),
                (byte)(value % 256)
            };
        }

        public override OSD Copy() { return new OSDBinary(m_bin); }
        public override string AsString() { return Convert.ToBase64String(m_bin); }
        public override byte[] AsBinary() { return m_bin; }

        public override uint AsUInteger()
        {
            return (uint)(
                (m_bin[0] << 24) +
                (m_bin[1] << 16) +
                (m_bin[2] << 8) +
                (m_bin[3] << 0));
        }

        public override long AsLong()
        {
            return (
                ((long)m_bin[0] << 56) +
                ((long)m_bin[1] << 48) +
                ((long)m_bin[2] << 40) +
                ((long)m_bin[3] << 32) +
                ((long)m_bin[4] << 24) +
                ((long)m_bin[5] << 16) +
                ((long)m_bin[6] << 8) +
                ((long)m_bin[7] << 0));
        }

        public override ulong AsULong()
        {
            return (
                ((ulong)m_bin[0] << 56) +
                ((ulong)m_bin[1] << 48) +
                ((ulong)m_bin[2] << 40) +
                ((ulong)m_bin[3] << 32) +
                ((ulong)m_bin[4] << 24) +
                ((ulong)m_bin[5] << 16) +
                ((ulong)m_bin[6] << 8) +
                ((ulong)m_bin[7] << 0));
        }

        public override string ToString()
        {
            return Utils.BytesToHexString(m_bin, null);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OSDMap : OSD, IDictionary<string, OSD>
    {
        readonly Dictionary<string, OSD> m_map;

        public override OSDType Type { get { return OSDType.Map; } }

        public OSDMap()
        {
            m_map = new Dictionary<string, OSD>();
        }

        public OSDMap(int capacity)
        {
            m_map = new Dictionary<string, OSD>(capacity);
        }

        public OSDMap(Dictionary<string, OSD> value)
        {
            m_map = value ?? new Dictionary<string, OSD>();
        }

        public override bool AsBoolean() { return m_map.Count > 0; }

        public override string ToString()
        {
            return OSDParser.SerializeJsonString(this, true);
        }

        public override OSD Copy()
        {
            return new OSDMap(new Dictionary<string, OSD>(m_map));
        }

        #region IDictionary Implementation

        public int Count { get { return m_map.Count; } }
        public bool IsReadOnly { get { return false; } }
        public ICollection<string> Keys { get { return m_map.Keys; } }
        public ICollection<OSD> Values { get { return m_map.Values; } }
        public OSD this[string key]
        {
            get
            {
                OSD llsd;
                return m_map.TryGetValue(key, out llsd) ? llsd : new OSD();
            }
            set { m_map[key] = value; }
        }

        public bool ContainsKey(string key)
        {
            return m_map.ContainsKey(key);
        }

        public void Add(string key, OSD llsd)
        {
            m_map.Add(key, llsd);
        }

        public void Add(KeyValuePair<string, OSD> kvp)
        {
            m_map.Add(kvp.Key, kvp.Value);
        }

        public bool Remove(string key)
        {
            return m_map.Remove(key);
        }

        public bool TryGetValue(string key, out OSD llsd)
        {
            return m_map.TryGetValue(key, out llsd);
        }

        public void Clear()
        {
            m_map.Clear();
        }

        public bool Contains(KeyValuePair<string, OSD> kvp)
        {
            // This is a bizarre function... we don't really implement it
            // properly, hopefully no one wants to use it
            return m_map.ContainsKey(kvp.Key);
        }

        public void CopyTo(KeyValuePair<string, OSD>[] array, int index)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, OSD> kvp)
        {
            return m_map.Remove(kvp.Key);
        }

        public IDictionaryEnumerator GetEnumerator ()
        {
            return m_map.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, OSD>> IEnumerable<KeyValuePair<string, OSD>>.GetEnumerator()
        {
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_map.GetEnumerator();
        }

        #endregion IDictionary Implementation
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OSDArray : OSD, IList<OSD>
    {
        readonly List<OSD> m_arry;

        public override OSDType Type { get { return OSDType.Array; } }

        public OSDArray()
        {
            m_arry = new List<OSD>();
        }

        public OSDArray(int capacity)
        {
            m_arry = new List<OSD>(capacity);
        }

        public OSDArray(List<OSD> value)
        {
            m_arry = value ?? new List<OSD>();
        }

        public override byte[] AsBinary()
        {
            byte[] binary = new byte[m_arry.Count];

            for (int i = 0; i < m_arry.Count; i++)
                binary[i] = (byte)m_arry[i].AsInteger();

            return binary;
        }

        public override long AsLong()
        {
            var binary = new OSDBinary(AsBinary());
            return binary.AsLong();
        }

        public override ulong AsULong()
        {
            var binary = new OSDBinary(AsBinary());
            return binary.AsULong();
        }

        public override uint AsUInteger()
        {
            var binary = new OSDBinary(AsBinary());
            return binary.AsUInteger();
        }

        public override Vector2 AsVector2()
        {
            Vector2 vector = Vector2.Zero;

            if (Count == 2)
            {
                vector.X = (float)this[0].AsReal();
                vector.Y = (float)this[1].AsReal();
            }

            return vector;
        }

        public override Vector3 AsVector3()
        {
            Vector3 vector = Vector3.Zero;

            if (Count == 3)
            {
                vector.X = (float)this[0].AsReal();
                vector.Y = (float)this[1].AsReal();
                vector.Z = (float)this[2].AsReal();
            }

            return vector;
        }

        public override Vector3d AsVector3d()
        {
            Vector3d vector = Vector3d.Zero;

            if (Count == 3)
            {
                vector.X = this[0].AsReal();
                vector.Y = this[1].AsReal();
                vector.Z = this[2].AsReal();
            }

            return vector;
        }

        public override Vector4 AsVector4()
        {
            Vector4 vector = Vector4.Zero;

            if (Count == 4)
            {
                vector.X = (float)this[0].AsReal();
                vector.Y = (float)this[1].AsReal();
                vector.Z = (float)this[2].AsReal();
                vector.W = (float)this[3].AsReal();
            }

            return vector;
        }

        public override Quaternion AsQuaternion()
        {
            Quaternion quaternion = Quaternion.Identity;

            if (Count == 4)
            {
                quaternion.X = (float)this[0].AsReal();
                quaternion.Y = (float)this[1].AsReal();
                quaternion.Z = (float)this[2].AsReal();
                quaternion.W = (float)this[3].AsReal();
            }

            return quaternion;
        }

        public override Color4 AsColor4()
        {
            Color4 color = Color4.Black;

            if (Count == 4)
            {
                color.R = (float)this[0].AsReal();
                color.G = (float)this[1].AsReal();
                color.B = (float)this[2].AsReal();
                color.A = (float)this[3].AsReal();
            }

            return color;
        }

        public override OSD Copy()
        {
            return new OSDArray(new List<OSD>(m_arry));
        }

        public override bool AsBoolean() { return m_arry.Count > 0; }

        public override string ToString()
        {
            return OSDParser.SerializeJsonString(this, true);
        }

        #region IList Implementation

        public int Count { get { return m_arry.Count; } }
        public bool IsReadOnly { get { return false; } }
        public OSD this[int index]
        {
            get { return m_arry[index]; }
            set { m_arry[index] = value; }
        }

        public int IndexOf(OSD llsd)
        {
            return m_arry.IndexOf(llsd);
        }

        public void Insert(int index, OSD llsd)
        {
            m_arry.Insert(index, llsd);
        }

        public void RemoveAt(int index)
        {
            m_arry.RemoveAt(index);
        }

        public void Add(OSD llsd)
        {
            m_arry.Add(llsd);
        }

        public void Clear()
        {
            m_arry.Clear();
        }

        public bool Contains(OSD llsd)
        {
            return m_arry.Contains(llsd);
        }

        public bool Contains(string element)
        {
            foreach (var el in m_arry)
            {
                if (el.Type == OSDType.String && el.AsString() == element)
                    return true;
            }

            return false;
        }

        public void CopyTo(OSD[] array, int index)
        {
            throw new NotImplementedException();
        }

        public bool Remove(OSD llsd)
        {
            return m_arry.Remove(llsd);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_arry.GetEnumerator();
        }

        IEnumerator<OSD> IEnumerable<OSD>.GetEnumerator()
        {
            return m_arry.GetEnumerator();
        }

        #endregion IList Implementation
    }

    public partial class OSDParser
    {
        const string LLSD_BINARY_HEADER = "<? llsd/binary ?>";
        const string LLSD_XML_HEADER = "<llsd>";
        const string LLSD_XML_ALT_HEADER = "<?xml";
        const string LLSD_XML_ALT2_HEADER = "<? llsd/xml ?>";

        public static OSD Deserialize(byte[] data)
        {
            string header = Encoding.ASCII.GetString(data, 0, data.Length >= 17 ? 17 : data.Length);

            try
            {
                string uHeader = Encoding.UTF8.GetString(data, 0, data.Length >= 17 ? 17 : data.Length).TrimStart();
                if (uHeader.StartsWith(LLSD_XML_HEADER, StringComparison.InvariantCultureIgnoreCase) ||
                    uHeader.StartsWith(LLSD_XML_ALT_HEADER, StringComparison.InvariantCultureIgnoreCase) ||
                    uHeader.StartsWith(LLSD_XML_ALT2_HEADER, StringComparison.InvariantCultureIgnoreCase))
                {
                    return DeserializeLLSDXml(data);
                }
            }
            catch { }

            if (header.StartsWith (LLSD_BINARY_HEADER, StringComparison.InvariantCultureIgnoreCase)) {
                return DeserializeLLSDBinary (data);
            }
            if (header.StartsWith (LLSD_XML_HEADER, StringComparison.InvariantCultureIgnoreCase) ||
                            header.StartsWith (LLSD_XML_ALT_HEADER, StringComparison.InvariantCultureIgnoreCase) ||
                            header.StartsWith (LLSD_XML_ALT2_HEADER, StringComparison.InvariantCultureIgnoreCase)) {
                return DeserializeLLSDXml (data);
            }
            return DeserializeJson (Encoding.UTF8.GetString (data));
        }

        public static OSD Deserialize(string data)
        {
            if (data.StartsWith (LLSD_BINARY_HEADER, StringComparison.InvariantCultureIgnoreCase)) {
                return DeserializeLLSDBinary (Encoding.UTF8.GetBytes (data));
            }
            if (data.StartsWith (LLSD_XML_HEADER, StringComparison.InvariantCultureIgnoreCase) ||
                            data.StartsWith (LLSD_XML_ALT_HEADER, StringComparison.InvariantCultureIgnoreCase) ||
                            data.StartsWith (LLSD_XML_ALT2_HEADER, StringComparison.InvariantCultureIgnoreCase)) {
                return DeserializeLLSDXml (data);
            }
            return DeserializeJson (data);
        }

        public static OSD Deserialize(Stream stream)
        {
            if (stream.CanSeek) {
                byte [] headerData = new byte [14];
                stream.Read (headerData, 0, 14);
                stream.Seek (0, SeekOrigin.Begin);
                string header = Encoding.ASCII.GetString (headerData);

                if (header.StartsWith (LLSD_BINARY_HEADER, StringComparison.Ordinal))
                    return DeserializeLLSDBinary (stream);
                if (header.StartsWith (LLSD_XML_HEADER, StringComparison.Ordinal) ||
                                         header.StartsWith (LLSD_XML_ALT_HEADER, StringComparison.Ordinal) ||
                                         header.StartsWith (LLSD_XML_ALT2_HEADER, StringComparison.Ordinal))
                    return DeserializeLLSDXml (stream);
                return DeserializeJson (stream);
            }
            throw new OSDException ("Cannot deserialize structured data from unseekable streams");
        }
    }
}
