using Microsoft.AspNetCore.Mvc;

namespace SM_ProyectoWeb.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult CapturarError()
        {
            return View("Error");
        }
    }
}
