using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyDairy.Service.DTOs.Attachment;
using MyDairy.Service.DTOs.Users;
using MyDairy.Service.Interfaces.Users;
using MyDairy.Web.Models;

namespace MyDairy.Web.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        return View(new UserCreationDto());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(UserCreationDto userDto)
    {
        var newUser = await _userService.CreateAsync(userDto);
        return RedirectToAction("Index");
    }

    [HttpGet("update")]
    public async Task<IActionResult> Update(long Id)
    {
        var user = await _userService.GetByIdAsync(Id);
        var mappedUser = _mapper.Map<UserUpdateDto>(user);
        return View(mappedUser);
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UserUpdateDto userDto)
    {
        var updatedUser = await _userService.UpdateAsync(userDto);
        return RedirectToAction("GetById", new { id = updatedUser.Id });
    }

    [HttpPost("upload-photo")]
    public async Task<IActionResult> UploadAvatar(long id, [FromForm] AttachmentCreationDto dto)
    {
        await _userService.UploadPhotoAsync(id, dto);

        return RedirectToAction(nameof(GetById), new { id });
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _userService.DeleteAsync(id);
        return RedirectToAction("index", "users");
    }

    [HttpGet("getbyid/{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var user = await _userService.GetByIdAsync(id);
        return View(user);
    }

    [HttpGet("getall")]
    [ActionName("index")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return View(users);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByName(string name)
    {
        var users = await _userService.GetAllByNameAsync(name);
        return View(users);
    }

    [HttpGet("searchByUsername")]
    public async Task<IActionResult> SearchByUsername(string username)
    {
        var users = await _userService.GetAllByUsernameAsync(username);
        return View(users);
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var model = new LoginViewModel();
        return View(model);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel loginView)
    {
        var flag = await _userService.CheckCredentialsAsync(loginView.Username,loginView.Password);
        if (flag)
        {
            var user = (await _userService.GetAllByUsernameAsync(loginView.Username)).FirstOrDefault();
            return RedirectToAction("getbyid", new { id = user.Id});
        }

        return RedirectToAction("login");
    }
}
