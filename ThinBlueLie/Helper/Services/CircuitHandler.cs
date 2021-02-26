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
            //Serilog.Log.Information("New connection {CircuitId}", circuit.Id);
            circuits.Add(circuit);
            return Task.CompletedTask;
        }

        public override Task OnConnectionDownAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            //Serilog.Log.Information("Closed connection {CircuitId}", circuit.Id);
            circuits.Remove(circuit);
            return Task.CompletedTask;
        }

        public int ConnectedCircuits => circuits.Count;
    }
}
