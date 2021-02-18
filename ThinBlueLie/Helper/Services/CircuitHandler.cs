using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace ThinBlueLie.Helper.Services
{
    public class TrackingCircuitHandler : CircuitHandler
    {
        private HashSet<Circuit> circuits = new HashSet<Circuit>();

        public override Task OnConnectionUpAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            circuits.Add(circuit);
            Serilog.Log.Information("New connection {CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }

        public override Task OnConnectionDownAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            circuits.Remove(circuit);
            Serilog.Log.Information("Closed connection {CircuitId}", circuit.Id);
            return Task.CompletedTask;
        }

        public int ConnectedCircuits => circuits.Count;
    }
}
