using Microsoft.AspNetCore.Mvc;
using pruebaSVG.Models;
using System.Diagnostics;
using Svg;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace pruebaSVG.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Caja _caja;
        private Circulo _circulo;
        private Linea _linea;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _caja = new Caja() { color = 1, height = 50, width = 50, x = 100, y = 100 };
            _circulo = new Circulo() { radius = 30, centerX = 50, centerY = 50, color = 0 };
            _linea = new Linea() { StartX = 125, StartY = 125, color = 0, id = "line", EndX = 50, EndY = 50 };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FromLibrary()
        {
            SvgDocument svgDoc = new SvgDocument();
            svgDoc.Width = 500; svgDoc.Height = 500;
            var group = new SvgGroup();
            svgDoc.Children.Add(group);

            group.Children.Add(new SvgCircle
            {
                Radius = _circulo.radius,
                CenterX = _circulo.centerX,
                CenterY = _circulo.centerY,
                Fill = new SvgColourServer(_circulo.color == 0?Color.Black:Color.Red),
                StrokeWidth = 2
            });

            group.Children.Add(new SvgRectangle
            {
                Height = _caja.height,
                Width = _caja.width,
                Y = _caja.y,
                X = _caja.x,
                Fill = new SvgColourServer(_caja.color == 0 ? Color.Black : Color.Red),
                StrokeWidth = 2
            });

            group.Children.Add(new SvgLine
            {
                StartX = _linea.StartX,
                StartY = _linea.StartY,
                EndX = _linea.EndX,
                EndY = _linea.EndY,
                Fill = new SvgColourServer(_linea.color == 0 ? Color.Black : Color.Red),
                Stroke = new SvgColourServer(_linea.color == 0 ? Color.Black : Color.Red),
                StrokeWidth = 2,
                ID = "line"
            });

            ViewBag.svgString = svgDoc.GetXML();

            return View();
        }

        public IActionResult SVGFromDB()
        {
            VMSVGFromDB vm = new VMSVGFromDB();
            vm.circulo = _circulo;
            vm.caja = _caja;
            vm.linea = _linea;
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
