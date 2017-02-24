using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MigrateVitalSignsData.Mappers
{
    public interface IMapper<T>
    {
        List<T> Map(DataTable table);
    }
}
