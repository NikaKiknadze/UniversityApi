**Splitted LINQ** 1920 მილიწამი
**Single LINQ** 25484 მილიწამი
**Splitted LINQ + AsNoTracking** 118 მილიწამი
**Splitted EF** 227 მილიწამი
**Single EF** 25121 მილიწამი
**Splitted EF + AsNoTracking** 79 მილიწამი

**Splitted LINQ + AsNoTracking**

var users = _context.Users.AsQueryable();

var filteredUsers = await FilterData(filter, users, cancellationToken);

var userLecturers = _context.UsersLecturersJoin.AsQueryable();
var lecturers = _context.Lecturers.AsQueryable();
var userCourses = _context.UsersCoursesJoin.AsQueryable();
var courses = _context.Courses.AsQueryable();
var faculties = _context.Faculty.AsQueryable();

users = users.Where(u => filteredUsers.Item1.Contains(u.Id));

var query4 = from u in users
            join uc in userCourses on u.Id equals uc.UserId into joinedUserCourses
            from ucs in joinedUserCourses.DefaultIfEmpty()
            join ul in userLecturers on u.Id equals ul.UserId into joinedUserLecturers
            from uls in joinedUserLecturers.DefaultIfEmpty()
            join f in faculties on u.FacultyId equals f.Id into joinedFaculties
            from fs in joinedFaculties.DefaultIfEmpty()
            group new { u, ucs, uls, fs } by new
            {
                u.Id,
                u.Name,
                u.SurName,
                u.Age,
                FacultyId = fs.Id,
                fs.FacultyName,
                ucs.CourseId,
                uls.LecturerId
            }
            into groupedResult
            select new UserGetDto
            {
                Id = groupedResult.Key.Id,
                Name = groupedResult.Key.Name,
                SurName = groupedResult.Key.SurName,
                Age = groupedResult.Key.Age,
                Faculty = new FacultyOnlyDto
                {
                    Id = groupedResult.Key.FacultyId,
                    FacultyName = groupedResult.Key.FacultyName
                },
                Courses = (from res in groupedResult
                           join c in courses on res.ucs.CourseId equals c.Id into joinedCourses
                           from cs in joinedCourses.DefaultIfEmpty()
                           select new CourseOnlyDto
                           {
                               Id = cs.Id,
                               CourseName = cs.CourseName
                           }),
                Lecturers = (from res in groupedResult
                             join l in lecturers on res.uls.LecturerId equals l.Id into joinedLecturers
                             from ls in joinedLecturers.DefaultIfEmpty()
                             select new LecturerOnlyDto
                             {
                                 Id = ls.Id,
                                 Name = ls.Name,
                                 SurName = ls.SurName,
                                 Age = ls.Age
                             })
            };

var res = await query.AsNoTracking().AsSplitQuery().ToListAsync();






**Splitted EF + AsNoTracking**

var users = _context.Users.AsQueryable();

var users = await users.Where(u => filteredUsers.Item1.Contains(u.Id)).Select(user => new UserGetDto
{
    Id = user.Id,
    Name = user.Name,
    SurName = user.SurName,
    Age = user.Age,
    Faculty = user.Faculty != null
    ? new FacultyOnlyDto
    {
        Id = user.Faculty.Id,
        FacultyName = user.Faculty.FacultyName
    }
    : null,
    Courses = user.UsersCourses
        .Where(uc => uc.Course != null)
        .Select(uc => new CourseOnlyDto
        {
            Id = uc.Course.Id,
            CourseName = uc.Course.CourseName
        }).ToList(),
    Lecturers = user.UsersLecturers
            .Where(ul => ul.Lecturer != null)
            .Select(ul => new LecturerOnlyDto
            {
                Id = ul.Lecturer.Id,
                Name = ul.Lecturer.Name,
                SurName = ul.Lecturer.SurName,
                Age = ul.Lecturer.Age
            }).ToList()
})
.AsNoTracking()
.AsSplitQuery()
.ToListAsync();
