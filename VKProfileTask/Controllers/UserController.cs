using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VKProfileTask.Entities;
using VKProfileTask.Models;
using VKProfileTask.UnitOfWorks;

namespace VKProfileTask.Controllers;

[ApiController]
[Route("API/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly PostgresUnitOfWork _postgresWork;

    public UserController(ILogger<UserController> logger, DataContext dataContext)
    {
        _logger = logger;
        _postgresWork = new PostgresUnitOfWork(dataContext);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] UserForAdding userForAdding)
    {
        var stopwatch = new Stopwatch();
        if (userForAdding.GroupCode is < 0 or > 1)
        {
            _logger.LogError("Couldn\'t add a new user. Bad user group code = {GroupCode}.", userForAdding.GroupCode);
            return BadRequest();
        }

        var userState = new UserStateEntity(StateCode.Undefined, userForAdding.StateDescription);
        var userGroup = new UserGroupEntity((GroupCode)userForAdding.GroupCode, userForAdding.GroupDescription);
        var user = new UserEntity(userForAdding.Login, userForAdding.Password, userGroup.Id, userState.Id);
        if (!await _postgresWork.AddUserAsync(user, (GroupCode)userForAdding.GroupCode))
        {
            _logger.LogError(
                "Couldn\'t add a new user. The user with login = {Login} is already being added{OrAdminAlreadyExists}",
                userForAdding.Login,
                (GroupCode)userForAdding.GroupCode == GroupCode.User ? "." : " or admin already exists.");
            return BadRequest();
        }

        await _postgresWork.Groups.AddAsync(userGroup);
        await _postgresWork.States.AddAsync(userState);
        
        stopwatch.Stop();
        await Task.Delay((int)(5000 - stopwatch.ElapsedMilliseconds));

        userState.Code = StateCode.Active;
        await _postgresWork.States.SaveAsync();
        
        _logger.LogInformation("Successfully added a new user with id = {UserId}.", user.Id);
        
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteUser([FromBody] UserWithLogin userWithLogin)
    {
        if (!await _postgresWork.DeleteUserByLoginAsync(userWithLogin.Login))
        {
            _logger.LogError("Couldn\'t delete the user. The user with id = {Login} doesn\'t exist.", userWithLogin.Login);
            return BadRequest();
        }
        _logger.LogInformation("Successfully deleted user with id = {Login}.", userWithLogin.Login);
        
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUser([FromBody] UserWithLogin userWithLogin)
    {
        var user = await _postgresWork.GetUserByLoginAsync(userWithLogin.Login);
        if (user == null)
        {
            _logger.LogError("Couldn\'t find the user with id = {Login}.", userWithLogin.Login);
            return NotFound();
        }
        
        _logger.LogInformation("Successfully found the user with id = {Login}.", userWithLogin.Login);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersByPages([FromBody] Page page)
    {
        if (page is { PageNumber: > 0, PageCapacity: > 0 })
            return Ok(await _postgresWork.GetFullInfoOfAllUsersAsync(page.PageNumber, page.PageCapacity));
        _logger.LogError(
            "Couldn\'t get users. Bad page number = {PagePageNumber} or page capacity = {PagePageCapacity}.",
            page.PageNumber, page.PageCapacity);
        return BadRequest();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUsers() =>
        Ok(await _postgresWork.GetFullInfoOfAllUsersAsync());
}