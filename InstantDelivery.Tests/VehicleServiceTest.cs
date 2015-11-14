﻿using InstantDelivery.Core;
using InstantDelivery.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using InstantDelivery.Domain;
using InstantDelivery.Domain.Entities;
using Xunit;

namespace InstantDelivery.Tests
{

    public class VehicleServiceTest
    {
        [Fact]
        public void GetAllVehicles_ShouldReturnAllVehicles()
        {
            var vehicles = new List<Vehicle>
            {
                new Vehicle() { Id=1},
                new Vehicle() { Id=2},
                new Vehicle() { Id=3}
            }
            .AsQueryable();
            var vehiclesMockSet = MockDbSetHelper.GetMockSet(vehicles);

            var mockContext = new Mock<InstantDeliveryContext>();
            mockContext.Setup(c => c.Vehicles).Returns(vehiclesMockSet.Object);
            var service = new VehiclesService(mockContext.Object);

            var result = service.GetAll();
            var count = result.Count();
            Assert.Equal(count, 3);
        }

        [Fact]
        public void GetAllVehiclesModels_ShouldReturnAllVehiclesModels()
        {
            var vehiclesModels = new List<VehicleModel>
            {
                new VehicleModel() { Id=1},
                new VehicleModel() { Id=2},
                new VehicleModel() { Id=3}
            }
            .AsQueryable();
            var vehiclesMockSet = MockDbSetHelper.GetMockSet(vehiclesModels);

            var mockContext = new Mock<InstantDeliveryContext>();
            mockContext.Setup(c => c.VehicleModels).Returns(vehiclesMockSet.Object);
            var service = new VehiclesService(mockContext.Object);

            var result = service.GetAllModels();
            var count = result.Count();
            Assert.Equal(count, 3);
        }

        [Fact]
        public void ReloadVehicle_ShouldReloadVehicleData()
        {


            using (var context = new InstantDeliveryContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var vehicle = new Vehicle
                    {
                        Id = 1,
                        RegistrationNumber = "1"
                    };
                    var service = new VehiclesService(context);
                    context.Vehicles.Add(vehicle);
                    context.SaveChanges();

                    vehicle.RegistrationNumber = "2";
                    service.Reload(vehicle);

                    Assert.Equal(vehicle.RegistrationNumber, "1");
                }
            }
        }

        [Fact]
        public void RemoveVehicle_ShouldRemoveVehicle()
        {
            var vehicles = new List<Vehicle>() { new Vehicle()
            {
                Id=1,
                RegistrationNumber="1"
            } }.AsQueryable();
            var vehiclesMockSet = MockDbSetHelper.GetMockSet(vehicles);

            var mockContext = new Mock<InstantDeliveryContext>();
            mockContext.Setup(c => c.Vehicles).Returns(vehiclesMockSet.Object);

            var service = new VehiclesService(mockContext.Object);

            service.Remove(vehicles.First());

            vehiclesMockSet.Verify(m => m.Remove(vehicles.First()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void AddVehicle_ShouldAlwaysAddVehicle()
        {
            var vehicles = new List<Vehicle>().AsQueryable();
            var vehiclesMockSet = MockDbSetHelper.GetMockSet(vehicles);
            var vehicleToAdd = new Vehicle() { Id = 1 };

            var mockContext = new Mock<InstantDeliveryContext>();
            mockContext.Setup(c => c.Vehicles).Returns(vehiclesMockSet.Object);
            var service = new VehiclesService(mockContext.Object);

            service.AddVehicle(vehicleToAdd);

            vehiclesMockSet.Verify(m => m.Add(It.IsAny<Vehicle>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void SaveVehicle_ShouldSaveVehicle()
        {
            var vehicles = new List<Vehicle>
            {
                new Vehicle() {Id=1, RegistrationNumber="1"},
            }.AsQueryable();
            var vehiclesMockSet = MockDbSetHelper.GetMockSet(vehicles);
            var mockContext = new Mock<InstantDeliveryContext>();
            mockContext.Setup(c => c.Vehicles).Returns(vehiclesMockSet.Object);
            var service = new VehiclesService(mockContext.Object);
            var selected = vehiclesMockSet.Object.FirstOrDefault();
            if (selected == null) return;
            vehiclesMockSet.Object.Attach(selected);
            selected.RegistrationNumber = "2";
            service.Save();

            var result = vehiclesMockSet.Object.FirstOrDefault();
            if (result != null)
            {
                Assert.Equal(result.RegistrationNumber, "2");
            }
        }

        [Fact]
        public void GetAllAvailableAndCurrentVehicle_ForSpecifiedVehicle()
        {
            var vehicles = new List<Vehicle>
            {
                new Vehicle() {Id=1, RegistrationNumber="1"},
                new Vehicle() {Id=1, RegistrationNumber="2"},
                new Vehicle() {Id=1, RegistrationNumber="3"},
            }.AsQueryable();
            var vehiclesMockSet = MockDbSetHelper.GetMockSet(vehicles);
            var mockContext = new Mock<InstantDeliveryContext>();
            mockContext.Setup(c => c.Vehicles).Returns(vehiclesMockSet.Object);
            var service = new VehiclesService(mockContext.Object);

            var employees = new List<Employee>
            {
                new Employee() { FirstName = "J.D", LastName = "Kyle", Vehicle=vehicles.Last() } ,
                new Employee() { FirstName = "Ted", LastName = "Mosby"},
                new Employee() { FirstName = "Robin", LastName = "Scherbatsky"}
            }
            .AsQueryable();
            var employeesMockSet = MockDbSetHelper.GetMockSet(employees);

            mockContext.Setup(c => c.Employees).Returns(employeesMockSet.Object);

            var result = service.GetAllAvailableAndCurrent(vehicles.First()).Count();
            Assert.Equal(result, 3);
        }
    }
}