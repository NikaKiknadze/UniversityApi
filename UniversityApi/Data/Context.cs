using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace UniversityApi.Data
{
    public class Context : DbContext
    {
        public Context()
        {

        }
        public Context(DbContextOptions options) : base(options)
        {

        }
    }


}
