using Microsoft.EntityFrameworkCore.Infrastructure;
using University.Data.Data.Entities;
using University.Data.Data.EntityGenericMethods;

namespace University.Data.ContextMethodsDirectory;

public interface IUniversityContext : IContextMethods
{
    DatabaseFacade Database { get; }
    IEntityGenericMethods<Course> Courses { get; }
    IEntityGenericMethods<Faculty> Faculties { get; }
    IEntityGenericMethods<Lecturer> Lecturers { get; }
    IEntityGenericMethods<User> Users { get; }
    IEntityGenericMethods<AuditLog> AuditLogs { get; }
    IEntityGenericMethods<AuditEntry> AuditEntries { get; }
}