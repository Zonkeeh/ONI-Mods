// Decompiled with JetBrains decompiler
// Type: Harmony.ILCopying.ByteBuffer
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;

namespace Harmony.ILCopying
{
  public class ByteBuffer
  {
    public byte[] buffer;
    public int position;

    public ByteBuffer(byte[] buffer)
    {
      this.buffer = buffer;
    }

    public byte ReadByte()
    {
      this.CheckCanRead(1);
      return this.buffer[this.position++];
    }

    public byte[] ReadBytes(int length)
    {
      this.CheckCanRead(length);
      byte[] numArray = new byte[length];
      Buffer.BlockCopy((Array) this.buffer, this.position, (Array) numArray, 0, length);
      this.position += length;
      return numArray;
    }

    public short ReadInt16()
    {
      this.CheckCanRead(2);
      short num = (short) ((int) this.buffer[this.position] | (int) this.buffer[this.position + 1] << 8);
      this.position += 2;
      return num;
    }

    public int ReadInt32()
    {
      this.CheckCanRead(4);
      int num = (int) this.buffer[this.position] | (int) this.buffer[this.position + 1] << 8 | (int) this.buffer[this.position + 2] << 16 | (int) this.buffer[this.position + 3] << 24;
      this.position += 4;
      return num;
    }

    public long ReadInt64()
    {
      this.CheckCanRead(8);
      long num = (long) (uint) ((int) this.buffer[this.position + 4] | (int) this.buffer[this.position + 5] << 8 | (int) this.buffer[this.position + 6] << 16 | (int) this.buffer[this.position + 7] << 24) << 32 | (long) (uint) ((int) this.buffer[this.position] | (int) this.buffer[this.position + 1] << 8 | (int) this.buffer[this.position + 2] << 16 | (int) this.buffer[this.position + 3] << 24);
      this.position += 8;
      return num;
    }

    public float ReadSingle()
    {
      if (!BitConverter.IsLittleEndian)
      {
        byte[] numArray = this.ReadBytes(4);
        Array.Reverse((Array) numArray);
        return BitConverter.ToSingle(numArray, 0);
      }
      this.CheckCanRead(4);
      float single = BitConverter.ToSingle(this.buffer, this.position);
      this.position += 4;
      return single;
    }

    public double ReadDouble()
    {
      if (!BitConverter.IsLittleEndian)
      {
        byte[] numArray = this.ReadBytes(8);
        Array.Reverse((Array) numArray);
        return BitConverter.ToDouble(numArray, 0);
      }
      this.CheckCanRead(8);
      double num = BitConverter.ToDouble(this.buffer, this.position);
      this.position += 8;
      return num;
    }

    private void CheckCanRead(int count)
    {
      if (this.position + count > this.buffer.Length)
        throw new ArgumentOutOfRangeException();
    }
  }
}
