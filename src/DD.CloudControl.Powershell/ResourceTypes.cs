using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DD.CloudControl.Powershell
{
    using Client.Models.Directory;
    using Client.Models.Network;
    using Client.Models.Server;

    /// <summary>
    ///     Resource type constants.
    /// </summary>
    public static class ResourceTypes
    {
        /// <summary>
        ///     Resource type descriptions.
        /// </summary>
        static readonly ImmutableDictionary<Type, string> Descriptions =
            ImmutableDictionary.CreateRange(new Dictionary<Type, string>
            {
                [typeof(UserAccount)] = "user account",
                [typeof(NetworkDomain)] = "network domain",
                [typeof(Vlan)] = "VLAN",
                [typeof(Server)] = "server",
                [typeof(NatRule)] = "NAT rule"
            });

        /// <summary>
        ///     Get a description of the specified resource type.
        /// </summary>
        public static string GetDescription<TResource>()
        {
            return GetDescription(typeof(TResource));
        }

        /// <summary>
        ///     Get a description of the specified resource type.
        /// </summary>
        public static string GetDescription(Type resourceType)
        {
            if (resourceType == null)
                throw new ArgumentNullException(nameof(resourceType));

            if (Descriptions.TryGetValue(resourceType, out string description))
                return description;

            return resourceType.Name;
        }
    }
}