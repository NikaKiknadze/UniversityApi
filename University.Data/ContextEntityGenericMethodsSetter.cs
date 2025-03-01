using Microsoft.EntityFrameworkCore;
using University.Data.Data.EntityGenericMethods;

namespace University.Data
{
    public class ContextEntityGenericMethodsSetter<T> (DbContext context) : EntityGenericMethods<T>(context) where T : class
    {
    }
}
