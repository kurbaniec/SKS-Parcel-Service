using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities;
using UrbaniecZelenay.SKS.Package.DataAccess.Entities.Exceptions;
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
                LocationCoordinates = new Point(13.884382, 47.247829),
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            };
            myDbMoq.Setup(m => m.Warehouses)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Warehouse>(new List<Warehouse>
                    ()));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
            Warehouse? w = warehouseRepository.Create(validWarehouse);
            Assert.NotNull(w);
            Assert.AreEqual(w.Code, code);
        }

        [Test]
        public void Create_RecreateWarehouse_WarehouseReturned()
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
                LocationCoordinates = new Point(13.884382, 47.247829),
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            };
            myDbMoq.Setup(m => m.Warehouses)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Warehouse>(new List<Warehouse>
                    { validWarehouse }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();


            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
            //Assert.Throws<DalDuplicateEntryException>(() => warehouseRepository.Create(validWarehouse));
            var w = warehouseRepository.Create(validWarehouse);
            Assert.NotNull(w);
            Assert.AreEqual(w.Code, code);
            // Warehouse? w = warehouseRepository.Create(validWarehouse);
            // Assert.NotNull(w);
            // Assert.AreEqual(w.Code, code);
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
                LocationCoordinates = new Point(13.884382, 47.247829),
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
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
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
                LocationCoordinates = new Point(13.884382, 47.247829),
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            };
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop> { validWarehouse }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
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
                LocationCoordinates = new Point(13.884382, 47.247829),
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            };
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop> { validWarehouse }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
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
                LocationCoordinates = new Point(13.884382, 47.247829),
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop> { validWarehouse }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
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
                LocationCoordinates = new Point(13.884382, 47.247829),
                Level = 0,
                NextHops = new List<WarehouseNextHops>()
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop> { validWarehouse }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
            Warehouse? w = warehouseRepository.GetWarehouseByCode(code);
            Assert.NotNull(w);
            Assert.AreEqual(w, validWarehouse);
        }

        [Test]
        public void GetTruckPyPoint_ValidPoint_TruckReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            var point = new Point(13.884382, 47.247829);
            // See: https://stackoverflow.com/a/50390990/12347616
            // See: https://gis.stackexchange.com/a/97360
            var coordinates = new List<Coordinate>()
            {
                new(10, 45), new(15, 45), new(15, 55), new(10, 55), new(10, 45)
            };
            var geometryFactory = new GeometryFactory();
            var polygon = geometryFactory.CreatePolygon(coordinates.ToArray());
            //Assert.IsTrue(polygon.Contains(point));
            var truck = new Truck()
            {
                HopType = "Truck",
                Code = "AUTA05",
                Description = "Truck Roaming",
                ProcessingDelayMins = 186,
                LocationName = "Roaming",
                LocationCoordinates = new Point(13.884382, 47.247829),
                Region = polygon
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop> { truck }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
            var t = warehouseRepository.GetTruckByPoint(point);
            Assert.NotNull(t);
            Assert.AreEqual(t, truck);
        }

        [Test]
        public void GetTruckPyPoint_InvalidPoint_NullReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            var point = new Point(100, 100);
            // See: https://stackoverflow.com/a/50390990/12347616
            // See: https://gis.stackexchange.com/a/97360
            var coordinates = new List<Coordinate>()
            {
                new(10, 45), new(15, 45), new(15, 55), new(10, 55), new(10, 45)
            };
            var geometryFactory = new GeometryFactory();
            var polygon = geometryFactory.CreatePolygon(coordinates.ToArray());
            var truck = new Truck()
            {
                HopType = "Truck",
                Code = "AUTA05",
                Description = "Truck Roaming",
                ProcessingDelayMins = 186,
                LocationName = "Roaming",
                LocationCoordinates = new Point(13.884382, 47.247829),
                Region = polygon
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop> { truck }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
            var t = warehouseRepository.GetTruckByPoint(point);
            Assert.IsNull(t);
        }
        
        [Test]
        public void GetTruckPyPoint_DbProblem_ThrowsDalConnectionException()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            var point = new Point(100, 100);
            // See: https://stackoverflow.com/a/50390990/12347616
            // See: https://gis.stackexchange.com/a/97360
            var coordinates = new List<Coordinate>()
            {
                new(10, 45), new(15, 45), new(15, 55), new(10, 55), new(10, 45)
            };
            var geometryFactory = new GeometryFactory();
            var polygon = geometryFactory.CreatePolygon(coordinates.ToArray());
            var truck = new Truck()
            {
                HopType = "Truck",
                Code = "AUTA05",
                Description = "Truck Roaming",
                ProcessingDelayMins = 186,
                LocationName = "Roaming",
                LocationCoordinates = new Point(13.884382, 47.247829),
                Region = polygon
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Throws(SqlExceptionCreator.NewSqlException());
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
            Assert.Throws<DalConnectionException>(() => warehouseRepository.GetTruckByPoint(point));
        }

        [Test]
        public void GetTransferwarehouseByPoint_ValidPoint_TransferwarehouseReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            var point = new Point(13.884382, 47.247829);
            // See: https://stackoverflow.com/a/50390990/12347616
            // See: https://gis.stackexchange.com/a/97360
            var coordinates = new List<Coordinate>()
            {
                new(10, 45), new(15, 45), new(15, 55), new(10, 55), new(10, 45)
            };
            var geometryFactory = new GeometryFactory();
            var polygon = geometryFactory.CreatePolygon(coordinates.ToArray());
            //Assert.IsTrue(polygon.Contains(point));
            var transferwarehouse = new Transferwarehouse()
            {
                HopType = "Transferwarehouse",
                Code = "AUTA05",
                Description = "TW Somewhere",
                ProcessingDelayMins = 186,
                LocationName = "Somewhere",
                LocationCoordinates = new Point(13.884382, 47.247829),
                Region = polygon
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop> { transferwarehouse }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
            var tw = warehouseRepository.GetTransferwarehouseByPoint(point);
            Assert.NotNull(tw);
            Assert.AreEqual(tw, transferwarehouse);
        }

        [Test]
        public void GetTransferwarehouseByPoint_InvalidPoint_NullReturned()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            var point = new Point(100, 100);
            // See: https://stackoverflow.com/a/50390990/12347616
            // See: https://gis.stackexchange.com/a/97360
            var coordinates = new List<Coordinate>()
            {
                new(10, 45), new(15, 45), new(15, 55), new(10, 55), new(10, 45)
            };
            var geometryFactory = new GeometryFactory();
            var polygon = geometryFactory.CreatePolygon(coordinates.ToArray());
            //Assert.IsTrue(polygon.Contains(point));
            var transferwarehouse = new Transferwarehouse()
            {
                HopType = "Transferwarehouse",
                Code = "AUTA05",
                Description = "TW Somewhere",
                ProcessingDelayMins = 186,
                LocationName = "Somewhere",
                LocationCoordinates = new Point(13.884382, 47.247829),
                Region = polygon
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Returns(ParcelLogisticsContextMock.GetQueryableMockDbSet<Hop>(new List<Hop> { transferwarehouse }));
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
            var tw = warehouseRepository.GetTransferwarehouseByPoint(point);
            Assert.IsNull(tw);
        }

        [Test]
        public void GetTransferwarehouseByPoint_DbProblem_ThrowsDalConnectionException()
        {
            var myDbMoq = new Mock<IParcelLogisticsContext>();
            var point = new Point(100, 100);
            // See: https://stackoverflow.com/a/50390990/12347616
            // See: https://gis.stackexchange.com/a/97360
            var coordinates = new List<Coordinate>()
            {
                new(10, 45), new(15, 45), new(15, 55), new(10, 55), new(10, 45)
            };
            var geometryFactory = new GeometryFactory();
            var polygon = geometryFactory.CreatePolygon(coordinates.ToArray());
            //Assert.IsTrue(polygon.Contains(point));
            var transferwarehouse = new Transferwarehouse()
            {
                HopType = "Transferwarehouse",
                Code = "AUTA05",
                Description = "TW Somewhere",
                ProcessingDelayMins = 186,
                LocationName = "Somewhere",
                LocationCoordinates = new Point(13.884382, 47.247829),
                Region = polygon
            } as Hop;
            myDbMoq.Setup(m => m.Hops)
                .Throws(SqlExceptionCreator.NewSqlException());
            // 
            Mock<DatabaseFacade> dbFacadeMock =
                new Mock<DatabaseFacade>(MockBehavior.Strict, new Mock<DbContext>().Object);
            Mock<IDbContextTransaction> dbTransactionMock = new Mock<IDbContextTransaction>();
            dbFacadeMock.Setup(m => m.BeginTransaction()).Returns(dbTransactionMock.Object);
            myDbMoq.Setup(m => m.Database).Returns(dbFacadeMock.Object);
            var mockLogger = new Mock<ILogger<WarehouseRepository>>();
            IWarehouseRepository warehouseRepository = new WarehouseRepository(mockLogger.Object, myDbMoq.Object);
            Assert.Throws<DalConnectionException>(() => warehouseRepository.GetTransferwarehouseByPoint(point));
        }
        
        // Create SqlException
        // See: https://stackoverflow.com/a/1387030/12347616
        private class SqlExceptionCreator
        {
            private static T Construct<T>(params object[] p)
            {
                var ctors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                return (T)ctors.First(ctor => ctor.GetParameters().Length == p.Length).Invoke(p);
            }

            internal static SqlException NewSqlException(int number = 1)
            {
                SqlErrorCollection collection = Construct<SqlErrorCollection>();
                SqlError error = Construct<SqlError>(number, (byte)2, (byte)3, "server name", "error message", "proc", 100, null);

                typeof(SqlErrorCollection)
                    .GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .Invoke(collection, new object[] { error });


                return (typeof(SqlException)
                    .GetMethod("CreateException", BindingFlags.NonPublic | BindingFlags.Static,
                        null,
                        CallingConventions.ExplicitThis,
                        new[] { typeof(SqlErrorCollection), typeof(string) },
                        new ParameterModifier[] { })!
                    .Invoke(null, new object[] { collection, "7.0.0" }) as SqlException)!;
            }
        }  
        
        // See: https://stackoverflow.com/a/6075100/12347616
        private static SqlException MakeSqlException() {
            SqlException exception = null;
            try {
                SqlConnection conn = new SqlConnection(@"Data Source=.;Database=GUARANTEED_TO_FAIL;Connection Timeout=1");
                conn.Open();
            } catch(SqlException ex) {
                exception = ex;
            }
            return(exception);
        }
    }
}