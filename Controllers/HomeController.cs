using Microsoft.AspNetCore.Mvc;
using pruebaSVG.Models;
using System.Diagnostics;
using Svg;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;

namespace pruebaSVG.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _hostingEnvironment;

        private Caja _caja;
        private Circulo _circulo;
        private Linea _linea;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _hostingEnvironment = environment;
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
                Fill = new SvgColourServer(_circulo.color == 0 ? Color.Black : Color.Red),
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


            ///////////////////////////// Texto dentro de un rectangulo y al lado una imagen cargada ///
            SvgDocument svgDoc2 = new SvgDocument();
            svgDoc2.Width = 200;
            svgDoc2.Height = 200;
            var group2 = new SvgGroup();
            svgDoc2.Children.Add(group2);

            SvgRectangle svgRect = new SvgRectangle();
            svgRect.Width = 180;
            svgRect.Height = 40;
            svgRect.Fill = new SvgColourServer(Color.Red);
            // el codigo en click no parece generar nada en frontend. Solo añade esto al elemento:  onclick="clickableRect/onclick"
            svgRect.Click += (s, e) => { Debug.WriteLine("Hola"); };
            svgRect.ID = "clickableRect";

            group2.Children.Add(svgRect);

            SvgText svgText1 = new SvgText();
            svgText1.Text = "Haz click aqui";
            svgText1.X = new SvgUnitCollection { 20 };
            svgText1.Y = new SvgUnitCollection { 35 };
            svgText1.Stroke = new SvgColourServer(Color.Black);
            svgText1.Color = new SvgColourServer(Color.Black);
            svgText1.Fill = new SvgColourServer(Color.Black);

            group2.Children.Add(svgText1);

            // Añadir una imagen bmp externa
            // La imagen no se carga bien. Puede ser porque es necesario poner algun tipo de configuracion extra            
            SvgImage svgImage1 = new SvgImage();
            svgImage1.Y = 100;
            svgImage1.Width = 50;
            svgImage1.Height = 50;
            svgImage1.Href = _hostingEnvironment.WebRootPath + "\\blackbuck.bmp";
            group2.Children.Add(svgImage1);

            ViewBag.svgString2 = svgDoc2.GetXML();

            ////////////// Leer SVG desde un fichero ///////////////
            var svgDoc3 = SvgDocument.Open(_hostingEnvironment.WebRootPath + "\\prueba.svg");
            ViewBag.svgString3 = svgDoc3.GetXML();

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
