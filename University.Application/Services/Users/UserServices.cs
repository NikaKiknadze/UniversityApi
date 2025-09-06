using Microsoft.EntityFrameworkCore;
using University.Application.Services.Users.Helpers;
using University.Data.Data.Entities;
using University.Data.Repositories.Interfaces;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;
using University.Domain.Models.UserModels;

namespace University.Application.Services.Users;

public class UserServices(IUserRepository userRepository) : IUserServices
{
    public async Task<ApiResponse<GetDtoWithCount<ICollection<UserGetDto>>>> Get(UserGetFilter filter,
        CancellationToken cancellationToken)
    {
        var users = userRepository.AllAsNoTracking.FilterData(filter);

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
        var userExists =
            await userRepository.AllAsNoTracking.AnyAsync(u => u.Username == input.UserName,
                cancellationToken);

        if (userExists)
            throw new BadRequestException($"User already exists with this username: {input.UserName}");

        var user = new User
        {
            Username = input.UserName,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(input.Password, BCrypt.Net.HashType.SHA384)
        };

        user = user.FillData(input);

        user.UserProfile = new UserProfile
        {
            UserId = user.Id,
            FirstName = input.FirstName,
            LastName = input.LastName,
            Age = input.Age,
            FacultyId = input.FacultyId
        };

        await userRepository.AddAsync(user, cancellationToken);

        return user.Id;
    }

    public async Task<int> Update(UserPutDto input, CancellationToken cancellationToken)
    {
        var user = await userRepository.All
            .Include(u => u.UsersCourses)
            .Include(u => u.UsersLecturers)
            .Include(u => u.UserProfile)
            .Where(u => u.Id == input.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new NotFoundException("User not found");

        user = user.FillData(input);

        await userRepository.UpdateAsync(user ,cancellationToken);

        return user.Id;
    }

    public async Task<int> Delete(int userId, CancellationToken cancellationToken)
    {
        var user = await userRepository.All
            .Include(u => u.UsersCourses)
            .Include(u => u.UsersLecturers)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);

        if (user == null)
            throw new NotFoundException("User not found");

        user.UsersLecturers.Clear();
        user.UsersCourses.Clear();
        user.IsActive = false;

        await userRepository.UpdateAsync(user ,cancellationToken);

        return user.Id;
    }
}