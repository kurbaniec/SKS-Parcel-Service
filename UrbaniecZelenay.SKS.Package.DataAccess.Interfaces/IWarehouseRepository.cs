﻿using UrbaniecZelenay.SKS.Package.DataAccess.Entities;

namespace UrbaniecZelenay.SKS.Package.DataAccess.Interfaces
{
    public interface IWarehouseRepository
    {
        Warehouse Create(Warehouse warehouse);
        Warehouse? GetAll();
        Hop? GetHopByCode(string code);
        Warehouse? GetWarehouseByCode(string code);
    }
}