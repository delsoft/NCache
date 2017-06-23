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

using System;
using Alachisoft.NCache.IO;
using Alachisoft.NCache.Serialization.Formatters;

namespace Alachisoft.NCache.Serialization.Surrogates
{
    /// <summary>
    /// Surrogate for <see cref="System.Decimal[]"/> type.
    /// </summary>
    sealed class DecimalArraySerializationSurrogate : ContextSensitiveSerializationSurrogate
    {
        public DecimalArraySerializationSurrogate() : base(typeof(Decimal)) { }

        public override object Instantiate(CompactBinaryReader reader)
        {
            int length = reader.ReadInt32();
            return new Decimal[length];
        }

        public override object ReadDirect(CompactBinaryReader reader, object graph)
        {
            Decimal[] array = (Decimal[])graph;
            for (int i = 0; i < array.Length; i++)
                array[i] = (decimal)CompactBinaryFormatter.Deserialize(reader, reader.CacheContext, false);
            return array;
        }

        public override void WriteDirect(CompactBinaryWriter writer, object graph)
        {
            Decimal[] array = (Decimal[])graph;
            writer.Write(array.Length);
            for (int i = 0; i < array.Length; i++)
                CompactBinaryFormatter.Serialize(writer, ((decimal)array[i]).ToString(), writer.CacheContext); 
        }

        public override void SkipDirect(CompactBinaryReader reader, object graph)
        {
            Decimal[] array = (Decimal[])graph;
            for (int i = 0; i < array.Length; i++)
                CompactBinaryFormatter.Deserialize(reader, reader.CacheContext, true);
        }
    }
}
