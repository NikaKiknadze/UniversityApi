using Microsoft.EntityFrameworkCore;
using University.Application.Services.Users.Helpers;
using University.Data.ContextMethodsDirectory;
using University.Data.Data.Entities;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;
using University.Domain.Models.UserModels;

namespace University.Application.Services.Users
{
    public class UserServices(IUniversityContext universityContext) : IUserServices
    {
        public async Task<ApiResponse<GetDtoWithCount<ICollection<UserGetDto>>>> Get(UserGetFilter filter,
            CancellationToken cancellationToken)
        {
            var users = universityContext.Users.AllAsNoTracking.FilterData(filter);

            var result = await users
                .MapDataToUserGetDto()
                .AsSplitQuery()
                .OrderByDescending(u => u.Id)
                .Skip(filter.Offset ?? 0)
                .Take(filter.Limit ?? 10)
                .ToListAsync(cancellationToken);
            
            return ApiResponse<GetDtoWithCount<ICollection<UserGetDto>>>.SuccessResult(
                new GetDtoWithCount<ICollection<UserGetDto>>
                {
                    Data = result,
                    Count = await users.CountAsync(cancellationToken)
                });
        }

        public async Task<int> Create(UserPostDto input, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = input.Name,
                SurName = input.SurName,
                Age = input.Age,
                FacultyId = input.FacultyId
            };

            user = user.FillData(input);
            
            await universityContext.Users.AddAsync(user, cancellationToken);
            
            return user.Id;
        }

        public async Task<int> Update(UserPutDto input, CancellationToken cancellationToken)
        {
            var user = await universityContext.Users.All
                .Include(u => u.UsersCourses)
                .Include(u => u.UsersLecturers)
                .Where(u => u.Id == input.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if(user is null)
                throw new NotFoundException("User not found");

            user = user.FillData(input);

            await universityContext.CompleteAsync(cancellationToken);
            
            return user.Id;
        }

        public async Task<int> Delete(int userId, CancellationToken cancellationToken)
        {
            var user = await universityContext.Users.All
                .Include(u => u.UsersCourses)
                .Include(u => u.UsersLecturers)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);

            if (user == null)
                throw new NotFoundException("User not found");
            
            user.UsersLecturers.Clear();
            user.UsersCourses.Clear();
            user.IsActive = false;
            
            await universityContext.CompleteAsync(cancellationToken);
            
            return user.Id;
        }
    }
}