using System.Diagnostics;
using CrudSP.Data;
using CrudSP.Models;
using Microsoft.AspNetCore.Mvc;


namespace CrudSP.Controllers;

public class HomeController : Controller
{

    private readonly DataAccess _dataAccess;
    public HomeController(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
    public IActionResult Index()
    {
        try 
        {
            var usuarios = _dataAccess.ListarUsuarios();
            return View(usuarios);
        }
        catch(Exception ex)
        {
            TempData["MensagemErro"] = "Ocorreu um erro na criação do usuário!";
            return View();
        }
    }

    public IActionResult Cadastrar()
    {
        return View();
    }

    public IActionResult Editar(int id)
    {
        var usuario = _dataAccess.BuscarUsuarioPorId(id);
        return View(usuario);
    }

    [HttpPost]
    public IActionResult Cadastrar(Usuario usuario)
    {
        //O model state valida os campos 
        if (ModelState.IsValid)
        {
            
            var result = _dataAccess.Cadastrar(usuario);

            if(result)
            {
                TempData["MensagemSucesso"] = "Usuário criado com sucesso!";
                return RedirectToAction("Index");
            }
            else 
            {
                TempData["MensagemErro"] = "Ocorreu um erro na criação do usuário!";
                return View(usuario);
            }
        
        }
        else
        {
            return View(usuario);
        }

    }

    public IActionResult Detalhes(int id)
    {
        var usuario = _dataAccess.BuscarUsuarioPorId(id);
        return View(usuario);
    }

    public IActionResult Remover(int id)
    {
        var result = _dataAccess.Remover(id) ; 
        if (result)
        {
            TempData["MensagemSucesso"] = "Usuário removido com Sucesso!";
        }
        else
        {
            TempData["MensagemErro"] = "Erro para remover usuário!";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Editar(Usuario usuario)
    {
        if(ModelState.IsValid)
        {
            var result = _dataAccess.Editar(usuario);

            if (result){
                TempData["MensagemSucesso"] = "Usuário editado com sucesso!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemErro"] = "Ocorreu um erro na edição do usuário!";
                return View(usuario);
            }

        }
        else
        {
            TempData["MensagemErro"] = "Ocorreu um erro na edição do usuário!";
            return View(usuario);

        }
    }



    
}
