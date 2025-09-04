using System.IO;
using System.Web.Mvc;

namespace HelloWorld.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Your application home page.";

            return View();
        }

        public JsonResult GetFileData()
        {
            var filePath = Path.Combine(@"C:\Apps\HelloWorld\IN", "input.txt");
            var fileContent = System.IO.File.Exists(filePath)
                ? System.IO.File.ReadAllText(filePath)
                : "(File not found)";

            return Json(new
            {
                FilePath = filePath,
                FileContent = fileContent
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}