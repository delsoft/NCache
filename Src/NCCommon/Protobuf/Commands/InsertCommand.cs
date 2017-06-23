// Copyright (c) 2017 Alachisoft
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: InsertCommand.proto
// Note: requires additional types generated from: QueryInfo.proto
// Note: requires additional types generated from: ObjectQueryInfo.proto
namespace Alachisoft.NCache.Common.Protobuf
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"InsertCommand")]
  public partial class InsertCommand : global::ProtoBuf.IExtensible
  {
    public InsertCommand() {}
    

    private string _key = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"key", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string key
    {
      get { return _key; }
      set { _key = value; }
    }

    private long _requestId = default(long);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"requestId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long requestId
    {
      get { return _requestId; }
      set { _requestId = value; }
    }

    private int _updateCallbackId = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"updateCallbackId", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int updateCallbackId
    {
      get { return _updateCallbackId; }
      set { _updateCallbackId = value; }
    }

    private int _removeCallbackId = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"removeCallbackId", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int removeCallbackId
    {
      get { return _removeCallbackId; }
      set { _removeCallbackId = value; }
    }

    private int _priority = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"priority", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int priority
    {
      get { return _priority; }
      set { _priority = value; }
    }

    private long _absExpiration = default(long);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"absExpiration", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long absExpiration
    {
      get { return _absExpiration; }
      set { _absExpiration = value; }
    }

    private long _sldExpiration = default(long);
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"sldExpiration", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(long))]
    public long sldExpiration
    {
      get { return _sldExpiration; }
      set { _sldExpiration = value; }
    }

    private int _flag = default(int);
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"flag", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int flag
    {
      get { return _flag; }
      set { _flag = value; }
    }

    private string _lockId = "";
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"lockId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string lockId
    {
      get { return _lockId; }
      set { _lockId = value; }
    }

    private int _lockAccessType = default(int);
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"lockAccessType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int lockAccessType
    {
      get { return _lockAccessType; }
      set { _lockAccessType = value; }
    }

    private Alachisoft.NCache.Common.Protobuf.QueryInfo _queryInfo = null;
    [global::ProtoBuf.ProtoMember(11, IsRequired = false, Name=@"queryInfo", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public Alachisoft.NCache.Common.Protobuf.QueryInfo queryInfo
    {
      get { return _queryInfo; }
      set { _queryInfo = value; }
    }
    private readonly global::System.Collections.Generic.List<byte[]> _data = new global::System.Collections.Generic.List<byte[]>();
    [global::ProtoBuf.ProtoMember(12, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<byte[]> data
    {
      get { return _data; }
    }
  

    private Alachisoft.NCache.Common.Protobuf.ObjectQueryInfo _objectQueryInfo = null;
    [global::ProtoBuf.ProtoMember(13, IsRequired = false, Name=@"objectQueryInfo", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public Alachisoft.NCache.Common.Protobuf.ObjectQueryInfo objectQueryInfo
    {
      get { return _objectQueryInfo; }
      set { _objectQueryInfo = value; }
    }
    private readonly global::System.Collections.Generic.List<byte[]> _objectQueryInfoEncrypted = new global::System.Collections.Generic.List<byte[]>();
    [global::ProtoBuf.ProtoMember(14, Name=@"objectQueryInfoEncrypted", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<byte[]> objectQueryInfoEncrypted
    {
      get { return _objectQueryInfoEncrypted; }
    }
  

    private int _updateDataFilter = (int)-1;
    [global::ProtoBuf.ProtoMember(15, IsRequired = false, Name=@"updateDataFilter", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
    [global::System.ComponentModel.DefaultValue((int)-1)]
    public int updateDataFilter
    {
      get { return _updateDataFilter; }
      set { _updateDataFilter = value; }
    }

    private int _removeDataFilter = (int)-1;
    [global::ProtoBuf.ProtoMember(16, IsRequired = false, Name=@"removeDataFilter", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
    [global::System.ComponentModel.DefaultValue((int)-1)]
    public int removeDataFilter
    {
      get { return _removeDataFilter; }
      set { _removeDataFilter = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
