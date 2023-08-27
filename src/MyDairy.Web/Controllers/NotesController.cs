using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyDairy.Service.DTOs.Notes;
using MyDairy.Service.Interfaces.Notes;

namespace MyDairy.Web.Controllers;

[Route("notes")]
public class NotesController : Controller
{
    private readonly INoteService _noteService;
    private readonly IMapper _mapper;

    public NotesController(INoteService noteService, IMapper mapper)
    {
        _noteService = noteService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var notes = await _noteService.GetAllAsync();
        return View(notes);
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details(long id)
    {
        var note = await _noteService.GetByIdAsync(id);
        return View(note);
    }

    [HttpGet("create")]
    public IActionResult Create(long userId)
    {
        return View(new NoteCreationDto() { UserId = userId });
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(NoteCreationDto noteDto)
    {
        var createdNote = await _noteService.CreateAsync(noteDto);
        return RedirectToAction(nameof(Details), new { id = createdNote.Id });
    }

    [HttpGet("update")]
    public async Task<IActionResult> Update(long id)
    {
        var note = await _noteService.GetByIdAsync(id);
        var mappedNote = _mapper.Map<NoteUpdateDto>(note);
        return View(mappedNote);
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(NoteUpdateDto noteDto)
    {
        var updatedNote = await _noteService.UpdateAsync(noteDto);
        return RedirectToAction("getbyid", "notes", new { id = updatedNote.Id });
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var note = await _noteService.GetByIdAsync(id);
        await _noteService.DeleteAsync(id);
        return RedirectToAction("getbyid", "users", new { id = note.User.Id });
    }
}
