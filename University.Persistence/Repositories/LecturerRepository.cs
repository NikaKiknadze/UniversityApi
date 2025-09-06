using University.Data.Data;
using University.Data.Data.Entities;
using University.Data.Repositories.Interfaces;

namespace University.Data.Repositories;

public class LecturerRepository(AppDbContext context) : GenericRepository<Lecturer>(context), ILecturerRepository;