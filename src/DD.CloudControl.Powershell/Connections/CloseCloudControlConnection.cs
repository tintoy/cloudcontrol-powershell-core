using System.Management.Automation;

namespace DD.CloudControl.Powershell.Connections
{
    using Client;

    /// <summary>
    ///     Cmdlet that closes a connection to CloudControl.
    /// </summary>
    [Cmdlet(VerbsCommon.Close, Nouns.Connection)]
    public class CloseCloudControlConnection
        : PSCmdlet
    {
        /// <summary>
        ///     Create a new <see cref="CloseCloudControlConnection"/> cmdlet.
        /// </summary>
        public CloseCloudControlConnection()
        {
        }

        /// <summary>
        ///     The name of the connection to close.
        /// </summary>
        [Parameter(HelpMessage = "The name of the connection to close")]
        public string Name { get; set; }

        /// <summary>
		///		Asynchronously perform Cmdlet processing.
		/// </summary>
		/// <param name="cancellationToken">
		///		A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task"/> representing the asynchronous operation.
		/// </returns>
        protected override void ProcessRecord()
        {
            CloudControlClient client;
            if (SessionState.Clients().TryGetValue(Name, out client))
            {
                WriteVerbose($"Closing CloudControl connection '{Name}'...");
                client.Dispose();
                WriteVerbose($"Closed CloudControl connection '{Name}'.");
            }
            else
                WriteVerbose($"No open connection named '{Name}'.");
        }
    }
}
