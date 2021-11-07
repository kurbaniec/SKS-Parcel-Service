using System.Linq.Expressions;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWarehouseManagementLogic
    {
        // TODO: Remove this later on
        public bool TriggerExportWarehouseException { set; get; }
        /// <summary>
        /// Exports the hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <returns></returns>
        public Warehouse? ExportWarehouses();

        /// <summary>
        /// Get a certain warehouse or truck by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Warehouse? GetWarehouse(string code);

        /// <summary>
        /// Imports a hierarchy of Warehouse and Truck objects. 
        /// </summary>
        /// <param name="body"></param>
        public void ImportWarehouses(Warehouse? body);
    }
}