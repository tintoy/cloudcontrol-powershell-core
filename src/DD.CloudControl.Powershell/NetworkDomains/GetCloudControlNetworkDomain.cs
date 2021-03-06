using PSReptile;
using System;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace DD.CloudControl.Powershell.NetworkDomains
{
    using Client;
    using Client.Models.Network;

    /// <summary>
    ///     Cmdlet that retrieves information about one or more network domains.
    /// </summary>
    [OutputType(typeof(NetworkDomain))]
    [Cmdlet(VerbsCommon.Get, Nouns.NetworkDomain, SupportsPaging = true)]
    [CmdletSynopsis("Retrieves information about one or more network domains")]
    [CmdletDescription(@"
        A network domain is the top-level container for resources in CloudControl.
    ")]
    public class GetCloudControlNetworkDomain
        : CloudControlCmdlet
    {
        /// <summary>
        ///     The Id of the network domain to retrieve.
        /// </summary>
        [Parameter(ParameterSetName = "By Id", Mandatory = true, HelpMessage = "The Id of the network domain to retrieve")]
        public Guid Id { get; set; }

		/// <summary>
        ///     The name of the network domain to retrieve.
        /// </summary>
        [ValidateNotNullOrEmpty]
        [Parameter(ParameterSetName = "By name", Mandatory = true, HelpMessage = "The name the network domain to retrieve")]
        public string Name { get; set; }

        /// <summary>
        ///     The Id of the target datacenter.
        /// </summary>
        [ValidateNotNullOrEmpty]
        [Parameter(ParameterSetName = "By name", Mandatory = true, HelpMessage = "The Id of the target datacenter")]
        [Parameter(ParameterSetName = "By datacenter", Mandatory = true, HelpMessage = "The Id of the target datacenter")]
        public string DatacenterId { get; set; }

        /// <summary>
        ///     Asynchronously perform Cmdlet processing.
        /// </summary>
        /// <param name="cancellationToken">
        ///     A <see cref="CancellationToken"/> that can be used to cancel Cmdlet processing.
        /// </param>
        /// <returns>
        ///     A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        protected override async Task ProcessRecordAsync(CancellationToken cancellationToken)
        {
            CloudControlClient client = GetClient();

            switch (ParameterSetName)
            {
                case "By Id":
                {
                    NetworkDomain networkDomain = await client.GetNetworkDomain(Id, cancellationToken);
                    if (networkDomain == null)
                    {
                        WriteError(
                            Errors.ResourceNotFoundById<NetworkDomain>(Id)
                        );
                    }
                    else
                        WriteObject(networkDomain);

                    break;
                }
				case "By name":
                {
                    NetworkDomain networkDomain = await client.GetNetworkDomainByName(Name, DatacenterId);
                    if (networkDomain == null)
                    {
                        WriteError(
                            Errors.ResourceNotFoundByName<NetworkDomain>(Name, 
                                message: $"No network domain named '{Name}' was found in datacenter '{DatacenterId}'."
                            )
                        );
                    }
                    else
                        WriteObject(networkDomain);
                    
                    break;
                }
				case "By datacenter":
                {
                    Paging paging = GetPagingConfiguration();
                    NetworkDomains networkDomains = await client.ListNetworkDomains(DatacenterId, paging, cancellationToken);
                    WriteObject(networkDomains.Items,
                        enumerateCollection: true
                    );

                    break;
                }
                
                default:
                {
                    ThrowTerminatingError(
                        Errors.UnrecognizedParameterSet(this)
                    );

                    return;
                }
            }
        }
    }
}
