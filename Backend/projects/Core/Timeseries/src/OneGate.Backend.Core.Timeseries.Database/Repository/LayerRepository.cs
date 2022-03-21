using OneGate.Backend.Core.Shared.Database.Repository;
using OneGate.Backend.Core.Timeseries.Database.Models;

namespace OneGate.Backend.Core.Timeseries.Database.Repository
{
    public class LayerRepository : GenericRepository<Layer>, ILayerRepository
    {
        private readonly DatabaseContext _db;

        public LayerRepository(DatabaseContext db) : base(db, db.Layers)
        {
            _db = db;
        }
    }
}