using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Sql;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Tests
{
    [ExcludeFromCodeCoverage]
    public class WarehouseRepositoryTests
    {
        [Test]
        public void Create_ValidWarehouse_WarehouseReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string code = "AUTA05";
            var validWarehouse = new Warehouse
            {
                HopType = "Warehouse",
                Code = code,
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            };
            myDbMoq.Setup(m => m.Warehouses)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Warehouse>(new List<Warehouse> { validWarehouse }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            IWarehouseRepository warehouseRepository = new WarehouseRepository(myDbMoq.Object);
            Warehouse? w = warehouseRepository.Create(validWarehouse);
            Assert.NotNull(w);
            Assert.AreEqual(w.Code, code);
        }
        
        [Test]
        public void GetAll_ValidWarehouse_WarehousesReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string code = "AUTA05";
            var validWarehouse = new Warehouse
            {
                HopType = "Warehouse",
                Code = code,
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            } as Hop;
            List<Hop> warehouses = new List<Hop> { validWarehouse };
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(warehouses));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            IWarehouseRepository warehouseRepository = new WarehouseRepository(myDbMoq.Object);
            Warehouse? w = warehouseRepository.GetAll();
            Assert.NotNull(w);
            Assert.AreEqual(w, validWarehouse);
        }
        
        [Test]
        public void GetHopByCode_ValidCode_HopReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string code = "AUTA05";
            var validWarehouse = new Warehouse
            {
                HopType = "Warehouse",
                Code = code,
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            };
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop>{validWarehouse}));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            IWarehouseRepository warehouseRepository = new WarehouseRepository(myDbMoq.Object);
            Hop? w = warehouseRepository.GetHopByCode(code);
            Assert.NotNull(w);
            Assert.AreEqual(w, validWarehouse);
        }
        
        [Test]
        public void GetHopByCode_InvalidCode_NullReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string code = "AUTA05";
            var validWarehouse = new Warehouse
            {
                HopType = "Warehouse",
                Code = code,
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            };
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop>{validWarehouse}));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            IWarehouseRepository warehouseRepository = new WarehouseRepository(myDbMoq.Object);
            Hop? w = warehouseRepository.GetHopByCode("");
            Assert.IsNull(w);
        }
        
        [Test]
        public void GetWarehouseByCode_InvalidCode_NullReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string code = "AUTA05";
            var validWarehouse = new Warehouse
            {
                HopType = "Warehouse",
                Code = code,
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop>{validWarehouse}));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            IWarehouseRepository warehouseRepository = new WarehouseRepository(myDbMoq.Object);
            Warehouse? w = warehouseRepository.GetWarehouseByCode("");
            Assert.IsNull(w);
        }
        [Test]
        public void GetWarehouseByCode_ValidCode_WarehouseReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            string code = "AUTA05";
            var validWarehouse = new Warehouse
            {
                HopType = "Warehouse",
                Code = code,
                Description = "Root Warehouse - Österreich",
                ProcessingDelayMins = 186,
                LocationName = "Root",
                LocationCoordinates = new GeoCoordinate { Lat = 47.247829, Lon = 13.884382 },
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop>{validWarehouse}));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            IWarehouseRepository warehouseRepository = new WarehouseRepository(myDbMoq.Object);
            Warehouse? w = warehouseRepository.GetWarehouseByCode(code);
            Assert.NotNull(w);
            Assert.AreEqual(w, validWarehouse);
        } 
        
    }
}