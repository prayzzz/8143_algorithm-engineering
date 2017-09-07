using AE.AuditPlanning.Connectivity.OsmNominatim;

using NUnit.Framework;

namespace AE.AuditPlanning.Test.Connectivity
{
    [TestFixture]
    public class OsmNominatimTest
    {
        [Test]
        public void TestSetGeoCords()
        {
            var cords = OsmNominatimServiceGermany.GetGeoCoordinates("04103", "Leipzig");

            Assert.That(cords.Longitude, Is.GreaterThan(0));
            Assert.That(cords.Latitude, Is.GreaterThan(0));
        }
    }
}