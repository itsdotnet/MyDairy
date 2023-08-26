using Microsoft.AspNetCore.Mvc;
using MyDairy.Service.DTOs.Attachment;
using MyDairy.Service.DTOs.Users;
using MyDairy.Service.Interfaces.Users;

namespace MyDairy.Web.Controllers;

public class UsersController : Controller
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreationDto userDto)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _userService.DeleteAsync(id);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var user = await _userService.GetByIdAsync(id);
        return View(user); 
    }

    [HttpGet]
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
}
