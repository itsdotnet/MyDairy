using Microsoft.AspNetCore.Mvc;
using MyDairy.Service.DTOs.Attachment;
using MyDairy.Service.DTOs.Users;
using MyDairy.Service.Interfaces.Users;

namespace MyDairy.Web.Controllers;

[Route("users")]
[AutoValidateAntiforgeryToken]
public class UsersController : Controller
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("create")]
    [ActionName("create")]
    public async Task<IActionResult> Create()
    {   
        return View(new UserCreationDto());
    }

    [HttpPost("Register")]
    [ActionName("Register")]
    public async Task<IActionResult> Register(UserCreationDto userDto)
    {
        var newUser = await _userService.CreateAsync(userDto);
        return View(newUser);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UserUpdateDto userDto)
    {
        var updatedUser = await _userService.UpdateAsync(userDto);
        return View(updatedUser);
    }

    [HttpPatch("upload-photo")]
    public async Task<IActionResult> UploadAvatar(long id, [FromForm] AttachmentCreationDto dto)
    {
        await _userService.UploadPhotoAsync(id, dto);

        return RedirectToAction(nameof(GetById), new { id });
    }


    [HttpDelete("delete/{id}")]
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
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var flag = await _userService.CheckCredentialsAsync(username, password);
        if (flag)
            return RedirectToAction("getall", "notes");

        return RedirectToAction("index", "home");
    }
}
