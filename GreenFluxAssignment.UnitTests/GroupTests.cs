using GreenFluxAssignment.Services.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenFluxAssignment.UnitTests
{
    [TestClass]
    public class GroupTests
    {
        [TestMethod]
        public void Group_CreateTest()
        {
            Guid id = Guid.NewGuid();
            string name = "Group 1";
            decimal capacity = 10;

            // Act
            GroupDomain group = new GroupDomain(id, name, capacity);

            // Assert
            Assert.AreEqual(id, group.Id);
            Assert.AreEqual(name, group.Name);
            Assert.AreEqual(capacity, group.Capacity);
        }

        [TestMethod]
        public void Group_UpdateTest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Group 1";
            decimal capacity = 10;
            string newName = "New Group Name";
            decimal newCapacity = 20.5M;

            GroupDomain group = new GroupDomain(id, name, capacity);

            // Act
            group.UpdateName(newName);
            group.UpdateCapacity(newCapacity);

            // Assert
            Assert.AreEqual(newName, group.Name);
            Assert.AreEqual(newCapacity, group.Capacity);
        }

        [TestMethod]
        public void Group_AddStationConnectorTest()
        {
            // Arrange           
            Guid id = Guid.NewGuid();
            string name = "Group 1";
            decimal capacity = 10;
            GroupDomain group = new GroupDomain(id, name, capacity);

            ConnectorDomain connector = new ConnectorDomain() { MaxCurrent = 2 };
            ChargeStationDomain station = new ChargeStationDomain(group, "New Charge Station", new List<ConnectorDomain>(){ connector });

            decimal expectedCurrent = 2;
            int expectedChargeStationCount = 1;
            // Act
            group.AddChargeStation(station);

            // Assert
            Assert.AreEqual(expectedChargeStationCount, group.ChargeStations.Count);
            Assert.AreEqual(expectedCurrent, group.ChargeStations.Sum(a => a.Connectors.Sum(a=> a.MaxCurrent)));
        }

        [TestMethod]
        public void Group_RemoveChargeStationTest()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            string name = "Group 1";
            decimal capacity = 10;
            GroupDomain group = new GroupDomain(id, name, capacity);

            ConnectorDomain connector = new ConnectorDomain() { MaxCurrent = 2 };
            ChargeStationDomain station = new ChargeStationDomain(group, "New Charge Station", new List<ConnectorDomain>() { connector });

            // Act - Add First
            group.AddChargeStation(station);

            decimal expectedCurrent = 2;
            int expectedChargeStationCount = 1;

            Assert.AreEqual(expectedChargeStationCount, group.ChargeStations.Count);
            Assert.AreEqual(expectedCurrent, group.ChargeStations.Sum(a => a.Connectors.Sum(a => a.MaxCurrent)));

            // Act - Remove
            group.RemoveChargeStation(station.Id);
            int removedChargeStationCount = 0;

            // Assert
            Assert.AreEqual(removedChargeStationCount, group.ChargeStations.Count);
        }
    }
}
