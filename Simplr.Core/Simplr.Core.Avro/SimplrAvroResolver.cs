using Microsoft.Hadoop.Avro;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Simplr.Core.Avro
{
    public class SimplrAvroResolver : AvroPublicMemberContractResolver
    {
        private bool AllowNullable;
        private Type[] knownTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroPublicMemberContractResolver"/> class.
        /// </summary>
        public SimplrAvroResolver() : base(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroPublicMemberContractResolver"/> class.
        /// </summary>
        /// <param name="allowNullable">If set to <c>true</c>, null values are allowed.</param>
        public SimplrAvroResolver(bool allowNullable)
            : base(allowNullable)
        {
            AllowNullable = allowNullable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvroPublicMemberContractResolver"/> class.
        /// </summary>
        /// <param name="allowNullable">If set to <c>true</c>, null values are allowed.</param>
        /// <param name="knownTypes">If types list provided and not empty, adds them to all known types</param>
        public SimplrAvroResolver(bool allowNullable, Type[] knownTypes)
            : base(allowNullable)
        {
            this.knownTypes = knownTypes;
        }
        public override IEnumerable<Type> GetKnownTypes(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return knownTypes;
        }
        public override TypeSerializationInfo ResolveType(Type type)
        {
            try
            {
                return base.ResolveType(type);
            }
            catch (SerializationException)
            {
                if (type == typeof(object) && knownTypes != null && knownTypes.Length != 0)
                {
                    return new TypeSerializationInfo
                    {
                        Name = StripAvroNonCompatibleCharacters(type.Name),
                        Namespace = StripAvroNonCompatibleCharacters(type.Namespace),
                        Nullable = AllowNullable && type.CanContainNull()
                    };
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
