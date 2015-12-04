using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;

namespace CommonLib.Common
{
    public class RawDeserializer
    {
        protected BinaryReader br;

        /// <summary>
        /// Helper instance for reading value types.
        /// </summary>
        protected TypeIO tr;
        protected System.IO.MemoryStream ms = new System.IO.MemoryStream();

        public RawDeserializer()
        {
            br = new BinaryReader(ms);
            tr = new TypeIO();
        }

        public bool DeserializeBool()
        {
            return br.ReadBoolean();
        }

        public byte DeserializeByte()
        {
            return br.ReadByte();
        }

        public byte[] DeserializeBytes()
        {
            int count = br.ReadInt32();
            return br.ReadBytes(count);
        }

        public char DeserializeChar()
        {
            return br.ReadChar();
        }

        public char[] DeserializeChars()
        {
            int count = br.ReadInt32();
            return br.ReadChars(count);
        }

        public decimal DeserializeDecimal()
        {
            return br.ReadDecimal();
        }

        public double DeserializeDouble()
        {
            return br.ReadDouble();
        }

        public short DeserializeShort()
        {
            return br.ReadInt16();
        }

        public int DeserializeInt()
        {
            return br.ReadInt32();
        }

        public long DeserializeLong()
        {
            return br.ReadInt64();
        }

        public sbyte DeserializeSByte()
        {
            return br.ReadSByte();
        }

        public float DeserializeFloat()
        {
            return br.ReadSingle();
        }

        public string DeserializeString()
        {
            return br.ReadString();
        }

        public ushort DeserializeUShort()
        {
            return br.ReadUInt16();
        }

        public uint DeserializeUInt()
        {
            return br.ReadUInt32();
        }

        public ulong DeserializeULong()
        {
            return br.ReadUInt64();
        }

        public Guid DeserializeGuid()
        {
            return (Guid)Deserialize(typeof(Guid));
        }

        public DateTime DeserializeDateTime()
        {
            return (DateTime)Deserialize(typeof(DateTime));
        }

        #region NullableTypes

        // Nullable value type support.

        public bool? DeserializeNBool()
        {
            bool? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadBoolean();
            }

            return ret;
        }

        public byte? DeserializeNByte()
        {
            byte? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadByte();
            }

            return ret;
        }

        public char? DeserializeNChar()
        {
            char? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadChar();
            }

            return ret;
        }

        public decimal? DeserializeNDecimal()
        {
            decimal? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadDecimal();
            }

            return ret;
        }

        public double? DeserializeNDouble()
        {
            double? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadDouble();
            }

            return ret;
        }

        public short? DeserializeNShort()
        {
            short? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadInt16();
            }

            return ret;
        }

        public int? DeserializeNInt()
        {
            int? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadInt32();
            }

            return ret;
        }

        public long? DeserializeNLong()
        {
            long? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadInt64();
            }

            return ret;
        }

        public sbyte? DeserializeNSByte()
        {
            sbyte? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadSByte();
            }

            return ret;
        }

        public float? DeserializeNFloat()
        {
            float? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadSingle();
            }

            return ret;
        }

        public ushort? DeserializeNUShort()
        {
            ushort? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadUInt16();
            }

            return ret;
        }

        public uint? DeserializeNUInt()
        {
            uint? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadUInt32();
            }

            return ret;
        }

        public ulong? DeserializeNULong()
        {
            ulong? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = br.ReadUInt64();
            }

            return ret;
        }

        public DateTime? DeserializeNDateTime()
        {
            DateTime? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = (DateTime?)Deserialize(typeof(DateTime));
            }

            return ret;
        }

        public Guid? DeserializeNGuid()
        {
            Guid? ret = null;

            if (br.ReadByte() == 2)
            {
                ret = (Guid?)Deserialize(typeof(Guid));
            }

            return ret;
        }

        #endregion NullableTypes

        public object Deserialize(Type type)
        {
            bool success;
            object ret = tr.Read(br, type, out success);

            if (!success)
            {
                if (type.IsValueType)
                {
                    int count = br.ReadInt32();
                    byte[] data = br.ReadBytes(count);
                    ret = Deserialize(data, type);
                }
                else
                {
                    throw new RawSerializerException("Cannot deserialize " + type.AssemblyQualifiedName);
                }
            }

            return ret;
        }

        public object DeserializeNullable(Type type)
        {
            object ret = null;

            byte code = br.ReadByte();

            if (code == 0)
            {
                ret = System.DBNull.Value;
            }
            else if (code == 1)
            {
                ret = null;
            }
            else if (code == 2)
            {
                ret = Deserialize(type);
            }
            else
            {
                throw new RawSerializerException("Expected a code byte during deserialization of " + type.Name);
            }

            return ret;
        }

        public virtual object Deserialize(byte[] bytes, Type type)
        {
            object structure = null;

            try
            {
                GCHandle h = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                structure = Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0), type);
                h.Free();
            }
            catch (Exception e)
            {
                throw new RawSerializerException(e.Message);
            }

            return structure;
        }

        public DataSet Deserialize(byte[] buf)
        {
            //ms.Flush();
            //ms.Position = 0;

            ms.Write(buf, 0, buf.Length);
            ms.Flush();
            ms.Position = 0;

            DataSet ds = new DataSet();
            int count = DeserializeInt();

            for (int j = 0; j < count; j++)
            {
                string tableName = DeserializeString();
                int columns = DeserializeInt();
                int rows = DeserializeInt();

                DataTable dtIn = new DataTable(tableName);

                ds.Tables.Add(dtIn);
                //Assertion.Assert(columns == 4, "Column count is wrong.");
                //Assertion.Assert(rows == 3, "Row count is wrong.");

                for (int x = 0; x < columns; x++)
                {
                    string columnName = DeserializeString();
                    bool allowNulls = DeserializeBool();
                    string type = DeserializeString();

                    DataColumn dc = new DataColumn(columnName, Type.GetType(type));
                    dc.AllowDBNull = allowNulls;
                    dtIn.Columns.Add(dc);
                }

                for (int y = 0; y < rows; y++)
                {
                    DataRow dr = dtIn.NewRow();

                    for (int x = 0; x < columns; x++)
                    {
                        DataColumn dc = dtIn.Columns[x];
                        object obj;

                        if (dc.AllowDBNull)
                        {
                            obj = DeserializeNullable(dc.DataType);
                        }
                        else
                        {
                            obj = Deserialize(dc.DataType);
                        }

                        dr[dc] = obj;
                    }

                    dtIn.Rows.Add(dr);
                }
            }

            count = DeserializeInt();
            for (int i = 0; i < count; i++)
            {
                string parentTableName = DeserializeString();
                string childTableName = DeserializeString();
                int parentColumnNum = DeserializeInt();
                int childColumnNum = DeserializeInt();

                DataColumn[] parentColumns = new DataColumn[parentColumnNum];
                DataColumn[] childColumns = new DataColumn[childColumnNum];

                for (int x = 0; x < parentColumnNum; x++)
                {
                    parentColumns[x] = ds.Tables[parentTableName].Columns[DeserializeString()];
                }

                for (int x = 0; x < childColumnNum; x++)
                {
                    childColumns[x] = ds.Tables[childTableName].Columns[DeserializeString()];
                }

                DataRelation rel = new DataRelation(parentTableName, parentColumns, childColumns, false);
                ds.Relations.Add(rel);
            }

            ds.AcceptChanges();

            return ds;
        }
    }
}
