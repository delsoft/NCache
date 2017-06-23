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

// Generated from: ReaderResultSet.proto
// Note: requires additional types generated from: RecordSet.proto
// Note: requires additional types generated from: OrderByArgument.proto
namespace Alachisoft.NCache.Common.Protobuf
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ReaderResultSet")]
  public partial class ReaderResultSet : global::ProtoBuf.IExtensible
  {
    public ReaderResultSet() {}
    

    private string _readerId = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"readerId", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string readerId
    {
      get { return _readerId; }
      set { _readerId = value; }
    }

    private Alachisoft.NCache.Common.Protobuf.RecordSet _recordSet = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"recordSet", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public Alachisoft.NCache.Common.Protobuf.RecordSet recordSet
    {
      get { return _recordSet; }
      set { _recordSet = value; }
    }

    private string _nodeAddress = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"nodeAddress", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string nodeAddress
    {
      get { return _nodeAddress; }
      set { _nodeAddress = value; }
    }
    private readonly global::System.Collections.Generic.List<Alachisoft.NCache.Common.Protobuf.OrderByArgument> _orderByArguments = new global::System.Collections.Generic.List<Alachisoft.NCache.Common.Protobuf.OrderByArgument>();
    [global::ProtoBuf.ProtoMember(4, Name=@"orderByArguments", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Alachisoft.NCache.Common.Protobuf.OrderByArgument> orderByArguments
    {
      get { return _orderByArguments; }
    }
  

    private bool _isGrouped = default(bool);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"isGrouped", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(default(bool))]
    public bool isGrouped
    {
      get { return _isGrouped; }
      set { _isGrouped = value; }
    }

    private int _nextIndex = default(int);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"nextIndex", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int nextIndex
    {
      get { return _nextIndex; }
      set { _nextIndex = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
