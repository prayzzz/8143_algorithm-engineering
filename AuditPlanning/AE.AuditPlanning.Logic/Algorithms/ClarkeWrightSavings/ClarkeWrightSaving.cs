using System.Diagnostics;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWrightSavings
{
    /// <summary>
    /// Datenklasse für die Berechnung der Einsparungen
    /// </summary>
    [DebuggerDisplay("Saving from {FromNode} to {ToNode} ({Saving})")]
    public class ClarkeWrightSaving<TN>
    {
        public ClarkeWrightSaving(double saving, TN fromNode, TN toNode)
        {
            this.Saving = saving;
            this.FromNode = fromNode;
            this.ToNode = toNode;
        }

        public double Saving { get; private set; }

        public TN FromNode { get; private set; }

        public TN ToNode { get; private set; }
    }
}